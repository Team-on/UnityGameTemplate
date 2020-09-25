/*Script created by Pierre Stempin*/

using UnityEngine;
using UnityEditor;

namespace EmptyAtZeroCreator 
{
	public class EmptyChildAtLocalZero_Creator 
	{
		const string _Space = EmptyCreator._Space;
		const string Slash = EmptyCreator.Slash;

		const string _local = "Local";
		const string featureName = EmptyCreator.CreateEmptyChildAt_ + _local + _Space + EmptyCreator.Zero;
		const string pathName = EmptyCreator._GameObject + EmptyCreator.Slash + featureName + _Space + shortcutName;
		const string shortcutName = EmptyCreator.ControlSymbol + EmptyCreator.AltSymbol + EmptyCreator.ShiftSymbol + EmptyCreator.ShortcutLetter;

        [MenuItem (pathName, false)]
#if UNITY_4_6 || UNITY_4_7 || UNITY_5 || UNITY_2017_1_OR_NEWER
        public static void CreateEmptyChildAtLocalZero (MenuCommand menuCommand)
		{
			EmptyCreator.CreateEmptyGameObject (featureName, false, true, menuCommand);
		}
#else
        public static void CreateEmptyChildAtLocalZero () 
        {
            EmptyCreator.CreateEmptyGameObject (featureName, false, true);
        }
#endif
    }
}