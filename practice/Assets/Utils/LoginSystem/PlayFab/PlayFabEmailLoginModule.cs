using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;

public class PlayFabEmailLoginModule
{
    public Action<string, string> ResponseSuccess;
    public Action<LoginResponse> ResponseError;

    private string _email = string.Empty;
    private string _password = string.Empty;
    private bool onPending = false;


    public void FastLogin(int number) 
        => Login($"{number:0}@ranking.com", "ranking", $"·©Å·Å×½ºÆ®{number:0}");

    public void Login(string email, string password, string displayName)
    {
        LoginResponse filtered = TextFilter.FilterLoginTextData(email, password, displayName);
        if (filtered != null) 
        {
            ResponseError?.Invoke(filtered);
            return;
        }

        if (onPending == true)
        {
            ResponseError?.Invoke(new LoginResponse(LoginError.OnPending));
            return;
        }

        onPending = true;

        try
        {
            _email = email;
            _password = password;

            var request = new RegisterPlayFabUserRequest
            {
                Email = email,
                Password = password,
                Username = displayName,                    
            };

            PlayFabClientAPI.RegisterPlayFabUser(request, OnSuccessRegister, OnErrorRegister);
        }
        catch (Exception e)
        {
            OnNetworkError(e.Message);
        }
    }

    private void OnSuccessRegister(RegisterPlayFabUserResult result) => Next();

    private void OnErrorRegister(PlayFabError error) => Next();

    private void Next()
    {
        try
        {
            var request = new LoginWithEmailAddressRequest
            {
                Email = _email,
                Password = _password
            };

            PlayFabClientAPI.LoginWithEmailAddress(request, OnSuccessLogin, OnErrorLogin);
        }
        catch (Exception e)
        {
            OnNetworkError(e.Message);
        }
    }

    private void OnSuccessLogin(LoginResult result)
    {
        onPending = false;
        ResponseSuccess?.Invoke(result.EntityToken.Entity.Id, "");
    }

    private void OnErrorLogin(PlayFabError error)
    {
        Debug.Log(error.Error.ToString());
        OnNetworkError(error.Error.ToString());
    }
             
    private void OnNetworkError(string message)
    {
        onPending = false;

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            ResponseError?.Invoke(new LoginResponse(LoginError.InternetNotReachable));
        }
        else if (message == "AccountBanned")
        {
            ResponseError?.Invoke(new LoginResponse(LoginError.AccountBanned));
        }
        else
        {
            ResponseError?.Invoke(new LoginResponse(LoginError.UnknownError, $"<{GetType()}> {message}"));
        }
    }
}

