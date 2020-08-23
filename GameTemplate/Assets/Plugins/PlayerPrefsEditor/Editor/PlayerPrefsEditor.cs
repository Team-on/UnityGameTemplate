using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Sabresaurus.PlayerPrefsExtensions;

public class PlayerPrefsEditor : EditorWindow
{
	// Represents a PlayerPref key-value record
    [Serializable]
    private struct PlayerPrefPair
    {
        public string Key
        {
            get;
            set;
        }

        public object Value
        {
            get;
            set;
        }
    }

	readonly DateTime MISSING_DATETIME = new DateTime(1601,1,1);

    // Natively player prefs can be one of these three types
    enum PlayerPrefType { Float = 0, Int, String };

    // The actual cached store of player pref records fetched from registry or plist
    List<PlayerPrefPair> deserializedPlayerPrefs = new List<PlayerPrefPair>();

	// When a search is in effect the search results are cached in this list
    List<PlayerPrefPair> filteredPlayerPrefs = new List<PlayerPrefPair>();

    // Track last successful deserialisation to prevent doing this too often. On OSX this uses the player prefs file
    // last modified time, on Windows we just poll repeatedly and use this to prevent polling again too soon.
    DateTime? lastDeserialization = null;

    // The view position of the player prefs scroll view
    Vector2 scrollPosition;

	// The scroll position from last frame (used with scrollPosition to detect user scrolling)
    Vector2 lastScrollPosition;

    // Prevent OnInspector() forcing a repaint every time it's called
    int inspectorUpdateFrame = 0;

	// Automatically attempt to decrypt keys and values that are detected as encrypted
    bool automaticDecryption = false;

	// Filter the keys by search
	string searchFilter = string.Empty;
	
	// Because of some issues with deleting from OnGUI, we defer it to OnInspectorUpdate() instead
	string keyQueuedForDeletion = null;

    // Company and product name for importing player prefs from other projects
    string importCompanyName = "";
    string importProductName = "";

    #region Adding New PlayerPref
    // This is the current type of player pref that the user is about to create
    PlayerPrefType newEntryType = PlayerPrefType.String;

	// Whether the player pref should be encrypted
    bool newEntryIsEncrypted = false;

    // The identifier of the new player pref
    string newEntryKey = "";

    // Value of the player pref about to be created (must be tracked differently for each type)
    float newEntryValueFloat = 0;
    int newEntryValueInt = 0;
	string newEntryValueString = "";
#endregion

    [MenuItem("Window/Player Prefs Editor")]
    private static void Init()
    {
        // Get existing open window or if none, make a new one:
        PlayerPrefsEditor editor = (PlayerPrefsEditor)EditorWindow.GetWindow(typeof(PlayerPrefsEditor));

        // Require the editor window to be at least 300 pixels wide
        Vector2 minSize = editor.minSize;
        minSize.x = 230;
        editor.minSize = minSize;

        editor.importCompanyName = PlayerSettings.companyName;
        editor.importProductName = PlayerSettings.productName;
    }

