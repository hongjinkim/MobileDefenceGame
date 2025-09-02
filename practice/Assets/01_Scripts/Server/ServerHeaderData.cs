using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ServerHeaderData
{
    public bool ForceOverwrite;
    public DateTimeOffset Stamp;
    public string DeviceUniqueId;
    public string PlayerId;
    public long SaveVersion;
    public DateTimeOffset RestoredTime;
    public bool Administrator;
}

