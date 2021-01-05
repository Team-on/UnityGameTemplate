using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Random = UnityEngine.Random;

public class ResourceUI : MonoBehaviour {
	[Header("Data"), Space]
	[SerializeField] ResourceType type;

	[Header("Audio"), Space]
	[SerializeField] AudioClip counterSound;
	AudioSource counteras;

	[Header("Refs"), Space]
	[SerializeField] TextMeshProUGUI textField;
	[SerializeField] Image image;

	Coroutine coroutine;
	int oldValue;
	int currValue;

#if UNITY_EDITOR
	private void Reset() {
		counterSound = this.LoadAssetRef<AudioClip>("ResourceCounter");
	}
#endif

	private void Awake() {
		TemplateGameManager.Instance.onResourceChange[(int)type] += OnValueUpdated;
	}

	private void OnDestroy() {
		TemplateGameManager.Instance.onResourceChange[(int)type] -= OnValueUpdated;
	}

	public void DropWithFlyingParticles(int delta, Vector3 worldPos) {
		int particles = delta / 10 + (delta % 10 != 0 ? 1 : 0);
		while (particles != 0) {
			--particles;
			bool isLastLoop = particles == 0;

			GameObject go = new GameObject($"Flying resource {type}");
			go.transform.SetParent(image.canvas.transform);
			go.transform.position = TemplateGameManager.Instance.Camera.WorldToScreenPoint(worldPos + (Vector3)Random.insideUnitCircle);
			go.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360f));

			Image img = go.AddComponent<Image>();
			img.sprite = image.sprite;
			img.color = img.color.SetA(0.0f);
			img.preserveAspect = true;
			img.SetNativeSize();
			img.rectTransform.sizeDelta = img.rectTransform.sizeDelta.SetX(76.17407f);

			LeanTween.value(0, 1, 0.5f)
				.setOnUpdate((float a) => {
					img.color = img.color.SetA(a);
				})
				.setOnComplete(() => {
					Vector3 startPos = go.transform.position;
					Vector3 startAngle = go.transform.localEulerAngles;
					float dist = (image.transform.position - startPos).magnitude;
					int endedTweens = 0;

					if (counteras == null && counterSound)
						counteras = AudioManager.Instance.Play(counterSound);

					LeanTween.value(0, 1, dist / Screen.height * 0.8f)
					.setDelay(0.1f * particles)
					.setOnUpdate((float t) => {
						go.transform.position = Vector3.Lerp(startPos, image.transform.position, t);
						go.transform.localEulerAngles = Vector3.Lerp(startAngle, Vector3.zero, t);
					})
					.setOnComplete(() => {
						OnFlyEnd();
					});


					LeanTween.value(1, 0, 0.2f)
					.setEase(LeanTweenType.easeInCubic)
					.setDelay(0.1f * particles + dist / Screen.height * 0.64f)
					.setOnUpdate((float t) => {
						img.color = img.color.SetA(t);
						go.transform.localScale = Vector3.one * t;
					})
					.setOnComplete(() => {
						OnFlyEnd();
					});

					void OnFlyEnd() {
						++endedTweens;
						if (endedTweens != 2)
							return;

						if (delta >= 10) {
							TemplateGameManager.Instance[type] += 10;
							delta -= 10;
						}
						else {
							TemplateGameManager.Instance[type] += delta;
							delta = 0;
						}

						if (counteras != null && isLastLoop) {
							counteras.Stop();
							counteras = null;
						}

						Destroy(go, Random.Range(0.1f, 1.0f));
					}
				});
		}
	}


	void OnValueUpdated(int newValue) {
		currValue = newValue;

		if (coroutine != null)
			StopCoroutine(coroutine);
		coroutine = StartCoroutine(UpdateValueRoutine());

		IEnumerator UpdateValueRoutine() {
			while (oldValue != currValue) {
				int difference = currValue - oldValue;

				if (difference > 0)
					++oldValue;
				else
					--oldValue;

				textField.text = oldValue.ToString();
				yield return null;
			}
		}
	}
}