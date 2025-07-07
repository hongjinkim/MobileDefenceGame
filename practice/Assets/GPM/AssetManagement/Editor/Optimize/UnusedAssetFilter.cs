using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Gpm.AssetManagement.Optimize.Ui
{
    using Gpm.AssetManagement.Const;
    using Gpm.AssetManagement.AssetMap;
    public class FilterPath
    {
        public FilterPath()
        {

        }

        public FilterPath(string path)
        {
            filterPath = path;
        }
        public string filterPath;
    }

    public class UnusedAssetFilter
    {
        public bool filterBuildIn = true;
        public bool filterAssetbundle = true;
        public bool filterPathList = true;

        public List<FilterPath> filterList = new List<FilterPath>();

        private bool dirty = false;
        private List<string> projectException = new List<string>();
        private List<string> assetbundlePaths = new List<string>();

        public void Init()
        {
            if(filterBuildIn == true)
            {
                CheckProjectFilter();
            }

            if(filterAssetbundle == true)
            {
                CheckAssetbundleFilter();
            }
        }

        public bool IsFilter(string path)
        {
            if (AssetDatabase.IsValidFolder(path) == true)
            {
                return false;
            }

            System.Type type = AssetDatabase.GetMainAssetTypeAtPath(path);
            if (type == typeof(DefaultAsset) == true ||
                typeof(UnityEngine.TextAsset).IsAssignableFrom(type) == true)
            {
                return false;
            }

            if (type == typeof(SceneAsset) == true)
            {
                foreach (var scene in EditorBuildSettings.scenes)
                {
                    if (path.EndsWith(scene.path) == true)
                    {
                        return false;
                    }
                }
            }

            string ext = System.IO.Path.GetExtension(path);
            if (ext.Equals("cs") == true ||
                ext.Equals("framework") == true ||
                ext.Equals("dll") == true ||
                ext.Equals("pdf") == true ||
                ext.Equals("exe") == true ||
                ext.Equals("h") == true ||
                ext.Equals("m") == true ||
                ext.Equals("mm") == true ||
                ext.Equals("aar") == true ||
                ext.Equals("jar") == true ||
                ext.Equals("XML") == true ||
                ext.Equals("xml") == true ||
                ext.Equals("plist") == true ||
                ext.Equals("html") == true ||
                ext.Equals("mdb") == true ||
                ext.Equals("txt") == true ||
                ext.Equals("islib") == true ||
                ext.Equals("a") == true ||
                ext.Equals("c") == true ||

                ext.Equals("podspec") == true ||
                ext.Equals("asmdef") == true)
            {
                return false;
            }

            if (path.Contains(".framework/") == true)
            {
                return false;
            }

            if (filterBuildIn == true)
            {
                if (path.Contains("/Resources/") == true)
                {
                    return false;
                }

                for (int i = 0; i < projectException.Count; i++)
                {
                    if (path.Equals(projectException[i]) == true)
                    {
                        return false;
                    }
                }
            }

            if (filterAssetbundle == true)
            {
                for (int i = 0; i < assetbundlePaths.Count; i++)
                {
                    if (path.Equals(assetbundlePaths[i]) == true)
                    {
                        return false;
                    }
                }
            }

            if (filterPathList == true)
            {
                for (int i = 0; i < filterList.Count; i++)
                {
                    if(string.IsNullOrEmpty(filterList[i].filterPath) == false)
                    {
                        if (path.Contains(filterList[i].filterPath) == true)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private void CheckProjectFilter()
        {
            EditorBuildSettingsScene[] buildInScenes = EditorBuildSettings.scenes;
            Dictionary<string, AssetMapData> dicAssetMapData = GpmAssetManagementManager.GetAssetDataDictionary();

            List<GetDependencyInfo> projectSettingList = new List<GetDependencyInfo>();

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo("ProjectSettings");
            foreach (System.IO.FileInfo file in di.GetFiles())
            {
                projectSettingList.Add(new GetDependencyInfo("ProjectSettings/" + file.Name));
            }
            projectException.Clear();
            for (int i = 0; i < projectSettingList.Count; i++)
            {
                foreach (string guid in projectSettingList[i].dependencys)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (projectException.Contains(path) == false)
                    {
                        projectException.Add(path);
                    }
                }
            }

            projectSettingList.Clear();
        }

        private void CheckAssetbundleFilter()
        {
            assetbundlePaths.Clear();
            foreach (string bundle in AssetDatabase.GetAllAssetBundleNames())
            {
                string[] list = AssetDatabase.GetAssetPathsFromAssetBundle(bundle);
                foreach (string path in list)
                {
                    assetbundlePaths.Add(path);
                }
            }
        }

        public void SetDirty()
        {
            dirty = true;
        }

        public void CheckSaved()
        {
            if(dirty == true)
            {
                Save();
                dirty = false;
            }
        }

        static public UnusedAssetFilter Load()
        {
            string path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), Constants.PATH_GPM);
            path = System.IO.Path.Combine(path, Constants.FILENAME_UNUSEDASSET_FILTER);
            if (System.IO.File.Exists(path) == true)
            {
                try
                {
                    using (System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open))
                    {
                        using (System.IO.TextReader tr = new System.IO.StreamReader(fs))
                        {
                            return Gpm.Common.ThirdParty.LitJson.JsonMapper.ToObject<UnusedAssetFilter>(tr);
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

        public void Save()
        {
            if (GpmAssetManagementManager.cache != null)
            {
                string path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), Constants.PATH_GPM);
                if (System.IO.Directory.Exists(path) == false)
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                path = System.IO.Path.Combine(path, Constants.FILENAME_UNUSEDASSET_FILTER);

                System.IO.File.WriteAllText(path, Gpm.Common.ThirdParty.LitJson.JsonMapper.ToJson(this));
            }
        }
    }
}
