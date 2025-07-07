using UnityEditor;

namespace Gpm.AssetManagement.AssetMap.Internal
{
    using Gpm.AssetManagement.Const;

    internal class AssetMapUpdater : AssetPostprocessor
    {
        internal static int updateCount = 0;

        internal delegate void OnAssetMapUpdate();

        internal static event OnAssetMapUpdate eventOnAssetMapUpdate;

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (GpmAssetManagementManager.Enable == false)
            {
                return;
            }

            for (var i = 0; i < movedAssets.Length; i++)
            {
                if (Ui.CheckPassByTime(Constants.CHECK_TIME_PROGRESS) == true)
                {
                    float rate = (float)i / (float)movedAssets.Length;
                    EditorUtility.DisplayProgressBar(Constants.SERVICE_NAME, string.Format(Constants.FORMAT_MOVE_ASSET, i, movedAssets.Length), rate);
                }

                string guid = AssetDatabase.AssetPathToGUID(movedAssets[i]);
                GpmAssetManagementManager.cache.MoveNodeFromGUID(guid);
            }

            for (var i = 0; i < importedAssets.Length; i++)
            {
                if (Ui.CheckPassByTime(Constants.CHECK_TIME_PROGRESS) == true)
                {
                    float rate = (float)i / (float)importedAssets.Length;
                    EditorUtility.DisplayProgressBar(Constants.SERVICE_NAME, string.Format(Constants.FORMAT_IMPORT_ASSET, i, importedAssets.Length), rate);
                }

                string guid = AssetDatabase.AssetPathToGUID(importedAssets[i]);
                GpmAssetManagementManager.cache.ReImportPullFromGUID(guid);
            }

            for (var i = 0; i < deletedAssets.Length; i++)
            {
                if (Ui.CheckPassByTime(Constants.CHECK_TIME_PROGRESS) == true)
                {
                    float rate = (float)i / (float)deletedAssets.Length;
                    EditorUtility.DisplayProgressBar(Constants.SERVICE_NAME, string.Format(Constants.FORMAT_DELETED_ASSET, i, deletedAssets.Length), rate);
                }

                string guid = AssetDatabase.AssetPathToGUID(deletedAssets[i]);
                GpmAssetManagementManager.cache.DeletePullFromGUID(guid);
            }

            for (var i = 0; i < GpmAssetManagementManager.cache.hasMissingAsset.Count; i++)
            {
                if (Ui.CheckPassByTime(Constants.CHECK_TIME_PROGRESS) == true)
                {
                    float rate = (float)i / (float)GpmAssetManagementManager.cache.hasMissingAsset.Count;
                    EditorUtility.DisplayProgressBar(Constants.SERVICE_NAME, string.Format(Constants.FORMAT_MISSING_CHECK, i, GpmAssetManagementManager.cache.hasMissingAsset.Count), rate);
                }

                GpmAssetManagementManager.cache.MissingReconnectCheck(GpmAssetManagementManager.cache.hasMissingAsset[i]);
            }

            GpmAssetManagementManager.cache.Save();
            EditorUtility.ClearProgressBar();

            updateCount++;

            if (eventOnAssetMapUpdate != null)
            {
                eventOnAssetMapUpdate();
            }
        }
    }
}