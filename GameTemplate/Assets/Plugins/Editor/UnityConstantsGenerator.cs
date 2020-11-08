using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace UnityToolbag {
    public static class UnityConstantsGenerator {
        [MenuItem("Tools/Generate UnityConstants.cs")]
        public static void Generate() {
            // Try to find an existing file in the project called "UnityConstants.cs"
            string filePath = string.Empty;
            foreach (var file in Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories)) {
                if (Path.GetFileNameWithoutExtension(file) == "UnityConstants") {
                    filePath = file;
                    break;
                }
            }

            // If no such file exists already, use the save panel to get a folder in which the file will be placed.
            if (string.IsNullOrEmpty(filePath)) {
                string directory = EditorUtility.OpenFolderPanel("Choose location for UnityConstants.cs", Application.dataPath, "");

                // Canceled choose? Do nothing.
                if (string.IsNullOrEmpty(directory)) {
                    return;
                }

                filePath = Path.Combine(directory, "UnityConstants.cs");
            }

            // Write out our file
            using (var writer = new StreamWriter(filePath)) {
                writer.WriteLine("// This file is auto-generated. Modifications are not saved.");
                writer.WriteLine();
                writer.WriteLine("namespace UnityConstants");
                writer.WriteLine("{");

                // Write out the tags
                writer.WriteLine("    public static class Tags");
                writer.WriteLine("    {");
                foreach (var tag in UnityEditorInternal.InternalEditorUtility.tags) {
                    writer.WriteLine("        /// <summary>");
                    writer.WriteLine("        /// Name of tag '{0}'.", tag);
                    writer.WriteLine("        /// </summary>");
                    writer.WriteLine("        public const string {0} = \"{1}\";", MakeSafeForCode(tag), tag);
                }
                writer.WriteLine("    }");
                writer.WriteLine();

                // Write out sorting layers
                writer.WriteLine("    public static class SortingLayers");
                writer.WriteLine("    {");
                foreach (var layer in SortingLayer.layers) {
                    writer.WriteLine("        /// <summary>");
                    writer.WriteLine("        /// ID of sorting layer '{0}'.", layer.name);
                    writer.WriteLine("        /// </summary>");
                    writer.WriteLine("        public const int {0} = {1};", MakeSafeForCode(layer.name), layer.id);
                }
                writer.WriteLine("    }");
                writer.WriteLine();

                // Write out layers
                writer.WriteLine("    public static class Layers");
                writer.WriteLine("    {");
                for (int i = 0; i < 32; i++) {
                    string layer = UnityEditorInternal.InternalEditorUtility.GetLayerName(i);
                    if (!string.IsNullOrEmpty(layer)) {
                        writer.WriteLine("        /// <summary>");
                        writer.WriteLine("        /// Index of layer '{0}'.", layer);
                        writer.WriteLine("        /// </summary>");
                        writer.WriteLine("        public const int {0} = {1};", MakeSafeForCode(layer), i);
                    }
                }
                writer.WriteLine();
                for (int i = 0; i < 32; i++) {
                    string layer = UnityEditorInternal.InternalEditorUtility.GetLayerName(i);
                    if (!string.IsNullOrEmpty(layer)) {
                        writer.WriteLine("        /// <summary>");
                        writer.WriteLine("        /// Bitmask of layer '{0}'.", layer);
                        writer.WriteLine("        /// </summary>");
                        writer.WriteLine("        public const int {0}Mask = 1 << {1};", MakeSafeForCode(layer), i);
                    }
                }
                writer.WriteLine("    }");
                writer.WriteLine();

                // Write out scenes
                writer.WriteLine("    public static class Scenes");
                writer.WriteLine("    {");
                int sceneIndex = 0;
                foreach (var scene in EditorBuildSettings.scenes) {
                    if (!scene.enabled) {
                        continue;
                    }

                    var sceneName = Path.GetFileNameWithoutExtension(scene.path);

                    writer.WriteLine("        /// <summary>");
                    writer.WriteLine("        /// ID of scene '{0}'.", sceneName);
                    writer.WriteLine("        /// </summary>");
                    writer.WriteLine("        public const int {0} = {1};", MakeSafeForCode(sceneName), sceneIndex);

                    sceneIndex++;
                }
                writer.WriteLine("    }");
                writer.WriteLine();

                // Write out Input axes
                writer.WriteLine("    public static class Axes");
                writer.WriteLine("    {");
                var axes = new HashSet<string>();
                var inputManagerProp = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
                foreach (SerializedProperty axe in inputManagerProp.FindProperty("m_Axes")) {
                    var name = axe.FindPropertyRelative("m_Name").stringValue;
                    var variableName = MakeSafeForCode(name);
                    if (!axes.Contains(variableName)) {
                        writer.WriteLine("        /// <summary>");
                        writer.WriteLine("        /// Input axis '{0}'.", name);
                        writer.WriteLine("        /// </summary>");
                        writer.WriteLine("        public const string {0} = \"{1}\";", variableName, name);
                        axes.Add(variableName);
                    }
                }
                writer.WriteLine("    }");

                // End of namespace UnityConstants
                writer.WriteLine("}");
                writer.WriteLine();
            }

            // Refresh
            AssetDatabase.Refresh();
        }

        private static string MakeSafeForCode(string str) {
            str = Regex.Replace(str, "[^a-zA-Z0-9_]", "_", RegexOptions.Compiled);
            if (char.IsDigit(str[0])) {
                str = "_" + str;
            }
            return str;
        }
    }
}