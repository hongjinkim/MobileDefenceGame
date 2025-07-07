using UnityEditor;
using System.Collections;
using System.Collections.Generic;

//using System;

using UnityEngine;

namespace Gpm.AssetManagement.AssetMap
{
    using Gpm.AssetManagement.Const;

    public class AssetMapData
    {
        public enum MissingState
        {
            UNKNOWN = 0,
            MISSING_OBSOLETE = 1,
            OK_OBSOLETE = 2,

            MISSING = 10,
            MISSING_MANAGED = 11,

            OK = 20,
        }

        public string guid;

        public List<AssetMapLink> referenceLinks;

        public List<AssetMapLink> dependencyLinks;

        public MissingState hasMissing;
        public int missingCount;

        internal int processCount = 0;

        private string cachePath;

        public string GetPath()
        {
            if (IsCachePath() == false)
            {
                CachePath();
            }

            return cachePath;
        }
        
        public void CachePath()
        {
            cachePath = AssetDatabase.GUIDToAssetPath(guid);
        }

        public bool IsCachePath()
        {
            return string.IsNullOrEmpty(cachePath) == false;
        }

        public void GetReference(ref List<AssetMapData> returnValue, bool recursive = true, System.Func<AssetMapData, bool> condition = null)
        {
            if (condition != null && condition(this) == false)
            {
                return;
            }

            foreach (AssetMapLink link in referenceLinks)
            {
                if (returnValue.Exists(value => 
                {
                    return link.Equals(value);
                }) == false)
                {
                    AssetMapData referenceData = link.GetData();
                    returnValue.Add(referenceData);

                    if (recursive == true)
                    {
                        referenceData.GetReference(ref returnValue, recursive, condition);
                    }
                    else
                    {
                        if (condition != null)
                        {
                            condition(referenceData);
                        }
                    }
                }
            }
        }

        public void GetDependency(ref List<AssetMapData> returnValue, bool recursive = true, System.Func<AssetMapData, bool> condition = null)
        {
            if (condition != null && condition(this) == false)
            {
                return;
            }

            foreach (AssetMapLink link in dependencyLinks)
            {
                if (returnValue.Exists(value =>
                {
                    return link.Equals(value);
                }) == false)
                {
                    AssetMapData dependencyData = link.GetData();
                    returnValue.Add(dependencyData);

                    if (recursive == true)
                    {
                        dependencyData.GetDependency(ref returnValue, recursive, condition);
                    }
                    else
                    {
                        if (condition != null)
                        {
                            condition(dependencyData);
                        }
                    }
                }
            }
        }

        internal void AttachDependency(string dependencyGUID)
        {
            /// <summary>
            ///  본인인가? 본인이면 return
            /// </summary>
            if (this.guid.Equals(dependencyGUID) == true)
            {
                return;
            }

            /// <summary>
            ///  dependencyAsset에 있는가? 있으면 return
            /// </summary>
            if (dependencyLinks.Exists(value =>
            {
                return value.Equals(dependencyGUID);
            }) == true )
            {
                return;
            }

            /// <summary>
            ///  pull에 에서 가져오기
            /// </summary>
            AssetMapData dependencyAssetData = GpmAssetManagementManager.PostAssetDataFromGUID(dependencyGUID);

            /// <summary>
            ///  childLink에 child를 넣기
            /// </summary>
            dependencyLinks.Add(new AssetMapLink(dependencyAssetData));

            /// <summary>
            ///  child의 parent에 없으면 넣기
            /// </summary>
            if (dependencyAssetData.referenceLinks.Exists(v =>
            {
                return v.Equals(this);
            }) == false)
            {
                dependencyAssetData.referenceLinks.Add(new AssetMapLink(this));
            }

            if(hasMissing != MissingState.OK)
            {
                List<string> missingList = new List<string>();
                if (GpmAssetManagementManager.cache.knownMissingGuid.TryGetValue(dependencyGUID, out missingList) == true)
                {
                    if (missingList.Remove(guid) == true)
                    {
                        if(missingList.Count == 0)
                        {
                            GpmAssetManagementManager.cache.knownMissingGuid.Remove(dependencyGUID);
                        }

                        missingCount--;
                        if (missingCount <= 0)
                        {
                            GpmAssetManagementManager.cache.hasMissingAsset.Remove(guid);
                            missingCount = 0;
                            hasMissing = MissingState.OK;
                        }
                    }
                }
            }
        }

