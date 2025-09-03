using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PlayFab;
using PlayFab.ClientModels;

public class ServerPlayerDataReceiver : MonoBehaviour
{
    public event Action OnSuccessGetEmptyData;
    public event Action<Dictionary<string, string>> OnSuccessGetJsonDictionary;
    public event Action OnFailed;

    private List<string> Keys = new List<string>
        {
            ServerDataKeys.ACCOUNT,
            ServerDataKeys.CONTENTS,
            ServerDataKeys.LEVEL,
            ServerDataKeys.RECORD,
            ServerDataKeys.MAX
        };


    public void GetUserData()
    {
        var request = new GetUserDataRequest
        {
            Keys = new List<string>(Keys.ToArray())
        };

        PlayFabClientAPI.GetUserData(request, OnSuccessGetUserData, OnError);
    }


    private void OnSuccessGetUserData(GetUserDataResult result)
    {
        try
        {
            if (result.Data == null || result.Data.Count == 0)
            {
                Debug.Log("서버 데이터가 비어있는 뎁쇼");
                OnSuccessGetEmptyData?.Invoke();
                return;
            }

            var dict = new Dictionary<string, string>();
            foreach ((string key, var json) in result.Data)
            {
                Debug.Log($"<받은데이터> [{key}] => {json.Value}");
                dict[key] = json.Value;
            }

            OnSuccessGetJsonDictionary?.Invoke(dict);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            OnFailed?.Invoke();
        }
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError(error.Error.ToString());
        OnFailed?.Invoke();
    }

}