	/// <summary>
	/// This returns an array of the stored PlayerPrefs from the file system (OSX) or registry (Windows), to allow 
	/// us to to look up what's actually in the PlayerPrefs. This is used as a kind of lookup table.
	/// </summary>
	private PlayerPrefPair[] RetrieveSavedPrefs(string companyName, string productName)
    {
		if(Application.platform == RuntimePlatform.OSXEditor)
		{
			// From Unity docs: On Mac OS X PlayerPrefs are stored in ~/Library/Preferences folder, in a file named unity.[company name].[product name].plist, where company and product names are the names set up in Project Settings. The same .plist file is used for both Projects run in the Editor and standalone players.
	
			// Construct the plist filename from the project's settings
			string plistFilename = string.Format("unity.{0}.{1}.plist", companyName, productName);
			// Now construct the fully qualified path
			string playerPrefsPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library/Preferences"), plistFilename);
	
			// Parse the player prefs file if it exists
			if(File.Exists(playerPrefsPath))
			{
				// Parse the plist then cast it to a Dictionary
				object plist = Plist.readPlist(playerPrefsPath);
				
				Dictionary<string, object> parsed = plist as Dictionary<string, object>;
	
				// Convert the dictionary data into an array of PlayerPrefPairs
				PlayerPrefPair[] tempPlayerPrefs = new PlayerPrefPair[parsed.Count];
				int i = 0;
				foreach (KeyValuePair<string, object> pair in parsed)
				{
					if(pair.Value.GetType () == typeof(double))
					{
						// Some float values may come back as double, so convert them back to floats
						tempPlayerPrefs[i] = new PlayerPrefPair() { Key = pair.Key, Value = (float)(double)pair.Value };
					}
					else
					{
						tempPlayerPrefs[i] = new PlayerPrefPair() { Key = pair.Key, Value = pair.Value };
					}
					i++;
				}
				// Return the results
				return tempPlayerPrefs;
			}
			else
			{
				// No existing player prefs saved (which is valid), so just return an empty array
				return new PlayerPrefPair[0];
			}
		}
		else if(Application.platform == RuntimePlatform.WindowsEditor)
		{
            // From Unity docs: On Windows, PlayerPrefs are stored in the registry under HKCU\Software\[company name]\[product name] key, where company and product names are the names set up in Project Settings.
#if UNITY_5_5_OR_NEWER
            // From Unity 5.5 editor player prefs moved to a specific location
            Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Unity\\UnityEditor\\" + companyName + "\\" + productName);
#else
            Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\" + companyName + "\\" + productName);
#endif

            // Parse the registry if the specified registryKey exists
            if (registryKey != null)
	        {
				// Get an array of what keys (registry value names) are stored
				string[] valueNames = registryKey.GetValueNames();
	
				// Create the array of the right size to take the saved player prefs
				PlayerPrefPair[] tempPlayerPrefs = new PlayerPrefPair[valueNames.Length];
	
				// Parse and convert the registry saved player prefs into our array
				int i = 0;
				foreach (string valueName in valueNames)
				{
					string key = valueName;
	
					// Remove the _h193410979 style suffix used on player pref keys in Windows registry
					int index = key.LastIndexOf("_");
					key = key.Remove(index, key.Length - index);
	
					// Get the value from the registry
					object ambiguousValue = registryKey.GetValue(valueName);
	
					// Unfortunately floats will come back as an int (at least on 64 bit) because the float is stored as
					// 64 bit but marked as 32 bit - which confuses the GetValue() method greatly! 
					if (ambiguousValue.GetType() == typeof(int))
					{
						// If the player pref is not actually an int then it must be a float, this will evaluate to true
						// (impossible for it to be 0 and -1 at the same time)
						if (PlayerPrefs.GetInt(key, -1) == -1 && PlayerPrefs.GetInt(key, 0) == 0)
						{
							// Fetch the float value from PlayerPrefs in memory
							ambiguousValue = PlayerPrefs.GetFloat(key);
						}
					}
					else if(ambiguousValue.GetType() == typeof(byte[]))
					{
						// On Unity 5 a string may be stored as binary, so convert it back to a string
						ambiguousValue = System.Text.Encoding.Default.GetString((byte[])ambiguousValue);
					}
					// Assign the key and value into the respective record in our output array
					tempPlayerPrefs[i] = new PlayerPrefPair() { Key = key, Value = ambiguousValue };
					i++;
				}
				// Return the results
				return tempPlayerPrefs;
	        }
	        else
	        {
				// No existing player prefs saved (which is valid), so just return an empty array
				return new PlayerPrefPair[0];
	        }
		}
		else
		{
			throw new NotSupportedException("PlayerPrefsEditor doesn't support this Unity Editor platform");
		}
    }

	private void UpdateSearch()
    {
		// Clear any existing cached search results
        filteredPlayerPrefs.Clear();

		// Don't attempt to find the search results if a search filter hasn't actually been supplied
        if (string.IsNullOrEmpty(searchFilter))
        {
            return;
        }

        int entryCount = deserializedPlayerPrefs.Count;

		// Iterate through all the cached results and add any matches to filteredPlayerPrefs
        for (int i = 0; i < entryCount; i++)
        {
            string fullKey = deserializedPlayerPrefs[i].Key;
            string displayKey = fullKey;

			// Special case for encrypted keys in auto decrypt mode, search should use decrypted values
            bool isEncryptedPair = PlayerPrefsUtility.IsEncryptedKey(deserializedPlayerPrefs[i].Key);
            if (automaticDecryption && isEncryptedPair)
            {
                displayKey = PlayerPrefsUtility.DecryptKey(fullKey);
            }

			// If the key contains the search filter (ToLower used on both parts to make this case insensitive)
            if (displayKey.ToLower().Contains(searchFilter.ToLower()))
            {
                filteredPlayerPrefs.Add(deserializedPlayerPrefs[i]);
            }
        }
    }

