using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadSelectionField : MonoBehaviour {
	[Header("Positioning"), Space]
	[SerializeField] /*[Range(5, 128)]*/ Vector2 sizeBounds = new Vector2(36, 10);
	[SerializeField] /*[Range(0, 20)]*/ Vector2 sizeBoundsOffsets = new Vector2(0, 5);
	[SerializeField] float xSpacing = 10;


	[Header("Refs"), Space]
	[SerializeField] GameObject optionPrefab;
	[SerializeField] RectTransform optionsParent;

	List<string> values;
	List<GamepadSelectionFieldSector> sectors;
	Action<int> callback;
	int id;

	public void SetId(int newId) {
		id = newId;
		StartCoroutine(PositionRoutine());
	}

	public void Init(List<string> _values, int selectedId) {
		values = _values;
		sectors = new List<GamepadSelectionFieldSector>(values.Count);
		id = selectedId;

		for (int i = 0; i < values.Count; ++i) {
			GamepadSelectionFieldSector sector = Instantiate(optionPrefab, optionsParent).GetComponent<GamepadSelectionFieldSector>();

			sector.SetText(values[i]);

			sectors.Add(sector);
		}

		SetId(selectedId);
	}

	public void AddCallback(Action<int> callback) {
		this.callback += callback;
	}

	public void RemoveCallback(Action<int> callback) {
		this.callback -= callback;
	}

	IEnumerator PositionRoutine() {
		for (int i = 0; i < values.Count; ++i) {
			int diff = Mathf.Abs(i - id);

			sectors[i].SetFontSize(sizeBounds.Lerp(sizeBoundsOffsets.InverseLerpInt(diff)));
		}

		yield return null;

		float lastX = 0;
		for (int i = 0; i < values.Count; ++i) {
			if (i != 0)
				lastX += sectors[i].Width / 2;

			sectors[i].transform.localPosition = new Vector3(lastX, 0);
			
			lastX += sectors[i].Width / 2;
			lastX += xSpacing;
		}

		float posx = 0;
		for (int i = 0; i <= id; ++i) {
			if (i != 0) {
				posx -= sectors[i].Width / 2;
				posx -= sectors[i - 1].Width / 2;
				posx -= xSpacing;
			}
		}
		optionsParent.localPosition = optionsParent.localPosition.SetX(posx);
	}
}
