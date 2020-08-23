#if UNITY_2017_3_OR_NEWER

using UnityEditor;
using UnityEngine;

namespace PumpEditor
{
    public class ForceReserializeAssetsEditorWindow : EditorWindow
    {
        [MenuItem("Window/Pump Editor/Force Reserialize Assets")]
        private static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<ForceReserializeAssetsEditorWindow>();
            var icon = EditorGUIUtility.Load("scriptableobject icon") as Texture2D;
            window.titleContent = new GUIContent("Force Reserialize Assets", icon);
            window.Show();
        }

        private void OnGUI()
        {
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                if (GUILayout.Button("Force Reserialize All Assets"))
                {
                    ForceReserializeAssetsUtils.ForceReserializeAllAssets();
                }

                if (GUILayout.Button("Force Reserialize Selected Assets"))
                {
                    ForceReserializeAssetsUtils.ForceReserializeSelectedAssets();
                }
            }
        }
    }
}

#endif
