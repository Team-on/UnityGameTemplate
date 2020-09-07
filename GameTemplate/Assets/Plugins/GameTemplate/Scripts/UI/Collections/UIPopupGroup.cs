using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupGroup : MonoBehaviour {
	[Header("Timings")]
	[SerializeField] float slideTime = 0.5f;
	[SerializeField] float defaultShowTime = 2.0f;

	[Header("This refs")]
	[SerializeField] RectTransform rt = null;

	[Header("Child refs")]
	[Space]
	[SerializeField] RectTransform gridTransform = null;
	[SerializeField] GridLayoutGroup grid = null;

	[Header("Assets refs")]
	[Space]
	[SerializeField] GameObject popupPrefab = null;
	[SerializeField] GameObject popupWithRawImagePrefab = null;

	float startYPos;
	Coroutine hideCoroutine;
	int currPopup;

	List<float> openTimes;

	private void Awake() {
		TemplateGameManager.Instance.debugPopups = this;

		openTimes = new List<float>(4);

		startYPos = gridTransform.anchoredPosition.y;
	}

	public void ShowPopup(string text) {
		ShowPopup(text, defaultShowTime);
	}

	public void ShowPopup(string text, Texture2D texture) {
		ShowPopup(text, texture, defaultShowTime);
	}

	public void ShowPopup(string text, float time) {
		openTimes.Add(Time.realtimeSinceStartup + time);

		UIPopup popupText = Instantiate(popupPrefab, transform.position, Quaternion.Euler(Vector2.zero), gridTransform).GetComponent<UIPopup>();
		popupText.transform.localEulerAngles = Vector3.zero;

		popupText.SetText(text);
		openTimes[openTimes.Count - 1] += popupText.PlayShowAnimation();

		if (hideCoroutine == null)
			hideCoroutine = StartCoroutine(HideCoroutine());
	}

	public void ShowPopup(string text, Texture2D texture, float time) {

		UIPopupWithRawImage popupText = Instantiate(popupWithRawImagePrefab, transform.position, Quaternion.Euler(Vector2.zero), gridTransform).GetComponent<UIPopupWithRawImage>();
		popupText.transform.localEulerAngles = Vector3.zero;

		popupText.SetText(text);
		popupText.SetRawImage(texture);

		openTimes.Add(Time.realtimeSinceStartup + time);
		openTimes[openTimes.Count - 1] += popupText.PlayShowAnimation();

		if (hideCoroutine == null)
			hideCoroutine = StartCoroutine(HideCoroutine());
	}

	IEnumerator HideCoroutine() {
		while (currPopup < openTimes.Count) {
			while (openTimes[currPopup] >= Time.realtimeSinceStartup) {
				yield return null;
			}

			++currPopup;

			LeanTween.cancel(gameObject);
			LeanTween.value(gameObject, gridTransform.anchoredPosition.y, grid.cellSize.y * (currPopup) + grid.spacing.y * (currPopup - 1) - rt.anchoredPosition.y, slideTime)
				.setOnUpdate((float y) => {
					Vector2 newPos = gridTransform.anchoredPosition;
					newPos.y = y;
					gridTransform.anchoredPosition = newPos;
				});

			yield return new WaitForSeconds(slideTime);
		}

		LeanTween.cancel(gameObject);

		grid.transform.DestroyAllChildrens();
		gridTransform.anchoredPosition = gridTransform.anchoredPosition.SetY(startYPos);

		openTimes.Clear();
		currPopup = 0;
		hideCoroutine = null;
	}
}
