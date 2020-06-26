#if !UNITY_2018_3_OR_NEWER

using System;
using System.Reflection;
using UnityEditor;

namespace PumpEditor
{
    public static class ProjectSettingsTypeHelper
    {
        private static Type inputManagerType;
        private static Type tagManagerType;
        private static Type audioManagerType;
        private static Type timeManagerType;
        private static Type playerSettingsType;
        private static Type physicsManagerType;
        private static Type physics2DSettingsType;
        private static Type qualitySettingsType;
        private static Type graphicsSettingsType;
        private static Type networkManagerType;
        private static Type editorSettingsType;
        private static Type monoManagerType;
        private static Type presetManagerType;

        static ProjectSettingsTypeHelper()
        {
            var editorAssembly = Assembly.GetAssembly(typeof(EditorWindow));
            var engineAssembly = Assembly.GetAssembly(typeof(UnityEngine.Object));

            inputManagerType = editorAssembly.GetType("UnityEditor.InputManager");
            tagManagerType = editorAssembly.GetType("UnityEditor.TagManager");
            audioManagerType = editorAssembly.GetType("UnityEditor.AudioManager");
            timeManagerType = editorAssembly.GetType("UnityEditor.TimeManager");
            playerSettingsType = editorAssembly.GetType("UnityEditor.PlayerSettings");
            physicsManagerType = editorAssembly.GetType("UnityEditor.PhysicsManager");
            physics2DSettingsType = editorAssembly.GetType("UnityEditor.Physics2DSettings");
            qualitySettingsType = engineAssembly.GetType("UnityEngine.QualitySettings");
            graphicsSettingsType = engineAssembly.GetType("UnityEngine.Rendering.GraphicsSettings");
            // Failed to find full type name for object used in
            // network manager project settings. Though debugger
            // shows UnityEngine.NetworkManager type value, getting
            // it from engine assembly returns null. Using plain
            // UnityEngine.Object is dangerous for other objects
            // like network manager settings one as IsProjectSettingsType
            // will return true for such objects.
            networkManagerType = typeof(UnityEngine.Object);
            editorSettingsType = editorAssembly.GetType("UnityEditor.EditorSettings");
            monoManagerType = editorAssembly.GetType("UnityEditor.MonoManager");
            presetManagerType = editorAssembly.GetType("UnityEditor.Presets.PresetManager");
        }

        public static bool IsProjectSettingsType(UnityEngine.Object obj)
        {
            var objType = obj.GetType();
            return objType == inputManagerType
                || objType == tagManagerType
                || objType == audioManagerType
                || objType == timeManagerType
                || objType == playerSettingsType
                || objType == physicsManagerType
                || objType == physics2DSettingsType
                || objType == qualitySettingsType
                || objType == graphicsSettingsType
                || objType == networkManagerType
                || objType == editorSettingsType
                || objType == monoManagerType
                || objType == presetManagerType;
        }

        // Ideal solution would be getting title via
        // ProjectSettingsBaseEditor's targetTitle property.
        public static string GetProjectSettingsTargetTitle(UnityEngine.Object obj)
        {
            var objType = obj.GetType();
            if (objType == inputManagerType)
            {
                return "InputManager";
            }
            else if (objType == tagManagerType)
            {
                return "Tags & Layers";
            }
            else if (objType == audioManagerType)
            {
                return "AudioManager";
            }
            else if (objType == timeManagerType)
            {
                return "TimeManager";
            }
            else if (objType == playerSettingsType)
            {
                return "PlayerSettings";
            }
            else if (objType == physicsManagerType)
            {
                return "PhysicsManager";
            }
            else if (objType == physics2DSettingsType)
            {
                return "Physics2DSettings";
            }
            else if (objType == qualitySettingsType)
            {
                return "QualitySettings";
            }
            else if (objType == graphicsSettingsType)
            {
                return "GraphicsSettings";
            }
            else if (objType == networkManagerType)
            {
                return "NetworkManager";
            }
            else if (objType == editorSettingsType)
            {
                return "Editor Settings";
            }
            else if (objType == monoManagerType)
            {
                return "Script Execution Order";
            }
            else if (objType == presetManagerType)
            {
                return "PresetManager";
            }
            return null;
        }
    }
}

#endif