        private void DetechDependency(AssetMapLink dependencyAssetDataGUID)
        {
            /// <summary>
            ///  본인인가? 본인이면 return
            /// </summary>
            if (guid.Equals(dependencyAssetDataGUID.guid) == true)
            {
                return;
            }

            /// <summary>
            ///  childLink에 있는가? 없으면 return
            /// </summary>
            if (this.dependencyLinks.Exists(v =>
            {
                return v.Equals(dependencyAssetDataGUID.guid);
            }) == false)
            {
                return;
            }

            /// <summary>
            ///  pull에 있는 child 가져오기
            /// </summary>
            AssetMapData dependencyData = dependencyAssetDataGUID.GetData();
            if (dependencyData != null)
            {
                /// <summary>
                ///  child parent에 있으면 제거
                /// </summary>
                int removeIdx = dependencyData.referenceLinks.FindIndex(v =>
                {
                    return v.guid.Equals(guid);
                });

                if (removeIdx >= 0)
                {
                    dependencyData.referenceLinks.RemoveAt(removeIdx);
                }
            }

            /// <summary>
            ///  childLink에서 child를 제거
            /// </summary>
            this.dependencyLinks.Remove(dependencyAssetDataGUID);
        }

        internal void Delete()
        {
            /// <summary>
            ///  자신이 가지고 있던 missing Dependency 처리
            /// </summary>
            GpmAssetManagementManager.cache.hasMissingAsset.Remove(guid);
            GpmAssetManagementManager.cache.unKnownMissingGuid.Remove(guid);

            List<string> missingList;
            if (GpmAssetManagementManager.cache.knownMissingGuid.TryGetValue(guid, out missingList) == false)
            {
            }

            foreach (var link in referenceLinks)
            {
                AssetMapData referenceData = link.GetData();
                if (referenceData != null)
                {
                    
                    Common.Log.GpmLogger.Warn(string.Format(Constants.FORMAT_MISSING_DEPENDENCY_MESSAGE, referenceData.GetPath(), GetPath()),
                        Constants.SERVICE_NAME, typeof(AssetMapData), "Delete");

                    referenceData.dependencyLinks.RemoveAll(v =>
                    {
                        return v.guid.Equals(guid) == true;
                    });

                    if (referenceData.hasMissing == MissingState.OK_OBSOLETE || 
                        referenceData.hasMissing == MissingState.OK)
                    {
                        referenceData.hasMissing = MissingState.MISSING_MANAGED;
                    }
                    else
                    {
                        referenceData.hasMissing = MissingState.MISSING;
                    }

                    if (GpmAssetManagementManager.cache.hasMissingAsset.Contains(referenceData.guid) == false)
                    {
                        GpmAssetManagementManager.cache.hasMissingAsset.Add(referenceData.guid);
                    }

                    if (missingList == null)
                    {
                        missingList = new List<string>();
                    }

                    if (missingList.Contains(referenceData.guid) == false)
                    {
                        missingList.Add(referenceData.guid);
                        referenceData.missingCount++;
                    }
                    
                    GpmAssetManagementManager.cache.knownMissingGuid[guid] = missingList;
                }
            }

            /// <summary>
            ///  자식과의 연결 제거
            /// </summary>
            foreach (var child in dependencyLinks.ToArray())
            {
                DetechDependency(child);
            }
        }

