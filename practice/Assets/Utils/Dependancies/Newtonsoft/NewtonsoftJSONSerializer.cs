using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;


public class NewtonsoftJSONSerializer : MonoBehaviour, IJSONSerializer
{
    private JsonSerializerSettings settings;
    private bool isInitialized = false;


    private void Awake()
    {
        InitializeSettings();
    }


    public T Deserialize<T>(string json)
    {
        if (isInitialized == false) InitializeSettings();
        return JsonConvert.DeserializeObject<T>(json, settings);
    }

    public string Serialize<T>(T dataObject)
    {
        if (isInitialized == false) InitializeSettings();
        return JsonConvert.SerializeObject(dataObject, settings);
    }


    private void InitializeSettings()
    {
        settings = new JsonSerializerSettings { DateFormatString = "o" };
        settings.Converters.Add(new StringEnumConverter { AllowIntegerValues = true });
        isInitialized = true;
    }

}


