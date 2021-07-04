using System.Runtime.CompilerServices;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

#if UNITY_EDITOR
    public static T LoadAssetRef<T>(this Component comp, string name) where T : Object {
        if (typeof(T) == typeof(AudioClip))
            return LoadAssetRef<T>(comp, name, "t:AudioClip");
        if (typeof(T) == typeof(GameObject))
            return LoadAssetRef<T>(comp, name, "t:prefab");

        return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets($"\"{name}\"")[0]));
    }

    public static T LoadAssetRef<T>(this Component comp, string name, string filter) where T : Object {
        return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets($"\"{name}\" {filter}")[0]));
    }
#endif


    //public static Component CopyComponent(this Component original, GameObject destination) {
    //    System.Type type = original.GetType();
    //    Component copy = destination.AddComponent(type);
    //    // Copied fields can be restricted with BindingFlags
    //    System.Reflection.FieldInfo[] fields = type.GetFields();
    //    foreach (System.Reflection.FieldInfo field in fields) {
    //        field.SetValue(copy, field.GetValue(original));
    //    }
    //    return copy;
    //}

    public static T CopyComponent<T>(this T original, GameObject destination) where T : Component {
        System.Type type = original.GetType();
        var dst = destination.GetComponent(type) as T;
        if (!dst)
            dst = destination.AddComponent(type) as T;
        var fields = type.GetFields();
        foreach (var field in fields) {
            if (field.IsStatic)
                continue;
            field.SetValue(dst, field.GetValue(original));
        }
        var props = type.GetProperties();
        foreach (var prop in props) {
            if (!prop.CanRead || !prop.CanWrite || prop.Name == "name" || prop.Name == "usedByComposite" || prop.Name == "density")
                continue;
            prop.SetValue(dst, prop.GetValue(original, null), null);
        }
        return dst;
    }
}
