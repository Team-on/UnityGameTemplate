using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArrowsController : MonoBehaviour {
	[Header("Data"), Space]
	[SerializeField] ContactFilter2D filterPointToPlayer = new ContactFilter2D();
	[SerializeField] float framesSize = 0.9f;

	[Header("Refs"), Space]
	[SerializeField] GameObject arrowPrefab;


	BoxCollider2D screenFrames;
	float frameToScreenDist;

	List<UIArrow2D> arrows = new List<UIArrow2D>(4);

	void Awake() {
		TemplateGameManager.Instance.arrows = this;
		RecalFrames();

		EventManager.OnScreenResolutionChange += RecalFrames;
	}

	private void OnDestroy() {
		EventManager.OnScreenResolutionChange -= RecalFrames;
	}

	void RecalFrames(EventData ed = null) {
		if(screenFrames == null) {
			screenFrames = TemplateGameManager.Instance.Camera.gameObject.GetComponent<BoxCollider2D>();
			if (screenFrames == null)
				screenFrames = TemplateGameManager.Instance.Camera.gameObject.AddComponent<BoxCollider2D>();
		}

		screenFrames.isTrigger = true;
		screenFrames.gameObject.layer = LayerMask.NameToLayer("Water");
		screenFrames.transform.position = Vector3.zero;
		screenFrames.size = TemplateGameManager.Instance.Camera.ViewportToWorldPoint(Vector3.one * framesSize) - TemplateGameManager.Instance.Camera.ViewportToWorldPoint(Vector3.zero);

		Vector2 dist = (TemplateGameManager.Instance.Camera.ViewportToWorldPoint(Vector3.one * (1.0f - framesSize) / 2) - TemplateGameManager.Instance.Camera.ViewportToWorldPoint(Vector3.zero));
		frameToScreenDist = (dist.x + dist.y) / 2;
	}

	public void AddArrow(Transform pointTo, float scale) {
		UIArrow2D arrow = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<UIArrow2D>();
		arrow.Init(pointTo, filterPointToPlayer, screenFrames, frameToScreenDist, scale);
		arrows.Add(arrow);
	}

	public void RemoveArrow(Transform pointTo) {
		foreach (var arrow in arrows) {
			if (arrow.pointTo == pointTo) {
				Destroy(arrow.gameObject);
				arrows.Remove(arrow);
				break;
			}
		}
	}
}
