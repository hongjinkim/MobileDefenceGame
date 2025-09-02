using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataUploader : MonoBehaviour
{
    public static PlayerDataUploader Instance { get; private set; } = null;

    public static event Action OnCatchDeviceChange;
    public static event Action OnSuccessUploadAll;

    private static bool forceStop = false;

    public static void ForceStop() => forceStop = true;


    public event Action OnSuccessUpload;

    private PlayerData PlayerData => GameDataManager.PlayerData;

    private IJSONSerializer serializer;
    private UserDataUploader uploader;
    private bool isAutoUploading = false;
    private float autoSaveSeconds = 600f;
    private Coroutine loop = null;
    private float autosave_t = 0;
    private bool forcedAllOnPending = false;
    private bool forcedDirtiesOnPending = false;
    private bool shouldSave = false;

    private float reuploadBufferSeconds = 0f;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            serializer = GetComponent<IJSONSerializer>();
            uploader = new UserDataUploader();
            uploader.OnSuccess += Recheck;
            uploader.OnSuccess += RaiseSuccessSaveAll;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        autosave_t = 0;
    }


    public void RunAutoSave(float seconds)
    {
        autoSaveSeconds = seconds;
        isAutoUploading = true;

        if (loop == null)
        {
            loop = StartCoroutine(SetServerSaveTime());
        }
    }

    public void ForceUploadAll()
    {
        print("강제로 모든 데이터 서버에 저장하기");

        if (forceStop == true) return;

        if (uploader.IsOnPending == true)
        {
            forcedAllOnPending = true;
            return;
        }

        autosave_t = 0;
        Dictionary<string, string> dict = new()
        {
            ["Header"] = ServerHeaderManager.GetHeaderJson()
        };

        var data = PlayerData.GetPlayerDataForServerUpload(serializer); //GetDirtyServerDataAsDictionary(serializer, true);
        foreach (var pair in data)
        {
            dict[pair.Key] = pair.Value;
        }

        uploader.Upload(dict, true);
    }

    public void ForceUploadAllIfValidHeader()
    {
        if (forceStop == true) return;

        if (uploader.IsOnPending == true)
        {
            forcedAllOnPending = true;
            return;
        }

        autosave_t = 0;

        var request = new GetUserDataRequest
        {
            Keys = new List<string> { "Header" },
        };

        PlayFabClientAPI.GetUserData(request, OnSuccessGetHeader, OnError);
    }

    public void SaveToServer()
    {
        if (forceStop == true) return;

        print("저장명령 등록");
        shouldSave = true;
    }



    [ContextMenu("업로드")]
    public void Upload()
    {
        if (forceStop == true) return;

        if (uploader.IsOnPending == true)
        {
            forcedDirtiesOnPending = true;
            return;
        }

        autosave_t = 0;

        var request = new GetUserDataRequest
        {
            Keys = new List<string>
            {
                "Header"
            },
        };

        PlayFabClientAPI.GetUserData(request, OnSuccessGetHeader, OnError);

        /*
        Dictionary<string, string> dict = new()
        {
            ["Header"] = ServerHeaderManager.GetHeaderJson()
        };

        // 정확하게 저장할 요소 구분이 안되는 이유로 임시로 전체저장
        var data = PlayerData.GetDirtyServerDataAsDictionary(serializer, true);
        foreach (var pair in data)
        {
            dict[pair.Key] = pair.Value;
        }

        //print("서버로 보낼 데이터 >> " + Newtonsoft.Json.JsonConvert.SerializeObject(dict));

        uploader.Upload(dict);
        */
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError(error.ErrorMessage);
        Debug.LogError("서버에 데이터 저장을 위한 헤더정보 요청이 실패하였습니다.");
    }

    private void OnSuccessGetHeader(GetUserDataResult result)
    {
        try
        {
            string headerJson = result.Data["Header"].Value;
            var serverHeader = serializer.Deserialize<ServerHeaderData>(headerJson);

            if (serverHeader.DeviceUniqueId == SystemInfo.deviceUniqueIdentifier)
            {
                UploadAfterCheckHeader();
            }
            else
            {
                ForceStop();
                OnCatchDeviceChange?.Invoke();
            }
        }
        catch (Exception e)
        {

            Debug.LogException(e);
            Debug.Log("서버 저장 실패. 헤더를 통한 기기비교를 할 수 없습니다.");
        }
    }

    public void UploadHeaderOnly()
    {
        if (uploader.IsOnPending == true)
        {
            forcedDirtiesOnPending = true;
            return;
        }

        Dictionary<string, string> dict = new()
        {
            ["Header"] = ServerHeaderManager.GetHeaderJson()
        };

        uploader.Upload(dict);
    }


    private void UploadAfterCheckHeader()
    {
        print("헤더 검증 후 저장 시도");

        Dictionary<string, string> dict = new()
        {
            ["Header"] = ServerHeaderManager.GetHeaderJson()
        };

        var data = PlayerData.GetPlayerDataForServerUpload(serializer); //GetDirtyServerDataAsDictionary(serializer, true);
        foreach (var pair in data)
        {
            dict[pair.Key] = pair.Value;
        }

        uploader.Upload(dict);
    }


    private IEnumerator SetServerSaveTime()
    {
        WaitForEndOfFrame waitForEndOfFrame = new();

        while (true)
        {
            yield return waitForEndOfFrame;

            if (forceStop == false)
            {
                if (isAutoUploading == false || autoSaveSeconds <= 0) { continue; }

                if (reuploadBufferSeconds > 0) reuploadBufferSeconds -= Time.unscaledDeltaTime;

                if (autosave_t < autoSaveSeconds) autosave_t += Time.unscaledDeltaTime;

                else
                {
                    autosave_t = 0;
                    if (uploader.IsOnPending == false) { Upload(); }
                }
            }
        }
    }


    //(클라저장이후) 서버저장하기 위해 호출되는 메서드 
    public void CheckServerManualSave()
    {
        if (forceStop == false)
        {
            if (isAutoUploading == false || autoSaveSeconds <= 0) { return; }

            if (shouldSave == true && reuploadBufferSeconds <= 0)
            {
                shouldSave = false;
                autosave_t = 0;
                Upload();
            }
        }
    }


    private void Recheck()
    {
        autosave_t = 0;
        reuploadBufferSeconds = 20f; //서버저장시 다음저장까지 버퍼타임

        if (forcedAllOnPending == true)
        {
            forcedAllOnPending = false;
            forcedDirtiesOnPending = false;
            shouldSave = false;

            ForceUploadAll();
            return;
        }
        else if (forcedDirtiesOnPending == true)
        {
            forcedDirtiesOnPending = false;
            shouldSave = false;

            Upload();
            return;
        }

        shouldSave = false;

        OnSuccessUpload?.Invoke();
    }

    private void RaiseSuccessSaveAll()
    {
        OnSuccessUploadAll?.Invoke();
    }

}
