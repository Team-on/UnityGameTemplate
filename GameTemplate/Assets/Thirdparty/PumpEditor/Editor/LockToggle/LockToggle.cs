using System;
using System.Reflection;
using UnityEditor;
#if UNITY_2019_1_OR_NEWER
using UnityEditor.ShortcutManagement;
#endif
using UnityEngine;

namespace PumpEditor
{
    // Inspired by https://forum.unity.com/threads/shortcut-key-for-lock-inspector.95815/#post-5013983
    public static class LockToggle
    {
#if UNITY_2019_1_OR_NEWER
        [Shortcut("Pump Editor/Toggle Lock Focused Window", KeyCode.W, ShortcutModifiers.Action)]
#else
        [MenuItem("Window/Pump Editor/Tools/Toggle Lock Focused Window %w")]
#endif
        private static void ToggleLockFocusedWindow()
        {
            ToggleLockEditorWindow(EditorWindow.focusedWindow);
        }

#if UNITY_2019_1_OR_NEWER
        [Shortcut("Pump Editor/Toggle Lock Mouse Over Window", KeyCode.E, ShortcutModifiers.Action)]
#else
        [MenuItem("Window/Pump Editor/Tools/Toggle Lock Mouse Over Window %e")]
#endif
        private static void ToggleLockMouseOverWindow()
        {
            ToggleLockEditorWindow(EditorWindow.mouseOverWindow);
        }

#if UNITY_2019_1_OR_NEWER
        [Shortcut("Pump Editor/Toggle Lock All Windows", KeyCode.W, ShortcutModifiers.Action | ShortcutModifiers.Shift)]
#else
        [MenuItem("Window/Pump Editor/Tools/Toggle Lock All Windows %#w")]
#endif
        private static void ToggleLockAllWindows()
        {
            var allWindows = Resources.FindObjectsOfTypeAll<EditorWindow>();
            foreach (var editorWindow in allWindows)
            { 
                ToggleLockEditorWindow(editorWindow);
            }
        }

        private static void ToggleLockEditorWindow(EditorWindow editorWindow)
        {
            Assembly editorAssembly = Assembly.GetAssembly(typeof(Editor));
            Type projectBrowserType = editorAssembly.GetType("UnityEditor.ProjectBrowser");
            Type inspectorWindowType = editorAssembly.GetType("UnityEditor.InspectorWindow");
            Type sceneHierarchyWindowType = editorAssembly.GetType("UnityEditor.SceneHierarchyWindow");

            Type editorWindowType = editorWindow.GetType();
            if (editorWindowType == projectBrowserType)
            {
                // Unity C# reference: https://github.com/Unity-Technologies/UnityCsReference/blob/c6ec7823//Editor/Mono/ProjectBrowser.cs#L113
                PropertyInfo propertyInfo = projectBrowserType.GetProperty("isLocked", BindingFlags.Instance | BindingFlags.NonPublic);

                bool value = (bool)propertyInfo.GetValue(editorWindow);
                propertyInfo.SetValue(editorWindow, !value);
            }
            else if (editorWindowType == inspectorWindowType)
            {
                // Unity C# reference: https://github.com/Unity-Technologies/UnityCsReference/blob/c6ec7823//Editor/Mono/Inspector/InspectorWindow.cs##L492
                PropertyInfo propertyInfo = inspectorWindowType.GetProperty("isLocked");

                bool value = (bool)propertyInfo.GetValue(editorWindow);
                propertyInfo.SetValue(editorWindow, !value);
            }
            else if (editorWindowType == sceneHierarchyWindowType)
            {
                // Unity C# reference: https://github.com/Unity-Technologies/UnityCsReference/blob/c6ec7823/Editor/Mono/SceneHierarchyWindow.cs#L34
                PropertyInfo sceneHierarchyPropertyInfo = sceneHierarchyWindowType.GetProperty("sceneHierarchy");
                var sceneHierarchy = sceneHierarchyPropertyInfo.GetValue(editorWindow);

                // Unity C# reference: https://github.com/Unity-Technologies/UnityCsReference/blob/c6ec7823/Editor/Mono/SceneHierarchy.cs#L88
                Type sceneHierarchyType = editorAssembly.GetType("UnityEditor.SceneHierarchy");
                PropertyInfo propertyInfo = sceneHierarchyType.GetProperty("isLocked", BindingFlags.Instance | BindingFlags.NonPublic);

                bool value = (bool)propertyInfo.GetValue(sceneHierarchy);
                propertyInfo.SetValue(sceneHierarchy, !value);
            }
            else
            {
                return;
            }

            editorWindow.Repaint();
        }
    }
}