	private void DrawSearchBar()
    {
        EditorGUILayout.BeginHorizontal();
        // Heading
		GUILayout.Label("Search", GUILayout.MaxWidth(50));
		// Actual search box
        string newSearchFilter = EditorGUILayout.TextField(searchFilter);

		// If the requested search filter has changed
        if (newSearchFilter != searchFilter)
        {
            searchFilter = newSearchFilter;
			// Trigger UpdateSearch to calculate new search results
            UpdateSearch();
        }

        EditorGUILayout.EndHorizontal();
    }

	private void DrawMainList()
    {
		// The bold table headings
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Key", EditorStyles.boldLabel);
        GUILayout.Label("Value", EditorStyles.boldLabel);
        GUILayout.Label("Type", EditorStyles.boldLabel, GUILayout.Width(37));
        GUILayout.Label("Del", EditorStyles.boldLabel, GUILayout.Width(25));
        EditorGUILayout.EndHorizontal();

		// Create a GUIStyle that can be manipulated for the various text fields
		GUIStyle textFieldStyle = new GUIStyle (GUI.skin.textField);

		// Could be dealing with either the full list or search results, so get the right list
        List<PlayerPrefPair> activePlayerPrefs = deserializedPlayerPrefs;

        if (!string.IsNullOrEmpty(searchFilter))
        {
            activePlayerPrefs = filteredPlayerPrefs;
        }

		// Cache the entry count
        int entryCount = activePlayerPrefs.Count;

		// Record the last scroll position so we can calculate if the user has scrolled this frame
        lastScrollPosition = scrollPosition;

		// Start the scrollable area
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
		// Ensure the scroll doesn't go below zero
		if(scrollPosition.y < 0)
		{
			scrollPosition.y = 0;
		}
		
		// The following code has been optimised so that rather than attempting to draw UI for every single PlayerPref
		// it instead only draws the UI for those currently visible in the scroll view and pads above and below those
		// results to maintain the right size using GUILayout.Space(). This enables us to work with thousands of 
		// PlayerPrefs without slowing the interface to a halt.

		// Fixed height of one of the rows in the table
        float rowHeight = 18;

        // Determine how many rows are visible on screen. For simplicity, use Screen.height (the overhead is negligible)
        int visibleCount = Mathf.CeilToInt(Screen.height / rowHeight);

		// Determine the index of the first player pref that should be drawn as visible in the scrollable area
        int firstShownIndex = Mathf.FloorToInt(scrollPosition.y / rowHeight);

		// Determine the bottom limit of the visible player prefs (last shown index + 1)
        int shownIndexLimit = firstShownIndex + visibleCount;

		// If the actual number of player prefs is smaller than the caculated limit, reduce the limit to match
        if (entryCount < shownIndexLimit)
        {
            shownIndexLimit = entryCount;
        }

		// If the number of displayed player prefs is smaller than the number we can display (like we're at the end
		// of the list) then move the starting index back to adjust
        if (shownIndexLimit - firstShownIndex < visibleCount)
        {
            firstShownIndex -= visibleCount - (shownIndexLimit - firstShownIndex);
        }

		// Can't have a negative index of a first shown player pref, so clamp to 0
        if (firstShownIndex < 0)
        {
            firstShownIndex = 0;
        }

		// Pad above the on screen results so that we're not wasting draw calls on invisible UI and the drawn player
		// prefs end up in the same place in the list
        GUILayout.Space(firstShownIndex * rowHeight);

		// For each of the on screen results
        for (int i = firstShownIndex; i < shownIndexLimit; i++)
        {
			// Detect if it's an encrypted player pref (these have key prefixes)
            bool isEncryptedPair = PlayerPrefsUtility.IsEncryptedKey(activePlayerPrefs[i].Key);

			// Colour code encrypted player prefs blue
            if (isEncryptedPair)
            {
				if(UsingProSkin)
				{
					textFieldStyle.normal.textColor = new Color(0.5f,0.5f,1);
					textFieldStyle.focused.textColor = new Color(0.5f,0.5f,1);
				}
				else
				{
					textFieldStyle.normal.textColor = new Color(0,0,1);
					textFieldStyle.focused.textColor = new Color(0,0,1);
				}
            }
            else
            {
				// Normal player prefs are just black
				textFieldStyle.normal.textColor = GUI.skin.textField.normal.textColor;
				textFieldStyle.focused.textColor = GUI.skin.textField.focused.textColor;
            }

			// The full key is the key that's actually stored in player prefs
            string fullKey = activePlayerPrefs[i].Key;

			// Display key is used so in the case of encrypted keys, we display the decrypted version instead (in
			// auto-decrypt mode).
            string displayKey = fullKey;

			// Used for accessing the type information stored against the player pref
			object deserializedValue = activePlayerPrefs[i].Value;

			// Track whether the auto decrypt failed, so we can instead fallback to encrypted values and mark it red
			bool failedAutoDecrypt = false;

			// If this is an encrypted play pref and we're attempting to decrypt them, try to decrypt it!
			if (isEncryptedPair && automaticDecryption)
            {
				// This may throw exceptions (e.g. if private key changes), so wrap in a try-catch
				try
				{
	                deserializedValue = PlayerPrefsUtility.GetEncryptedValue(fullKey, (string)deserializedValue);
	                displayKey = PlayerPrefsUtility.DecryptKey(fullKey);
				}
				catch
				{
					// Change the colour to red to highlight the decrypt failed
					textFieldStyle.normal.textColor = Color.red;
					textFieldStyle.focused.textColor = Color.red;

					// Track that the auto decrypt failed, so we can prevent any editing 
					failedAutoDecrypt = true;
				}
            }

            EditorGUILayout.BeginHorizontal();

			// The type of player pref being stored (in auto decrypt mode this works with the decrypted values too)
            Type valueType;

			// If it's an encrypted playerpref, we're automatically decrypting and it didn't fail the earlier 
			// auto decrypt test
			if (isEncryptedPair && automaticDecryption && !failedAutoDecrypt)
            {
				// Get the encrypted string
                string encryptedValue = PlayerPrefs.GetString(fullKey);
				// Set valueType appropiately based on which type identifier prefix the encrypted string starts with
                if (encryptedValue.StartsWith(PlayerPrefsUtility.VALUE_FLOAT_PREFIX))
                {
                    valueType = typeof(float);
                }
                else if (encryptedValue.StartsWith(PlayerPrefsUtility.VALUE_INT_PREFIX))
                {
                    valueType = typeof(int);
                }
                else if (encryptedValue.StartsWith(PlayerPrefsUtility.VALUE_STRING_PREFIX) || string.IsNullOrEmpty (encryptedValue))
                {
					// Special case here, empty encrypted values will also report as strings
                    valueType = typeof(string);
                }
                else
                {
                    throw new InvalidOperationException("Could not decrypt item, no match found in known encrypted key prefixes");
                }
            }
            else
            {
				// Otherwise fallback to the type of the cached value (for non-encrypted values this will be 
				// correct). For encrypted values when not in auto-decrypt mode, this will return string type
                valueType = deserializedValue.GetType();
            }

			// Display the PlayerPref key
            EditorGUILayout.TextField(displayKey, textFieldStyle);

            // Value display and user editing
			// If we're dealing with a float
            if (valueType == typeof(float))
            {
                float initialValue;
                if (isEncryptedPair && automaticDecryption)
                {
					// Automatically decrypt the value if encrypted and in auto-decrypt mode
                    initialValue = PlayerPrefsUtility.GetEncryptedFloat(displayKey);
                }
                else
                {
					// Otherwise fetch the latest plain value from PlayerPrefs in memory
                    initialValue = PlayerPrefs.GetFloat(fullKey);
                }

				// Display the float editor field and get any changes in value
				float newValue = EditorGUILayout.FloatField(initialValue, textFieldStyle);

				// If the value has changed
                if (newValue != initialValue)
                {
					// Store the changed value in player prefs, encrypting if necessary
                    if (isEncryptedPair)
                    {
                        string encryptedValue = PlayerPrefsUtility.VALUE_FLOAT_PREFIX + SimpleEncryption.EncryptFloat(newValue);
                        PlayerPrefs.SetString(fullKey, encryptedValue);
                    }
                    else
                    {
                        PlayerPrefs.SetFloat(fullKey, newValue);
                    }
					// Save PlayerPrefs
                    PlayerPrefs.Save();
                }
				// Display the PlayerPref type
                GUILayout.Label("float", GUILayout.Width(37));
            }
            else if (valueType == typeof(int)) // if we're dealing with an int
            {
                int initialValue;
                if (isEncryptedPair && automaticDecryption)
                {
					// Automatically decrypt the value if encrypted and in auto-decrypt mode
                    initialValue = PlayerPrefsUtility.GetEncryptedInt(displayKey);
                }
                else
                {
					// Otherwise fetch the latest plain value from PlayerPrefs in memory
                    initialValue = PlayerPrefs.GetInt(fullKey);
                }

				// Display the int editor field and get any changes in value
				int newValue = EditorGUILayout.IntField(initialValue, textFieldStyle);
                
				// If the value has changed
				if (newValue != initialValue)
                {
					// Store the changed value in player prefs, encrypting if necessary
                    if (isEncryptedPair)
                    {
                        string encryptedValue = PlayerPrefsUtility.VALUE_INT_PREFIX + SimpleEncryption.EncryptInt(newValue);
                        PlayerPrefs.SetString(fullKey, encryptedValue);
                    }
                    else
                    {
                        PlayerPrefs.SetInt(fullKey, newValue);
                    }
					// Save PlayerPrefs
                    PlayerPrefs.Save();
                }
				// Display the PlayerPref type
                GUILayout.Label("int", GUILayout.Width(37));
            }
            else if (valueType == typeof(string)) // if we're dealing with a string
            {
                string initialValue;
				if (isEncryptedPair && automaticDecryption && !failedAutoDecrypt)
                {
					// Automatically decrypt the value if encrypted and in auto-decrypt mode
                    initialValue = PlayerPrefsUtility.GetEncryptedString(displayKey);
                }
                else
                {
					// Otherwise fetch the latest plain value from PlayerPrefs in memory
                    initialValue = PlayerPrefs.GetString(fullKey);
                }

				// Display the text (string) editor field and get any changes in value
				string newValue = EditorGUILayout.TextField(initialValue, textFieldStyle);

				// If the value has changed
				if (newValue != initialValue && !failedAutoDecrypt)
                {
					// Store the changed value in player prefs, encrypting if necessary
                    if (isEncryptedPair)
                    {
                        string encryptedValue = PlayerPrefsUtility.VALUE_STRING_PREFIX + SimpleEncryption.EncryptString(newValue);
                        PlayerPrefs.SetString(fullKey, encryptedValue);
                    }
                    else
                    {
                        PlayerPrefs.SetString(fullKey, newValue);
                    }
					// Save PlayerPrefs
                    PlayerPrefs.Save();
                }
                if (isEncryptedPair && !automaticDecryption && !string.IsNullOrEmpty(initialValue))
                {
					// Because encrypted values when not in auto-decrypt mode are stored as string, determine their
					// encrypted type and display that instead for these encrypted PlayerPrefs
                    PlayerPrefType playerPrefType = (PlayerPrefType)(int)char.GetNumericValue(initialValue[0]);
                    GUILayout.Label(playerPrefType.ToString().ToLower(), GUILayout.Width(37));
                }
                else
                {
					// Display the PlayerPref type
                    GUILayout.Label("string", GUILayout.Width(37));
                }
            }

            // Delete button
            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
				// Delete the key from player prefs
                PlayerPrefs.DeleteKey(fullKey);
				// Tell Unity to Save PlayerPrefs
                PlayerPrefs.Save();
				// Delete the cached record so the list updates immediately
                DeleteCachedRecord(fullKey);
            }
            EditorGUILayout.EndHorizontal();
        }

