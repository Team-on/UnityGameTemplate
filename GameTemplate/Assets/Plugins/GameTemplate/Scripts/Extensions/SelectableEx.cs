using UnityEngine;
using UnityEngine.UI;

public static class SelectableEx {
	public static void SetNavigationUp(this Selectable selectable, Selectable target) {
		Navigation nav = selectable.navigation;
		nav.selectOnUp = target;
		selectable.navigation = nav;
	}

	public static void SetNavigationDown(this Selectable selectable, Selectable target) {
		Navigation nav = selectable.navigation;
		nav.selectOnDown = target;
		selectable.navigation = nav;
	}

	public static void SetNavigationLeft(this Selectable selectable, Selectable target) {
		Navigation nav = selectable.navigation;
		nav.selectOnLeft = target;
		selectable.navigation = nav;
	}

	public static void SetNavigationRight(this Selectable selectable, Selectable target) {
		Navigation nav = selectable.navigation;
		nav.selectOnRight = target;
		selectable.navigation = nav;
	}
}
