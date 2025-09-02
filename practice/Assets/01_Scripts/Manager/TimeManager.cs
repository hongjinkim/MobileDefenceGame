using PlayFab;
using System;
using UnityEngine;


public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; } = null;

    public static event Action SetClockReady;
    public static event Action<DateTimeOffset> OnChangedFrameUTC;
    public static event Action<DateTimeOffset> OnChangedDayUTC;
    public static event Action<DateTimeOffset> OnSecondChanged;
    public static event Action<DateTimeOffset> OnMinuteChanged;
    public static event Action<DateTimeOffset> OnHourChanged;
    public static event Action<DateTimeOffset> OnDateChanged;


    public static DateTimeOffset NowLocal => NowUTC.ToLocalTime();

    public static DateTimeOffset NowUTC => ServerUTC + FlowTimeSpan;

    public static TimeSpan TodayRestTimeLocal => NowLocal.Date.AddDays(1) - NowLocal;

    public static TimeSpan Until(DateTimeOffset dateTimeOffsetUTC) => dateTimeOffsetUTC - NowUTC;

    public static Day TodayDayOfWeek => NowLocal.DayOfWeek switch
    {
        DayOfWeek.Sunday => Day.Sunday,
        DayOfWeek.Monday => Day.Monday,
        DayOfWeek.Tuesday => Day.Tuesday,
        DayOfWeek.Wednesday => Day.Wednesday,
        DayOfWeek.Thursday => Day.Thursday,
        DayOfWeek.Friday => Day.Friday,
        DayOfWeek.Saturday => Day.Saturday,
        _ => throw new Exception("[!] 요일 변환 에러")
    };

    private static TimeSpan FlowTimeSpan = TimeSpan.Zero;

    private static DateTimeOffset ServerUTC { get; set; } = DateTimeOffset.UtcNow;

    //private static DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.UtcNow;

    public string TestServerId = "";
    public bool UseDeviceTime = false;

    private DateTimeOffset nowLocal;
    private DateTimeOffset prevLocal;
    private DateTimeOffset nowUTC;
    private DateTimeOffset prevUTC;
    private bool isInitialized = false;
    private bool isCredential = false;
    private float savedScale;

    public static float MainTimeScale = 1.2f; //전체 게임진행속도기준 (개발단에서 조정, 플레잉코드에서는 변경하지 말것)
    public static float DebugWeight = 1; //디버그에서 조절하는 속도

    public static void ControlTimeScale(float ratio)
    {
        Time.timeScale = Mathf.Max(0, MainTimeScale) * DebugWeight * ratio;
    }

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else if (Instance != this) { Destroy(gameObject); }

        if (UseDeviceTime == true) { SetTime(DateTimeOffset.UtcNow); }
    }

    private void Start()
    {
        MainTimeScale = DataBase.Instance.initialData.GamePlayingSpeed;
        ControlTimeScale(1f);
    }


    private void Update()
    {
        if (isInitialized == false) return;
        if (isCredential == false) return;

        FlowTimeSpan += TimeSpan.FromSeconds(Time.unscaledDeltaTime);

        nowUTC = NowUTC;
        nowLocal = NowLocal;

        OnChangedFrameUTC?.Invoke(NowUTC);

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
            OnSecondChanged?.Invoke(nowLocal);
            OnMinuteChanged?.Invoke(nowLocal);
            OnHourChanged?.Invoke(nowLocal);
            OnDateChanged?.Invoke(nowLocal);
        }
        else if (nowLocal.Hour != prevLocal.Hour)
        {
            OnSecondChanged?.Invoke(nowLocal);
            OnMinuteChanged?.Invoke(nowLocal);
            OnHourChanged?.Invoke(nowLocal);
        }
        else if (nowLocal.Minute != prevLocal.Minute)
        {
            OnSecondChanged?.Invoke(nowLocal);
            OnMinuteChanged?.Invoke(nowLocal);
        }
        else if (nowLocal.Second != prevLocal.Second)
        {
            OnSecondChanged?.Invoke(nowLocal);
        }

        prevUTC = nowUTC;
        prevLocal = nowLocal;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (isInitialized == false) return;

        if (PlayFabSettings.staticSettings.TitleId == TestServerId) return;

        if (focus == false)
        {
            isCredential = false;
        }
    }

    public void SetTime(DateTimeOffset utc)
    {
        print("STAMPED UTC : " + utc.ToString("o"));
        ServerUTC = utc;
        FlowTimeSpan = TimeSpan.Zero;
        //TimeStamp = DateTimeOffset.UtcNow;

        isInitialized = true;
        SetClockReady?.Invoke();
        isCredential = true;
    }

    public void SaveTimeScale()
    {
        savedScale = Time.timeScale;
    }

    public void LoadTimeScale()
    {
        Time.timeScale = savedScale;
    }

    // ------ Debug ------

    public void DebugSetTime(DateTimeOffset utc) => SetTime(utc);

}


public enum Day
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday,
}
