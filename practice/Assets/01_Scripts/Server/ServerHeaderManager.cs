using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ServerHeaderManager : MonoBehaviour
{
    public static ServerHeaderData StaticHeader = null;

    public static event Action OnDifferPlayerId;
    public static event Action OnDifferDeviceId;
    public static event Action OnEmptyServerRecord;
    public static event Action OnFailedComparison;
    public static event Action OnForceOverwrite;
    public static event Action OnNext;


    public static string GetHeaderJson()
    {
        if (StaticHeader == null) throw new Exception("Header is null.");

        StaticHeader.ForceOverwrite = false;
        StaticHeader.Stamp = TimeManager.NowUTC;
        StaticHeader.DeviceUniqueId = SystemInfo.deviceUniqueIdentifier;
        StaticHeader.SaveVersion++;

        return JsonConvert.SerializeObject(StaticHeader);
    }

    public static void UpdateRestoreDateTime(DateTimeOffset nowUTC)
    {
        if (StaticHeader != null)
        {
            StaticHeader.RestoredTime = nowUTC;
        }
    }


    // ------ 

    private readonly string HEADER_DATA_KEY = "Header";


    public void CompareHeader()
    {
        try
        {
            var request = new GetUserDataRequest
            {
                Keys = new List<string> { HEADER_DATA_KEY }
            };

            PlayFabClientAPI.GetUserData(request, OnSuccess, OnError);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            OnFailedComparison?.Invoke();
        }
    }


    private void OnSuccess(GetUserDataResult result)
    {
        if (result.Data == null || result.Data.ContainsKey(HEADER_DATA_KEY) == false)
        {
            StaticHeader = new ServerHeaderData
            {
                ForceOverwrite = false,
                Stamp = TimeManager.NowUTC,
                DeviceUniqueId = SystemInfo.deviceUniqueIdentifier,
                SaveVersion = 0,
                PlayerId = PlayFabSettings.staticPlayer.EntityId,
                RestoredTime = TimeManager.NowUTC,
                Administrator = false
            };

            OnEmptyServerRecord?.Invoke();
            return;
        }

        string json = result.Data[HEADER_DATA_KEY].Value;
        StaticHeader = JsonConvert.DeserializeObject<ServerHeaderData>(json);

        if (StaticHeader.ForceOverwrite == true)
        {
            OnForceOverwrite?.Invoke();
            return;
        }

        if (StaticHeader.DeviceUniqueId != SystemInfo.deviceUniqueIdentifier)
        {
            OnDifferDeviceId?.Invoke();
            return;
        }

        if (StaticHeader.PlayerId != PlayFabSettings.staticPlayer.EntityId)
        {
            OnDifferPlayerId?.Invoke();
            return;
        }

        // 모두 일치하는데, 클라이언트 데이터가 없을 때
        OnNext?.Invoke();
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError(error.Error.ToString());
        OnFailedComparison?.Invoke();
    }

}
