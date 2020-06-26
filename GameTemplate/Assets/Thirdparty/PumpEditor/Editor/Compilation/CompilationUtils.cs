using System;
using System.Reflection;
using UnityEditor;

namespace PumpEditor
{
    public static class CompilationUtils
    {
        // Request script compilation the way Unity 2019.3 does via calling EditorCompilation.DirtyAllScripts
        // Unity C# reference: https://github.com/Unity-Technologies/UnityCsReference/blob/2019.3/Editor/Mono/Scripting/ScriptCompilation/CompilationPipeline.cs#L512
        public static void RequestScriptCompilationViaReflection()
        {
            Assembly editorAssembly = Assembly.GetAssembly(typeof(Editor));
            // Unity C# reference: https://github.com/Unity-Technologies/UnityCsReference/blob/2017.1/Editor/Mono/Scripting/ScriptCompilation/EditorCompilationInterface.cs#L12
            Type editorCompilationInterfaceType = editorAssembly.GetType("UnityEditor.Scripting.ScriptCompilation.EditorCompilationInterface");
            // Unity C# reference: https://github.com/Unity-Technologies/UnityCsReference/blob/2017.1/Editor/Mono/Scripting/ScriptCompilation/EditorCompilationInterface.cs#L40
            MethodInfo dirtyAllScriptsMethod = editorCompilationInterfaceType.GetMethod("DirtyAllScripts", BindingFlags.Static | BindingFlags.Public);
            dirtyAllScriptsMethod.Invoke(editorCompilationInterfaceType, null);
        }
    }
}
