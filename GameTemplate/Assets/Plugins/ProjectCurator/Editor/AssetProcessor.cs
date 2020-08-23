using UnityEditor;
using System.Collections.Generic;
using System;

namespace Nanolabo
{
    /// <summary>
    /// The purpose of this class is to try detecting asset changes to automatically update the ProjectCurator database.
    /// </summary>
    public class AssetProcessor : UnityEditor.AssetModificationProcessor
    {
        [InitializeOnLoadMethod]
        public static void Init()
        {
            EditorApplication.update += OnUpdate;
        }

        /// <summary>
        /// Some callbacks must be delayed on next frame
        /// </summary>
        private static void OnUpdate()
        {
            while (Actions.Count > 0) {
                Actions.Dequeue()?.Invoke();
            }
        }

        private static Queue<Action> Actions = new Queue<Action>();

        static string[] OnWillSaveAssets(string[] paths)
        {
            if (ProjectCuratorData.IsUpToDate) {
                Actions.Enqueue(() => {
                    foreach (string path in paths) {
                        ProjectCurator.RemoveAssetFromDatabase(path);
                        ProjectCurator.AddAssetToDatabase(path);
                    }
                    ProjectCurator.SaveDatabase();
                });
            }
            return paths;
        }

        static void OnWillCreateAsset(string assetName)
        {
            if (ProjectCuratorData.IsUpToDate) {
                Actions.Enqueue(() => {
                    ProjectCurator.AddAssetToDatabase(assetName);
                    ProjectCurator.SaveDatabase();
                });
            }
        }

        static AssetDeleteResult OnWillDeleteAsset(string assetName, RemoveAssetOptions removeAssetOptions)
        {
            if (ProjectCuratorData.IsUpToDate) {
                ProjectCurator.RemoveAssetFromDatabase(assetName);
                ProjectCurator.SaveDatabase();
            }
            return AssetDeleteResult.DidNotDelete;
        }

        static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            if (ProjectCuratorData.IsUpToDate) {
                Actions.Enqueue(() => {
                    ProjectCurator.RemoveAssetFromDatabase(sourcePath);
                    ProjectCurator.AddAssetToDatabase(destinationPath);
                    ProjectCurator.SaveDatabase();
                });
            }
            return AssetMoveResult.DidNotMove;
        }
    }
}