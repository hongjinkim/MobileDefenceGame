using System.Collections.Generic;

using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace Gpm.AssetManagement.AssetFind
{
    static public class SceneRootManager
    {
        private static List<SceneRoot> activeSceneRootList = new List<SceneRoot>();

        public delegate void SceneRootChangeCallback(SceneRoot addSceneRoot, SceneRoot removeSceneRoot);
        public static event SceneRootChangeCallback changeSceneRoot;

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorSceneManager.newSceneCreated += newSceneCreated;
            EditorSceneManager.sceneOpened += sceneOpened;
            EditorSceneManager.sceneClosed += sceneClosed;
            EditorSceneManager.sceneSaved += sceneSaved;

            activeSceneRootList.Clear();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.isLoaded)
                {
                    activeSceneRootList.Add(new SceneRoot(scene));
                }
            }
        }

        public static List<SceneRoot> GetSceneRootList()
        {
            return activeSceneRootList;
        }

        public static SceneRoot GetSceneRoot(string path)
        {
            for (int i = 0; i < activeSceneRootList.Count; i++)
            {
                if (path.Equals(activeSceneRootList[i].path) == true)
                {
                    return activeSceneRootList[i];
                }
            }

            return null;
        }

        public static SceneRoot GetSceneRoot(int handle)
        {
            for (int i = 0; i < activeSceneRootList.Count; i++)
            {
                if (handle == activeSceneRootList[i].handle)
                {
                    return activeSceneRootList[i];
                }
            }

            return null;
        }

        public static SceneRoot AddSceneRoot(string path)
        {
            SceneRoot sceneRoot = GetSceneRoot(path);
            if(sceneRoot != null)
            {
                return sceneRoot;
            }

            EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

            return GetSceneRoot(path);
        }

        private static void newSceneCreated(Scene scene, NewSceneSetup setup, NewSceneMode mode)
        {
            SceneRoot root = GetSceneRoot(scene.path);
            if (root != null)
            {
                root.ReOpen(scene);
            }
            else
            {
                root = new SceneRoot(scene);
            }

            if (mode == NewSceneMode.Single)
            {
                for (int i = 0; i < activeSceneRootList.Count; i++)
                {
                    if (scene.path.Equals(activeSceneRootList[i].path) == false)
                    {
                        activeSceneRootList[i].Remove();

                        if (changeSceneRoot != null)
                        {
                            changeSceneRoot(null, activeSceneRootList[i]);
                        }
                    }
                }
                activeSceneRootList.Clear();
            }

            activeSceneRootList.Add(root);

            if (changeSceneRoot != null)
            {
                changeSceneRoot(root, null);
            }
        }

        private static void sceneOpened(Scene scene, OpenSceneMode mode)
        {
            SceneRoot root = GetSceneRoot(scene.path);
            if (root != null)
            {
                root.ReOpen(scene);
            }
            else
            {
                root = new SceneRoot(scene);

                if (mode != OpenSceneMode.AdditiveWithoutLoading)
                {
                    if (changeSceneRoot != null)
                    {
                        changeSceneRoot(root, null);
                    }
                }   
            }

            if (mode == OpenSceneMode.Single)
            {
                for(int i=0;i< activeSceneRootList.Count;i++)
                {
                    if(scene.handle.Equals(activeSceneRootList[i].handle) == false)
                    {
                        activeSceneRootList[i].Remove();

                        if (changeSceneRoot != null)
                        {
                            changeSceneRoot(null, activeSceneRootList[i]);
                        }
                    }
                }
                activeSceneRootList.Clear();
            }

            if (mode != OpenSceneMode.AdditiveWithoutLoading)
            {
                activeSceneRootList.Add(root);
            }
        }

        private static void sceneClosed(Scene scene)
        {
            SceneRoot root = GetSceneRoot(scene.path);
            if(root != null)
            {
                root.Remove();
                activeSceneRootList.Remove(root);

                if (changeSceneRoot != null)
                {
                    changeSceneRoot(null, root);
                }
            }
        }

        private static void sceneSaved(Scene scene)
        {
            SceneRoot root = GetSceneRoot(scene.handle);
            if (root != null)
            {
                root.path = scene.path;
            }
        }
    }
}