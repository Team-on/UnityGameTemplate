using UnityEngine;

namespace Hierarchy2
{
    [System.Serializable]
    public class CustomRowItem
    {
        public GameObject gameObject;

        public bool useBackground = false;
        public Color backgroundColor = new Color32(255, 255, 255, 41);
        public enum BackgroundStyle { Solid = 0, Ramp = 1 }
        public BackgroundStyle backgroundStyle = BackgroundStyle.Solid;
        public enum BackgroundMode { Full = 0, Name = 1 }
        public BackgroundMode backgroundMode = BackgroundMode.Full;

        public bool overrideLabel = false;
        public Vector2 labelOffset = Vector2.zero;
        public Color labelColor = Color.white;
        
        public CustomRowItem() { }

        public CustomRowItem(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }
    }
}
