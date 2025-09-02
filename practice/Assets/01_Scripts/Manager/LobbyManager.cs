using DG.Tweening;
using PlayFab;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : BasicSingleton<LobbyManager>
{
    public static event Action<string> OnSuccessRegisterRandomDisplayName;
    public static event Action OnReadyToLoadMainScene;

    [Header("Server Ids")]
    public string LiveServerId = "";
    public string TestServerId = "";

    [Header("Managers")]
    public ClockManager ClockManager;
    public ServerHeaderManager HeaderDataManager;

    private PlayerData Player => GameDataManager.PlayerData;

    private void RegisterDataToSaveManager(string playerID)
    {
        print("�ڵ� �����ϵ��� ��� �Ϸ�");

        ClientSaveManager.Add($"SAVE_KEY_VALUE_{playerID}", Player.Value);

        // ���� �����Ͱ� �������� ���⿡ �߰� ���

        ClientSaveManager.Run();
    }
}

