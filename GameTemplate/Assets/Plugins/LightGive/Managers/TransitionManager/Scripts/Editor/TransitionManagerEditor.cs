using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace LightGive
{
	[CustomEditor(typeof(TransitionManager))]
	public class TransitionManagerEditor : Editor
	{
		private Vector3 m_centerPosition;
		private Material m_previewMat;
		private float m_lerp = 0.5f;

		private SerializedProperty m_defaultFlashDurationProp;
		private SerializedProperty m_defaultFlashWhiteDurationProp;
		private SerializedProperty m_defaultFlashColorProp;

		private SerializedProperty m_defaultEffectTypeProp;
		private SerializedProperty m_defaultTransDurationProp;
		private SerializedProperty m_defaultEffectColorProp;
		private SerializedProperty m_ruleTexProp;
		private SerializedProperty m_transShaderProp;
		private SerializedProperty m_animCurveProp;
		private SerializedProperty m_isInvertProp;

		private bool m_isCustom
		{
			get
			{
				if (m_defaultEffectTypeProp == null)
					return false;
				return m_defaultEffectTypeProp.enumValueIndex == (int)TransitionManager.EffectType.Custom;
			}
		}

		private void OnEnable()
		{
			//SerializedProperty取得
			m_defaultFlashDurationProp = serializedObject.FindProperty("m_defaultFlashDuration");
			m_defaultFlashWhiteDurationProp = serializedObject.FindProperty("m_defaultFlashWhiteDuration");
			m_defaultFlashColorProp = serializedObject.FindProperty("m_defaultFlashColor");

			m_defaultEffectTypeProp = serializedObject.FindProperty("m_defaultEffectType");
			m_defaultTransDurationProp = serializedObject.FindProperty("m_defaultTransDuration");
			m_defaultEffectColorProp = serializedObject.FindProperty("m_defaultEffectColor");
			m_ruleTexProp = serializedObject.FindProperty("m_ruleTex");
			m_transShaderProp = serializedObject.FindProperty("m_transShader");
			m_animCurveProp = serializedObject.FindProperty("m_animCurve");
			m_isInvertProp = serializedObject.FindProperty("m_isInvert");

			var transShader = Shader.Find(TransitionManager.TransitionShaderName);
			serializedObject.Update();
			m_transShaderProp.objectReferenceValue = transShader;

			m_previewMat = new Material((Shader)transShader);
			if ((Texture)m_ruleTexProp.objectReferenceValue != null)
			{
				SetMaterialParamAll();
			}
			serializedObject.ApplyModifiedProperties();
		}


		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Flash Default Parameter", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(m_defaultFlashDurationProp, new GUIContent("Duration"));
			EditorGUILayout.PropertyField(m_defaultFlashWhiteDurationProp, new GUIContent("Flash Duration"));
			EditorGUILayout.PropertyField(m_defaultFlashColorProp, new GUIContent("Flash Color"));

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Transition Default Parameter", EditorStyles.boldLabel);
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(m_defaultEffectTypeProp, new GUIContent("TransitionType"));
			if (EditorGUI.EndChangeCheck())
			{
				SetMaterialParamAll();
			}

			EditorGUILayout.PropertyField(m_defaultTransDurationProp, new GUIContent("Duration"));
			EditorGUILayout.PropertyField(m_animCurveProp, new GUIContent("AnimationCurve"));

			//ChangeColor
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(m_defaultEffectColorProp, new GUIContent("TextureColor"));
			if (EditorGUI.EndChangeCheck())
			{
				m_previewMat.SetColor(TransitionManager.ShaderParamColor, m_defaultEffectColorProp.colorValue);
			}

			//TransitionTypeがCustomになっている時
			//if (m_isCustom)
			//{
				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Custom Setting", EditorStyles.boldLabel);

				if (m_transShaderProp.objectReferenceValue == null)
				{
					EditorGUILayout.HelpBox("Please add a 'LightGive/Unlit/TransitionShader'shader to the project", MessageType.Error);
				}
				else
				{
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.PropertyField(m_transShaderProp);
					EditorGUI.EndDisabledGroup();

					//ルール画像の変更をチェック***
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(m_ruleTexProp);
					if (EditorGUI.EndChangeCheck())
					{
						SetMaterialParamAll();
					}

					//反転のトグルをチェック***
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(m_isInvertProp);
					if (EditorGUI.EndChangeCheck())
					{
						m_previewMat.SetFloat(TransitionManager.ShaderParamFloatInvert, m_isInvertProp.boolValue ? 1.0f : 0.0f);
					}

					EditorGUILayout.Space();
					EditorGUILayout.LabelField("All default parameter Preview");

					//ルール画像が設定されているかで処理を分ける
					if ((Texture)m_ruleTexProp.objectReferenceValue != null)
					{
						//ルール画像が設定されている時
						//スライダーの変更をチェック
						EditorGUI.BeginChangeCheck();
						m_lerp = EditorGUILayout.Slider(m_lerp, 0.0f, 1.0f);
						var val = Mathf.Clamp01(m_animCurveProp.animationCurveValue.Evaluate(m_lerp));

						if (EditorGUI.EndChangeCheck())
						{
							m_previewMat.SetFloat(TransitionManager.ShaderParamFloatCutoff, val);
						}

						float contextWidth = (float)typeof(EditorGUIUtility).GetProperty("contextWidth", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, null);
						var w = contextWidth - 30f;

						var sizes = UnityStats.screenRes.Split('x');


						var h = w * (float.Parse(sizes[1]) / float.Parse(sizes[0]));
						GUILayout.Box(GUIContent.none, GUILayout.Width(w), GUILayout.Height(h));
						var lastRect = GUILayoutUtility.GetLastRect();
						lastRect.width -= 4;
						lastRect.height -= 4;
						lastRect.x += 2;
						lastRect.y += 2;
						EditorGUI.DrawPreviewTexture(lastRect, Texture2D.whiteTexture, m_previewMat);
					}
					else
					{
						EditorGUILayout.HelpBox("Please set rule texture.", MessageType.Info);
					}
			//	}
			}
			serializedObject.ApplyModifiedProperties();

			EditorGUILayout.Space();
			EditorGUILayout.Space();
			base.OnInspectorGUI();
		}

		void SetMaterialParamAll()
		{
			m_previewMat.SetTexture(TransitionManager.ShaderParamTextureGradation, (Texture)m_ruleTexProp.objectReferenceValue);
			m_previewMat.SetColor(TransitionManager.ShaderParamColor, m_defaultEffectColorProp.colorValue);
			m_previewMat.SetFloat(TransitionManager.ShaderParamFloatInvert, m_isInvertProp.boolValue ? 1.0f : 0.0f);
			m_previewMat.SetFloat(TransitionManager.ShaderParamFloatCutoff, m_lerp);

		}
	}
}