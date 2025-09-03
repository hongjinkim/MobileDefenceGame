//using GooglePlayGames;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;

public class PlayFabGoogleLoginModule
{
    public Action<string, string> ResponseSuccess;
    public Action<LoginResponse> ResponseError;

    private bool onPending = false;



    public void LoginWithGooglePlayGamesService()
    {
        //if (onPending == true)
        //{
        //    ResponseError?.Invoke(new PoomLoginResponse(PoomLoginError.OnPending));
        //    return;
        //}

        //onPending = true;

        //try
        //{
        //    PlayGamesPlatform.Instance.GetAnotherServerAuthCode(true, (serverAuthCode) => {

        //        var request = new LoginWithGooglePlayGamesServicesRequest
        //        {
        //            ServerAuthCode = serverAuthCode,
        //            CreateAccount = true,
        //            TitleId = PlayFabSettings.TitleId
        //        };

        //        PlayFabClientAPI.LoginWithGooglePlayGamesServices(request, OnLoginWithGooglePlayGamesServicesSuccess, OnLoginWithGooglePlayGamesServicesFailure);
        //    });
        //}
        //catch (Exception e)
        //{
        //    if (Application.internetReachability == NetworkReachability.NotReachable)
        //    {
        //        ResponseError?.Invoke(new PoomLoginResponse(PoomLoginError.InternetNotReachable));
        //    }
        //    else
        //    {
        //        ResponseError?.Invoke(new PoomLoginResponse(PoomLoginError.UnknownError, e.Message));
        //    }
        //}
    }

    private void OnLoginWithGooglePlayGamesServicesSuccess(LoginResult result)
    {
        onPending = false;

        Debug.Log("PlayFab LoginWithGooglePlayGamesServices Success.");
        ResponseSuccess?.Invoke(result.EntityToken.Entity.Id, "GPGS");
    }

    private void OnLoginWithGooglePlayGamesServicesFailure(PlayFabError error)
    {
        onPending = false;

        Debug.Log("PlayFab LoginWithGooglePlayGamesServices Failure: " + error.GenerateErrorReport());
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            ResponseError?.Invoke(new LoginResponse(LoginError.InternetNotReachable));
        }
        else if (error.Error == PlayFabErrorCode.AccountBanned)
        {
            ResponseError?.Invoke(new LoginResponse(LoginError.AccountBanned, (string)error.CustomData));
        }
        else
        {
            ResponseError?.Invoke(new LoginResponse(LoginError.UnknownError, error.Error.ToString()));
        }
    }

}

