using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugTesterNumberLoginButton : MonoBehaviour
{
    public static event Action<int> OnClickTesterLogin;

    private Button Button;

    private void Awake()
    {
        Button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        Button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        Button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        int number = transform.GetSiblingIndex() + 1;
        OnClickTesterLogin?.Invoke(number);
    }

}
