using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildManagerSettings : ScriptableObject{
	public List<BuildSequence> sequences = new List<BuildSequence>() { new BuildSequence() };

	public string scriptingDefineSymbols;
}
