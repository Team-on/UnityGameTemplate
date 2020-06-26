#if UNITY_2018_1_OR_NEWER

using System;
using System.Reflection;
using UnityEditor;

namespace PumpEditor
{
    public class UnityProjectTemplateWindowMenuItem
    {
        [MenuItem("Window/Pump Editor/Project Templates/Unity Project Template Window")]
        private static void ShowWindow()
        {
            // Unity C# reference: https://github.com/Unity-Technologies/UnityCsReference/blob/73bda32d/Editor/Mono/ProjectTemplateWindow.cs#L11
            Assembly editorAssembly = Assembly.GetAssembly(typeof(Editor));
            Type projectTemplateWindowType = editorAssembly.GetType("UnityEditor.ProjectTemplateWindow");

            // Unity C# reference: https://github.com/Unity-Technologies/UnityCsReference/blob/73bda32d/Editor/Mono/ProjectTemplateWindow.cs#L20
            MethodInfo methodInfo = projectTemplateWindowType.GetMethod("SaveAsTemplate", BindingFlags.Static | BindingFlags.NonPublic);
            methodInfo.Invoke(projectTemplateWindowType, null);
        }
    }
}

#endif
