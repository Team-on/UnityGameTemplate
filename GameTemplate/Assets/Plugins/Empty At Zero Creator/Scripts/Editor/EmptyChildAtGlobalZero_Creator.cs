/*Script created by Pierre Stempin*/

using UnityEngine;
using UnityEditor;

namespace EmptyAtZeroCreator 
{
	public class EmptyChildAtGlobalZero_Creator 
	{
		const string _Space = EmptyCreator._Space;
		const string Slash = EmptyCreator.Slash;

		const string _global = "Global";
		const string featureName = EmptyCreator.CreateEmptyChildAt_ + _global + _Space + EmptyCreator.Zero;
		const string pathName = EmptyCreator._GameObject + EmptyCreator.Slash + featureName + _Space + shortcutName;
		const string shortcutName = EmptyCreator.ControlSymbol + EmptyCreator.AltSymbol + EmptyCreator.ShortcutLetter;

		[MenuItem (pathName, false)]
#if UNITY_4_6 || UNITY_4_7 || UNITY_5 || UNITY_2017_1_OR_NEWER
        public static void CreateEmptyChildAtGlobalZero (MenuCommand menuCommand)
		{
			EmptyCreator.CreateEmptyGameObject (featureName, false, false, menuCommand);
		}
#else
        public static void CreateEmptyChildAtGlobalZero () 
        {
            EmptyCreator.CreateEmptyGameObject (featureName, false, false);
        }
#endif
    }
}
