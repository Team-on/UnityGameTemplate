using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Diagnostics;

public static class HelperFunctions {
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetCurrentMethod() {
		StackTrace st = new StackTrace();
		StackFrame sf = st.GetFrame(1);

		return sf.GetMethod().Name;
	}

	public static bool GetEventWithChance(int percent) {
		int number = UnityEngine.Random.Range(1, 101);
		return number <= percent;
	}

	public static void Shuffle<T>(this IList<T> list) {
		int n = list.Count;
		while (n > 1) {
			n--;
			int rand = UnityEngine.Random.Range(0, n + 1);
			T value = list[rand];
			list[rand] = list[n];
			list[n] = value;
		}
	}

	public static T Random<T>(this IList<T> list) {
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	public static void Hide(this Graphic img) {
		Color c = img.color;
		c.a = 0;
		img.color = c;
	}

	public static void Show(this Graphic img) {
		Color c = img.color;
		c.a = 1;
		img.color = c;
	}
}
