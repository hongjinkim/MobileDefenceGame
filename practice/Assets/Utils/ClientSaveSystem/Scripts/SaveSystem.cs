using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;


public class SaveSystem
{
    private static readonly SHA256 sha256 = new SHA256Managed();

    public SaveResponse Save(string key, string json, string timeStamp)
    {
        var response = new SaveResponse();

        if (string.IsNullOrWhiteSpace(key) == true)
        {
            response.Status = ESaveLoadStatus.EmptyKey;
            return response;
        }

        if (string.IsNullOrWhiteSpace(timeStamp) == true)
        {
            response.Status = ESaveLoadStatus.TimeStampMissing;
            return response;
        }

        if (string.IsNullOrWhiteSpace(json) == true)
        {
            response.Status = ESaveLoadStatus.EmptySaveData;
            return response;
        }
            
        try
        {
            SaveDataSchema saveData = PlayerPrefs.HasKey(key) == true
                ? JsonUtility.FromJson<SaveDataSchema>(PlayerPrefs.GetString(key, "{}"))
                : new SaveDataSchema { SaveVersion = 0 };

            saveData.SaveVersion++;
            saveData.TimeStamp = timeStamp;
            saveData.JSON = json;
            saveData.Crypto = Encrypt(saveData);
                                
            var saveJson = JsonUtility.ToJson(saveData);  
            PlayerPrefs.SetString(key, saveJson);

            response.Status = ESaveLoadStatus.Success;
        }
        catch (Exception e) 
        {
            response.Status = ESaveLoadStatus.Failed;
            response.Message = e.Message;
        }

        return response;
    }

    public LoadResponse Load(string key)
    {
        var response = new LoadResponse();

        if (string.IsNullOrWhiteSpace(key) == true)
        {
            response.Status = ESaveLoadStatus.EmptyKey;
            return response;
        }

        if (PlayerPrefs.HasKey(key) == false)
        {
            //Debug.Log($"<PoomSaveSystem> [{key}]키값으로 저장된 데이터를 찾을 수 없습니다.");
            response.Status = ESaveLoadStatus.NotFound;
            return response;
        }

        try
        {
            var loadDataJson = PlayerPrefs.GetString(key, "{}");
            var loadData = JsonUtility.FromJson<SaveDataSchema>(loadDataJson);
            var encrypted = Encrypt(loadData);
                
            if (encrypted.CompareTo(loadData.Crypto) == 0)
            {
                response.TimeStamp = loadData.TimeStamp;
                response.SaveVersion = loadData.SaveVersion;
                response.Status = ESaveLoadStatus.Success;
                response.JSON = loadData.JSON;
            }
            else
            {
                response.Status = ESaveLoadStatus.Hacked;
                response.JSON = loadDataJson;
            }
        }
        catch (Exception e)
        {
            response.Status = ESaveLoadStatus.Failed;
            Debug.LogError($"<PoomSaveSystem> Load Error : {e.Message}");
        }

        return response;
    }

    private string Encrypt(SaveDataSchema schema)
    {
        return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes($"{schema.SaveVersion}{schema.TimeStamp}{schema.JSON}")));
    }



    [Serializable]
    public class SaveDataSchema
    {
        public int SaveVersion;
        public string TimeStamp;
        public string Crypto;
        public string JSON;
    }

}

