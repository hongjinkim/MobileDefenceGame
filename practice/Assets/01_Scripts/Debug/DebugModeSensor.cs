using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugModeSensor : MonoBehaviour
{
    public static DebugModeSensor Instance;

    [HideInInspector]public bool IsDebugMode = false;
    public GameObject DebugObject;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        IsDebugMode = DebugObject.activeSelf;
    }
}
