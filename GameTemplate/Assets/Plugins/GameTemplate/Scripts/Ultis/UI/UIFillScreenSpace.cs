using UnityEngine;
using UnityEngine.UI;

public class UIFillScreenSpace : MonoBehaviour {
	[Header("This refs")]
	[SerializeField] Image img;
	[SerializeField] Canvas canvas;

#if UNITY_EDITOR
	private void OnValidate() {
		if (img == null)
			img = GetComponent<Image>();
		if (canvas == null && img != null)
			canvas = img.canvas;
	}
#endif

	private void Start() {
		EventManager.OnScreenResolutionChange += RecalFill;
		RecalFill();
	}

	private void OnDestroy() {
		EventManager.OnScreenResolutionChange -= RecalFill;
	}

	void RecalFill(EventData ed = null) {
		Vector2 originalSize = img.sprite.rect.size;
		float originalApsect = originalSize.x / originalSize.y;
		Vector2 neededSize = canvas.GetComponent<RectTransform>().rect.size;
		Vector2 calcSize = originalSize;

		calcSize.x = neededSize.x;
		calcSize.y = calcSize.x / originalApsect;

		if (calcSize.y < neededSize.y) {
			calcSize.y = neededSize.y;
			calcSize.x = calcSize.y * originalApsect;
		}

		img.rectTransform.pivot = new Vector2(0.5f, 0.5f);
		img.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
		img.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
		img.rectTransform.sizeDelta = calcSize;
	}
}
