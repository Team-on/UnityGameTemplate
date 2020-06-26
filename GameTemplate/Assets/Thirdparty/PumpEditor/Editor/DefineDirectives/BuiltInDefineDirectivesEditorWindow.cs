using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace PumpEditor
{
    public class BuiltInDefineDirectivesEditorWindow : EditorWindow
    {
        // Platform define directives from https://docs.unity3d.com/2019.3/Documentation/Manual/PlatformDependentCompilation.html
        private const string UNITY_EDITOR_DEFINE = "UNITY_EDITOR";
        private const string UNITY_EDITOR_WIN_DEFINE = "UNITY_EDITOR_WIN";
        private const string UNITY_EDITOR_OSX_DEFINE = "UNITY_EDITOR_OSX";
        private const string UNITY_EDITOR_LINUX_DEFINE = "UNITY_EDITOR_LINUX";
        private const string UNITY_STANDALONE_OSX_DEFINE = "UNITY_STANDALONE_OSX";
        private const string UNITY_STANDALONE_WIN_DEFINE = "UNITY_STANDALONE_WIN";
        private const string UNITY_STANDALONE_LINUX_DEFINE = "UNITY_STANDALONE_LINUX";
        private const string UNITY_STANDALONE_DEFINE = "UNITY_STANDALONE";
        private const string UNITY_WII_DEFINE = "UNITY_WII";
        private const string UNITY_IOS_DEFINE = "UNITY_IOS";
        private const string UNITY_IPHONE_DEFINE = "UNITY_IPHONE";
        private const string UNITY_ANDROID_DEFINE = "UNITY_ANDROID";
        private const string UNITY_PS4_DEFINE = "UNITY_PS4";
        private const string UNITY_XBOXONE_DEFINE = "UNITY_XBOXONE";
        private const string UNITY_LUMIN_DEFINE = "UNITY_LUMIN";
        private const string UNITY_TIZEN_DEFINE = "UNITY_TIZEN";
        private const string UNITY_TVOS_DEFINE = "UNITY_TVOS";
        private const string UNITY_WSA_DEFINE = "UNITY_WSA";
        private const string UNITY_WSA_10_0_DEFINE = "UNITY_WSA_10_0";
        private const string UNITY_WINRT_DEFINE = "UNITY_WINRT";
        private const string UNITY_WINRT_10_0_DEFINE = "UNITY_WINRT_10_0";
        private const string UNITY_WEBGL_DEFINE = "UNITY_WEBGL";
        private const string UNITY_FACEBOOK_DEFINE = "UNITY_FACEBOOK";
        private const string UNITY_ADS_DEFINE = "UNITY_ADS";
        private const string UNITY_ANALYTICS_DEFINE = "UNITY_ANALYTICS";
        private const string UNITY_ASSERTIONS_DEFINE = "UNITY_ASSERTIONS";
        private const string UNITY_64_DEFINE = "UNITY_64";

        private const string CSHARP_7_3_OR_NEWER_DEFINE = "CSHARP_7_3_OR_NEWER";
        private const string ENABLE_MONO_DEFINE = "ENABLE_MONO";
        private const string ENABLE_IL2CPP_DEFINE = "ENABLE_IL2CPP";
        private const string NET_2_0_DEFINE = "NET_2_0";
        private const string NET_2_0_SUBSET_DEFINE = "NET_2_0_SUBSET";
        private const string NET_LEGACY_DEFINE = "NET_LEGACY";
        private const string NET_4_6_DEFINE = "NET_4_6";
        private const string NET_STANDARD_2_0_DEFINE = "NET_STANDARD_2_0";
        private const string ENABLE_WINMD_SUPPORT_DEFINE = "ENABLE_WINMD_SUPPORT";
        private const string ENABLE_INPUT_SYSTEM_DEFINE = "ENABLE_INPUT_SYSTEM";
        private const string ENABLE_LEGACY_INPUT_MANAGER_DEFINE = "ENABLE_LEGACY_INPUT_MANAGER";

        private static readonly List<DefineInfo> platformDefines = new List<DefineInfo>
        {
            new DefineInfo(UNITY_EDITOR_DEFINE, PlatformDefines.UnityEditorDefined),
            new DefineInfo(UNITY_EDITOR_WIN_DEFINE, PlatformDefines.UnityEditorWinDefined),
            new DefineInfo(UNITY_EDITOR_OSX_DEFINE, PlatformDefines.UnityEditorOsxDefined),
            new DefineInfo(UNITY_EDITOR_LINUX_DEFINE, PlatformDefines.UnityEditorLinuxDefined),
            new DefineInfo(UNITY_STANDALONE_OSX_DEFINE, PlatformDefines.UnityStandaloneOsxDefined),
            new DefineInfo(UNITY_STANDALONE_WIN_DEFINE, PlatformDefines.UnityStandaloneWinDefined),
            new DefineInfo(UNITY_STANDALONE_LINUX_DEFINE, PlatformDefines.UnityStandaloneLinuxDefined),
            new DefineInfo(UNITY_STANDALONE_DEFINE, PlatformDefines.UnityStandaloneDefined),
            new DefineInfo(UNITY_WII_DEFINE, PlatformDefines.UnityWiiDefined),
            new DefineInfo(UNITY_IOS_DEFINE, PlatformDefines.UnityIosDefined),
            new DefineInfo(UNITY_IPHONE_DEFINE, PlatformDefines.UnityIphoneDefined),
            new DefineInfo(UNITY_ANDROID_DEFINE, PlatformDefines.UnityAndroidDefined),
            new DefineInfo(UNITY_PS4_DEFINE, PlatformDefines.UnityPs4Defined),
            new DefineInfo(UNITY_XBOXONE_DEFINE, PlatformDefines.UnityXboxoneDefined),
            new DefineInfo(UNITY_LUMIN_DEFINE, PlatformDefines.UnityLuminDefined),
            new DefineInfo(UNITY_TIZEN_DEFINE, PlatformDefines.UnityTizenDefined),
            new DefineInfo(UNITY_TVOS_DEFINE, PlatformDefines.UnityTvosDefined),
            new DefineInfo(UNITY_WSA_DEFINE, PlatformDefines.UnityWsaDefined),
            new DefineInfo(UNITY_WSA_10_0_DEFINE, PlatformDefines.UnityWsa10Defined),
            new DefineInfo(UNITY_WINRT_DEFINE, PlatformDefines.UnityWinrtDefined),
            new DefineInfo(UNITY_WINRT_10_0_DEFINE, PlatformDefines.UnityWinrt10Defined),
            new DefineInfo(UNITY_WEBGL_DEFINE, PlatformDefines.UnityWebglDefined),
            new DefineInfo(UNITY_FACEBOOK_DEFINE, PlatformDefines.UnityFacebookDefined),
            new DefineInfo(UNITY_ADS_DEFINE, PlatformDefines.UnityAdsDefined),
            new DefineInfo(UNITY_ANALYTICS_DEFINE, PlatformDefines.UnityAnalyticsDefined),
            new DefineInfo(UNITY_ASSERTIONS_DEFINE, PlatformDefines.UnityAssertionsDefined),
            new DefineInfo(UNITY_64_DEFINE, PlatformDefines.Unity64Defined),
        };

        private static readonly List<DefineInfo> codeCompilationDefines = new List<DefineInfo>
        {
            new DefineInfo(CSHARP_7_3_OR_NEWER_DEFINE, CodeCompilationDefines.Csharp73OrNewerDefined),
            new DefineInfo(ENABLE_MONO_DEFINE, CodeCompilationDefines.EnableMonoDefined),
            new DefineInfo(ENABLE_IL2CPP_DEFINE, CodeCompilationDefines.EnableIl2cppDefined),
            new DefineInfo(NET_2_0_DEFINE, CodeCompilationDefines.Net20Defined),
            new DefineInfo(NET_2_0_SUBSET_DEFINE, CodeCompilationDefines.Net20SubsetDefined),
            new DefineInfo(NET_LEGACY_DEFINE, CodeCompilationDefines.NetLegacyDefined),
            new DefineInfo(NET_4_6_DEFINE, CodeCompilationDefines.Net46Defined),
            new DefineInfo(NET_STANDARD_2_0_DEFINE, CodeCompilationDefines.NetStandard20Defined),
            new DefineInfo(ENABLE_WINMD_SUPPORT_DEFINE, CodeCompilationDefines.EnableWinmdSupportDefined),
            new DefineInfo(ENABLE_INPUT_SYSTEM_DEFINE, CodeCompilationDefines.EnableInputSystemDefined),
            new DefineInfo(ENABLE_LEGACY_INPUT_MANAGER_DEFINE, CodeCompilationDefines.EnableLegacyInputManagerDefined),
        };

        private SearchField searchField;
        private string searchFieldInputString;
        private List<DefineInfo> platformDefinesToDraw;
        private List<DefineInfo> codeCompilationDefinesToDraw;
        private Vector2 scrollPos;

        [MenuItem("Window/Pump Editor/Built-in Define Directives")]
        private static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<BuiltInDefineDirectivesEditorWindow>();
            var icon = EditorGUIUtility.Load("d_cs script icon") as Texture2D;
            window.titleContent = new GUIContent("Built-in Define Directives", icon);
            window.Show();
        }

        private static void DrawDefines(string defineTypeTitle, ICollection<DefineInfo> definesToDraw)
        {
            if (definesToDraw.Count != 0)
            {
                DrawTableHeader(defineTypeTitle);
            }

            foreach (var defineToDraw in definesToDraw)
            {
                DrawDefine(defineToDraw.CompilationSymbol, defineToDraw.IsDefined);
            }
        }

        private static void DrawTableHeader(string defineTypeTitle)
        {
            using (new EditorGUILayout.HorizontalScope(Styles.headerStyle))
            {
                EditorGUILayout.LabelField(defineTypeTitle, EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Value", EditorStyles.boldLabel);
            }
        }

        private static void DrawDefine(string compilationSymbol, bool isDefined)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(compilationSymbol);
                EditorGUILayout.LabelField(isDefined ? "Set" : "Not Set");
            }
        }

        private void DrawSearchField()
        {
            var rect = EditorGUILayout.GetControlRect();

            using (var change = new EditorGUI.ChangeCheckScope())
            {
                searchFieldInputString = searchField.OnToolbarGUI(rect, null);
                if (change.changed)
                {
                    UpdateSearchResult();
                }
            }
        }

        private void UpdateSearchResult()
        {
            platformDefinesToDraw = platformDefines.FindAll(IsSearchMatch);
            codeCompilationDefinesToDraw = codeCompilationDefines.FindAll(IsSearchMatch);
        }

        private bool IsSearchMatch(DefineInfo defineInfo)
        {
            return defineInfo.CompilationSymbol.ToLower().Contains(searchFieldInputString.ToLower());
        }


        private void DrawDefines()
        {
            using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPos))
            {
                scrollPos = scrollView.scrollPosition;

                DrawPlatformDefines();
                EditorGUILayout.Space();
                DrawCodeCompilationDefines();
            }
        }

        private void DrawPlatformDefines()
        {
            DrawDefines("Platform Define", platformDefinesToDraw);
        }

        private void DrawCodeCompilationDefines()
        {
            DrawDefines("Code Compilation Define", codeCompilationDefinesToDraw);
        }

        private void OnGUI()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.toolbar))
            {
                DrawSearchField();
            }

            DrawDefines();
        }

        private void OnEnable()
        {
            searchField = new SearchField();
            platformDefinesToDraw = platformDefines;
            codeCompilationDefinesToDraw = codeCompilationDefines;
        }

        private static class Styles
        {
            public static readonly GUIStyle headerStyle;

            static Styles()
            {
                headerStyle = new GUIStyle(GUI.skin.box);
                headerStyle.border = new RectOffset();
                headerStyle.margin = new RectOffset();
                headerStyle.padding = new RectOffset();
            }
        }

        private class DefineInfo
        {
            public DefineInfo(string compilationSymbol, bool isDefined)
            {
                CompilationSymbol = compilationSymbol;
                IsDefined = isDefined;
            }

            public string CompilationSymbol { get; private set; }
            public bool IsDefined { get; private set; }
        }
    }
}
