using TMPro;
using UnityEngine;



public class PoomTestServerChecker : MonoBehaviour
{
    [Header("PlayFab")]
    public PlayFabSharedSettings PlayFabSettingsAsset;
    public string LiveServer;
    public string TestServer;

    [Header("UI Elements")]
    public TMP_Text NoticeTMP_Test;
    public TMP_Text NoticeTMP_Live;

    private void Start()
    {
        if (PlayFabSettingsAsset == null ||
            string.IsNullOrWhiteSpace(LiveServer) == true ||
            string.IsNullOrWhiteSpace(LiveServer) == true)
        {
            Debug.LogError($"<{GetType()}> Error. �ν����Ϳ� ��� ���� ����/�Է����ּ���.");
            return;
        }

        if (PlayFabSettingsAsset.TitleId == LiveServer.ToUpper())
        {
#if UNITY_EDITOR
            NoticeTMP_Live.enabled = true;
            NoticeTMP_Test.enabled = false;
#else
            NoticeTMP_Live.enabled = false;
            NoticeTMP_Test.enabled = false;
#endif
        }
        else if (PlayFabSettingsAsset.TitleId == TestServer.ToUpper())
        {
#if UNITY_EDITOR
            NoticeTMP_Live.enabled = false;
            NoticeTMP_Test.enabled = true;
#else
            NoticeTMP_Live.enabled = false;
            NoticeTMP_Test.enabled = false;
#endif
        }
        else
        {
            Debug.LogError("Unknown Server Setting !!");
        }
    }


    public void ForceHide() => gameObject.SetActive(false);

}