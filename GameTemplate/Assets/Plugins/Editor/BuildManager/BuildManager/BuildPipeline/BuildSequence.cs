using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[Serializable]
public class BuildSequence : ICloneable {
	public bool isEnabled;

	public string editorName;
	public string itchGameLink;

	public string scriptingDefineSymbolsOverride;

	public List<BuildData> builds = new List<BuildData>() { new BuildData() };

	public BuildSequence() : this("New build sequence", $"teamon/gametemplate", new BuildData()) {
	}

	public BuildSequence(string editorName, string itchGameLink, params BuildData[] builds) {
		this.editorName = editorName;
		this.itchGameLink = itchGameLink;
		this.builds = new List<BuildData>(builds);

		scriptingDefineSymbolsOverride = "";
		
		isEnabled = true;
	}

	public object Clone() {
		BuildSequence sequence = this.MemberwiseClone() as BuildSequence;

		sequence.builds = new List<BuildData>(builds.Count);
		for (int i = 0; i < builds.Count; ++i) {
			sequence.builds.Add(builds[i].Clone() as BuildData);
		}

		return sequence;
	}
}
