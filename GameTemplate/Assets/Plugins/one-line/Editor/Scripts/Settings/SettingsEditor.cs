using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using RectEx;
using System;

namespace OneLine.Settings {
    [CustomEditor(typeof(Settings))]
    public class SettingsEditor : Editor {

        private const String SETTINGS_README = 
@"<b>OneLine Settings</b>

Here you can manage OneLine features (every column represents a separate feature, read the tooltips over buttons). 

There are features of two types: <b>drawing feature</b> and <b>optimization feature</b>.

All of them are Enabled by default. You can disable them globally (store in ScriptableObject and share with your team) or locally (store on your PC only).
If value <b>is not null</b>, it <b>overrides</b> column above.

Resulting values are shown in the bottom row (RESULTS row).
Do not forget to click <b>Save</b>.

By clicking <b>Remove</b> you reset your global <b>and</b> local settings to default, remove this ScriptableObject and add #define ONE_LINE_DEFAULTS_ONLY=true to the solution. It is shared with your team.
If you just remove this ScriptableObject, your local settings stay changed.";

        private new Settings target { get { return (Settings) base.target;} }

        public override void OnInspectorGUI() {
#if ONE_LINE_DEFAULTS_ONLY
            PrintErrorUnusedSettingsFile();
#else
            if (SettingsMenu.LoadSettingsFromResources() != target) {
                PrintErrorUnusedSettingsFile();
                EditorGUI.BeginDisabledGroup(true);
            }

            DrawReadme();

            var height = EditorGUIUtility.singleLineHeight;
            var rect = EditorGUILayout.GetControlRect(false, height);

            //In order to beat EventType.Layout
            rect.height = 16;
            var startRect = rect;

            DrawHeader(rect);
            DrawReadOnlyLayer(rect = rect.MoveDown(), "Defaults", target.Defaults);
            DrawLayer(rect = rect.MoveDown(), "Global override", target.Layer);
            DrawLayer(rect = rect.MoveDown(), "Local override", target.Local);
            DrawReadOnlyLayer(rect = rect.MoveDown(20), "Results", target);

            DrawSaveButton(rect = rect.MoveDown(20));
            DrawRemoveButton(rect = rect.MoveDown(20));

            //In order to beat EventType.Layout
            EditorGUILayout.GetControlRect(false, rect.yMax - startRect.yMin);
#endif
        }

        private void PrintErrorUnusedSettingsFile(){
            EditorGUILayout.HelpBox("This settings file is not actually used by OneLine.\nDelete it, please.", MessageType.Error);
        }

        private void DrawReadme() {
            var style = new GUIStyle(GUI.skin.box);
            style.padding = new RectOffset(10, 10, 10, 10);
            style.richText = true;
            style.alignment = TextAnchor.MiddleLeft;

            GUILayout.Box(SETTINGS_README, style);
        }

        private void DrawHeader(Rect rect) {
            var rects = Row(rect);

            EditorGUI.LabelField(rects[1], "Enabled");
            EditorGUI.LabelField(rects[2], "V Separator");
            EditorGUI.LabelField(rects[3], "H Separator");
            EditorGUI.LabelField(rects[4], "Expandable");
            EditorGUI.LabelField(rects[5], "Custom Drawer");
            EditorGUI.LabelField(rects[6], "Culling");
            EditorGUI.LabelField(rects[7], "Cache");
        }

        private void DrawReadOnlyLayer(Rect rect, string label, ISettings layer) {
            EditorGUI.BeginDisabledGroup(true);
            DrawLayer(rect, label, layer);
            EditorGUI.EndDisabledGroup();
        }

        private void DrawLayer(Rect rect, string label, ISettings layer) {
            var rects = Row(rect);

            EditorGUI.LabelField(rects[0], label);
            Draw(rects[1], layer.Enabled, "Enable OneLine");
            Draw(rects[2], layer.DrawVerticalSeparator, "Draw Vertical Separator");
            Draw(rects[3], layer.DrawHorizontalSeparator, "Draw Horizontal Separator");
            Draw(rects[4], layer.Expandable, "Expand Object references via [Expandable]");
            Draw(rects[5], layer.CustomDrawer, "Draw custom property drawers");
            Draw(rects[6], layer.CullingOptimization, "Use culling optimization");
            Draw(rects[7], layer.CacheOptimization, "Use cache optimization");
        }

        private void Draw(Rect rect, TernaryBoolean value, string tooltip) {
            var content = new GUIContent(value.ToString(), tooltip); 
            if (GUI.Button(rect, content)){
                value.SwitchToNext();
            }
        }

        private Rect[] Row(Rect rect) {
            return rect.Row(
                new float[]{0,   0,  0,  0,  0,  0, 0, 0}, 
                new float[]{100, 50, 50, 50, 50, 50, 50, 50}
            );
        }

        private void DrawSaveButton(Rect rect) {
            if (GUI.Button(rect.CutFromLeft(50)[0], "Save")){
                target.SaveAndApply();
            }
        }

        private void DrawRemoveButton(Rect rect) {
            var rects = rect.CutFromRight(75);

            EditorGUI.LabelField(rects[0], "Remove Settings File And Always Use Default Parameters");
            if (GUI.Button(rects[1], "Remove")){
                SettingsMenu.RemoveSettingsForever(target);
            }
        }

    }
}