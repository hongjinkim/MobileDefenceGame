using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Clock))]
public class ClockManager : MonoBehaviour
{
    public static ClockManager Instance { get; private set; } = null;

    public static Action OnInternetNotReachable;
    public static Action OnFailedSetClock;
    public static Action OnSuccessSetValidClockAfterLogin;
    public static Action OnSuccessSetValidClock;

    [Header("����")]
    public EClockSettings Settings = EClockSettings.UseNetworkTime;
    [Tooltip("������ 0000-00-00T00:00:00Z")]
    public string CustomTimeUTC = string.Empty;
    public bool RequestAgainOnRefocus = false;
    public bool RequestOnStart = false;
    public bool LogOnSuccess = false;

    [Header("���� ����")]
    public List<BaseUTCRequestHandler> Order = new();

    public Func<bool> ShouldRequestOnRefocusFunc = null;

    private Clock clock = null;
    private int index = 0;
    private bool isActivated = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            clock = GetComponent<Clock>();
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        if (RequestOnStart == true) { StartRequest(); }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (isActivated == false) return;

        if (RequestAgainOnRefocus == false) return;

        if (ShouldRequestOnRefocusFunc != null &&
            ShouldRequestOnRefocusFunc() == false)
        {
            return;
        }

        if (focus == true)
        {
            index = 0;
            Order[index].Request(OnSuccess, OnFailed);
        }
        else
        {
            clock.Stop();
        }
    }


    public void StartRequest()
    {
        if (Settings == EClockSettings.UseDeviceTime)
        {
            OnSuccess(DateTimeOffset.UtcNow);
            return;
        }

        if (Settings == EClockSettings.UseCustomTime)
        {
            if (string.IsNullOrWhiteSpace(CustomTimeUTC) == true)
            {
                Debug.LogError($"<{GetType()}> �ð��Է°��� ����ֽ��ϴ�.");
                OnFailedSetClock?.Invoke();
                return;
            }

            try
            {
                var customTime = DateTimeOffset.Parse(CustomTimeUTC).ToUniversalTime();
                OnSuccess(customTime);
            }
            catch
            {
                Debug.LogError($"<{GetType()}> �Է��� �ð����� ��ȯ�� �� �����ϴ�.");
                OnFailedSetClock?.Invoke();
            }

            return;
        }

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.LogError($"<{GetType()}> ���ͳ� ������ �Ұ����մϴ�.");
            OnInternetNotReachable?.Invoke();
            OnFailedSetClock?.Invoke();
            return;
        }

        if (Order.Count == 0)
        {
            Debug.LogError($"<{GetType()}> ���ӹ���� �������ּ���.");
            OnFailedSetClock?.Invoke();
            return;
        }

        index = 0;
        Order[index].Request(OnSuccess, OnFailed);
    }


    private void OnSuccess(DateTimeOffset utc)
    {
        clock.SetUTC(utc);

        if (isActivated == false)
        {
            isActivated = true;
            OnSuccessSetValidClockAfterLogin?.Invoke();
        }
        else
        {
            OnSuccessSetValidClock?.Invoke();
        }

        index = 0;

        if (LogOnSuccess == true)
        {
            print($"<{GetType()}> �ð��� �������� ���� = {utc:o}");
        }
    }

    private void OnFailed()
    {
        index++;
        if (index < Order.Count)
        {
            Order[index].Request(OnSuccess, OnFailed);
        }
        else
        {
            index = 0;
            OnFailedSetClock?.Invoke();
        }
    }



    public enum EClockSettings
    {
        UseNetworkTime,
        UseDeviceTime,
        UseCustomTime
    }

}
