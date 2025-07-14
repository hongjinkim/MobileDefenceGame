
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  Poom Client Save Manager : Version 2.1  ( Last Updated 2023-08-24 )
 *  패치 내역 : 사용성 전면 개편, 의존성 개선
 */

public class ClientSaveManager : MonoBehaviour
{
    // 데이터 조작 발견 시 실행되는 이벤트
    public static event Action<string> OnDataHackingDetected;

    private static bool forceStop = false;


    // 저장데이터 등록하기
    public static void Add<T>(string saveKey, T poomSaveDataObject)
        where T : IDirtyFlag, ISaveObject
        => instance.Register(saveKey, poomSaveDataObject, false);

    // 등록한 저장데이터 초기화
    public static void Clear() => instance.ClearRegistered();

    // 강제로 즉시 저장하기
    public static void ForceSave()
    {
        if (forceStop == true) return;        
        instance.SaveImmediate();
    }

    // 강제로 즉시 모두 저장하기
    public static void ForceSaveAll()
    {
        if (forceStop == true) return;
        instance.SaveAll();
    }

    // 데이터 불러오기
    public static T Load<T>(string key, T init = default) => instance.TryLoad(key, init);

    // 저장데이터 등록하기(덮어쓰기)
    public static void Overwrite<T>(string saveKey, T poomSaveDataObject)
        where T : IDirtyFlag, ISaveObject 
        => instance.Register(saveKey, poomSaveDataObject, true);

    // 자동저장기능 실행하기 (등록 완료 후 실행)
    public static void Run() => instance.SetEnableAutoSave();
    
    // 저장하기
    public static void Save() 
    {
        if (forceStop == true) return;
        instance.CheckToSave();
    }

    // 강제 저장 멈춤
    public static void ForceStop() => forceStop = true;


    // ------ 기능 구현 ------ //

    private static ClientSaveManager instance = null;
    private static IJSONSerializer serializer = null;
    private static IClockResponser clock = null;

//#if UNITY_EDITOR
    [Header("UNITY EDITOR ONLY")]
    [SerializeField]
    private List<string> RegisteredSaveKeys = new();
//#endif

