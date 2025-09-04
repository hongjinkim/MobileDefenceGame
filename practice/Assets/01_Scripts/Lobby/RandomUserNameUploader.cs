using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomUsernameUploader
{
    public event Action<string> OnSuccess;

    public void Create()
    {
        string username = IdGenerator.CreateCustomID();

        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = username,
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnSuccessUpdate, OnError);
    }

    private void OnSuccessUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("서버에 임시 DisplayName 생성 성공 : " + result.DisplayName + " (클라이언트에는 저장 전)");
        OnSuccess?.Invoke(result.DisplayName);
    }

    private void OnError(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.NameNotAvailable)
        {
            Debug.LogError("중복된 닉네임이 있거나 등의 이유로, 다른 닉네임으로 재설정 시도");
            Create();
        }
        else
        {
            Debug.LogError(error.Error);
        }
    }
}
