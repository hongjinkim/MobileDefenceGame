using System;
using UnityEngine;


#pragma warning disable 0067
public class LoginProcessManager : MonoBehaviour
{
    public static event Action OnInternetNotReachable;
    public static event Action OnFailedOrDeniedGPGSLogin;

    public LoginManager LoginManager;
    public GameObject LoginMethodPopup;


    private void OnEnable()
    {
        LoginManager.OnGooglePlayGameServiceUnavailable += PopupLoginMethodWindow;
        LoginManager.OnDeniedGooglePlayServiceTerms += PopupLoginMethodWindow;
    }

    private void OnDisable()
    {
        LoginManager.OnGooglePlayGameServiceUnavailable -= PopupLoginMethodWindow;
        LoginManager.OnDeniedGooglePlayServiceTerms -= PopupLoginMethodWindow;
    }


    private void PopupLoginMethodWindow()
    {
        LoginMethodPopup.SetActive(true);
    }

}

