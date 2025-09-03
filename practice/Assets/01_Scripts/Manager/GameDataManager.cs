using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameDataManager : BasicSingleton<GameDataManager>
{
    // 서버에서 불러오는 플레이어 데이터
    [ShowInInspector]private PlayerData playerData = new PlayerData();

    private bool _playerLoaded; // 중복 방지


    private void OnEnable()
    {
        // DataBase 로드 완료 시그널 구독
        DataBase.OnLoaded += HandleMasterLoaded;

        // 이미 DataBase가 로드 끝난 상태로 진입할 수 있으니 한번 체크
        if (DataBase.IsLoaded)
            HandleMasterLoaded();
    }

    private void OnDisable()
    {
        DataBase.OnLoaded -= HandleMasterLoaded;
    }

    private void HandleMasterLoaded()
    {
        if (_playerLoaded) return; // 중복 호출 방지

        // 1) PlayerData 로드 (서버 연동 시 여기서 await/콜백 연결)
        playerData.LoadData();

        _playerLoaded = true;

        // 2) 이제 최종 준비 완료 이벤트 발행 (UI/시스템들이 이걸 듣게)
        EventManager.Raise(EEventType.DataLoaded);
    }


    public static DataBase DataBase => DataBase.Instance;
    public static PlayerData PlayerData => Instance.playerData;

}
