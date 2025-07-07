using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

using System.IO;
using Gpm.Common.ThirdParty.LitJson;



namespace Gpm.AssetManagement.AssetMap.Internal
{
    using Gpm.AssetManagement.Const;

    public class AssetMapCache
    {
        public Dictionary<string, AssetMapData> assetDataDictionary;

        public List<string> hasMissingAsset;

        public Dictionary<string, List<string>> knownMissingGuid;

        public List<string> unKnownMissingGuid;

        public bool bDirty = false;


        public AssetMapCache()
        {
        }

        public AssetMapCache(AssetMapCache source)
        {
            this.assetDataDictionary = new Dictionary<string, AssetMapData>();
            this.hasMissingAsset = new List<string>();
            this.knownMissingGuid = new Dictionary<string, List<string>>();
            this.unKnownMissingGuid = new List<string>();
            this.bDirty = false;

            foreach (var pair in source.assetDataDictionary)
            {
                assetDataDictionary.Add(pair.Key, new AssetMapData(pair.Value));
            }

            foreach (var value in source.hasMissingAsset)
            {
                hasMissingAsset.Add(value);
            }
        }

        internal void Create()
        {
            this.assetDataDictionary = new Dictionary<string, AssetMapData>();
            this.hasMissingAsset = new List<string>();
            this.knownMissingGuid = new Dictionary<string, List<string>>();
            this.unKnownMissingGuid = new List<string>();
            this.bDirty = false;
        }

        internal void CacheAssetData(string[] pathList)
        {
            AssetMapUpdater.updateCount = 0;
            this.assetDataDictionary = new Dictionary<string, AssetMapData>();
            this.hasMissingAsset = new List<string>();
            this.knownMissingGuid = new Dictionary<string, List<string>>();
            this.unKnownMissingGuid = new List<string>();
            this.bDirty = false;

            for (int i = 0; i < pathList.Length; i++)
            {
                if (pathList[i].StartsWith("Assets/") == false)
                {
                    continue;
                }

                if (Ui.CheckPassByTime(Constants.CHECK_TIME_PROGRESS) == true)
                {
                    float rate = (float)i / (float)pathList.Length;
                    EditorUtility.DisplayProgressBar(Constants.SERVICE_NAME, string.Format(Constants.FORMAT_CACHE_ASSETDATA, i, pathList.Length), rate);
                }

                var guid = AssetDatabase.AssetPathToGUID(pathList[i]);

                PostAssetDataFromGUID(guid);
            }
            EditorUtility.ClearProgressBar();
        }

        internal AssetMapData GetAssetDataFromGUID(string guid)
        {
            AssetMapData node;
            if (assetDataDictionary.TryGetValue(guid, out node) == false)
            {
#if UNITY_EDITOR
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Common.Log.GpmLogger.Warn(string.Format(Constants.FORMAT_NOT_FOUND_DEPENDENCY, path), Constants.SERVICE_NAME, typeof(AssetMapCache), "GetAssetDataFromGUID");

                node = ReImportPullFromGUID(guid);
#endif
            }

            return node;
        }

        internal AssetMapData PostAssetDataFromGUID(string guid)
        {
            AssetMapData node;
            if (assetDataDictionary.TryGetValue(guid, out node) == false)
            {
                node = new AssetMapData(guid);
                assetDataDictionary.Add(guid, node);

                node.ReImport();
            }
            return node;
        }

        
        public void ReConnectMissingID(AssetMapData node)
        {
            List<string> missingList = new List<string>();
            if(knownMissingGuid.TryGetValue(node.guid, out missingList) == true)
            {
                string[] cloneMissingList = missingList.ToArray();
                for (int i = 0; i < cloneMissingList.Length; i++)
                {
                    AssetMapData parent = GetAssetDataFromGUID(cloneMissingList[i]);

                    if(parent != null)
                    {
                        parent.AttachDependency(node.guid);
                    }
                }
            }
        }
        

        internal void MoveNodeFromGUID(string guid)
        {
            AssetMapData node;
            if (assetDataDictionary.TryGetValue(guid, out node) == true)
            {
                node.CachePath();
            }
        }

        internal AssetMapData ReImportPullFromGUID(string guid)
        {
            AssetMapData node;
            if (assetDataDictionary.TryGetValue(guid, out node) == false)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                System.Type type = AssetDatabase.GetMainAssetTypeAtPath(path);
                if (type == null)
                {
                    return null;
                }
                
                node = new AssetMapData(guid);
                assetDataDictionary.Add(guid, node);
                node.ReImport();
            }
            else
            {
                node.ReImport();
            }

            return node;
        }

        internal void DeletePullFromGUID(string guid)
        {
            AssetMapData node;
            if (assetDataDictionary.TryGetValue(guid, out node) == true)
            {
                node.Delete();

                assetDataDictionary.Remove(guid);
            }
        }

        internal void MissingReconnectCheck(string guid)
        {
            AssetMapData node;
            if (assetDataDictionary.TryGetValue(guid, out node) == true)
            {
                node.MissingReconnectCheck();
            }
        }

        private const string CACHE_FOLDER_NAME = "AssetMapCache";
        private const string CACHE_FILE_NAME = "data.bytes";

        internal static bool HasCache()
        {
            string path;
            path = Path.Combine(Directory.GetCurrentDirectory(), CACHE_FOLDER_NAME);
            path = Path.Combine(path, CACHE_FILE_NAME);
            if (File.Exists(path) == true)
            {
                return true;
            }
            return false;
        }

        internal static AssetMapCache LoadCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), CACHE_FOLDER_NAME);
            path = Path.Combine(path, CACHE_FILE_NAME);
            if (File.Exists(path) == true)
            {
                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open))
                    {
                        using (TextReader tr = new StreamReader(fs))
                        {
                            return JsonMapper.ToObject<AssetMapCache>(tr);
                        }

                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                    return null;
                }
            }

            return null;
        }

        internal static void SaveCache()
        {
            if(GpmAssetManagementManager.cache != null)
            {
                GpmAssetManagementManager.cache.Save();
            }
        }

        internal void Save()
        {
            if (GpmAssetManagementManager.cache != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), CACHE_FOLDER_NAME);
                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                }
                path = Path.Combine(path, CACHE_FILE_NAME);

                bDirty = false;

                AssetMapCache PurePack = new AssetMapCache(this);
                File.WriteAllText(path, JsonMapper.ToJson(PurePack));
            }
        }
    }
}