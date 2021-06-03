using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//TODO: poll this
public class UIArrow2D : MonoBehaviour {
	[NonSerialized] public Transform pointTo;
	[NonSerialized] public ContactFilter2D filterPointToPlayer = new ContactFilter2D();

	[Header("Scale tween"), Space]
	[SerializeField] Vector3 minScale = new Vector3(0.95f, 0.95f, 1.0f);
	[SerializeField] Vector3 maxScale = new Vector3(1f, 1f, 1f);
	[SerializeField] float scaleTime = 1.2f;
	float currScaleTime = 0.0f;
	bool isScaleUp = true;

	[Header("This refs"), Space]
	[SerializeField] RectTransform rect;
	[SerializeField] Image img;
	[SerializeField] CanvasGroup cg;

	List<RaycastHit2D> hits = new List<RaycastHit2D>(16);

	BoxCollider2D screenFrames;
	float frameToScreenDist;

	bool isShowed;
	float maxA;

	Vector3 dir;
	int hitted;

	public void Init(Transform pointTo, ContactFilter2D filterPointToPlayer, BoxCollider2D screenFrames, float frameToScreenDist, float scale) {
		this.pointTo = pointTo;
		this.screenFrames = screenFrames;
		this.frameToScreenDist = frameToScreenDist;
		this.filterPointToPlayer = filterPointToPlayer;

		minScale *= scale;
		maxScale *= scale;

		isShowed = false;

		maxA = img.color.a;
		img.color = img.color.SetA(0.0f);
	}

	void OnDestroy() {
		LeanTween.cancel(img.gameObject);
	}

	void Update() {
		if (!pointTo) {
			return;
		}

		bool isInCameraView = screenFrames.OverlapPoint(pointTo.position);

		if (!isShowed && !isInCameraView) {
			Show();
		}

		if (isShowed) {
			if (isInCameraView) {
				Hide();
			}
			else {
				dir = (pointTo.position - TemplateGameManager.Instance.Camera.transform.position).normalized;
				dir.z = 0.0f;
				hitted = Physics2D.Raycast(pointTo.position, -dir, filterPointToPlayer, hits, 100.0f);

				for (int i = 0; i < hitted; ++i) {
					if (
						hits[i].collider == screenFrames
					) {
						rect.position = TemplateGameManager.Instance.Camera.WorldToScreenPoint(hits[i].point);
						rect.rotation = Quaternion.LookRotation(Vector3.forward, dir);
						break;
					}
				}
			}

			if (isScaleUp) {
				currScaleTime += Time.deltaTime;
				if (currScaleTime >= scaleTime) {
					currScaleTime = scaleTime;
					isScaleUp = false;
				}
			}
			else {
				currScaleTime -= Time.deltaTime;
				if (currScaleTime <= 0) {
					currScaleTime = 0.0f;
					isScaleUp = true;
				}
			}
			transform.localScale = Vector3.Lerp(minScale, maxScale, currScaleTime / scaleTime);

			img.color = img.color.SetA(Mathf.Lerp(0.0f, maxA, (((Vector2)pointTo.position - screenFrames.ClosestPoint(pointTo.position)).magnitude) / frameToScreenDist));
			cg.alpha = img.color.a;
		}
	}

	void Show() {
		if (isShowed)
			return;
		isShowed = true;

		LeanTween.cancel(img.gameObject);

		LeanTween.value(img.gameObject, img.color.a, maxA, 0.3f)
		.setOnUpdate((float a) => {
			img.color = img.color.SetA(a);
			cg.alpha = a;
		});

		transform.localScale = minScale;
		isScaleUp = false;
		currScaleTime = UnityEngine.Random.Range(0.0f, scaleTime);
	}

	void Hide() {
		if (!isShowed)
			return;
		isShowed = false;

		LeanTween.cancel(img.gameObject);

		LeanTween.value(img.gameObject, img.color.a, 0.0f, 0.1f)
		.setOnUpdate((float a) => {
			img.color = img.color.SetA(a);
			cg.alpha = a;
		});
	}
}
