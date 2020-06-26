#if UNITY_2018_1_OR_NEWER

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace PumpEditor.PresetsBrowser
{
    public class PresetsBrowserWindow : EditorWindow
    {
        private static readonly string[] VALIDITY_TOOLBAR_STRINGS = new string[3]
        {
            "All",
            "Only Valid",
            "Only Invalid",
        };

        private const int ALL_VALIDITY_TOOLBAR_INDEX = 0;
        private const int ONLY_VALID_VALIDITY_TOOLBAR_INDEX = 1;
        private const int ONLY_INVALID_VALIDITY_TOOLBAR_INDEX = 2;

        private List<Preset> presets = new List<Preset>();
        private HashSet<string> presetTargetFullTypeNames = new HashSet<string>();
        private List<Preset> presetsToDraw = new List<Preset>();
        private int validityToolbarIndex;
        private bool filterByPresetType;
        private Vector2 scrollPosition;
        private string filterPresetTargetFullTypeName;

        [MenuItem("Window/Pump Editor/Presets Browser")]
        private static void Init()
        {
            var window = GetWindow<PresetsBrowserWindow>();
            var icon = EditorGUIUtility.FindTexture("preset.context");
            window.titleContent = new GUIContent("Presets", icon);
            window.Show();
        }

        // If preset is not valid, GetTargetFullTypeName returns empty string
        private static bool TryGetValidPresetTargetFullTypeName(Preset preset, out string targetFullTypeName)
        {
            targetFullTypeName = preset.GetTargetFullTypeName();
            return targetFullTypeName != String.Empty;
        }

        private static void DrawPresetItem(Preset preset)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUILayout.ObjectField(preset, null, false);
                }

                if (GUILayout.Button("Select", GUILayout.Width(100)))
                {
                    Selection.activeObject = preset;
                }
            }
        }

        private void OnGUI()
        {
            presets.Clear();
            presetTargetFullTypeNames.Clear();
            presetsToDraw.Clear();

            var presetGuids = AssetDatabase.FindAssets("t:preset");
            foreach (var presetGuid in presetGuids)
            {
                var presetPath = AssetDatabase.GUIDToAssetPath(presetGuid);
                var preset = AssetDatabase.LoadAssetAtPath<Preset>(presetPath);
                presets.Add(preset);

                string targetFullTypeName;
                if (TryGetValidPresetTargetFullTypeName(preset, out targetFullTypeName))
                {
                    presetTargetFullTypeNames.Add(targetFullTypeName);
                }
            }

            validityToolbarIndex = GUILayout.Toolbar(validityToolbarIndex, VALIDITY_TOOLBAR_STRINGS);

            // Do not show filter by preset type for invalid presets since for them
            // coressponding class is not present:
            // https://forum.unity.com/threads/presets-feature.491263/#post-3210492
            if (validityToolbarIndex != ONLY_INVALID_VALIDITY_TOOLBAR_INDEX)
            {
                filterByPresetType = EditorGUILayout.Toggle("Filter by preset type", filterByPresetType);
                if (filterByPresetType)
                {
                    DrawFilterByType();
                }
                else
                {
                    presetsToDraw.AddRange(presets);
                }
            }
            else
            {
                presetsToDraw.AddRange(presets);
            }

            FilterByValidity();
            DrawPresets();
        }

        private void DrawFilterByType()
        {
            var typeNames = presetTargetFullTypeNames.ToArray();
            Array.Sort(typeNames);
            var index = Array.IndexOf(typeNames, filterPresetTargetFullTypeName);
            index = EditorGUILayout.Popup("Preset type", index, typeNames);
            if (index >= 0 && index < typeNames.Length)
            {
                filterPresetTargetFullTypeName = typeNames[index];

                foreach (var preset in presets)
                {
                    string targetFullTypeName;
                    if (TryGetValidPresetTargetFullTypeName(preset, out targetFullTypeName))
                    {
                        if (filterPresetTargetFullTypeName == targetFullTypeName)
                        {
                            presetsToDraw.Add(preset);
                        }
                    }
                }
            }
            else
            {
                filterPresetTargetFullTypeName = null;
            }
        }

        private void FilterByValidity()
        {
            switch (validityToolbarIndex)
            {
                case ALL_VALIDITY_TOOLBAR_INDEX:
                    break;
                case ONLY_VALID_VALIDITY_TOOLBAR_INDEX:
                    presetsToDraw.RemoveAll(p => !p.IsValid());
                    break;
                case ONLY_INVALID_VALIDITY_TOOLBAR_INDEX:
                    presetsToDraw.RemoveAll(p => p.IsValid());
                    break;
            }
        }

        private void DrawPresets()
        {
            if (presetsToDraw.Count != 0)
            {
                using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPosition))
                {
                    scrollPosition = scrollView.scrollPosition;

                    foreach (var preset in presetsToDraw)
                    {
                        DrawPresetItem(preset);
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField("No presets.");
            }
        }
    }
}

#endif
