using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UIPopupWithRawImage : UIPopup {
	[SerializeField] RawImage rawImage;

	public void SetRawImage(Texture2D texture2D) {
		float oldX = rawImage.rectTransform.rect.width;
		float newX = texture2D.width * rawImage.rectTransform.rect.height / texture2D.height;

		rawImage.texture = texture2D;
		rawImage.rectTransform.SetSizeDeltaX(newX);
		textFields[0].rectTransform.ChangeSizeDeltaX(oldX - newX);
		textFields[0].rectTransform.ChangeX((newX - oldX));
	}
}
