using UnityEditor;
#if UNITY_2019_3_OR_NEWER
using UnityEditor.Compilation;
#endif
using UnityEngine;

namespace PumpEditor
{
    public class CompilationWindow : EditorWindow
    {
        [MenuItem("Window/Pump Editor/Compilation")]
        private static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<CompilationWindow>();
            window.titleContent = new GUIContent("Compilation");
            window.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Request Script Compilation"))
            {
#if UNITY_2019_3_OR_NEWER
                CompilationPipeline.RequestScriptCompilation();
#elif UNITY_2017_1_OR_NEWER
                CompilationUtils.RequestScriptCompilationViaReflection();
#endif
            }
        }
    }
}
