using UnityEngine;

namespace UnityForge
{
    public enum AnchorPreset
    {
        TopLeft,
        TopCenter,
        TopRight,

        MiddleLeft,
        MiddleCenter,
        MiddleRight,

        BottomLeft,
        BottomCenter,
        BottomRight,

        HorStretchTop,
        HorStretchMiddle,
        HorStretchBottom,

        VertStretchLeft,
        VertStretchCenter,
        VertStretchRight,

        StretchAll,
    }

    public enum PivotPreset
    {
        TopLeft,
        TopCenter,
        TopRight,

        MiddleLeft,
        MiddleCenter,
        MiddleRight,

        BottomLeft,
        BottomCenter,
        BottomRight,
    }

    // Inspired by http://answers.unity3d.com/questions/1225118/solution-set-ui-recttransform-anchor-presets-from.html
    public static partial class RectTransformExtensions
    {
        public static void SetAnchor(this RectTransform rectTransform, AnchorPreset anchorPreset)
        {
            rectTransform.SetAnchor(anchorPreset, 0, 0);
        }

        public static void SetAnchor(this RectTransform rectTransform, AnchorPreset anchorPreset, float offsetX, float offsetY)
        {
            rectTransform.anchoredPosition = new Vector2(offsetX, offsetY);

            switch (anchorPreset)
            {
                case AnchorPreset.TopLeft:
                    rectTransform.anchorMin = new Vector2(0, 1);
                    rectTransform.anchorMax = new Vector2(0, 1);
                    break;

                case AnchorPreset.TopCenter:
                    rectTransform.anchorMin = new Vector2(0.5f, 1);
                    rectTransform.anchorMax = new Vector2(0.5f, 1);
                    break;

                case AnchorPreset.TopRight:
                    rectTransform.anchorMin = new Vector2(1, 1);
                    rectTransform.anchorMax = new Vector2(1, 1);
                    break;

                case AnchorPreset.MiddleLeft:
                    rectTransform.anchorMin = new Vector2(0, 0.5f);
                    rectTransform.anchorMax = new Vector2(0, 0.5f);
                    break;

                case AnchorPreset.MiddleCenter:
                    rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    break;

                case AnchorPreset.MiddleRight:
                    rectTransform.anchorMin = new Vector2(1, 0.5f);
                    rectTransform.anchorMax = new Vector2(1, 0.5f);
                    break;

                case AnchorPreset.BottomLeft:
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(0, 0);
                    break;

                case AnchorPreset.BottomCenter:
                    rectTransform.anchorMin = new Vector2(0.5f, 0);
                    rectTransform.anchorMax = new Vector2(0.5f, 0);
                    break;

                case AnchorPreset.BottomRight:
                    rectTransform.anchorMin = new Vector2(1, 0);
                    rectTransform.anchorMax = new Vector2(1, 0);
                    break;

                case AnchorPreset.HorStretchTop:
                    rectTransform.anchorMin = new Vector2(0, 1);
                    rectTransform.anchorMax = new Vector2(1, 1);
                    break;

                case AnchorPreset.HorStretchMiddle:
                    rectTransform.anchorMin = new Vector2(0, 0.5f);
                    rectTransform.anchorMax = new Vector2(1, 0.5f);
                    break;

                case AnchorPreset.HorStretchBottom:
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(1, 0);
                    break;

                case AnchorPreset.VertStretchLeft:
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(0, 1);
                    break;

                case AnchorPreset.VertStretchCenter:
                    rectTransform.anchorMin = new Vector2(0.5f, 0);
                    rectTransform.anchorMax = new Vector2(0.5f, 1);
                    break;

                case AnchorPreset.VertStretchRight:
                    rectTransform.anchorMin = new Vector2(1, 0);
                    rectTransform.anchorMax = new Vector2(1, 1);
                    break;

                case AnchorPreset.StretchAll:
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(1, 1);
                    break;

                default:
                    Debug.LogErrorFormat("Not supported anchor preset {0}", anchorPreset);
                    break;
            }
        }

        public static void SetPivot(this RectTransform rectTransform, PivotPreset pivotPreset)
        {
            switch (pivotPreset)
            {
                case PivotPreset.TopLeft:
                    rectTransform.pivot = new Vector2(0, 1);
                    break;

                case PivotPreset.TopCenter:
                    rectTransform.pivot = new Vector2(0.5f, 1);
                    break;

                case PivotPreset.TopRight:
                    rectTransform.pivot = new Vector2(1, 1);
                    break;

                case PivotPreset.MiddleLeft:
                    rectTransform.pivot = new Vector2(0, 0.5f);
                    break;

                case PivotPreset.MiddleCenter:
                    rectTransform.pivot = new Vector2(0.5f, 0.5f);
                    break;

                case PivotPreset.MiddleRight:
                    rectTransform.pivot = new Vector2(1, 0.5f);
                    break;

                case PivotPreset.BottomLeft:
                    rectTransform.pivot = new Vector2(0, 0);
                    break;

                case PivotPreset.BottomCenter:
                    rectTransform.pivot = new Vector2(0.5f, 0);
                    break;

                case PivotPreset.BottomRight:
                    rectTransform.pivot = new Vector2(1, 0);
                    break;

                default:
                    Debug.LogErrorFormat("Not supported pivot preset {0}", pivotPreset);
                    break;
            }
        }
    }
}
