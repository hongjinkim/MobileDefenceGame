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
        Debug.Log("������ �ӽ� DisplayName ���� ���� : " + result.DisplayName + " (Ŭ���̾�Ʈ���� ���� ��)");
        OnSuccess?.Invoke(result.DisplayName);
    }

    private void OnError(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.NameNotAvailable)
        {
            Debug.LogError("�ߺ��� �г����� �ְų� ���� ������, �ٸ� �г������� �缳�� �õ�");
            Create();
        }
        else
        {
            Debug.LogError(error.Error);
        }
    }
}
