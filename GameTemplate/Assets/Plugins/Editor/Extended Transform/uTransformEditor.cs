using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects, CustomEditor(typeof(Transform))]
public class uTransformEditor : Editor {

    private const float FIELD_WIDTH = 250; //controls the width of the input fields
    //private const bool WIDE_MODE = true; //makes our controls draw inline

    private const float POSITION_MAX = 100000.0f;

    //private static GUIContent positionGUIContent = new GUIContent(LocalString("Position"), LocalString("The local position of this Game Object relative to the parent."));
    private static GUIContent rotationGUIContent = new GUIContent(LocalString("Rotation"), LocalString("The local rotation of this Game Object relative to the parent."));
    //private static GUIContent scaleGUIContent = new GUIContent(LocalString("Scale"), LocalString("The local scaling of this Game Object relative to the parent."));

    private static string positionWarningText = LocalString("Due to floating-point precision limitations, it is recommended to bring the world coordinates of the GameObject within a smaller range.");

    private SerializedProperty positionProperty; //The position of this transform
    private SerializedProperty rotationProperty; //The rotation of this transform
    private SerializedProperty scaleProperty; //The scale of this transform

    //References to some images for our GUI
    private static Texture2D icon_revert;
    private static Texture2D icon_locked;
    private static Texture2D icon_unlocked;

    public static bool UniformScaling = false; //Are we using uniform scaling mode?

    private static bool SHOW_UTILS = false; //Should we show the utilities section?



    #region INITIALISATION

    public void OnEnable()
    {
        this.positionProperty = this.serializedObject.FindProperty("m_LocalPosition");
        this.rotationProperty = this.serializedObject.FindProperty("m_LocalRotation");
        this.scaleProperty = this.serializedObject.FindProperty("m_LocalScale");
        icon_revert = EditorGUIUtility.isProSkin ? Resources.Load("uEditor_Revert_pro") as Texture2D : Resources.Load("uEditor_Revert") as Texture2D;
        icon_locked = Resources.Load("uEditor_locked") as Texture2D;
        icon_unlocked = Resources.Load("uEditor_unlocked") as Texture2D;
        EditorApplication.update += EditorUpdate;
    }

    private void OnDisable()
    {
        EditorApplication.update -= EditorUpdate;
    }

    private void EditorUpdate()
    {
        Repaint();
    }
    #endregion

    /// <summary>
    /// Draws the inspector
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.serializedObject.Update();
        //Draw the inputs
        DrawPositionElement();
        DrawRotationElement();
        DrawScaleElement();

