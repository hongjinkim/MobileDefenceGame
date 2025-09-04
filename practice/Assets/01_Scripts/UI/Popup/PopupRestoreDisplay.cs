using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupRestoreDisplay : MonoBehaviour
{
    public event Action OnExit;
    public event Action OnSuccessRestore;
    public event Action OnEmptyData;      // 비어있음(신규 유저 처리)

    [SerializeField] private PopupEvent PopupEvent;
    [SerializeField] private EventButton RestoreButton;

    [SerializeField] private EventButton ExitButton;
    [SerializeField] private TextMeshProUGUI RestoreDateText;
    [SerializeField] private GameObject ContinuousRestorePopup;
    [SerializeField] private GameObject BlockSpinner;
    public ServerPlayerDataReceiver ServerToClient;

    private void OnEnable()
    {
        RestoreButton.OnClick += DoRestoreButton;
        ExitButton.OnClick += DoExitButton;

        ServerToClient.OnSuccessGetEmptyData += DataReceiveEmpty;
        ServerToClient.OnSuccessGetJsonDictionary += DataReceiveJson;
        PopupEvent.Open();

        UpdateUI();
    }

    private void OnDisable()
    {
        RestoreButton.OnClick -= DoRestoreButton;
        ExitButton.OnClick -= DoExitButton;
    }

    private void UpdateUI()
    {
        var utc = ServerHeaderManager.StaticHeader.RestoredTime;
        RestoreDateText.text = utc.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
    }

    private void DoRestoreButton()
    {
        Debug.Log("Restore Button");
        TimeSpan span = TimeManager.NowUTC - ServerHeaderManager.StaticHeader.RestoredTime;     // 복원까지의 시간차
        if (DebugRestoreLimitOnOffButton.Ignore == true || span.TotalSeconds >= 600)
        {
            Debug.Log("복원");
            BlockSpinner.SetActive(true);
            ServerToClient.GetUserData();
        }
        else
        {
            Debug.Log($"복원 안됨(남은 시간 : {span.TotalSeconds})");
            ContinuousRestorePopup.gameObject.SetActive(true);
        }
    }

    private void DoExitButton()
    {
        OnExit?.Invoke();
        ClosePopup();
        Application.Quit();
    }

    private void ClosePopup()
    {
        PopupEvent.Close();
        Invoke(nameof(DisablePopup), 1f);
    }

    private void DisablePopup() => gameObject.SetActive(false);

    private void DataReceiveEmpty()
    {
        OnEmptyData?.Invoke();
        ClosePopup();
    }

    private void DataReceiveJson(Dictionary<string, string> dict)
    {
        print("데이터 복원 (서버 -> 클라이언트)");
        foreach (var k in dict.Keys)
        {
            Debug.Log(dict[k]);
        }

        GameDataManager.PlayerData.Overwrite(dict);
        ServerHeaderManager.UpdateRestoreDateTime(TimeManager.NowUTC);
        PlayerDataUploader.Instance.UploadHeaderOnly();

        OnSuccessRestore?.Invoke();

        ClosePopup();
    }
}
