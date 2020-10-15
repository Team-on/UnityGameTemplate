/*Script created by Pierre Stempin*/

using UnityEngine;
using UnityEditor;

namespace EmptyAtZeroCreator
{
	public class EmptyAtZero_Creator 
	{
		const string _Space = EmptyCreator._Space;

		const string featureName = EmptyCreator.CreateEmpty_ + EmptyCreator.At + _Space + EmptyCreator.Zero;
		const string pathName = EmptyCreator._GameObject + EmptyCreator.Slash + featureName + _Space + shortcutName;
		const string shortcutName = EmptyCreator.AltSymbol + EmptyCreator.ShortcutLetter;

		[MenuItem (pathName, false, -1)]
#if UNITY_4_6 || UNITY_4_7 || UNITY_5 || UNITY_2017_1_OR_NEWER
        public static void CreateEmptyAtZero (MenuCommand menuCommand)
		{
            EmptyCreator.CreateEmptyGameObject (featureName, true, false, menuCommand);
        }
#else
        public static void CreateEmptyAtZero ()
		{
            EmptyCreator.CreateEmptyGameObject (featureName, true, false);
		}
#endif
	}
}
