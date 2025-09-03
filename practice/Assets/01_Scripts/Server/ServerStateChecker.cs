using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerStateChecker : MonoBehaviour
{
    public static event Action<string> OnServerUnderConstruction;

    private IJSONSerializer serializer;


    private void Awake()
    {
        serializer = GetComponent<IJSONSerializer>();
    }

    private void OnEnable()
    {
        ServerTitleDataManager.OnSuccess += CheckServer;
    }

    private void OnDisable()
    {
        ServerTitleDataManager.OnSuccess -= CheckServer;
    }


    private void CheckServer(Dictionary<string, string> dictionary)
    {
        //print($"���� ���� Ȯ�� �� >> {serializer.Serialize(dictionary)}");


        if (dictionary.ContainsKey("ServerState") == false)
        {
            Debug.Log("���� ���¸� Ȯ���� �����Ͱ� �����ϴ�");
        }

        var server = serializer.Deserialize<ServerState>(dictionary["ServerState"]);

        if (server.State == "Running") return;

        if (server.State == "Stop")
        {
            OnServerUnderConstruction?.Invoke(server.Message);
            return;
        }

        Debug.LogError($"�������¸� �� �� �����ϴ�. ����: {server.State} - {server.Message}");
    }



    public class ServerState
    {
        public string State;
        public string Message;
    }

}
