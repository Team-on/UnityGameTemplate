using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LayerAttribute))]
public class LayerAttributeEditor : PropertyDrawer {
    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label) {
        EditorGUI.BeginProperty(pos, label, prop);
        int index = prop.intValue;
        if (index > 31) {
            Debug.Log("CustomPropertyDrawer(LayerAttributeEditor), layer index is to high '" + index + "', is set to 31.");
            index = 31;
        }
        else if (index < 0) {
            Debug.Log("CustomPropertyDrawer(LayerAttributeEditor), layer index is to low '" + index + "', is set to 0");
            index = 0;
        }
        prop.intValue = EditorGUI.LayerField(pos, label, index);
        EditorGUI.EndProperty();
    }
}