        //Draw the Utilities
        SHOW_UTILS = uEditorUtils.DrawHeader("Transform Utilities");
        if (SHOW_UTILS)
            DrawUtilities();
        //Validate the transform of this object
        if (!ValidatePosition(((Transform)this.target).position))
        {
            EditorGUILayout.HelpBox(positionWarningText, MessageType.Warning);
        }
        //Apply the settings to the object
        this.serializedObject.ApplyModifiedProperties();
        EditorGUIUtility.labelWidth = 0;
    }

    /// <summary>
    /// Draws the input for the position
    /// </summary>
    private void DrawPositionElement()
    {
        if (ThinInspectorMode)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Position");
            DrawPositionReset();
            GUILayout.EndHorizontal();
        }

        string label = ThinInspectorMode ? "" : "Position";

        GUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - uTransformEditor.FIELD_WIDTH - 64; // align field to right of inspector
        this.positionProperty.vector3Value = uEditorUtils.Vector3InputField(label, this.positionProperty.vector3Value);
        if (!ThinInspectorMode)
            DrawPositionReset();
        GUILayout.EndHorizontal();
        EditorGUIUtility.labelWidth = 0;
    }
    private void DrawPositionReset()
    {
        GUILayout.Space(18);
        if (GUILayout.Button(new GUIContent("", icon_revert, "Reset this objects position"), uEditorUtils.uEditorSkin.GetStyle("ResetButton"), GUILayout.Width(18), GUILayout.Height(18)))
        {
            this.positionProperty.vector3Value = Vector3.zero;
        }
    }

    /// <summary>
    /// Draws the input for the rotation
    /// </summary>
    private void DrawRotationElement()
    {
        if (ThinInspectorMode)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Rotation");
            DrawRotationReset();
            GUILayout.EndHorizontal();
        }

        //Rotation layout
        GUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - uTransformEditor.FIELD_WIDTH - 64; // align field to right of inspector
        this.RotationPropertyField(this.rotationProperty, ThinInspectorMode ? GUIContent.none : rotationGUIContent);
        if (!ThinInspectorMode)
            DrawRotationReset();
        GUILayout.EndHorizontal();
        EditorGUIUtility.labelWidth = 0;
    }
    private void DrawRotationReset()
    {
        GUILayout.Space(18);
        if (GUILayout.Button(new GUIContent("", icon_revert, "Reset this objects rotation"), uEditorUtils.uEditorSkin.GetStyle("ResetButton"), GUILayout.Width(18), GUILayout.Height(18)))
        {
            this.rotationProperty.quaternionValue = Quaternion.identity;
        }
    }

    /// <summary>
    /// Draws the input for the scale
    /// </summary>
    private void DrawScaleElement()
    {
        if (ThinInspectorMode)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Scale");
            DrawScaleReset();
            GUILayout.EndHorizontal();
        }
        string label = ThinInspectorMode ? "" : "Scale";

        //Scale Layout
        GUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - uTransformEditor.FIELD_WIDTH - 64; // align field to right of inspector
        this.scaleProperty.vector3Value = uEditorUtils.Vector3InputField(label, this.scaleProperty.vector3Value, false, UniformScaling, UniformScaling);
        if (!ThinInspectorMode)
            DrawScaleReset();
        GUILayout.EndHorizontal();
        EditorGUIUtility.labelWidth = 0;
    }
    private void DrawScaleReset()
    {
        if (GUILayout.Button(new GUIContent("", (UniformScaling ? icon_locked : icon_unlocked), (UniformScaling ? "Unlock Scale" : "Lock Scale")), uEditorUtils.uEditorSkin.GetStyle("ResetButton"), GUILayout.Width(18), GUILayout.Height(18)))
        {
            UniformScaling = !UniformScaling;
        }
        if (GUILayout.Button(new GUIContent("", icon_revert, "Reset this objects scale"), uEditorUtils.uEditorSkin.GetStyle("ResetButton"), GUILayout.Width(18), GUILayout.Height(18)))
        {
            this.scaleProperty.vector3Value = Vector3.one;
        }
    }

    #region UTILITIES
    private static float snap_offset = 0f;
    private static Vector3 minRotation;
    private static Vector3 maxRotation = new Vector3(360,360,360);

    private void DrawUtilities()
    {
        GUILayout.Space(5);
        //Snap to ground
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Snap To Ground", uEditorUtils.uEditorSkin.button, GUILayout.Width(ThinInspectorMode ? 100 : 160)))
        {
            foreach (var tar in this.targets)
            {
                Transform t = (Transform)tar;
                Undo.RecordObject(t, "Snap to Ground");
                t.TransformSnapToGround(snap_offset);
            }
        }
        EditorGUIUtility.labelWidth = 50f;
        snap_offset = EditorGUILayout.FloatField("Offset", snap_offset);
        GUILayout.EndHorizontal();


        //Random rotation
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Random Rotation", uEditorUtils.uEditorSkin.button, GUILayout.Width(ThinInspectorMode ? 100 : 160), GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
        {
            foreach (var tar in this.targets)
            {
                Transform t = (Transform)tar;
                Undo.RecordObject(t, "Random Rotation");
                t.RandomiseRotation(minRotation, maxRotation);
            }
        }

        GUILayout.BeginVertical();
        minRotation = EditorGUILayout.Vector3Field(ThinInspectorMode ? "" : "Min", minRotation);
        maxRotation = EditorGUILayout.Vector3Field(ThinInspectorMode ? "" : "Max", maxRotation);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        EditorGUIUtility.labelWidth = 0;

    }
    #endregion

    /// <summary>
    /// Returns the localised version of a string
    /// </summary>
    private static string LocalString(string text)
    {
        return text;
        //return LocalizationDatabase.GetLocalizedString(text);
    }

    private static bool ThinInspectorMode
    {

        get
        {
            return EditorGUIUtility.currentViewWidth <= 300;
        }

    }

    private bool ValidatePosition(Vector3 position)
    {
        if (Mathf.Abs(position.x) > uTransformEditor.POSITION_MAX) return false;
        if (Mathf.Abs(position.y) > uTransformEditor.POSITION_MAX) return false;
        if (Mathf.Abs(position.z) > uTransformEditor.POSITION_MAX) return false;
        return true;
    }

    private void RotationPropertyField(SerializedProperty rotationProperty, GUIContent content)
    {
        Transform transform = (Transform)this.targets[0];
        Quaternion localRotation = transform.localRotation;
        foreach (UnityEngine.Object t in (UnityEngine.Object[])this.targets)
        {
            if (!SameRotation(localRotation, ((Transform)t).localRotation))
            {
                EditorGUI.showMixedValue = true;
                break;
            }
        }

        EditorGUI.BeginChangeCheck();

        Vector3 eulerAngles = uEditorUtils.Vector3InputField(content.text, localRotation.eulerAngles);
        //Vector3 eulerAngles = EditorGUILayout.Vector3Field(content, localRotation.eulerAngles);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObjects(this.targets, "Rotation Changed");
            foreach (UnityEngine.Object obj in this.targets)
            {
                Transform t = (Transform)obj;
                t.localEulerAngles = eulerAngles;
            }
            rotationProperty.serializedObject.SetIsDifferentCacheDirty();
        }

        EditorGUI.showMixedValue = false;
    }

    private bool SameRotation(Quaternion rot1, Quaternion rot2)
    {
        if (rot1.x != rot2.x) return false;
        if (rot1.y != rot2.y) return false;
        if (rot1.z != rot2.z) return false;
        if (rot1.w != rot2.w) return false;
        return true;
    }
}
