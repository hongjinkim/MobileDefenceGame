using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PopupPrivatePolicy : MonoBehaviour
{
    public Button AgreeButton;
    private void Awake()
    {
        AgreeButton.onClick.AddListener(AgreeToPrivacyPolicy);
    }

    public void AgreeToPrivacyPolicy()
    {
        gameObject.SetActive(false);
    }
}