        internal void ReImport(bool bForce = false)
        {
            if (hasMissing != MissingState.UNKNOWN)
            {
                if (Internal.AssetMapUpdater.updateCount > 0 &&
                    Internal.AssetMapUpdater.updateCount == processCount)
                {
                    return;
                }
            }

            string path = GetPath();

            if (AssetDatabase.IsValidFolder(path) == true)
            {
                return;
            }

            GpmAssetManagementManager.cache.ReConnectMissingID(this);


            System.Type type = AssetDatabase.GetMainAssetTypeAtPath(path);

            if (type == null)
            {
                return;
            }

            if (type.Equals(typeof(UnityEditor.DefaultAsset)) == true)
            {
                if (hasMissing != MissingState.OK)
                {
                    GpmAssetManagementManager.cache.bDirty = true;
                }

                hasMissing = MissingState.OK;

                return;
            }

            string extension = System.IO.Path.GetExtension(path);

            if (typeof(UnityEngine.TextAsset).IsAssignableFrom(type) == true ||
                typeof(UnityEngine.Texture).IsAssignableFrom(type) == true ||
                //typeof(UnityEngine.AnimationClip).IsAssignableFrom(type) == true ||
                typeof(UnityEngine.AudioClip).IsAssignableFrom(type) == true ||
                typeof(UnityEngine.Video.VideoClip).IsAssignableFrom(type) == true ||
                extension.Equals(".fbx") == true || extension.Equals(".FBX") == true ||
                extension.Equals(".blend") == true || extension.Equals(".BLEND") == true)
            {
                if (hasMissing != MissingState.OK)
                {
                    GpmAssetManagementManager.cache.bDirty = true;
                }

                hasMissing = MissingState.OK;

                return;
            }

            AssetFind.SceneRoot sceneRoot = null;
            string[] guidList = null;
            if (typeof(UnityEngine.Font).IsAssignableFrom(type) == true)
            {
                guidList = AssetDatabase.GetDependencies(path, false);
                for (int i = 0; i < guidList.Length; i++)
                {
                    guidList[i] = AssetDatabase.AssetPathToGUID(guidList[i]);
                }

                if (hasMissing != MissingState.OK)
                {
                    GpmAssetManagementManager.cache.bDirty = true;
                }

                hasMissing = MissingState.OK;
            }
            else
            {
                bool readable = true;
                if (ReadAble(path) == false)
                {
                    if (bForce == true)
                    {
                        sceneRoot = AssetFind.SceneRootManager.AddSceneRoot(path);
                    }
                    else
                    {
                        guidList = AssetDatabase.GetDependencies(path, false);
                        for (int i = 0; i < guidList.Length; i++)
                        {
                            guidList[i] = AssetDatabase.AssetPathToGUID(guidList[i]);
                        }

                        if (hasMissing != MissingState.UNKNOWN)
                        {
                            GpmAssetManagementManager.cache.bDirty = true;
                        }

                        hasMissing = MissingState.UNKNOWN;
                        if (GpmAssetManagementManager.cache.unKnownMissingGuid.Contains(guid) == false)
                        {
                            GpmAssetManagementManager.cache.unKnownMissingGuid.Add(guid);
                            GpmAssetManagementManager.cache.bDirty = true;
                        }

                        readable = false;
                    }
                }

                if (readable == true)
                {
                    GetDependencyInfo dependencyValue = new GetDependencyInfo(path);
                    guidList = dependencyValue.dependencys.ToArray();

                    missingCount = 0;
                    int unkonwnMissing = 0;
                    string missing_guid;
                    long missing_localid;

                    foreach (int instanceID in dependencyValue.missingInstanceIDs)
                    {
                        if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(instanceID, out missing_guid, out missing_localid) == true)
                        {
                            missingCount++;

                            List<string> missingList = null;
                            if (GpmAssetManagementManager.cache.knownMissingGuid.TryGetValue(missing_guid, out missingList) == true)
                            {
                            }

                            if (missingList == null)
                            {
                                missingList = new List<string>();
                            }

                            if (missingList.Contains(guid) == false)
                            {
                                missingList.Add(guid);
                                GpmAssetManagementManager.cache.knownMissingGuid[missing_guid] = missingList;
                            }
                        }
                        else
                        {
                            unkonwnMissing++;
                        }
                    }

                    missingCount += unkonwnMissing;
                    
                    if (missingCount == 0)
                    {
                        if (hasMissing == MissingState.MISSING_OBSOLETE ||
                            hasMissing == MissingState.MISSING ||
                            hasMissing == MissingState.MISSING_MANAGED)
                        {
                            GpmAssetManagementManager.cache.knownMissingGuid.Remove(guid);
                            GpmAssetManagementManager.cache.hasMissingAsset.Remove(guid);
                        }

                        if (hasMissing != MissingState.OK)
                        {
                            GpmAssetManagementManager.cache.bDirty = true;
                        }

                        hasMissing = MissingState.OK;
                    }
                    else
                    {
                        if (unkonwnMissing > 0)
                        {
                            if (hasMissing != MissingState.MISSING)
                            {
                                hasMissing = MissingState.MISSING;
                                GpmAssetManagementManager.cache.bDirty = true;
                            }
                        }
                        else
                        {
                            if (hasMissing != MissingState.MISSING_MANAGED)
                            {
                                hasMissing = MissingState.MISSING_MANAGED;
                                GpmAssetManagementManager.cache.bDirty = true;
                            }
                        }

                        if (GpmAssetManagementManager.cache.hasMissingAsset.Contains(guid) == false)
                        {
                            GpmAssetManagementManager.cache.hasMissingAsset.Add(guid);
                            GpmAssetManagementManager.cache.bDirty = true;
                        }
                    }
                }

            }

