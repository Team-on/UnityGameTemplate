using UnityEngine;

namespace PumpEditor
{
    public static class PumpEditorStyles
    {
        private static readonly GUIStyle buttonTextMiddleLeft;

        static PumpEditorStyles()
        {
            buttonTextMiddleLeft = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleLeft
            };
        }

        public static GUIStyle ButtonTextMiddleLeft { get { return buttonTextMiddleLeft;  } }
    }
}
