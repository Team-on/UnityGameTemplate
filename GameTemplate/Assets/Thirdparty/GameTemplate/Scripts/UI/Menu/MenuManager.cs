using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
	[SerializeField] byte FirstMenuId;
	[SerializeField] MenuBase[] Menus;
	Stack<MenuBase> currMenu;

	void Start() {
		currMenu = new Stack<MenuBase>();
		currMenu.Push(Menus[FirstMenuId]);

		foreach (var menu in Menus) {
			if (menu != currMenu.Peek())
				menu.Hide();
			else
				menu.Show();

			menu.MenuManager = this;
		}
	}

	public void HideAll() {
		while(currMenu.Count != 0)
			currMenu.Pop().Hide();
	}

	public void ShowMenuFromStack() {
		currMenu.Pop().Hide();
	}

	public void TransitTo(MenuBase menu, bool hidePrev = true) {
		if (hidePrev && currMenu.Count > 0) {
			currMenu.Pop().Hide();
		}

		currMenu.Push(menu);
		menu.Show();
	}

	public T GetNeededMenu<T>() where T : MenuBase {
		return Menus.First((m)=>m is T) as T;
	}
}
