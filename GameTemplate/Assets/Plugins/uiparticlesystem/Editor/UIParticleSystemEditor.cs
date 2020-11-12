using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(UIParticleSystem))]
public class UIParticleSystemEditor : Editor
{
    SerializedProperty _materialProp;
    SerializedProperty _colorProp;
    SerializedProperty _raycastTargetProp;
    SerializedProperty _onCullStateChangedProp;

    SerializedProperty _particleTextureProp;

    SerializedProperty _sortModeProp;
    SerializedProperty _renderModeProp;
    SerializedProperty _speedScaleProp;
    SerializedProperty _lengthScaleProp;
    
    SerializedProperty _pivotProp;

    SerializedProperty _materialDirtyProp;
    SerializedProperty _scaleShapeProp;

    private void OnEnable()
    {
        _materialProp = serializedObject.FindProperty("m_Material");
        _colorProp = serializedObject.FindProperty("m_Color");
        _raycastTargetProp = serializedObject.FindProperty("m_RaycastTarget");
        _onCullStateChangedProp = serializedObject.FindProperty("m_OnCullStateChanged");

        _particleTextureProp = serializedObject.FindProperty("ParticleImage");
        _sortModeProp = serializedObject.FindProperty("SortMode");

        _renderModeProp = serializedObject.FindProperty("RenderMode");
        _speedScaleProp = serializedObject.FindProperty("SpeedScale");
        _lengthScaleProp = serializedObject.FindProperty("LengthScale");

        _pivotProp = serializedObject.FindProperty("Pivot");
        _scaleShapeProp = serializedObject.FindProperty("ScaleShapeByRectTransform");
        _materialDirtyProp = serializedObject.FindProperty("UpdateMaterialDirty");
    }

    public override void OnInspectorGUI()
    {
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((UIParticleSystem)target), typeof(UIParticleSystem), false);
        GUI.enabled = true;

        serializedObject.Update();

        EditorGUILayout.PropertyField(_renderModeProp);
        var renderMode = (UIParticleSystemRenderMode)_renderModeProp.enumValueIndex;
        if (renderMode == UIParticleSystemRenderMode.StretchedBillboard)
        {
            EditorGUILayout.PropertyField(_speedScaleProp);
            EditorGUILayout.PropertyField(_lengthScaleProp);
        }
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(_sortModeProp);
        EditorGUILayout.PropertyField(_pivotProp);
        EditorGUILayout.PropertyField(_materialDirtyProp);
        EditorGUILayout.PropertyField(_scaleShapeProp);

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(_particleTextureProp);        
        EditorGUILayout.PropertyField(_materialProp);
        EditorGUILayout.PropertyField(_colorProp);
        EditorGUILayout.PropertyField(_raycastTargetProp);
        EditorGUILayout.PropertyField(_onCullStateChangedProp);
        

        serializedObject.ApplyModifiedProperties();
    }
}
