using System;
using UnityEditor;
using UnityEngine;

public class InspectorList<T> where T : class, new() {
	const byte listStringHeight = 15;
	const byte listStringSpace = listStringHeight + 3;
	const byte maxListSize = 150;

	public Action<T> OnChangeSelectionAction;

	public T Selected {
		get {
			return (index < (array?.Length ?? 0)) ? array[index] : null;
		}
		set {
			if (index < (array?.Length ?? 0)) {
				array[index] = value;
			}
		}
	}

	Func<T, int, string> itemNameFunc;

	string label;
	Vector2 scrollPos;

	T[] array;
	int index;

	public void Init(T[] arr, string _label, Func<T, int, string> _itemNameFunc) {
		array = arr;
		label = _label;
		itemNameFunc = _itemNameFunc;

		index = 0;
		scrollPos = Vector2.zero;
		OnChangeSelectionAction?.Invoke(array[index]);
	}

	public T[] Show() {
		if (array == null)
			return null;

		EditorGUILayout.LabelField(label);

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height((listStringSpace * array.Length < maxListSize ? listStringSpace * array.Length : maxListSize) + 5));
		for (int i = 0; i < array.Length; ++i) {
			EditorGUILayout.BeginHorizontal();

			string levelName = itemNameFunc(array[i], i);
			if (index == i) {
				GUIStyle centeredStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
				centeredStyle.alignment = TextAnchor.MiddleCenter;
				EditorGUILayout.LabelField(levelName, centeredStyle, GUILayout.Height(listStringHeight), GUILayout.MinWidth(0));
			}
			else {
				if (GUILayout.Button(levelName, GUILayout.Height(listStringHeight), GUILayout.MinWidth(0))) {
					index = i;
					OnChangeSelectionAction?.Invoke(array[index]);
					return array;
				}
			}

			if (GUILayout.Button("Up", GUILayout.Width(50), GUILayout.Height(listStringHeight)) && array.Length > 1 && i != 0) {
				T tmp = array[i - 1];
				array[i - 1] = array[i];
				array[i] = tmp;

				if (i == index) {
					--index;
					OnChangeSelectionAction?.Invoke(array[index]);
				}
				return array;
			}

			if (GUILayout.Button("Down", GUILayout.Width(50), GUILayout.Height(listStringHeight)) && array.Length > 1 && i != array.Length - 1) {
				T tmp = array[i + 1];
				array[i + 1] = array[i];
				array[i] = tmp;

				if (i == index) {
					++index;
					OnChangeSelectionAction?.Invoke(array[index]);
				}

				return array;
			}

			if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(listStringHeight)) && array.Length != 0) {
				T[] newArr = new T[array.Length - 1];
				for (int j = 0; j < array.Length; ++j) {
					if (j < i)
						newArr[j] = array[j];
					else if (j > i)
						newArr[j - 1] = array[j];
				}

				if ((i < index || (i == array.Length - 1 && index == i)) && index != 0) {
					--index;
					OnChangeSelectionAction?.Invoke(newArr[index]);
				}

				if (index == i && newArr.Length != 0)
					OnChangeSelectionAction?.Invoke(newArr[index]);

				array = newArr;
				return array;
			}

			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndScrollView();

		if (GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(listStringHeight))) {
			T[] newArr = new T[array.Length + 1];
			Array.Copy(array, newArr, array.Length);
			newArr[array.Length] = new T();
			index = array.Length;
			array = newArr;
			OnChangeSelectionAction?.Invoke(array[index]);
			return array;
		}

		return array;
	}
}