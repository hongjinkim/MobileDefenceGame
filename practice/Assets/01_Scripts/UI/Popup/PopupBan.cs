using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupBan : MonoBehaviour
{
    [SerializeField] private PopupEvent PopupEvent;
    //[SerializeField] private LocalizedTMPKeyParams LocalExitText;
    [SerializeField] private EventButton ExitButton;

    private float currentTimer = 0;
    private float exitTime = 5f;

    private void OnEnable()
    {
        PopupEvent.Open();
        UpdateUI();
        StartCoroutine(ExitTimer());
        ExitButton.OnClick += DoExitButton;
    }

    private void OnDisable()
    {
        ExitButton.OnClick -= DoExitButton;
    }

    private void DoExitButton()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }

    private void UpdateUI()
    {
        //LocalExitText.UpdateParameters((false, $"{(exitTime - currentTimer)}"));
    }

    private IEnumerator ExitTimer()
    {
        currentTimer = 0;
        while (currentTimer < exitTime)
        {
            yield return new WaitForSeconds(1f);
            currentTimer += 1;
            UpdateUI();
        }

        Debug.Log("게임 종료");
        Application.Quit();
    }
}
