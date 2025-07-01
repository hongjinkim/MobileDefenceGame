using System.Collections;
using System.Collections.Generic;
using UGS;
using UnityEngine;

public class DataManager : BasicSingleton<DataManager>
{
    private PlayerData _playerData = new();
    private InitialData _initialData = new();
    private Dictionary<int, HeroData> heroDict = new Dictionary<int, HeroData>();

    private void Awake()
    {
        UnityGoogleSheet.LoadAllData();
        // 영웅 데이터 로드 및 Dictionary에 삽입
        LoadAllHeroData();
    }

    private void LoadAllHeroData()
    {
        // 이 메소드는 Awake에서 호출되어 영웅 데이터를 초기화합니다.
        foreach (var hero in DataTable.영웅.영웅List)
        {
            heroDict[hero.영웅_id] = new HeroData(hero);
        }
    }

    public static PlayerData GetPlayerData()
    {
        return Instance._playerData;
    }

    public static InitialData GetInitialData()
    {
        return Instance._initialData;
    }
    public static HeroData GetHeroData(int id)
    {
        if (Instance.heroDict.TryGetValue(id, out HeroData heroData))
        {
            return heroData;
        }
        else
        {
            Debug.LogError($"Hero with ID {id} not found.");
            return null;
        }
    }
    // Uncomment the following lines to debug the data loading

    //foreach (var value in DefaultTable.Data.DataList)
    //       {
    //           Debug.Log(value.index + "," + value.intValue + "," + value.strValue);
    //       }
    //var dataFromMap = DefaultTable.Data.DataMap[0];
    //Debug.Log("dataFromMap : " + dataFromMap.index + ", " + dataFromMap.intValue + "," + dataFromMap.strValue);

}
