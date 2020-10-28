using UnityEditor;
using UnityEditorInternal;

namespace LightGive
{
	[CustomEditor(typeof(ManagerCreator))]
	public class ManagerCreatorEditor : Editor
	{
		private SerializedProperty m_isCheckLogProp;
		private SerializedProperty m_createManagersProp;
		private ReorderableList m_reorderableListcreateManager;

		private void OnEnable()
		{
			m_isCheckLogProp = serializedObject.FindProperty("m_isCheckLog");
			m_createManagersProp = serializedObject.FindProperty("m_createManagers");
			m_reorderableListcreateManager = new ReorderableList(serializedObject, m_createManagersProp);
		}

		public override void OnInspectorGUI()
		{
			//確認用
			//base.DrawDefaultInspector();

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Don't Destroy this file.", EditorStyles.boldLabel);
			EditorGUILayout.Space();
			serializedObject.Update();


			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(m_isCheckLogProp);
			EditorGUILayout.Space();

			m_reorderableListcreateManager.DoLayoutList();
			m_reorderableListcreateManager.drawElementCallback = (rect, index, isActive, isFocused) =>
			{
				var element = m_createManagersProp.GetArrayElementAtIndex(index);
				rect.height -= 4;
				rect.y += 2;
				EditorGUI.PropertyField(rect, element);
			};

			serializedObject.ApplyModifiedProperties();
		}
	}
}