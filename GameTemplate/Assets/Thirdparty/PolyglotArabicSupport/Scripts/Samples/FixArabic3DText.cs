using UnityEngine;
using System.Collections;
using ArabicSupport;

public class FixArabic3DText : MonoBehaviour {

    public bool showTashkeel = true;
    public bool useHinduNumbers = true;

    // Use this for initialization
    void Start () {
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();

        string fixedText = ArabicFixer.Fix(textMesh.text, showTashkeel, useHinduNumbers);

        gameObject.GetComponent<TextMesh>().text = fixedText;

		Debug.Log(fixedText);
    }

}
