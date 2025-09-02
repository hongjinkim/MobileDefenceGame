using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UserDataUploader
{
    public event Action OnSuccess;
    public event Action OnSuccessForcedAllCallback;

    public bool IsOnPending { get; private set; } = false;

    private bool shouldInvokeCallback = false;


    public void Upload(Dictionary<string, string> data, bool isForcedUploadAll = false)
    {
        if (data == null || data.Count == 0) return;

        if (IsOnPending == true) { return; }

        IsOnPending = true;

        try
        {
            var request = new UpdateUserDataRequest { Data = data };

            Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(request));

            PlayFabClientAPI.UpdateUserData(request, OnSuccessUpdate, OnError);

            shouldInvokeCallback = isForcedUploadAll;
        }
        catch (Exception e)
        {
            IsOnPending = false;
            Debug.LogError(e.Message);
        }
    }


    private void OnSuccessUpdate(UpdateUserDataResult result)
    {
        IsOnPending = false;

        Debug.Log($"서버에 저장완료");

        OnSuccess?.Invoke();

        if (shouldInvokeCallback == true)
        {
            OnSuccessForcedAllCallback?.Invoke();
            shouldInvokeCallback = false;
        }
    }

    private void OnError(PlayFabError error)
    {
        IsOnPending = false;
        Debug.LogError(error.Error.ToString());
    }
}
