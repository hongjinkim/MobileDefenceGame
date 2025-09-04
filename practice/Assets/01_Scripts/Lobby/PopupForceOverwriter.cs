using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupForceOverwriter : MonoBehaviour
{
    public static Action OnFinishOverwrite;

    public ServerPlayerDataReceiver ServerToClient;
    public PlayerDataUploader ClientToServer;


    private void OnEnable()
    {
        ServerToClient.OnSuccessGetJsonDictionary += Overwrite;
        ServerToClient.GetUserData();
    }

    private void OnDisable()
    {
        ServerToClient.OnSuccessGetJsonDictionary -= Overwrite;
        ClientToServer.OnSuccessUpload -= End;
    }


    private void Overwrite(Dictionary<string, string> dictionary)
    {
        GameDataManager.PlayerData.Overwrite(dictionary);
        ClientToServer.ForceUploadAll();

        ClientToServer.OnSuccessUpload += End;
    }

    private void End()
    {
        OnFinishOverwrite?.Invoke();
        gameObject.SetActive(false);
    }

}
