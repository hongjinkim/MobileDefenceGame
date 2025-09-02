using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;


public class PlayFabServerTimeHandler : BaseUTCRequestHandler
{
    private bool onPending = false;

    protected override void OnRequest()
    {
        if (onPending == true) return;

        onPending = true;

        if (printLog) Debug.Log($"[!] <{GetType()}> PlayFab�� �ð��� ��û�߽��ϴ�.");

        try
        {
            PlayFabClientAPI.GetTime(new GetTimeRequest(), OnSuccessGetTime, OnErrorGetTime);
        }
        catch (Exception e)
        {
            if (printLog) Debug.LogError(e.Message);
            OnErrorGetTime(null);
        }
    }
        
    private void OnSuccessGetTime(GetTimeResult result)
    {
        onPending = false;
        var playfabServerUTC = DateTimeOffset.Parse(result.Time.ToString("o"));
        OnSuccess?.Invoke(playfabServerUTC);
        ClearCallbacks();
    }

    private void OnErrorGetTime(PlayFabError error)
    {
        onPending = false;
        if (printLog) Debug.Log($"[!] <{GetType()}> PlayFab���� �ð��� �������� ���߽��ϴ�.");
        OnFailure?.Invoke();            
        ClearCallbacks();
    }
        
}

