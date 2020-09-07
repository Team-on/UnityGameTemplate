using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformEx {
	public static void SetAnchoredPositionX(this RectTransform rectTransform, float value) {
		rectTransform.anchoredPosition = rectTransform.anchoredPosition.SetX(value);
	}

	public static void SetAnchoredPositionY(this RectTransform rectTransform, float value) {
		rectTransform.anchoredPosition = rectTransform.anchoredPosition.SetY(value);
	}

	public static void SetAnchorMaxX(this RectTransform rectTransform, float value) {
		rectTransform.anchorMax = rectTransform.anchorMax.SetX(value);
	}

	public static void SetAnchorMaxY(this RectTransform rectTransform, float value) {
		rectTransform.anchorMax = rectTransform.anchorMax.SetY(value);
	}

	public static void SetAnchorMinX(this RectTransform rectTransform, float value) {
		rectTransform.anchorMin = rectTransform.anchorMin.SetX(value);
	}

	public static void SetAnchorMinY(this RectTransform rectTransform, float value) {
		rectTransform.anchorMin = rectTransform.anchorMin.SetY(value);
	}

	public static void SetOffsetMaxX(this RectTransform rectTransform, float value) {
		rectTransform.offsetMax = rectTransform.offsetMax.SetX(value);
	}

	public static void SetOffsetMaxY(this RectTransform rectTransform, float value) {
		rectTransform.offsetMax = rectTransform.offsetMax.SetY(value);
	}

	public static void SetOffsetMinX(this RectTransform rectTransform, float value) {
		rectTransform.offsetMin = rectTransform.offsetMin.SetX(value);
	}

	public static void SetOffsetMinY(this RectTransform rectTransform, float value) {
		rectTransform.offsetMin = rectTransform.offsetMin.SetY(value);
	}

	public static void SetPivotX(this RectTransform rectTransform, float value) {
		rectTransform.pivot = rectTransform.pivot.SetX(value);
	}

	public static void SetPivotY(this RectTransform rectTransform, float value) {
		rectTransform.pivot = rectTransform.pivot.SetY(value);
	}

	public static void SetSizeDeltaX(this RectTransform rectTransform, float value) {
		rectTransform.sizeDelta = rectTransform.sizeDelta.SetX( value);
	}

	public static void SetSizeDeltaY(this RectTransform rectTransform, float value) {
		rectTransform.sizeDelta = rectTransform.sizeDelta.SetY(value);
	}

	public static void ChangeSizeDeltaX(this RectTransform rectTransform, float valueBy) {
		rectTransform.sizeDelta = rectTransform.sizeDelta.SetX(rectTransform.sizeDelta.x + valueBy);
	}

	public static void ChangeSizeDeltaY(this RectTransform rectTransform, float valueBy) {
		rectTransform.sizeDelta = rectTransform.sizeDelta.SetY(rectTransform.sizeDelta.y + valueBy);
	}

	public static void SetAnchor(this RectTransform rectTransform, AnchorPreset anchorPreset) {
		rectTransform.SetAnchor(anchorPreset, 0, 0);
	}

	public static void SetAnchor(this RectTransform rectTransform, AnchorPreset anchorPreset, float offsetX, float offsetY) {
		rectTransform.anchoredPosition = new Vector2(offsetX, offsetY);

		switch (anchorPreset) {
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

	public static void SetPivot(this RectTransform rectTransform, PivotPreset pivotPreset) {
		switch (pivotPreset) {
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


public enum AnchorPreset {
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

public enum PivotPreset {
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
