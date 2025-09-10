using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BriefBackButton : MonoBehaviour
{
    private Button TargetButton;

    private void Start()
    {
        TargetButton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) == true)
        {
            TargetButton?.onClick.Invoke();
        }
    }
}

