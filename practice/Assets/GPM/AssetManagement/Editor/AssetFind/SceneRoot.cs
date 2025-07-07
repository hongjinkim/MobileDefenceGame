using UnityEngine.SceneManagement;

namespace Gpm.AssetManagement.AssetFind
{

    public class SceneRoot
    {
        public SceneRoot(Scene scene)
        {
            this.scene = scene;
            this.path = scene.path;
            this.handle = scene.handle;

            SceneRootManager.changeSceneRoot += ChangeSceneRoot;
        }

        public void ReOpen(Scene scene)
        {
            this.scene = scene;
            this.handle = scene.handle;
        }

        public void Remove()
        {
            removed = true;
        }

        public Scene scene;
        public string path;
        public int handle;
        public bool removed = false;

        private void ChangeSceneRoot(SceneRoot addSceneRoot, SceneRoot removeSceneRoot)
        {
            if (removed == true)
            {
                if (addSceneRoot != null)
                {
                    if (path.Equals(addSceneRoot.path) == true)
                    {
                        ReOpen(addSceneRoot.scene);
                        removed = false;
                    }
                }
            }
        }
    }
}