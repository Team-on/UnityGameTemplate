using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorEx {
	public static bool ContainsParam(this Animator _Anim, string _ParamName) {
		foreach (AnimatorControllerParameter param in _Anim.parameters) {
			if (param.name == _ParamName)
				return true;
		}
		return false;
	}
}
