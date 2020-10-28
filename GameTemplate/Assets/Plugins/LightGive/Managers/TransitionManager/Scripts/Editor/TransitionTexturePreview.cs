//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomPreview(typeof(TransitionManager))]
//public class TransitionTexturePreview : ObjectPreview
//{
//	private GUIContent m_previewTitle = new GUIContent("TransitionPreview");
//	private Material m_mat;
//	private float m_lerp;


//	public override bool HasPreviewGUI()
//	{
//		return true;
//	}

//	public override GUIContent GetPreviewTitle()
//	{
//		return m_previewTitle;
//	}

//	public override void OnPreviewSettings()
//	{
//		//base.OnPreviewSettings();


//		m_mat = new Material(Shader.Find(TransitionManager.TransitionShaderName));
//		var preSlider = new GUIStyle("preSlider");
//		var preSliderThumb = new GUIStyle("preSliderThumb");
//		var preLabel = new GUIStyle("preLabel");

//		m_lerp = GUILayout.HorizontalSlider(m_lerp, 0.0f, 1.0f, preSlider, preSliderThumb, GUILayout.Width(150));
//		GUILayout.Label((m_lerp * 100).ToString("0") + "%", preLabel, GUILayout.Width(40));

//	}

//	public override void OnPreviewGUI(Rect r, GUIStyle background)
//	{
//		TransitionManager m_target = (TransitionManager)target;

//		GUI.Slider(r, 1, 1, 0, 1, background, background, true, 0);
//		GUI.DrawTexture(r, m_target.ruleTex, ScaleMode.ScaleToFit);

//		//GUI.Box(r, "表示領域");

//	}
//}
