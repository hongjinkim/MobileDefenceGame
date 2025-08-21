using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameDataManager : BasicSingleton<GameDataManager>
{
    // 서버에서 불러오는 플레이어 데이터
    public PlayerData playerData = new PlayerData();



    public static DataBase DataBase => DataBase.Instance;
    public static PlayerData PlayerData => Instance.playerData;

}
