using ArabicSupport;
using UnityEditor;
using UnityEngine;

public class ArabicSupportTool : EditorWindow
{
    string rawText;
    string fixedText;

    bool showTashkeel = true;
    bool useHinduNumbers = true;

    // Add menu item named "Arabic Support Tool" to the Tools menu
    [MenuItem("Tools/Arabic Support Tool")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(ArabicSupportTool));
    }

    void OnGUI()
    {
        if (string.IsNullOrEmpty(rawText))
        {
            fixedText = "";
        }
        else
        {
            fixedText = ArabicFixer.Fix(rawText, showTashkeel, useHinduNumbers);
        }

        GUILayout.Label("Options:", EditorStyles.boldLabel);
        showTashkeel = EditorGUILayout.Toggle("Use Tashkeel", showTashkeel);
        useHinduNumbers = EditorGUILayout.Toggle("Use Hindu Numbers", useHinduNumbers);

        GUILayout.Label("Input (Not Fixed)", EditorStyles.boldLabel);
        rawText = EditorGUILayout.TextArea(rawText);

        GUILayout.Label("Output (Fixed)", EditorStyles.boldLabel);
        fixedText = EditorGUILayout.TextArea(fixedText);
        if (GUILayout.Button("Copy")) {
          var tempTextEditor = new TextEditor();
          tempTextEditor.text = fixedText;
          tempTextEditor.SelectAll();
          tempTextEditor.Copy();
        }

    }

}