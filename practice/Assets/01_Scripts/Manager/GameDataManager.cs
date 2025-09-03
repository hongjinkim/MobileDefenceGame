using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameDataManager : BasicSingleton<GameDataManager>
{
    // �������� �ҷ����� �÷��̾� ������
    [ShowInInspector]private PlayerData playerData = new PlayerData();

    private bool _playerLoaded; // �ߺ� ����


    private void OnEnable()
    {
        // DataBase �ε� �Ϸ� �ñ׳� ����
        DataBase.OnLoaded += HandleMasterLoaded;

        // �̹� DataBase�� �ε� ���� ���·� ������ �� ������ �ѹ� üũ
        if (DataBase.IsLoaded)
            HandleMasterLoaded();
    }

    private void OnDisable()
    {
        DataBase.OnLoaded -= HandleMasterLoaded;
    }

    private void HandleMasterLoaded()
    {
        if (_playerLoaded) return; // �ߺ� ȣ�� ����

        // 1) PlayerData �ε� (���� ���� �� ���⼭ await/�ݹ� ����)
        playerData.LoadData();

        _playerLoaded = true;

        // 2) ���� ���� �غ� �Ϸ� �̺�Ʈ ���� (UI/�ý��۵��� �̰� ���)
        EventManager.Raise(EEventType.DataLoaded);
    }


    public static DataBase DataBase => DataBase.Instance;
    public static PlayerData PlayerData => Instance.playerData;

}
