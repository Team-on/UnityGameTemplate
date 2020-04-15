using UnityEngine;
using System.Collections;
using ArabicSupport;

public class SetArabicTextExample : MonoBehaviour {

	[TextArea]
	public string text;

	public bool ShowTashkeel = false;
	public bool UseHinduNumbers = false;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<UnityEngine.UI.Text>().text = "This sentence (wrong display):\n" + text +
			"\n\nWill appear correctly as:\n" + ArabicFixer.Fix(text, ShowTashkeel, UseHinduNumbers);
	}

	// Update is called once per frame
	void Update () {

	}
}
