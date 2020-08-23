PlayerPrefs Editor & Utilities, Version 1.1.0
Sabresaurus Ltd.
web: http://www.sabresaurus.com
contact: http://www.sabresaurus.com/contact

For a more comprehensive quick start guide and API documentation please go to http://sabresaurus.com/docs/playerprefs-editor-utilities-documentation/


== PlayerPrefs Editor ==

To open the PlayersPrefs Editor go to Window -> Player Prefs Editor

This will open an editor window which you can dock like any other Unity window.


= The Player Prefs List = 

If you have existing saved player prefs you should see them listed in the main window. You can change the values just by changing the value text box, you can also delete one of these existing player prefs by clicking the X button on the right.


= Search =

The editor supports filtering keys by enterring a keyword in the search textbox at the top. As you type the search results will refine. Search is case-insensitive and if auto-decrypt is turned on it will also work with encrypted player prefs.


= Adding A New Player Pref = 

At the bottom of the editor you'll see a section for adding a new player pref. There are toggle options to determine what type it is and a checkbox for whether the player pref should be encrypted. Once you've selected the right settings and filled in a key and value hit the Add button to instantly add the player pref.


= Deleting An Existing Player Pref =

To delete an existing player pref, click the X button next to the player pref in the list. You can also delete all player prefs by clicking the Delete All button at the bottom of the editor.



== PlayerPrefs Utilities ==

IMPORTANT: If using encryption, make sure you change the key specified in SimpleEncryption.cs, this will make sure your key is unique and make the protection stronger.

In PlayerPrefsUtility.cs you'll find a set of utility methods for dealing with encryption and also new data types. There is documentation within this file explaining how each method works. There is also additional documentation at http://sabresaurus.com/docs/playerprefs-editor-utilities-documentation/