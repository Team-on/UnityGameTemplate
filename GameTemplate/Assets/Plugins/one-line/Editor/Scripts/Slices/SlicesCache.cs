using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using OneLine.Settings;

namespace OneLine {
	internal class SlicesCache {

		private Dictionary<string, Slices> cache;
		private Action<SerializedProperty, Slices> calculate;

		public bool IsDirty { get; private set; }

		public SlicesCache(Action<SerializedProperty, Slices> calculate){
			cache = new Dictionary<string, Slices>();
			this.calculate = calculate;
		}

		private string lastId = null;

		public Slices this[SerializedProperty property] {
			get {
				if (!SettingsMenu.Value.CacheOptimization) {
					var slices = new SlicesImpl();
					calculate(property, slices);
					return slices;
				}

				lastId = GetId(property);
				if (cache.ContainsKey(lastId)){
					return cache[lastId];
				}
				else {
					var slices = new SlicesImpl();
					calculate(property, slices);
					cache.Add(lastId, slices);
					IsDirty = true;
					return slices;
				}
			}
		}

		private string GetId(SerializedProperty property){
			return property.propertyPath;
		}

		public void InvalidateLastUsedId(SerializedProperty property){
			var id = GetId(property);
			var list = new List<string>();
			foreach (var key in cache.Keys) {
				if (id.StartsWith(key + ".")) {
					list.Add(key);
				}
			}
			foreach(var key in list) {
				cache.Remove(key);
			}
		}

	}
}