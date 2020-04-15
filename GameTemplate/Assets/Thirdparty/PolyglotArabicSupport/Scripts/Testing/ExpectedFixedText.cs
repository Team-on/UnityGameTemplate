using UnityEngine;
using ArabicSupport;

public class ExpectedFixedText : MonoBehaviour
{
	[TextArea]
	public string Unfixed;

	[TextArea]
	public string Expected;

	public string Fixed { get; private set; }

	public bool ShowTashkeel = false;
	public bool UseHinduNumbers = true;

	public void Fix()
	{
		Fixed = ArabicFixer.Fix(Unfixed, ShowTashkeel, UseHinduNumbers);
	}
}
