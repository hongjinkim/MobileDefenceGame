using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gpm.AssetManagement.AssetFind
{
    using Gpm.AssetManagement.Const;

    public class ProjectSceneFindModule : FindModule
    {
        public SceneAsset rootSceneAsset;
        
        public int handle;
        public SceneRoot sceneRoot;

        public ProjectSceneFindModule(SceneAsset rootScene)
        {
            rootSceneAsset = rootScene;

            string path = AssetDatabase.GetAssetPath(rootSceneAsset);
            sceneRoot = SceneRootManager.GetSceneRoot(path);
            if (sceneRoot != null)
            {
                handle = sceneRoot.handle;
            }

            if (EditorUtility.IsPersistent(rootSceneAsset) == true)
            {
                long localid;
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(rootSceneAsset, out guid, out localid) == false)
                {
                    Common.Log.GpmLogger.Warn(string.Format(Constants.FORMAT_IS_NOT_FILE, rootSceneAsset.name),
                        Constants.SERVICE_NAME, typeof(ProjectSceneFindModule), "ProjectSceneFindModule");
                }

                isPersistent = true;
            }
            else
            {
                isPersistent = false;
            }
        }

        public bool IsChangeRoot()
        {
            if (sceneRoot != null)
            {
                return handle != sceneRoot.handle;
            }

            return false;
        }

        public void ChangedRoot()
        {
            if (sceneRoot != null)
            {
                handle = sceneRoot.handle;
            }
        }

        public override bool IsValid()
        {
            if (handle != 0)
            {
                if (sceneRoot.scene.isLoaded == false)
                {
                    return false;
                }
                return sceneRoot.scene.IsValid();
            }
            else
            {
                return rootSceneAsset != null;
            }
        }

        public override bool Reload()
        {
            Clear();

            if (rootSceneAsset == null)
            {
                return false;
            }

            if (isPersistent == true)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (string.IsNullOrEmpty(path) == false)
                {
                    sceneRoot = SceneRootManager.AddSceneRoot(path);
                    if (sceneRoot != null)
                    {
                        handle = sceneRoot.handle;
                        if (sceneRoot.scene.isLoaded == true)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool IsLoadable()
        {
            if (rootSceneAsset == null)
            {
                return false;
            }

            if (AssetDatabase.Contains(rootSceneAsset) == true)
            {
                return true;
            }

            if (isPersistent == true)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                return string.IsNullOrEmpty(path) == false;
            }

            return false;
        }

        public override string GetName()
        {
            return rootSceneAsset.name;
        }

        public override int GetID()
        {
            return rootSceneAsset.GetInstanceID();
        }

        public override Texture2D GetTypeIcon()
        {
            return AssetPreview.GetMiniThumbnail(rootSceneAsset);
        }

        public override bool CheckFindAble()
        {
            if (sceneRoot == null)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                sceneRoot = SceneRootManager.GetSceneRoot(path);
                if (sceneRoot != null)
                {
                    handle = sceneRoot.handle;
                }
            }

            if (sceneRoot != null)
            {
                if (sceneRoot.scene.handle != 0)
                {
                    if (sceneRoot.scene.IsValid() == true)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        public override void Find(bool bForce = false)
        {
            string path = AssetDatabase.GetAssetPath(rootSceneAsset);

            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
                if (scene.isLoaded == true &&
                    path.Equals(scene.path) == true)
                {
                    Find(scene.GetRootGameObjects());
                }
            }
        }

    }
}