using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppVersionChecker : BasicSingleton<AppVersionChecker>
{
    [SerializeField] private GameObject AppUpdatePopup;

    public void RequestAppUpdate()
    {
        StopSaving();
        AppUpdatePopup.SetActive(true);
    }

    private void StopSaving()
    {
        ClientSaveManager.ForceStop();
        PlayerDataUploader.ForceStop();
    }
}

