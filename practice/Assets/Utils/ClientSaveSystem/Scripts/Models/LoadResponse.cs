using System;


[Serializable]
public class LoadResponse
{
    public ESaveLoadStatus Status = ESaveLoadStatus.None;
    public long SaveVersion;
    public string TimeStamp;
    public string JSON;
}