		// Calculate the padding at the bottom of the scroll view (because only visible player pref rows are drawn)
        float bottomPadding = (entryCount - shownIndexLimit) * rowHeight;

		// If the padding is positive, pad the bottom so that the layout and scroll view size is correct still
        if (bottomPadding > 0)
        {
            GUILayout.Space(bottomPadding);
        }

        EditorGUILayout.EndScrollView();

		// Display the number of player prefs
        GUILayout.Label("Entry Count: " + entryCount);
    }

	private void DrawAddEntry()
	{
		// Create a GUIStyle that can be manipulated for the various text fields
		GUIStyle textFieldStyle = new GUIStyle (GUI.skin.textField);
		
		// Create a space
        EditorGUILayout.Space();

		// Heading
        GUILayout.Label("Add Player Pref", EditorStyles.boldLabel);

		// UI for whether the new player pref is encrypted and what type it is
        EditorGUILayout.BeginHorizontal();
        newEntryIsEncrypted = GUILayout.Toggle(newEntryIsEncrypted, "Encrypt");
        newEntryType = (PlayerPrefType)GUILayout.SelectionGrid((int)newEntryType, new string[] { "float", "int", "string" }, 3);
        EditorGUILayout.EndHorizontal();

		// Key and Value headings
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Key", EditorStyles.boldLabel);
        GUILayout.Label("Value", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();

		// If the new value will be encrypted tint the text boxes blue (in line with the display style for existing
		// encrypted player prefs)
        if (newEntryIsEncrypted)
        {
			if(UsingProSkin)
			{
				textFieldStyle.normal.textColor = new Color(0.5f,0.5f,1);
				textFieldStyle.focused.textColor = new Color(0.5f,0.5f,1);
			}
			else
			{
				textFieldStyle.normal.textColor = new Color(0,0,1);
				textFieldStyle.focused.textColor = new Color(0,0,1);
			}
        }

        EditorGUILayout.BeginHorizontal();

		// Track the next control so we can detect key events in it
        GUI.SetNextControlName("newEntryKey");
		// UI for the new key text box
		newEntryKey = EditorGUILayout.TextField(newEntryKey, textFieldStyle);

		// Track the next control so we can detect key events in it
        GUI.SetNextControlName("newEntryValue");
        
		// Display the correct UI field editor based on what type of player pref is being created
		if (newEntryType == PlayerPrefType.Float)
        {
			newEntryValueFloat = EditorGUILayout.FloatField(newEntryValueFloat, textFieldStyle);
        }
        else if (newEntryType == PlayerPrefType.Int)
        {
			newEntryValueInt = EditorGUILayout.IntField(newEntryValueInt, textFieldStyle);
        }
        else
        {
			newEntryValueString = EditorGUILayout.TextField(newEntryValueString, textFieldStyle);
        }

		// If the user hit enter while either the key or value fields were being edited
        bool keyboardAddPressed = Event.current.isKey && Event.current.keyCode == KeyCode.Return && Event.current.type == EventType.KeyUp && (GUI.GetNameOfFocusedControl() == "newEntryKey" || GUI.GetNameOfFocusedControl() == "newEntryValue");

		// If the user clicks the Add button or hits return (and there is a non-empty key), create the player pref
        if ((GUILayout.Button("Add", GUILayout.Width(40)) || keyboardAddPressed) && !string.IsNullOrEmpty(newEntryKey))
        {
			// If the player pref we're creating is encrypted
            if (newEntryIsEncrypted)
            {
				// Encrypt the key
                string encryptedKey = PlayerPrefsUtility.KEY_PREFIX + SimpleEncryption.EncryptString(newEntryKey);

                // Note: All encrypted values are stored as string
                string encryptedValue;

				// Calculate the encrypted value
                if (newEntryType == PlayerPrefType.Float)
                {
                    encryptedValue = PlayerPrefsUtility.VALUE_FLOAT_PREFIX + SimpleEncryption.EncryptFloat(newEntryValueFloat);
                }
                else if (newEntryType == PlayerPrefType.Int)
                {
                    encryptedValue = PlayerPrefsUtility.VALUE_INT_PREFIX + SimpleEncryption.EncryptInt(newEntryValueInt);
                }
                else
                {
                    encryptedValue = PlayerPrefsUtility.VALUE_STRING_PREFIX + SimpleEncryption.EncryptString(newEntryValueString);
                }

				// Record the new player pref in PlayerPrefs
                PlayerPrefs.SetString(encryptedKey, encryptedValue);

                // Cache the addition
                CacheRecord(encryptedKey, encryptedValue);
            }
            else
            {
                if (newEntryType == PlayerPrefType.Float)
                {
					// Record the new player pref in PlayerPrefs
                    PlayerPrefs.SetFloat(newEntryKey, newEntryValueFloat);
                    // Cache the addition
                    CacheRecord(newEntryKey, newEntryValueFloat);
                }
                else if (newEntryType == PlayerPrefType.Int)
                {
					// Record the new player pref in PlayerPrefs
                    PlayerPrefs.SetInt(newEntryKey, newEntryValueInt);
                    // Cache the addition
                    CacheRecord(newEntryKey, newEntryValueInt);
                }
                else
                {
					// Record the new player pref in PlayerPrefs
                    PlayerPrefs.SetString(newEntryKey, newEntryValueString);
                    // Cache the addition
                    CacheRecord(newEntryKey, newEntryValueString);
                }
            }

			// Tell Unity to save the PlayerPrefs
            PlayerPrefs.Save();

            // Force a repaint since hitting the return key won't invalidate layout on its own
            Repaint();

            // Reset the values
            newEntryKey = "";
            newEntryValueFloat = 0;
            newEntryValueInt = 0;
            newEntryValueString = "";

			// Deselect
			GUI.FocusControl("");
        }

        EditorGUILayout.EndHorizontal();
    }

	private void DrawBottomMenu()
    {
        EditorGUILayout.Space();
        // Allow the user to import player prefs from another project (helpful when renaming product name)
        GUILayout.Label("Import", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();

        GUIStyle textFieldStyle = new GUIStyle(GUI.skin.textField);

        if (newEntryIsEncrypted)
        {
            if (UsingProSkin)
            {
                textFieldStyle.normal.textColor = new Color(0.5f, 0.5f, 1);
                textFieldStyle.focused.textColor = new Color(0.5f, 0.5f, 1);
            }
            else
            {
                textFieldStyle.normal.textColor = new Color(0, 0, 1);
                textFieldStyle.focused.textColor = new Color(0, 0, 1);
            }
        }

        importCompanyName = EditorGUILayout.TextField(importCompanyName, textFieldStyle);
        importProductName = EditorGUILayout.TextField(importProductName, textFieldStyle);
        if(GUILayout.Button("Import"))
        {
            // Walk through all the imported player prefs and apply them to the current player prefs
            PlayerPrefPair[] importedPairs = RetrieveSavedPrefs(importCompanyName, importProductName);
            for (int i = 0; i < importedPairs.Length; i++)
            {
                Type type = importedPairs[i].Value.GetType();
                if (type == typeof(float))
                    PlayerPrefs.SetFloat(importedPairs[i].Key, (float)importedPairs[i].Value);
                else if (type == typeof(int))
                    PlayerPrefs.SetInt(importedPairs[i].Key, (int)importedPairs[i].Value);
                else if (type == typeof(string))
                    PlayerPrefs.SetString(importedPairs[i].Key, (string)importedPairs[i].Value);

                // Cache any new records until they are reimported from disk
                CacheRecord(importedPairs[i].Key, importedPairs[i].Value);
            }

            // Force a save
            PlayerPrefs.Save();
        }
        EditorGUILayout.EndHorizontal();

        // UI for toggling automatic decryption on and off
        automaticDecryption = EditorGUILayout.Toggle("Auto-Decryption", automaticDecryption);

        EditorGUILayout.BeginHorizontal();
		// Delete all player prefs
        if (GUILayout.Button("Delete All Preferences"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            // Clear the cache too, for an instant visibility update for OSX
            deserializedPlayerPrefs.Clear();
        }
        GUILayout.Space(15);

        // Mainly needed for OSX, this will encourage PlayerPrefs to save to file (but still may take a few seconds)
        if (GUILayout.Button("Force Save"))
        {
            PlayerPrefs.Save();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void OnGUI()
    {
		EditorGUILayout.Space();

		if(Application.platform == RuntimePlatform.OSXEditor)
		{
			// Construct the plist filename from the project's settings
			string plistFilename = string.Format("unity.{0}.{1}.plist", PlayerSettings.companyName, PlayerSettings.productName);
			// Now construct the fully qualified path
			string playerPrefsPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library/Preferences"), plistFilename);
	
			// Determine when the plist was last written to
			DateTime lastWriteTime = File.GetLastWriteTimeUtc(playerPrefsPath); 

			// If we haven't deserialized the player prefs already, or the written file has changed then deserialize 
			// the latest version
			if(!lastDeserialization.HasValue || lastDeserialization.Value != lastWriteTime)
			{
				// Deserialize the actual player prefs from file into a cache
				deserializedPlayerPrefs = new List<PlayerPrefPair>(RetrieveSavedPrefs(PlayerSettings.companyName, PlayerSettings.productName));

				// Record the version of the file we just read, so we know if it changes in the future
				lastDeserialization = lastWriteTime;
			}
			
			if(lastWriteTime != MISSING_DATETIME)
			{
				GUILayout.Label("PList Last Written: " + lastWriteTime.ToString());
			}
			else
			{
				GUILayout.Label("PList Does Not Exist");
			}
		}
		else if(Application.platform == RuntimePlatform.WindowsEditor)
		{
	        // Windows works a bit differently to OSX, we just regularly query the registry. So don't query too often
	        if (!lastDeserialization.HasValue || DateTime.UtcNow - lastDeserialization.Value > TimeSpan.FromMilliseconds(500))
	        {
	            // Deserialize the actual player prefs from registry into a cache
	            deserializedPlayerPrefs = new List<PlayerPrefPair>(RetrieveSavedPrefs(PlayerSettings.companyName, PlayerSettings.productName));
	
	            // Record the latest time, so we don't fetch again too quickly
	            lastDeserialization = DateTime.UtcNow;
	        }
		}

        DrawSearchBar();

        DrawMainList();

        DrawAddEntry();

        DrawBottomMenu();

        // If the user has scrolled, deselect - this is because control IDs within carousel will change when scrolled
        // so we'd end up with the wrong box selected.
        if (scrollPosition != lastScrollPosition)
        {
            // Deselect
            GUI.FocusControl("");
        }
    }

	private void CacheRecord(string key, object value)
    {
		// First of all check if this key already exists, if so replace it's value with the new value
		bool replaced = false;
		
		int entryCount = deserializedPlayerPrefs.Count;
        for (int i = 0; i < entryCount; i++)
        {
			// Found the key - it exists already
            if (deserializedPlayerPrefs[i].Key == key)
            {
				// Update the cached pref with the new value
				deserializedPlayerPrefs[i] = new PlayerPrefPair() { Key = key, Value = value };
				// Mark the replacement so we no longer need to add it
				replaced = true;
				break;
			}
		}
		
		// Player pref doesn't already exist (and wasn't replaced) so add it as new
		if(!replaced)
		{
			// Cache a player pref the user just created so it can be instantly display (mainly for OSX)
	        deserializedPlayerPrefs.Add(new PlayerPrefPair() { Key = key, Value = value });
		}

		// Update the search if it's active
        UpdateSearch();
    }

	private void DeleteCachedRecord(string fullKey)
    {
		keyQueuedForDeletion = fullKey;		
    }

	// OnInspectorUpdate() is called by Unity at 10 times a second
	private void OnInspectorUpdate()
    {
		// If a player pref has been specified for deletion
		if(!string.IsNullOrEmpty(keyQueuedForDeletion))
		{
			// If the user just deleted a player pref, find the ID and defer it for deletion by OnInspectorUpdate()
	        if (deserializedPlayerPrefs != null)
	        {
	            int entryCount = deserializedPlayerPrefs.Count;
	            for (int i = 0; i < entryCount; i++)
	            {
	                if (deserializedPlayerPrefs[i].Key == keyQueuedForDeletion)
	                {
	                    deserializedPlayerPrefs.RemoveAt(i);
	                    break;
	                }
	            }
	        }

			// Remove the queued key since we've just deleted it
            keyQueuedForDeletion = null;

			// Update the search results and repaint the window
            UpdateSearch();
            Repaint();
        }
        else if (inspectorUpdateFrame % 10 == 0) // Once a second (every 10th frame)
        {
			// Force the window to repaint
            Repaint();
        }

		// Track what frame we're on, so we can call code less often
        inspectorUpdateFrame++;
    }
	
	bool UsingProSkin
	{
		get
		{
#if UNITY_3_4
			if(EditorPrefs.GetInt("UserSkin") == 1)		
			{
				return true;
			}
			else
			{
				return false;
			}
#else
			return EditorGUIUtility.isProSkin;
#endif
		}
	}
}