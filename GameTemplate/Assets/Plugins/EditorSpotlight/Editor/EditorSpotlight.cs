// TODO Adjust the window size when input is empty

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class EditorSpotlight : EditorWindow, IHasCustomMenu
{
     /*************************************************************************************************
     *** Variables
     *************************************************************************************************/
     private static EditorSpotlight instance;

     private const string PlaceholderInput = "Spotlight Search...";
     private const string SearchHistoryKey = "SearchHistoryKey";
     public const int BaseHeight = 90;

     private List<string> hits = new List<string>();
     private string input;
     private int selectedIndex;

     private SearchHistory history;

     private bool close;

     /*************************************************************************************************
     *** OnGUI
     *************************************************************************************************/
     private void OnGUI()
     {
          if (close)
          {
               Close();
               return;
          }

          HandleEvents();

          GUILayout.BeginHorizontal();
          GUILayout.Space(15);
          GUILayout.BeginVertical();
          GUILayout.Space(15);

          GUI.SetNextControlName("SpotlightInput");
          string prevInput = input;
          input = GUILayout.TextField(input, Styles.inputFieldStyle, GUILayout.Height(60));
          EditorGUI.FocusTextInControl("SpotlightInput");

          if (input != prevInput)
               ProcessInput();

          if (selectedIndex >= hits.Count)
               selectedIndex = hits.Count - 1;
          else if (selectedIndex <= 0)
               selectedIndex = 0;

          if (string.IsNullOrEmpty(input))
               GUI.Label(GUILayoutUtility.GetLastRect(), PlaceholderInput, Styles.placeholderStyle);

          GUILayout.BeginHorizontal();
          GUILayout.Space(6);

          if (!string.IsNullOrEmpty(input))
               VisualizeHits();

          GUILayout.Space(6);
          GUILayout.EndHorizontal();

          GUILayout.Space(15);
          GUILayout.EndVertical();
          GUILayout.Space(15);
          GUILayout.EndHorizontal();
     }

     /*************************************************************************************************
     *** OnLostFocus
     *************************************************************************************************/
     private void OnLostFocus()
     {
          close = true;
     }

     /*************************************************************************************************
     *** Methods
     *************************************************************************************************/
     [MenuItem("Window/Spotlight &f")]
     private static void Init()
     {
          if (instance == null)
          {
               instance = CreateInstance<EditorSpotlight>();
               instance.titleContent = new GUIContent("Spotlight");
               instance.close = false;

               Rect newPosition = instance.position;
               newPosition.height = BaseHeight;
               newPosition.xMin = (Screen.currentResolution.width / 2) - (500 / 2);
               newPosition.yMin = Screen.currentResolution.height * 0.3f;
               instance.position = newPosition;
               instance.EnforceWindowSize();

               instance.ShowUtility();
               instance.Reset();
          }
     }

     // Input
     private void HandleEvents()
     {
          Event current = Event.current;

          if (current.type == EventType.KeyDown)
          {
               switch (current.keyCode)
               {
                    case KeyCode.UpArrow:
                         current.Use();
                         selectedIndex--;
                         break;

                    case KeyCode.DownArrow:
                         current.Use();
                         selectedIndex++;
                         break;

                    case KeyCode.Return:
                         current.Use();
                         OpenSelectedAssetAndClose();
                         break;

                    case KeyCode.Escape:
                         close = true;
                         break;

                    case KeyCode.Tab:
                         FocusSelection();
                         break;
               }
          }
     }

     private void Reset()
     {
          input = "";
          hits.Clear();

          string json = EditorPrefs.GetString(SearchHistoryKey, JsonUtility.ToJson(new SearchHistory()));
          history = JsonUtility.FromJson<SearchHistory>(json);

          Focus();
     }

     private void ProcessInput()
     {
          input = input.ToLower();
          string[] assetHits = AssetDatabase.FindAssets(input) ?? new string[0];
          hits = assetHits.ToList();

          // Sort the search hits
          hits.Sort((x, y) =>
          {
               // Generally, use click history
               int xScore;
               int yScore;

               history.clicks.TryGetValue(x, out xScore);
               history.clicks.TryGetValue(y, out yScore);

               // Value files that actually begin with the search input higher
               if (xScore != 0 && yScore != 0)
               {
                    string xName = Path.GetFileName(AssetDatabase.GUIDToAssetPath(x)).ToLower();
                    string yName = Path.GetFileName(AssetDatabase.GUIDToAssetPath(y)).ToLower();

                    if (xName.StartsWith(input) && !yName.StartsWith(input))
                         return -1;
                    if (!xName.StartsWith(input) && yName.StartsWith(input))
                         return 1;
               }

               return yScore - xScore;
          });

          hits = hits.Take(10).ToList();
     }

     private void VisualizeHits()
     {
          Event current = Event.current;

          Rect windowRect = position;
          windowRect.height = BaseHeight;

          GUILayout.BeginVertical();
          GUILayout.Space(5);

          if (hits.Count == 0)
          {
               windowRect.height += EditorGUIUtility.singleLineHeight;
               GUILayout.Label("No hits");
          }

          for (int i = 0; i < hits.Count; i++)
          {
               GUIStyle style = i % 2 == 0 ? Styles.entryOdd : Styles.entryEven;

               GUILayout.BeginHorizontal(GUILayout.Height(EditorGUIUtility.singleLineHeight * 2),
                   GUILayout.ExpandWidth(true));

               Rect elementRect = GUILayoutUtility.GetRect(0, 0, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

               GUILayout.EndHorizontal();

               windowRect.height += EditorGUIUtility.singleLineHeight * 2;

               if (current.type == EventType.Repaint)
               {
                    style.Draw(elementRect, false, false, i == selectedIndex, false);
                    string assetPath = AssetDatabase.GUIDToAssetPath(hits[i]);
                    Texture icon = AssetDatabase.GetCachedIcon(assetPath);

                    Rect iconRect = elementRect;
                    iconRect.x = 30;
                    iconRect.width = 25;
                    GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);

                    string assetName = Path.GetFileName(assetPath);
                    StringBuilder coloredAssetName = new StringBuilder();

                    int start = assetName.ToLower().IndexOf(input);
                    int end = start + input.Length;

                    string highlightColor = EditorGUIUtility.isProSkin
                        ? Styles.proSkinHighlightColor
                        : Styles.personalSkinHighlightColor;

                    string normalColor = EditorGUIUtility.isProSkin
                        ? Styles.proSkinNormalColor
                        : Styles.personalSkinNormalColor;

                    // Sometimes the AssetDatabase finds assets without the search input in it.
                    if (start == -1)
                    {
                         coloredAssetName.Append(string.Format("<color=#{0}>{1}</color>", normalColor, assetName));
                    }
                    else
                    {
                         if (0 != start)
                              coloredAssetName.Append(string.Format("<color=#{0}>{1}</color>", normalColor, assetName.Substring(0, start)));

                         coloredAssetName.Append(string.Format("<color=#{0}><b>{1}</b></color>", highlightColor, assetName.Substring(start, end - start)));

                         if (end != assetName.Length - end)
                              coloredAssetName.Append(string.Format("<color=#{0}>{1}</color>", normalColor, assetName.Substring(end, assetName.Length - end)));
                    }

                    Rect labelRect = elementRect;
                    labelRect.x = 60;
                    GUI.Label(labelRect, coloredAssetName.ToString(), Styles.resultLabelStyle);
               }

               if (current.type == EventType.MouseDown && elementRect.Contains(current.mousePosition))
               {
                    selectedIndex = i;

                    if (current.clickCount == 2)
                    {
                         OpenSelectedAssetAndClose();
                    }
                    else
                    {
                         FocusSelection();
                    }

                    Repaint();
               }
          }

          windowRect.height += 5;
          position = windowRect;

          GUILayout.EndVertical();
     }

     private void OpenSelectedAssetAndClose()
     {
          close = true;

        if (AssetDatabase.OpenAsset(GetSelectedAsset()))
          {
               var guid = hits[selectedIndex];
               if (!history.clicks.ContainsKey(guid))
                    history.clicks[guid] = 0;

               history.clicks[guid]++;
               EditorPrefs.SetString(SearchHistoryKey, JsonUtility.ToJson(history));
          }
     }

     private UnityEngine.Object GetSelectedAsset()
     {
          string assetPath = null;

          if (selectedIndex >= 0 && selectedIndex < hits.Count)
               assetPath = AssetDatabase.GUIDToAssetPath(hits[selectedIndex]);

          return (AssetDatabase.LoadMainAssetAtPath(assetPath));
     }

     private void EnforceWindowSize()
     {
          var newPosition = position;
          newPosition.width = 500;
          newPosition.height = BaseHeight;
          position = newPosition;
     }

     public void AddItemsToMenu(GenericMenu menu)
     {
          menu.AddItem(new GUIContent("Reset history"), false, () =>
          {
               EditorPrefs.SetString(SearchHistoryKey, JsonUtility.ToJson(new SearchHistory()));
               Reset();
          });

          menu.AddItem(new GUIContent("Output history"), false, () =>
          {
               var json = EditorPrefs.GetString(SearchHistoryKey, JsonUtility.ToJson(new SearchHistory()));
               Debug.Log(json);
          });
     }

     private void FocusSelection()
     {
          UnityEngine.Object selectedAsset = GetSelectedAsset();

          if (selectedAsset != null)
          {
               EditorUtility.FocusProjectWindow();
               Selection.activeObject = GetSelectedAsset();
               EditorGUIUtility.PingObject(Selection.activeGameObject);
          }
     }

     /*************************************************************************************************
     *** Classes
     *************************************************************************************************/
     private static class Styles
     {
          public static readonly GUIStyle inputFieldStyle;
          public static readonly GUIStyle placeholderStyle;
          public static readonly GUIStyle resultLabelStyle;
          public static readonly GUIStyle entryEven;
          public static readonly GUIStyle entryOdd;

          public static readonly string proSkinHighlightColor = "eeeeee";
          public static readonly string proSkinNormalColor = "cccccc";

          public static readonly string personalSkinHighlightColor = "eeeeee";
          public static readonly string personalSkinNormalColor = "222222";

          static Styles()
          {
               inputFieldStyle = new GUIStyle(EditorStyles.textField)
               {
                    contentOffset = new Vector2(10, 10),
                    fontSize = 32,
                    focused = new GUIStyleState()
               };

               placeholderStyle = new GUIStyle(inputFieldStyle)
               {
                    normal = { textColor = EditorGUIUtility.isProSkin ? new Color(1, 1, 1, .2f) : new Color(.2f, .2f, .2f, .4f) }
               };

               resultLabelStyle = new GUIStyle(EditorStyles.largeLabel)
               {
                    alignment = TextAnchor.MiddleLeft,
                    richText = true
               };

               entryOdd = new GUIStyle("CN EntryBackOdd");
               entryEven = new GUIStyle("CN EntryBackEven");
          }
     }

     [Serializable]
     private class SearchHistory : ISerializationCallbackReceiver
     {
          public readonly Dictionary<string, int> clicks = new Dictionary<string, int>();

          [SerializeField] private List<string> clickKeys = new List<string>();
          [SerializeField] private List<int> clickValues = new List<int>();

          public void OnBeforeSerialize()
          {
               clickKeys.Clear();
               clickValues.Clear();

               foreach (var pair in clicks)
               {
                    clickKeys.Add(pair.Key);
                    clickValues.Add(pair.Value);
               }
          }

          public void OnAfterDeserialize()
          {
               clicks.Clear();
               for (int i = 0; i < clickKeys.Count; i++)
                    clicks.Add(clickKeys[i], clickValues[i]);
          }
     }
}
