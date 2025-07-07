using UnityEditor;
using System.Collections.Generic;

using UnityEngine;

namespace Gpm.AssetManagement
{
    using Gpm.AssetManagement.Const;
    using Gpm.AssetManagement.AssetMap;
    using Gpm.AssetManagement.AssetMap.Internal;

    static public class GpmAssetManagementManager
    {
        private const string USE_ASSET_MANAGEMENT = "useAssetManagement";

        internal static AssetMapCache cache = null;
        private static bool m_bEnable = false;

        [InitializeOnLoadMethod]
        static public void Initialize()
        {
            cache = new AssetMapCache();
            cache.Create();
            if (PlayerPrefs.HasKey(USE_ASSET_MANAGEMENT) == true)
            {
                if(InitCache() == true)
                {
                    m_bEnable = true;
                }
                else
                {
                    m_bEnable = false;
                }
            }
            else
            {
                m_bEnable = false;
            }
        }

        static public bool Enable
        {
            get
            {
                return m_bEnable;
            }

            set
            {
                m_bEnable = value;
                if (m_bEnable == true)
                {
                    CacheAssetDataAll();
                    PlayerPrefs.SetInt(USE_ASSET_MANAGEMENT, 1);
                }
                else
                {
                    PlayerPrefs.DeleteKey(USE_ASSET_MANAGEMENT);
                }
                PlayerPrefs.Save();
            }
        }

        static public void CacheAssetDataAll()
        {
            CacheAssetData(AssetDatabase.GetAllAssetPaths());

            cache.Save();
        }

        static public void CacheAssetData(string[] pathList)
        {
            cache.CacheAssetData(pathList);
        }

        static public AssetMapData PostAssetMapDataFromPath(string path)
        {
            var guid = AssetDatabase.AssetPathToGUID(path);
            if (string.IsNullOrEmpty(guid) == true)
            {
                Common.Log.GpmLogger.Warn(string.Format(Constants.FORMAT_NOT_FOUND_GUID, path),
                    Constants.SERVICE_NAME, typeof(GpmAssetManagementManager), "PostAssetMapDataFromPath");

                return null;
            }
            return PostAssetDataFromGUID(guid);
        }

        static public AssetMapData PostAssetDataFromGUID(string guid)
        {
            return cache.PostAssetDataFromGUID(guid);
        }

        static public AssetMapData GetAssetDataFromPath(string path)
        {
            var guid = AssetDatabase.AssetPathToGUID(path);
            if (string.IsNullOrEmpty(guid) == true)
            {
                Common.Log.GpmLogger.Warn(string.Format(Constants.FORMAT_NOT_FOUND_GUID, path),
                    Constants.SERVICE_NAME, typeof(GpmAssetManagementManager), "GetAssetDataFromPath");

                return null;
            }
            return GetAssetDataFromGUID(guid);
        }

        static public AssetMapData GetAssetDataFromGUID(string guid)
        {
            return cache.GetAssetDataFromGUID(guid);
        }

        static public Dictionary<string, AssetMapData> GetAssetDataDictionary()
        {
            return cache.assetDataDictionary;
        }
  
        static public void GetReferenceListFromPath(ref List<AssetMapData> returnValue, string path, bool recursive = true, System.Func<AssetMapData, bool> condition = null)
        {
            var assetData = GpmAssetManagementManager.GetAssetDataFromPath(path);
            if (assetData != null)
            {
                assetData.GetReference(ref returnValue, recursive, condition);
            }
        }

        static public void GetDependencyFromPath(ref List<AssetMapData> returnValue, string path, bool recursive = true, System.Func<AssetMapData, bool> condition = null)
        {
            var assetData = GpmAssetManagementManager.GetAssetDataFromPath(path);
            if (assetData != null)
            {
                assetData.GetDependency(ref returnValue, recursive, condition);
            }
        }

        static internal bool InitCache()
        {
            if(AssetMapCache.HasCache() == true)
            {
                AssetMapCache loadCache = AssetMapCache.LoadCache();

                if(loadCache != null)
                {
                    cache = loadCache;

                    return true;
                }
            }

            return false;
        }
    }
}