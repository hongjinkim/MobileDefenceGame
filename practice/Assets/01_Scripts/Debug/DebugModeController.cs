using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugModeController : MonoBehaviour
{
    public static DebugModeController Instance;

    public static bool IsDebugMode = true;

    public GameObject TargetCamera;


    private void Awake()
    {
        Instance = this;

        var sensor = DebugModeSensor.Instance;

        if (sensor.IsDebugMode == false)
        {
            IsDebugMode = false;
            TargetCamera.SetActive(false);
            gameObject.SetActive(false);
        }
        else
        {
            IsDebugMode = true;
        }
    }

    public void SetOnOff()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
