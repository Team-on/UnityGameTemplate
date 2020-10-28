using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenInit : MonoBehaviour {
	[SerializeField] int maxSimultaneousTweens = 400;

	void Awake() {
		LeanTween.init(maxSimultaneousTweens);
	}
}
