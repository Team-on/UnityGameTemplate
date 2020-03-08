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
				menu.Hide(true);
			else
				menu.Show(true);

			menu.MenuManager = this;
		}
	}

	public void Show(string menuScriptName) {
		Show(menuScriptName, currMenu.Peek() is PopupMenuBase);
	}

	public void Show(string menuScriptName, bool hidePrev = true) {
		MenuBase menu = null;
		for (int i = 0; i < Menus.Length; ++i) {
			if (Menus[i].GetType().Name == menuScriptName) {
				menu = Menus[i];
				break;
			}
		}

		if(menu != null) {
			if (currMenu.Count > 0 && hidePrev) 
				currMenu.Pop().Hide(false);

			currMenu.Push(menu);
			menu.Show(false);
		}
		else {
			Debug.LogError($"Cant find menu with name {menuScriptName}");
		}
		
	}

	public void Show(MenuBase menu, bool hidePrev = true) {
		if (hidePrev && currMenu.Count > 0) 
			currMenu.Pop().Hide(false);

		currMenu.Push(menu);
		menu.Show(false);
	}

	public void HideTopMenu(bool isForce = false) {
		currMenu.Pop().Hide(isForce);
	}

	public void HideAll() {
		while (currMenu.Count != 0)
			currMenu.Pop().Hide(true);
	}


	public T GetNeededMenu<T>() where T : MenuBase {
		for (int i = 0; i < Menus.Length; ++i)
			if(Menus[i] is T)
				return Menus[i] as T;
		return null;
	}
}
