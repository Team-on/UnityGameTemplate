/*Script created by Pierre Stempin*/

using UnityEngine;
using UnityEditor;

namespace EmptyAtZeroCreator 
{
	public class EmptyCreator 
	{
		public const string _GameObject = "GameObject";
		public const string _Tools = "Tools";

		public const string _Space = " ";
		public const string Slash = "/";

		public const string CreateEmpty_ = create + _Space + empty + _Space;
		public const string EmptyAtZeroCreator = empty + _Space + At + _Space + Zero + _Space + creator;
		public const string CreateEmptyChildAt_ = CreateEmpty_ + Child + _Space + At + _Space;

		const string create = "Create";
		const string empty = "Empty";
		const string creator = "Creator";
		const string Child = "Child";

		public const string At = "At";
		public const string Zero = "Zero";

		public const string ShortcutLetter = "N";
		public const string ControlSymbol = "%";
		public const string ShiftSymbol = "#";
		public const string AltSymbol = "&";

#if UNITY_4_6 || UNITY_4_7 || UNITY_5 || UNITY_2017_1_OR_NEWER
        public static void CreateEmptyGameObject (string _featureName, bool hasToDeselect, bool hasToResetLocalValues, MenuCommand menuCommand)
#else
        public static void CreateEmptyGameObject (string _featureName, bool hasToDeselect, bool hasToResetLocalValues) 
#endif
        {
            if (hasToDeselect)
			{
				//Reset selection
				Selection.activeGameObject = null;
			}

			//Create the new empty gameObject
			string gameObjectName = _GameObject;
			GameObject spawnedGameObject = new GameObject (gameObjectName);


			if (hasToDeselect)
			{
#if UNITY_4_6 || UNITY_4_7 || UNITY_5 || UNITY_2017_1_OR_NEWER
                GameObjectUtility.SetParentAndAlign (spawnedGameObject, menuCommand.context as GameObject);
#endif
            }

            //Undo
            string undoMethodName = _featureName;
			Undo.RegisterCreatedObjectUndo (spawnedGameObject, undoMethodName);

			if (Selection.activeGameObject != null)
			{
				//Set parent
				spawnedGameObject.transform.parent = Selection.activeGameObject.transform;

				if (hasToResetLocalValues)
				{
					//Reset local values
					spawnedGameObject.transform.localPosition = Vector3.zero;
					spawnedGameObject.transform.localRotation = Quaternion.identity;
					spawnedGameObject.transform.localScale = Vector3.one;
				}
			}

			//Select the spawned gameObject
			Selection.activeGameObject = spawnedGameObject;

#if UNITY_4_6 || UNITY_4_7 || UNITY_5 || UNITY_2017_1_OR_NEWER
			//Add a RectTransform if needed
			if (spawnedGameObject.transform.parent != null)
			{
				RectTransform parentRectTransform = spawnedGameObject.transform.parent.GetComponent <RectTransform> ();
				
				if (parentRectTransform != null)
				{
					RectTransform rectTransform = spawnedGameObject.gameObject.AddComponent (typeof (RectTransform)) as RectTransform;
					rectTransform.anchorMin = Vector2.zero;
					rectTransform.anchorMax = Vector2.one;
					rectTransform.offsetMin = Vector2.zero;
					rectTransform.offsetMax = Vector2.zero;
				}
			}
#endif
		}
	}
}

