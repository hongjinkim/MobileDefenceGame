using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerTitleDataManager : MonoBehaviour
{
    public static ServerTitleDataManager Instance { get; private set; } = null;

    public static event Action<Dictionary<string, string>> OnSuccess;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        //LoginManager.OnFinishPlayFabLoginProcess += RequestTitleData;
    }

    private void OnDisable()
    {
        //LoginManager.OnFinishPlayFabLoginProcess -= RequestTitleData;
    }


    private void RequestTitleData()
    {
        var request = new GetTitleDataRequest();
        PlayFabClientAPI.GetTitleData(request, OnSuccessGetTitleData, OnError);
    }

    private void OnSuccessGetTitleData(GetTitleDataResult result)
    {
        OnSuccess?.Invoke(result.Data);
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

}
