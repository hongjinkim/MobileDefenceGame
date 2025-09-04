using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLoginButtonController : MonoBehaviour
{
    public PlayFabSharedSettings Settings;

    public LoginManager LoginManager;
    public GameObject TesterLoginPopup;

    private void OnEnable()
    { 
        DebugTesterNumberLoginButton.OnClickTesterLogin += TesterLogin;
    }

    private void OnDisable()
    {
        DebugTesterNumberLoginButton.OnClickTesterLogin -= TesterLogin;
    }

    private void TesterLogin(int loginNumber)
    {
        print($"테스터 로그인 번호 : [ {loginNumber} ]");

        LoginManager.LoginWithEmail(
            email: $"{loginNumber:0}@ranking.com",
            password: "ranking",
            username: $"ranking{loginNumber:0}"
        );

        TesterLoginPopup.gameObject.SetActive(false);
    }
}
