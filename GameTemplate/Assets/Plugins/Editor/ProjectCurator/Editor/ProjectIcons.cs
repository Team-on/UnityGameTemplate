using UnityEditor;
using UnityEngine;

namespace Nanolabo
{
    public static class ProjectIcons
    {
        private const string PLUGIN_LOCATION = "Assets/Plugins/ProjectCurator/";

        private static Texture2D linkBlack;
        public static Texture2D LinkBlack => linkBlack ?? (linkBlack = (Texture2D)AssetDatabase.LoadAssetAtPath(PLUGIN_LOCATION + "Images/link_black.png", typeof(Texture2D)));

        private static Texture2D linkWhite;
        public static Texture2D LinkWhite => linkWhite ?? (linkWhite = (Texture2D)AssetDatabase.LoadAssetAtPath(PLUGIN_LOCATION + "Images/link_white.png", typeof(Texture2D)));

        private static Texture2D linkBlue;
        public static Texture2D LinkBlue => linkBlue ?? (linkBlue = (Texture2D)AssetDatabase.LoadAssetAtPath(PLUGIN_LOCATION + "Images/link_blue.png", typeof(Texture2D)));
    }
}