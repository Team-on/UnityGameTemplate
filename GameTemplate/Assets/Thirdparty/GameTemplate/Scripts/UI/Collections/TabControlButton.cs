using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TabControlButton : MonoBehaviour {
	[SerializeField] Color selectedColor;
	Color defaultColor;

	Button button;
	bool isDisabledByDefault;

	Action<byte> onClick;
	byte id;

	public void Init(Action<byte> _onClick, byte _id) {
		button = GetComponent<Button>();
		defaultColor = button.colors.disabledColor;
		isDisabledByDefault = !button.interactable;

		if (isDisabledByDefault)
			return;

		onClick = _onClick;
		id = _id;
		button.onClick.AddListener(OnClick);
	}

	void OnClick() {
		if (isDisabledByDefault)
			return;

		onClick?.Invoke(id);
	}

	public void SetSelection(bool isSelected) {
		if (isDisabledByDefault)
			return;

		var c = button.colors;
		if (isSelected) 
			c.disabledColor = selectedColor;
		else
			c.disabledColor = defaultColor;
		button.colors = c;

		button.interactable = !isSelected;
	}
}
