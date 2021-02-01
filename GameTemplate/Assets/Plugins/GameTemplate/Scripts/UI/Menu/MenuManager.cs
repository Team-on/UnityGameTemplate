using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour {
	[Header("Menus"), Space]
	[SerializeField] byte FirstMenuId;
	[SerializeField] MenuBase[] Menus;

	Stack<MenuBase> currMenu;

	void Start() {
		currMenu = new Stack<MenuBase>();
		currMenu.Push(Menus[FirstMenuId]);
		MenuBase firstMenu = currMenu.Peek();

		TemplateGameManager.Instance.uiinput.SetSelectedButton(null);
	
		foreach (var menu in Menus) {
			if (menu != firstMenu)
				menu.Hide(true);

			if(menu.rt.anchorMin == Vector2.zero && menu.rt.anchorMax == Vector2.one && menu.rt.pivot.x == 0.5f && menu.rt.pivot.y == 0.5f && !(menu is PopupMenuBase)) {
				menu.rt.transform.localPosition = Vector2.zero;
			}

			menu.MenuManager = this;
		}

		TemplateGameManager.Instance.inputSystem.cancel.action.performed += OnCancelClick;

		StartCoroutine(DelayedShow());

		IEnumerator DelayedShow() {
			yield return null;
			yield return null;
			firstMenu.Show(true);
		}
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

		if (menu != null) {
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
		if (IsCanHideTopMenu()) {
			currMenu.Pop().Hide(isForce);

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
			if (Menus[i] is T)
				return Menus[i] as T;
		return null;
	}

	public bool IsCanHideTopMenu() {
		if (currMenu.Count >= 2) {
			MenuBase topMenu = currMenu.Pop();
			bool isCanReturn = currMenu.Peek().IsCanReturnToMenu;
			currMenu.Push(topMenu);

			return isCanReturn;
		}

		return false;
	}

	void OnCancelClick(InputAction.CallbackContext context) {
		if (context.ReadValueAsButton() && context.phase == InputActionPhase.Performed)
			HideTopMenu();
	}
}
