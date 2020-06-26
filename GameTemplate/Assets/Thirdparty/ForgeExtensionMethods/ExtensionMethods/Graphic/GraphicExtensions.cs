using UnityEngine.UI;

namespace UnityForge
{
    public static class GraphicExtensions
    {
        public static void SetColorR(this Graphic graphic, float value)
        {
            graphic.color = ColorUtils.SetR(graphic.color, value);
        }

        public static void SetColorG(this Graphic graphic, float value)
        {
            graphic.color = ColorUtils.SetG(graphic.color, value);
        }

        public static void SetColorB(this Graphic graphic, float value)
        {
            graphic.color = ColorUtils.SetB(graphic.color, value);
        }

        public static void SetColorA(this Graphic graphic, float value)
        {
            graphic.color = ColorUtils.SetA(graphic.color, value);
        }
    }
}
