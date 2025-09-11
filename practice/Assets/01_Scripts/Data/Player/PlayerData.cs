using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class PlayerData
{
    //������ ������ (������ �ְ� ���� �����͸� ���⿡ �ۼ�)
    [TabGroup("Tabs", "PlayerValue"), HideLabel][InlineProperty] public PlayerValue Value = new PlayerValue();
    [TabGroup("Tabs", "Hero"), HideLabel][InlineProperty] public PlayerHeroValue Hero = new PlayerHeroValue();

    // ������ �ε�, ���� �������� �޾ƿ��� �κ� ���� ����
    public void LoadData()
	{
		//TestLoadPlayerValue();
		//TestLoadPlayerHeroValue();
    }

	private void TestLoadPlayerValue()
	{
		// �׽�Ʈ�� ������ �ε�
		Value.MaxEnergy = 30;
		Value.CurrentEnergy = 5;
		Value.Gold = 1000;
		Value.Crystal = 200;
    }
	private void TestLoadPlayerHeroValue()
	{
		var master = DataBase.GetHeroData();

        // 1) ����(�÷���) Ű ���� (������ ��ü�� Hidden���� �ʱ�ȭ)
        Hero.EnsureCollectionKeys(master);

        // 2) �׽�Ʈ�� ���� ����: ������ ��ü�� Seen���� ����
        Hero.SetAllSeen(master);

        // 3) �׽�Ʈ�� ���� ����: �������� �� 5���� ȹ���Ѵٰ� ����
        var first5 = master.HeroDict.Keys.Take(5).ToList();
        if (first5.Count >= 1) Hero.Acquire(first5[0], level: 5, star: 1);
        if (first5.Count >= 2) Hero.Acquire(first5[1], level: 10, star: 2);
        if (first5.Count >= 3) Hero.Acquire(first5[2], level: 10, star: 3);
        if (first5.Count >= 4) Hero.Acquire(first5[3], level: 10, star: 3);
        if (first5.Count >= 5) Hero.Acquire(first5[4], level: 10, star: 3);


        // 4) �� ����(������ �ֵ鸸, �ߺ� ����, �ִ� 5ĭ)
        Hero.SetDeck("Team1", first5, maxSize: 5, ownedOnly: true);
    }

    public void Overwrite(Dictionary<string, string> dict)
    {
        if (dict.ContainsKey("Header") == true)
        {
            ServerHeaderManager.StaticHeader
                = JsonConvert.DeserializeObject<ServerHeaderData>(dict["Header"]);
        }

        if (dict.ContainsKey(ServerDataKeys.ACCOUNT) == true)
        {
            var data = JsonConvert.DeserializeObject<ServerSaveDataAccount>(dict[ServerDataKeys.ACCOUNT]);
            Overwrite(data);
        }

        if (dict.ContainsKey(ServerDataKeys.CONTENTS) == true)
        {
            var data = JsonConvert.DeserializeObject<ServerSaveDataContents>(dict[ServerDataKeys.CONTENTS]);
            Overwrite(data);
        }

        if (dict.ContainsKey(ServerDataKeys.LEVEL) == true)
        {
            var data = JsonConvert.DeserializeObject<ServerSaveDataLevel>(dict[ServerDataKeys.LEVEL]);
            Overwrite(data);
        }

        if (dict.ContainsKey(ServerDataKeys.RECORD) == true)
        {
            var data = JsonConvert.DeserializeObject<ServerSaveDataRecord>(dict[ServerDataKeys.RECORD]);
            Overwrite(data);
        }

        if (dict.ContainsKey(ServerDataKeys.MAX) == true)
        {
            var data = JsonConvert.DeserializeObject<ServerSaveDataMax>(dict[ServerDataKeys.MAX]);
            Overwrite(data);
        }
    }

    public void Overwrite(ServerSaveDataAccount data)
    {
        Value = data.Account;

    }

    public void Overwrite(ServerSaveDataContents data)
    {
 
    }

    public void Overwrite(ServerSaveDataLevel data)
    {

    }

    public void Overwrite(ServerSaveDataRecord data)
    {

    }

    public void Overwrite(ServerSaveDataMax data)
    {

    }

    public Dictionary<string, string> GetPlayerDataForServerUpload(IJSONSerializer serializer)
    {
        Dictionary<string, string> result = new();

        var account = new ServerSaveDataAccount
        {
            Account = Value,
        };

        var contents = new ServerSaveDataContents
        {

        };

        var level = new ServerSaveDataLevel
        {

        };

        var record = new ServerSaveDataRecord
        {

        };

        var max = new ServerSaveDataMax
        {

        };

        result["Account"] = serializer.Serialize(account);
        result["Contents"] = serializer.Serialize(contents);
        result["Level"] = serializer.Serialize(level);
        result["Record"] = serializer.Serialize(record);
        result["Max"] = serializer.Serialize(max);

        return result;
    }

    [Serializable]
    public class ServerSaveDataAccount
    { 
        public PlayerValue Account;
    }

    [Serializable]
    public class ServerSaveDataContents
    {


    }

    [Serializable]
    public class ServerSaveDataLevel
    {


    }

    [Serializable]
    public class ServerSaveDataRecord
    {


    }

    [Serializable]
    public class ServerSaveDataMax
    {

    }
}