            if (guidList != null)
            {
                /// <summary>
                ///  동기화 시켜준다.
                /// </summary>
                List<AssetMapLink> dependencyListClone = new List<AssetMapLink>(dependencyLinks);

                /// <summary>
                ///  guidList에 없는것 삭제
                /// </summary>
                for (int i = 0; i < dependencyListClone.Count; i++)
                {
                    if (System.Array.Exists(guidList, value =>
                     {
                         return value.Equals(dependencyListClone[i].guid);
                     }) == false)
                    {
                        DetechDependency(dependencyListClone[i]);
                    }
                }

                /// <summary>
                ///  guidList 있는데 없는것 추가
                /// </summary>
                for (int i = 0; i < guidList.Length; i++)
                {
                    if (dependencyListClone.Exists(value =>
                     {
                         return guidList[i].Equals(value.guid);
                     }) == false)
                    {
                        AttachDependency(guidList[i]);
                    }
                }
            }

            if (bForce == true)
            {
                if (sceneRoot != null)
                {
                    UnityEditor.SceneManagement.EditorSceneManager.CloseScene(sceneRoot.scene, true);
                }
            }

            processCount = Internal.AssetMapUpdater.updateCount;
        }

        private bool ReadAble(string path)
        {
            System.Type type = AssetDatabase.GetMainAssetTypeAtPath(path);
            if (type != null)
            {
                if (type.Equals(typeof(UnityEditor.SceneAsset)) == true)
                {
                    var sceneRoot = AssetFind.SceneRootManager.GetSceneRoot(path);
                    if (sceneRoot == null ||
                        sceneRoot.scene.handle == 0 ||
                        sceneRoot.scene.IsValid() == false ||
                        sceneRoot.scene.isLoaded == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void MissingReconnectCheck()
        {
            if (missingCount > 0)
            {
                if (hasMissing == AssetMapData.MissingState.MISSING_OBSOLETE ||
                    hasMissing == AssetMapData.MissingState.MISSING)
                {
                    string path = GetPath();
                    if (ReadAble(path) == true)
                    {
                        ReImport();
                    }
                }
            }
        }

        public AssetMapData()
        {
        }

        public AssetMapData(string guid)
        {
            Create();

            this.guid = guid;
        }

        public AssetMapData(AssetMapData source)
        {
            this.referenceLinks = new List<AssetMapLink>(source.referenceLinks);
            this.dependencyLinks = new List<AssetMapLink>(source.dependencyLinks);
            this.hasMissing = source.hasMissing;
            this.missingCount = source.missingCount;
            
            this.guid = source.guid;
        }

        private void Create()
        {
            this.referenceLinks = new List<AssetMapLink>();
            this.dependencyLinks = new List<AssetMapLink>();
            this.hasMissing = MissingState.UNKNOWN;
            this.missingCount = 0;

            this.guid = "";
        }

        public bool Equals(AssetMapLink link)
        {
            return this.guid.Equals(link.guid);
        }
    }
}