using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour {
	[SerializeField] byte FirstMenuId;
	[SerializeField] MenuBase[] Menus;
	Stack<MenuBase> currMenu;

	void Start() {
		currMenu = new Stack<MenuBase>();
		currMenu.Push(Menus[FirstMenuId]);

		foreach (var menu in Menus) {
			TemplateGameManager.Instance.uiinput.SetFirstButton(null);
			if (menu != currMenu.Peek())
				menu.Hide(true);
			else
				menu.Show(true);

			menu.MenuManager = this;
		}

		currMenu.Peek().SelectButton();

		TemplateGameManager.Instance.inputSystem.cancel.action.performed += OnCancelClick;
	}

	private void OnDestroy() {
		TemplateGameManager.Instance.inputSystem.cancel.action.performed -= OnCancelClick;
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
			if (currMenu.Count > 0) {
				if (hidePrev) {
					currMenu.Pop().Hide(false);
				}
				else {
					currMenu.Peek().SaveLastButton();
					currMenu.Peek().DisableAllSelectable();
				}
			}

			currMenu.Push(menu);
			menu.Show(false);
		}
		else {
			Debug.LogError($"Cant find menu with name {menuScriptName}");
		}
		
	}

	public void Show(MenuBase menu, bool hidePrev = true) {
		if (currMenu.Count > 0) {
			if (hidePrev) {
				currMenu.Pop().Hide(false);
			}
			else {
				currMenu.Peek().SaveLastButton();
				currMenu.Peek().DisableAllSelectable();
			}
		}
		
		currMenu.Push(menu);
		menu.Show(false);
	}

	public void HideTopMenu(bool isForce = false) {
		if(currMenu.Count > 1)
			currMenu.Pop().Hide(isForce);
		if(currMenu.Count >= 1) {
			currMenu.Peek().EnableAllSelectable();
			currMenu.Peek().SelectButton();
		}
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

	void OnCancelClick(InputAction.CallbackContext context) {
		if (context.ReadValueAsButton() && context.phase == InputActionPhase.Performed)
			HideTopMenu();
	}
}
