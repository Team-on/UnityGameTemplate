using System.Runtime.CompilerServices;
using UnityEngine;

public static class ComponentEx {
	public static T GetComponentThisOrParent<T>(this GameObject go) where T : Component {
		T val = default(T);
		if (val == null) {
			val = go.GetComponent<T>();
			if (val == null)
				val = go.GetComponentInParent<T>();
		}
		return val;
	}

	public static T GetComponentThisOrParent<T>(this Component comp) where T : Component {
		T val = default(T);
		if (val == null) {
			val = comp.GetComponent<T>();
			if (val == null)
				val = comp.GetComponentInParent<T>();
		}
		return val;
	}
}
