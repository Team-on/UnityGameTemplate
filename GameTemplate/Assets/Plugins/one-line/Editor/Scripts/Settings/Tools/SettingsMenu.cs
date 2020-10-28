using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace OneLine.Settings {
    [InitializeOnLoad]
    public class SettingsMenu {

        private const string PATH = "Assets/Editor/Resources/OneLine";
        private const string SETTINGS_FILE_NAME = "OneLineSettings";
        private const string SETTINGS_FILE_NAME_WITH_EXTENSION = SETTINGS_FILE_NAME + ".asset";
        private const string SETTINGS_RESOURCES_PATH = "OneLine/" + SETTINGS_FILE_NAME;

        static SettingsMenu() {
            LoadSettings();
        }

#if ONE_LINE_DEFAULTS_ONLY
        [MenuItem(itemName: "Window/OneLine Settings - Create")]
        public static void RestoreSettingsAsset(){
            ApplyDirectiveDefaultsOnly(false);
        }

        public static Settings LoadSettings() {
            var settings = Settings.CreateInstance<Settings>();
            return settings;
        }
#else
        [MenuItem(itemName: "Window/OneLine Settings")]
        public static void OpenSettings(){
            Selection.activeObject = LoadSettings();
        }

        public static Settings LoadSettings() {
            var settings = LoadSettingsFromResources();
            if (settings == null) {
                settings = CreateSettings();
            }
            return settings;
        }
#endif

        public static Settings LoadSettingsFromResources() {
            return Resources.Load<Settings>(SETTINGS_RESOURCES_PATH);
        }

        public static Settings Value { get { return SettingsMenu.LoadSettings(); } }

        private static Settings CreateSettings() {
            PrepareResourcesDirectory();

            var result = Settings.CreateInstance<Settings>();
            AssetDatabase.CreateAsset(result, PATH + "/" + SETTINGS_FILE_NAME_WITH_EXTENSION);
            AssetDatabase.SaveAssets();
            return result;
        }

        private static void PrepareResourcesDirectory(){
            foreach (string directory in GetPathElements(PATH)) {
                if (!AssetDatabase.IsValidFolder(directory)) {
                    AssetDatabase.CreateFolder(Path.GetDirectoryName(directory), Path.GetFileName(directory));
                }
            }
        }

        private static IEnumerable<string> GetPathElements(string path) {
            var result = "";
            foreach (var part in path.Split('/')) {
                if (part.Length > 0) {
                    result = Path.Combine(result, part);
                    yield return result;
                }
            }
        }

        public static void RemoveSettingsForever(Settings settings) {
            var path = AssetDatabase.GetAssetPath(settings);
            AssetDatabase.DeleteAsset(path);
            foreach (string directory in GetPathElements(Directory.GetParent(path).ToString()).Reverse()){
                if (directory == "Assets") continue;

                if (Directory.GetFiles(directory).Length == 0) {
                    FileUtil.DeleteFileOrDirectory(directory);
                    FileUtil.DeleteFileOrDirectory(directory + ".meta");
                }
            }
            AssetDatabase.Refresh();

            ApplyDirectiveDefaultsOnly(true);
            ApplyDirectivesInOrderToCurrentSettings(new DefaultSettingsLayer());

        }

        public static void ApplyDirectivesInOrderToCurrentSettings(ISettings settings){
            var directives = new PreprocessorDirectives();

            directives.add("ONE_LINE_DISABLED", !settings.Enabled);
            directives.add("ONE_LINE_VERTICAL_SEPARATOR_DISABLE", !settings.DrawVerticalSeparator);
            directives.add("ONE_LINE_HORIZONTAL_SEPARATOR_DISABLE", !settings.DrawHorizontalSeparator);
            directives.add("ONE_LINE_EXPANDABLE_DISABLE", !settings.Expandable);
            directives.add("ONE_LINE_CUSTOM_DRAWER_DISABLE", !settings.CustomDrawer);

            directives.DefineForCurrentBuildTarget();
        }

        public static void ApplyDirectiveDefaultsOnly(bool defaultOnly) {
            var directives = new PreprocessorDirectives();
            directives.add("ONE_LINE_DEFAULTS_ONLY", defaultOnly);
            directives.DefineForCurrentBuildTarget();
        }

    }
}