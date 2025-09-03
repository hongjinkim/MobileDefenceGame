using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;


public class PlayFabGuestLoginModule
{
#if UNITY_EDITOR
    public Action<string, string> ResponseSuccess;
    public Action<LoginResponse> ResponseError;

    private bool onPending = false;


    public void Login()
    {
        if (onPending == true)
        {
            ResponseError?.Invoke(new LoginResponse(LoginError.OnPending));
            return;
        }

        onPending = true;

        try
        {
            var request = new LoginWithAndroidDeviceIDRequest
            {
                AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
                CreateAccount = true
            };

            PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnSuccess, OnError);
        }
        catch (Exception e)
        {
            OnNetworkError(e.Message);
        }
    }

    private void OnSuccess(LoginResult result)
    {
        onPending = false;
        ResponseSuccess?.Invoke(result.EntityToken.Entity.Id, "GUEST");
    }

    private void OnError(PlayFabError error) => OnNetworkError(error.Error.ToString());

    private void OnNetworkError(string message)
    {
        onPending = false;

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            ResponseError?.Invoke(new LoginResponse(LoginError.InternetNotReachable));
        }
        else
        {
            ResponseError?.Invoke(new LoginResponse(LoginError.UnknownError, $"<{GetType()}> {message}"));
        }
    }
#endif
}

