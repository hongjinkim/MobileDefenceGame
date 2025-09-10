using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIImageAutoSpinner : MonoBehaviour
{
    public RectTransform RectTransform;
    public float Speed;
    public bool ClockWise = true;
    public bool AutoActive = true;

    private bool active = false;
    private float rotZ = 0;


    void Start()
    {
        if (AutoActive == true) { Run(); }
    }

    private void Update()
    {
        if (active == false || RectTransform == null) return;

        rotZ += (ClockWise ? -1f : 1f) * Time.deltaTime * Speed;

        if (rotZ > 360f) rotZ -= 360f;
        else if (rotZ < -360f) rotZ += 360f;

        RectTransform.eulerAngles = new Vector3(0, 0, rotZ);
    }


    public void Run() => active = true;

    public void Stop() => active = false;


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (RectTransform == null)
        {
            RectTransform = GetComponent<RectTransform>();
        }
    }
#endif

}

