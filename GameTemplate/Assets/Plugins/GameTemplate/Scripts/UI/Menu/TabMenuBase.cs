using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class TabMenuBase : PopupMenuBase {
	[Header("Tab menu settings")]
	[SerializeField] float alphaChangeSpeed = 0.2f;
	[SerializeField] bool canTabWithMoveAction = false;
	[SerializeField] GameObject ArrowLeft;
	[SerializeField] GameObject ArrowRight;
	[SerializeField] CanvasGroup[] Tabs;

	byte currTab = 0;

	protected override void Awake() {
		base.Awake();

		foreach (var tab in Tabs) {
			tab.alpha = 0;
			tab.interactable = tab.blocksRaycasts = false;
		}

		if (Tabs.Length != 0) {
			Tabs[currTab].alpha = 1;
			Tabs[currTab].interactable = Tabs[currTab].blocksRaycasts = true;
		}

		ArrowLeft.gameObject.SetActive(false);
	}

	internal override void Show(bool isForce) {
		if (Tabs.Length != 0) {
			Tabs[currTab].alpha = 0;
			Tabs[currTab].interactable = Tabs[currTab].blocksRaycasts = false;

			currTab = 0;
			Tabs[currTab].alpha = 1;
			Tabs[currTab].interactable = Tabs[currTab].blocksRaycasts = true;
		}

		if (Tabs.Length > 1) {
			ArrowLeft.gameObject.SetActive(false);
			ArrowRight.gameObject.SetActive(true);
		}
		else {
			ArrowLeft.gameObject.SetActive(false);
			ArrowRight.gameObject.SetActive(false);
		}

		base.Show(isForce);

		if (canTabWithMoveAction) {
			TemplateGameManager.Instance.inputSystem.move.action.started += OnMove;
		}
	}

	internal override void Hide(bool isForce) {
		Hide(isForce);

		if (canTabWithMoveAction) {
			TemplateGameManager.Instance.inputSystem.move.action.started -= OnMove;
		}
	}

	private void Update() {
		if (!isShowed)
			return;

		if (Gamepad.current != null) {
			if (Gamepad.current.leftShoulder.wasPressedThisFrame)
				TabLeft();
			else if (Gamepad.current.rightShoulder.wasPressedThisFrame)
				TabRight();
		}

		if (Keyboard.current != null) {
			if (Keyboard.current.qKey.wasPressedThisFrame)
				TabLeft();
			else if (Keyboard.current.eKey.wasPressedThisFrame)
				TabRight();
		}
	}

	public void TabLeft() {
		if (currTab != 0) {
			if (currTab == Tabs.Length - 1)
				ArrowRight.gameObject.SetActive(true);

			LeanTween.alphaCanvas(Tabs[currTab], 0, alphaChangeSpeed);
			Tabs[currTab].interactable = Tabs[currTab].blocksRaycasts = false;

			--currTab;
			LeanTween.alphaCanvas(Tabs[currTab], 1, alphaChangeSpeed);
			Tabs[currTab].interactable = Tabs[currTab].blocksRaycasts = true;

			if (currTab == 0)
				ArrowLeft.gameObject.SetActive(false);
		}
	}

	public void TabRight() {
		if (currTab != Tabs.Length - 1) {
			if (currTab == 0)
				ArrowLeft.gameObject.SetActive(true);

			LeanTween.alphaCanvas(Tabs[currTab], 0, alphaChangeSpeed);
			Tabs[currTab].interactable = Tabs[currTab].blocksRaycasts = false;

			++currTab;
			LeanTween.alphaCanvas(Tabs[currTab], 1, alphaChangeSpeed);
			Tabs[currTab].interactable = Tabs[currTab].blocksRaycasts = true;

			if (currTab == Tabs.Length - 1)
				ArrowRight.gameObject.SetActive(false);
		}
	}

	void OnMove(InputAction.CallbackContext context) {
		if (context.phase == InputActionPhase.Started) {
			Vector2 v = context.ReadValue<Vector2>();
			if (v.x < 0)
				TabLeft();
			else if (v.x > 0)
				TabRight();
		}
	}
}
