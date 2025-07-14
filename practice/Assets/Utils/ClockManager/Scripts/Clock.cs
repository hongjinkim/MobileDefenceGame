using System;
using UnityEngine;

public class Clock : MonoBehaviour
{
    // 로컬시간 기준으로 '초' 값이 바뀔 때마다 실행됨
    public static event Action<DateTimeOffset> OnChangedSecondLocal;

    // 로컬시간 기준으로 '분' 값이 바뀔 때마다 실행됨
    public static event Action<DateTimeOffset> OnChangedMinuteLocal;

    // 로컬시간 기준으로 '시' 값이 바뀔 때마다 실행됨
    public static event Action<DateTimeOffset> OnChangedHourLocal;

    // 로컬시간 기준으로 날짜가 변경될 때마다 실행됨
    public static event Action<DateTimeOffset> OnChangedDayLocal;

    // UTC 기준으로 날짜가 변경될 때마다 실행됨
    public static event Action<DateTimeOffset> OnChangedDayUTC;

    // UTC 값을 매 프레임 던져줌
    public static event Action<DateTimeOffset> OnChangedFrameUTC;


    // 로컬 시간값을 반환
    public static DateTimeOffset NowLocal => NowUTC.ToLocalTime();

    // UTC 시간값을 반환
    public static DateTimeOffset NowUTC => ServerUTC + FlowTimeSpan;



    // ------ 시간 흐름 구현부 ------ //

    private static TimeSpan FlowTimeSpan => DateTimeOffset.UtcNow - TimeStamp;

    private static DateTimeOffset ServerUTC = DateTimeOffset.UtcNow;
    private static DateTimeOffset TimeStamp = DateTimeOffset.UtcNow;

    private DateTimeOffset nowLocal;
    private DateTimeOffset nowUTC;
    private DateTimeOffset prevLocal;
    private DateTimeOffset prevUTC;
    private bool isInitialized = false;


    private void Update()
    {
        if (isInitialized == false) return;

        nowLocal = NowLocal;
        nowUTC = NowUTC;

        OnChangedFrameUTC?.Invoke(nowUTC);

        if (nowUTC.Day != prevUTC.Day ||
            nowUTC.Month != prevUTC.Month ||
            nowUTC.Year != prevUTC.Year)
        {
            OnChangedDayUTC?.Invoke(nowUTC);
        }

        if (nowLocal.Day != prevLocal.Day ||
            nowLocal.Month != prevLocal.Month ||
            nowLocal.Year != prevLocal.Year)
        {
            OnChangedSecondLocal?.Invoke(nowLocal);
            OnChangedMinuteLocal?.Invoke(nowLocal);
            OnChangedHourLocal?.Invoke(nowLocal);
            OnChangedDayLocal?.Invoke(nowLocal);
        }
        else if (nowLocal.Hour != prevLocal.Hour)
        {
            OnChangedSecondLocal?.Invoke(nowLocal);
            OnChangedMinuteLocal?.Invoke(nowLocal);
            OnChangedHourLocal?.Invoke(nowLocal);
        }
        else if (nowLocal.Minute != prevLocal.Minute)
        {
            OnChangedSecondLocal?.Invoke(nowLocal);
            OnChangedMinuteLocal?.Invoke(nowLocal);
        }
        else if (nowLocal.Second != prevLocal.Second)
        {
            OnChangedSecondLocal?.Invoke(nowLocal);
        }

        prevLocal = nowLocal;
        prevUTC = nowUTC;
    }


    public void SetUTC(DateTimeOffset utc)
    {
        ServerUTC = utc;
        TimeStamp = DateTimeOffset.UtcNow;
        isInitialized = true;
    }

    public void Stop()
    {
        isInitialized = false;
    }

}
