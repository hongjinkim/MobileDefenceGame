using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeValueAdaptor : MonoBehaviour
{
    public TimeManager timeManager;


    public void OnEnable()
    {
        ClockManager.OnSuccessSetValidClockAfterLogin += SetValidTime;
        ClockManager.OnSuccessSetValidClock += SetValidTime;
    }

    private void OnDisable()
    {
        ClockManager.OnSuccessSetValidClockAfterLogin -= SetValidTime;
        ClockManager.OnSuccessSetValidClock -= SetValidTime;
    }


    private void SetValidTime()
    {
        var nowUtc = Clock.NowUTC;
        timeManager.SetTime(nowUtc);
    }

}


