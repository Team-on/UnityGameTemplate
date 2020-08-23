using UnityEngine;

namespace UnityForge
{
    public static class SpriteRendererExtensions
    {
        public static void SetColorR(this SpriteRenderer spriteRenderer, float value)
        {
            spriteRenderer.color = ColorUtils.SetR(spriteRenderer.color, value);
        }

        public static void SetColorG(this SpriteRenderer spriteRenderer, float value)
        {
            spriteRenderer.color = ColorUtils.SetG(spriteRenderer.color, value);
        }

        public static void SetColorB(this SpriteRenderer spriteRenderer, float value)
        {
            spriteRenderer.color = ColorUtils.SetB(spriteRenderer.color, value);
        }

        public static void SetColorA(this SpriteRenderer spriteRenderer, float value)
        {
            spriteRenderer.color = ColorUtils.SetA(spriteRenderer.color, value);
        }
    }
}
