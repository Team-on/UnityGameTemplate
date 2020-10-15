using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace PrefabAssetUtility.Editor
{
    public static class PrefabUtils
    {
        private const string PREFAB_TO_GUID_PATH = "Library/PrefabToGUID.json";
        private const string GUID_TO_PREFAB_PATH = "Library/GUIDToPrefab.json";

        private const string PREFAB_TO_COMPONENT_PATH = "Library/PrefabToComponent.json";
        private const string COMPONENT_TO_PREFAB_PATH = "Library/ComponentToPrefab.json";

        private static string _basePath;

        private static Dictionary<string, HashSet<string>> _prefabToGUID = new Dictionary<string, HashSet<string>>();
        private static Dictionary<string, HashSet<string>> _GUIDToPrefab = new Dictionary<string, HashSet<string>>();

        private static Dictionary<string, HashSet<string>> _prefabToComponent =
            new Dictionary<string, HashSet<string>>();

        private static Dictionary<string, HashSet<string>> _componentToPrefab =
            new Dictionary<string, HashSet<string>>();

        private static readonly Regex _guidRegex = new Regex(@"guid: (.*?),",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex _componentRegex = new Regex("^(?=[a-zA-Z0-9_])(.*):$", RegexOptions.Compiled);

        [InitializeOnLoadMethod]
        private static void Init()
        {
            string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            _basePath = Path.GetFullPath(
                new Uri(Path.Combine(Path.GetDirectoryName(path) ?? "", "../../")).AbsolutePath);

            if (!EditorPrefs.GetBool(PrefabAssetSettings.LAZY_LOAD_PREFAB_CACHE, true))
                LoadCache();

            PrefabUtility.prefabInstanceUpdated += PrefabInstanceUpdated;
            EditorApplication.quitting += EditorApplicationOnQuitting;
            CompilationPipeline.compilationStarted += CompilationPipelineOnCompilationStarted;
        }

        private static void CompilationPipelineOnCompilationStarted(object obj)
        {
            SaveCache();

            PrefabUtility.prefabInstanceUpdated -= PrefabInstanceUpdated;
            EditorApplication.quitting -= EditorApplicationOnQuitting;
            CompilationPipeline.compilationStarted -= CompilationPipelineOnCompilationStarted;
        }

        private static void EditorApplicationOnQuitting()
        {
            SaveCache();
        }

        private static List<string> GetAllPrefabs()
        {
            return AssetDatabase.GetAllAssetPaths().Where(s => s.Contains(".prefab")).ToList();
        }

        private static void PrefabInstanceUpdated(GameObject instance)
        {
            string path = AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromSource(instance));
            ProcessCacheForAsset(path);

            if (EditorPrefs.GetBool(PrefabAssetSettings.STORE_CACHE_ON_PREFAB_CHANGE))
            {
                SaveCache();
            }
        }

        internal static void RefreshPrefabCache()
        {
            List<string> allPrefabs = GetAllPrefabs();

            _prefabToGUID.Clear();
            _GUIDToPrefab.Clear();

            _prefabToComponent.Clear();
            _componentToPrefab.Clear();

            int current = 0;
            int total = allPrefabs.Count;

            foreach (string asset in allPrefabs)
            {
                current++;
                EditorUtility.DisplayProgressBar("Processing prefabs", $"Processed {current} out of {total} prefabs",
                    current / (float) total);

                ProcessCacheForAsset(asset);
            }

            EditorUtility.ClearProgressBar();

            SaveCache();
        }

        private static void ProcessCacheForAsset(string asset)
        {
            string path = $"{_basePath}{asset}";
            HashSet<string> GUIDs = new HashSet<string>();
            HashSet<string> Components = new HashSet<string>();

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains("guid"))
                        {
                            GUIDs.Add(_guidRegex.Split(line)[1]);
                        }

                        if (_componentRegex.IsMatch(line))
                        {
                            Components.Add(line.Remove(line.Length - 1));
                        }
                    }

                    AddToLists(asset, Components, ref _prefabToComponent, ref _componentToPrefab);
                    AddToLists(asset, GUIDs, ref _prefabToGUID, ref _GUIDToPrefab);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        private static void AddToLists(string asset, HashSet<string> Components,
            ref Dictionary<string, HashSet<string>> _aToB, ref Dictionary<string, HashSet<string>> _bToA)
        {
            if (_aToB.ContainsKey(asset))
                _aToB[asset] = Components;
            else
                _aToB.Add(asset, Components);

            foreach (string component in Components)
            {
                if (_bToA.ContainsKey(component))
                    _bToA[component].Add(asset);
                else
                    _bToA.Add(component, new HashSet<string> {asset});
            }
        }

        private static bool IsCacheAvailable()
        {
            return File.Exists(_basePath + PREFAB_TO_GUID_PATH) &&
                   File.Exists(_basePath + GUID_TO_PREFAB_PATH) &&
                   File.Exists(_basePath + PREFAB_TO_COMPONENT_PATH) &&
                   File.Exists(_basePath + COMPONENT_TO_PREFAB_PATH);
        }

        private static bool IsCacheReady()
        {
            return _prefabToGUID.Count > 0 && 
                   _GUIDToPrefab.Count > 0 &&
                   _prefabToComponent.Count > 0 &&
                   _componentToPrefab.Count > 0;
        }

        private static void LoadCache()
        {
            if (IsCacheAvailable())
            {
                using (StreamReader reader = new StreamReader(_basePath + PREFAB_TO_GUID_PATH))
                {
                    _prefabToGUID =
                        JsonUtility.FromJson<Dictionary<string, HashSet<string>>>(reader.ReadToEnd());
                }

                using (StreamReader reader = new StreamReader(_basePath + GUID_TO_PREFAB_PATH))
                {
                    _GUIDToPrefab =
                        JsonUtility.FromJson<Dictionary<string, HashSet<string>>>(reader.ReadToEnd());
                }

                using (StreamReader reader = new StreamReader(_basePath + PREFAB_TO_COMPONENT_PATH))
                {
                    _prefabToComponent =
                         JsonUtility.FromJson<Dictionary<string, HashSet<string>>>(reader.ReadToEnd());
                }

                using (StreamReader reader = new StreamReader(_basePath + COMPONENT_TO_PREFAB_PATH))
                {
                    _componentToPrefab =
                         JsonUtility.FromJson<Dictionary<string, HashSet<string>>>(reader.ReadToEnd());
                }
            }
            else
            {
                RefreshPrefabCache();
            }
        }

        private static void SaveCache()
        {
            File.Delete(_basePath + PREFAB_TO_GUID_PATH);
            using (StreamWriter writer = new StreamWriter(Path.Combine(_basePath, PREFAB_TO_GUID_PATH)))
            {
                writer.Write(JsonUtility.ToJson(_prefabToGUID));
            }

            File.Delete(_basePath + GUID_TO_PREFAB_PATH);
            using (StreamWriter writer = new StreamWriter(Path.Combine(_basePath, GUID_TO_PREFAB_PATH)))
            {
                writer.Write(JsonUtility.ToJson(_GUIDToPrefab));
            }

            File.Delete(_basePath + PREFAB_TO_COMPONENT_PATH);
            using (StreamWriter writer = new StreamWriter(Path.Combine(_basePath, PREFAB_TO_COMPONENT_PATH)))
            {
                writer.Write(JsonUtility.ToJson(_prefabToComponent));
            }

            File.Delete(_basePath + COMPONENT_TO_PREFAB_PATH);
            using (StreamWriter writer = new StreamWriter(Path.Combine(_basePath, COMPONENT_TO_PREFAB_PATH)))
            {
                writer.Write(JsonUtility.ToJson(_componentToPrefab));
            }
        }

        /// <summary>
        /// Get the list of prefab paths that reference the given GUID
        /// </summary>
        /// <param name="GUID">GUID to check for</param>
        /// <returns>List of prefabs using the GUID</returns>
        public static List<string> GetPrefabsForGUID(string GUID)
        {
            if (!IsCacheReady())
                LoadCache();
            return _GUIDToPrefab.ContainsKey(GUID) ? _GUIDToPrefab[GUID].ToList() : new List<string>();
        }

        /// <summary>
        /// Get the list of GUIDs that the prefab uses
        /// </summary>
        /// <param name="prefabPath">The relative path to the prefab</param>
        /// <returns>List of GUIDs that this prefab uses</returns>
        public static List<string> GetGUIDsForPrefab(string prefabPath)
        {
            if (!IsCacheReady())
                LoadCache();
            return _prefabToGUID.ContainsKey(prefabPath) ? _prefabToGUID[prefabPath].ToList() : new List<string>();
        }

        /// <summary>
        /// Get the list of prefab paths that reference the given Component
        /// </summary>
        /// <typeparam name="T">Type to check for</typeparam>
        /// <returns>List of prefabs using the Component</returns>
        public static List<string> GetPrefabsWithComponent<T>()
        {
            return GetPrefabsWithComponent(typeof(T));
        }
        
        /// <summary>
        /// Get the list of prefab paths that reference the given Component
        /// </summary>
        /// <param name="type">Type to check for</param>
        /// <returns>List of prefabs using the Component</returns>
        public static List<string> GetPrefabsWithComponent(Type type)
        {
            if (!IsCacheReady())
                LoadCache();
            string name = type.Name;
            return _componentToPrefab.ContainsKey(name) ? _componentToPrefab[name].ToList() : new List<string>();
        }

        /// <summary>
        /// Get the list of Components that the prefab uses
        /// </summary>
        /// <param name="prefabPath">The relative path to the prefab</param>
        /// <returns>List of all Components attached to the prefab</returns>
        public static List<string> GetComponentsForPrefab(string prefabPath)
        {
            if (!IsCacheReady())
                LoadCache();
            return _prefabToComponent.ContainsKey(prefabPath)
                ? _prefabToComponent[prefabPath].ToList()
                : new List<string>();
        }
    }
}
