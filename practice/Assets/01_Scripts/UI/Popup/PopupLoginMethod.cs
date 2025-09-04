using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupLoginMethod : MonoBehaviour
{
    [HideInInspector]public LoginManager LoginManager;
    public GameObject Block;
    public Button GPGSButton;
    public Button GuestButton;


    private void OnEnable()
    {
        Block.SetActive(false);
        GPGSButton.onClick.AddListener(OnClickGPGSLogin);
#if UNITY_EDITOR
        GuestButton.onClick.AddListener(OnClickGuestLogin);
#endif
    }

    private void OnDisable()
    {
        GPGSButton.onClick.RemoveListener(OnClickGPGSLogin);
#if UNITY_EDITOR
        GuestButton.onClick.RemoveListener(OnClickGuestLogin);
#endif
    }

    private void OnClickGPGSLogin()
    {
        //Block.SetActive(true);
        gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    private void OnClickGuestLogin()
    {
        //Block.SetActive(true);
        LoginManager.LoginGuest();
        gameObject.SetActive(false);
    }
#endif
}

