using Newtonsoft.Json;
using System;


[Serializable]
public class SaveResponse
{
    public ESaveLoadStatus Status = ESaveLoadStatus.None;
    public string Message = string.Empty;

}
