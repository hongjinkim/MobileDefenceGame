using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSaveDataLoader : MonoBehaviour
{
    private string playerID => PlayFabSettings.staticPlayer.EntityId;

    private IJSONSerializer serializer;


    private void Awake()
    {
        serializer = GetComponent<IJSONSerializer>();
    }


    public bool TryLoadClientData(out PlayerData clientData)
    {
        string checkerKey = $"SAVE_KEY_VALUE_{playerID}";
        bool hasData = PlayerPrefs.HasKey(checkerKey);

        if (hasData == false)
        {
            clientData = null;
            return false;
        }

        var account = ClientSaveManager.Load<PlayerValue>(checkerKey);

        if (string.IsNullOrWhiteSpace(account.NickName) == true)
        {
            print(serializer.Serialize(account));
            print("�г����� ����. �߸��� ������");
            clientData = null;
            return false;
        }

        Debug.Log($"TryLoadClientData playerID:{playerID}");

        clientData = new PlayerData();

        clientData.Value = ClientSaveManager.Load($"SAVE_KEY_VALUE_{playerID}", clientData.Value);
        

        return true;
    }


    // �÷��� �߰��� ������ �ٸ� �����ͷ� �ٲ��־�� �� �� ����... ���� ������ ���� ���� ��ȹ��, ������ ����. 
    public void OverWritePlayerData(string playerID, PlayerData Player)
    {
        ClientSaveManager.Overwrite($"SAVE_KEY_VALUE_{playerID}", Player.Value);
       
        ClientSaveManager.Run();
    }

}