    private readonly Dictionary<string, IDirtyFlag> dirtyFlags = new();
    private readonly Dictionary<string, ISaveObject> saveObjects = new();
    private readonly SaveSystem saveSystem = new();
    private Coroutine autosaveProcess = null;
    private bool isAutoSaveEnabled = false;
    private bool shouldSaveThisSecond = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Initialize();
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (serializer == null || clock == null)
        {
            Debug.LogError($"<{GetType()}>가 준비되지 않았습니다. 자동저장 기능을 수행하지 않습니다.");
            return;
        }
    }


    // 초기화 : JSON 시리얼라이저 및 시간관리자 연결
    private void Initialize()
    {
        if (transform.TryGetComponent(out IJSONSerializer jsonSerializer) == true)
        {
            serializer = jsonSerializer;
        }
        else
        {
            Debug.LogError("PoomSaveManager에는 IJSONSerializer를 연결해야만 합니다.");
        }

        if (transform.TryGetComponent(out IClockResponser clockResponser) == true)
        {
            clock = clockResponser;
        }
        else
        {
            Debug.LogError("PoomSaveManager에는 IPoomClockResponser를 연결해야만 합니다.");
        }
    }

    // 자동 저장할 데이터 클래스 등록. 데이터는 PoomBaseSaveData를 상속해야만 함
    private void Register<T>(string saveKey, T dataObject, bool overwrite)
        where T : IDirtyFlag, ISaveObject
    {
        if (overwrite == true || saveObjects.ContainsKey(saveKey) == false)
        {
            dirtyFlags[saveKey] = dataObject;
            saveObjects[saveKey] = dataObject;

#if UNITY_EDITOR
            if (RegisteredSaveKeys.Contains(saveKey) == true) { RegisteredSaveKeys.Remove(saveKey); }
            RegisteredSaveKeys.Add(saveKey);
#endif
        }
        else
        {
            Debug.LogError($"[!] <{GetType()}> 같은 저장키 값으로 등록할 수 없습니다. 저장키값='{saveKey}'");
        }
    }

    private void ClearRegistered()
    {
        dirtyFlags.Clear();
        saveObjects.Clear();
        RegisteredSaveKeys.Clear();
        isAutoSaveEnabled = false;
    }

    // 자동 저장할 데이터 등록을 마친 뒤 실행하면 자동저장 프로세스 가동됨
    private void SetEnableAutoSave()
    {
        if (serializer == null || clock == null)
        {
            Debug.LogError($"<{GetType()}>가 준비되지 않아 자동저장 기능을 가동할 수 없습니다.");
        }

        isAutoSaveEnabled = true;

        if (autosaveProcess == null)
        {
            autosaveProcess = StartCoroutine(OnEndOfFrame());
#if UNITY_EDITOR
            print($"<PoomSaveManager> 자동저장 시스템이 작동을 시작합니다.");
#endif 
        }
    }

    // 매 프레임 끝나는 시점에 실행되는 자동저장 프로세스
    private IEnumerator OnEndOfFrame()
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        WaitForSeconds tick = new WaitForSeconds(1f);

        while (true)
        {
            yield return tick;
            yield return waitForEndOfFrame;

            if (forceStop == false)
            {
                if (isAutoSaveEnabled == false) continue;

                //이번 second에 클라저장할게 있으면 서버보다 먼저 저장
                if (shouldSaveThisSecond == true)
                {
                    shouldSaveThisSecond = false;
                    SaveDirtyDataObjects();
                }

                //서버저장 여부를 체크 (이번 second에 클라저장할게 없어도 작동필요 -> 이전 second에 서버저장요청했지만 대기중일수 있음)
                //PlayerDataUploader.Instance.CheckServerManualSave();
            }
        }
    }
    // 저장할 것이 있는지 확인하여 SaveSystem의 설정에 따라 저장함
    private void SaveDirtyDataObjects()
    {
        foreach ((var key, var checker) in dirtyFlags)
        {
            if (checker.IsDirty() == true)
            {
                TrySave(key, saveObjects[key], clock.NowUTCString());
                checker.Refresh();
            }
        }
    }

    // 자동저장할 것이 생겼다고 체크
    private void CheckToSave() => shouldSaveThisSecond = true;

    // 자동저장과 관계없이 즉시 변경된 데이터 저장
    private void SaveImmediate() => SaveDirtyDataObjects();

    // 자동저장과 관계없이 즉시 모든 데이터 저장
    private void SaveAll()
    {
        string timeStamp = clock.NowUTCString();
        foreach ((var key, var data) in saveObjects)
        {
            print($"강제클라저장:{key} >> {Newtonsoft.Json.JsonConvert.SerializeObject(data)}"); 
            TrySave(key, data, timeStamp);
            dirtyFlags[key].Refresh();
        }
    }

    // 저장 시도. 에러 로그 처리
    private void TrySave(string key, ISaveObject data, string timeStamp)
    {
        var response = saveSystem.Save(key, data.ToJSON(serializer), timeStamp);

        if (response.Status == ESaveLoadStatus.Success)
        {
            return;
        }
        else
        {
            Debug.LogError($"<{GetType()}.Save> 저장 에러. 이유 = {response.Status}. {response.Message}");
        }
    }

    // 불러오기 시도. 에러 로그 처리
    private T TryLoad<T>(string key, T init)
    {
        var response = saveSystem.Load(key);

        if (response.Status == ESaveLoadStatus.Success)
        {
            try
            {
                if (string.IsNullOrEmpty(response.JSON) == true)
                {
                    return init;
                }
                else
                {
                    T data = serializer.Deserialize<T>(response.JSON);
                    return data;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);  
                return init;
            }
        }
        else if (response.Status == ESaveLoadStatus.NotFound)
        {
            return init;
        }
        else
        {
            Debug.LogError($"<{GetType()}.Load> 로드 에러. 이유 = {response.Status}");
            
            if (response.Status == ESaveLoadStatus.Hacked)
            {
                OnDataHackingDetected?.Invoke(response.JSON);
            }

            return init;
        }
    }

}