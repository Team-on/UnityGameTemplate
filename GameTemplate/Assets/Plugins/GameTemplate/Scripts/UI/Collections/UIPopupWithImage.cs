using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UIPopupWithImage : UIPopup {
	[SerializeField] Image img;

	public void SetImage(Sprite spr) {
		float oldX = img.rectTransform.rect.width;
		float newX = spr.rect.width * img.rectTransform.rect.height / spr.rect.height;

		img.sprite = spr;
		img.rectTransform.SetSizeDeltaX(newX);
		textFields[0].rectTransform.ChangeSizeDeltaX(oldX - newX);
		textFields[0].rectTransform.ChangeX((newX - oldX) / 2);
	}
}
