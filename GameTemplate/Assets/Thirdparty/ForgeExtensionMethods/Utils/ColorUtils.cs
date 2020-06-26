using UnityEngine;

namespace UnityForge
{
    public static class ColorUtils
    {
        public static Color SetR(Color color, float value)
        {
            return new Color(value, color.g, color.b, color.a);
        }

        public static Color SetG(Color color, float value)
        {
            return new Color(color.r, value, color.b, color.a);
        }

        public static Color SetB(Color color, float value)
        {
            return new Color(color.r, color.g, value, color.a);
        }

        public static Color SetA(Color color, float value)
        {
            return new Color(color.r, color.g, color.b, value);
        }
    }
}
