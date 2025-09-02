using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOnRefocusHandler : MonoBehaviour
{
    public ClockManager ClockManager;

    public string LiveServerId = "";
    public DebugModeSensor DebugSensor;

    private void Awake()
    {
        ClockManager.ShouldRequestOnRefocusFunc = GetTimeRequestCondition;
    }

    private bool GetTimeRequestCondition()
    {
        return DebugSensor.IsDebugMode == false ||
               PlayFabSettings.staticSettings.TitleId == LiveServerId;
    }

}

