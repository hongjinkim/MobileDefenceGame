using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupRestoreDisplay : MonoBehaviour
{
    public event Action OnExit;
    public event Action OnSuccessRestore;
    public event Action OnEmptyData;      // �������(�ű� ���� ó��)

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
        TimeSpan span = TimeManager.NowUTC - ServerHeaderManager.StaticHeader.RestoredTime;     // ���������� �ð���
        if (DebugRestoreLimitOnOffButton.Ignore == true || span.TotalSeconds >= 600)
        {
            Debug.Log("����");
            BlockSpinner.SetActive(true);
            ServerToClient.GetUserData();
        }
        else
        {
            Debug.Log($"���� �ȵ�(���� �ð� : {span.TotalSeconds})");
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
        print("������ ���� (���� -> Ŭ���̾�Ʈ)");
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
