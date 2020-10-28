using UnityEngine;

namespace LightGive
{
	[CreateAssetMenu(menuName = CreatorPath, fileName = CreatorName)]
	public class ManagerCreator : ScriptableObject
	{
		public const string CreatorName = "ManagerCreator";
		public const string CreatorPath = "LightGive/Create ManagerCreator";

		/// <summary>
		/// マネージャークラスを生成した時にログを出すかどうか
		/// </summary>
		[SerializeField, Tooltip("Whether to issue a log when generated")]
		private bool m_isCheckLog = true;
		[SerializeField]
		private GameObject[] m_createManagers;
		public GameObject[] createManagers { get { return m_createManagers; } }

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InitializeBeforeSceneLoad()
		{
			var managerCreator = Resources.Load<ManagerCreator>("ManagerCreator");
			if (managerCreator == null)
			{
				Debug.Log("Manager Creator does not exist.\nIn project view Create/" + ManagerCreator.CreatorPath + " from generate.");
				return;
			}

			string objectNames = "";
			for (int i = 0; i < managerCreator.createManagers.Length; i++)
			{
				if (managerCreator.createManagers[i] == null)
					continue;
				var obj = Instantiate(managerCreator.createManagers[i]);
				objectNames += "\n" + (i + 1).ToString("0") + "." + obj.name + ",";
			}

			if (managerCreator.m_isCheckLog)
				Debug.Log("Create manager class complete." + objectNames);
		}
	}
}