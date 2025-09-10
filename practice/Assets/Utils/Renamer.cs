using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Renamer : MonoBehaviour
{
    [ContextMenu("Rename")]
    public void Rename()
    {
        int num = 1;
        foreach (Transform t in transform)
        {
            var tmp = t.GetComponentInChildren<TMP_Text>();
            tmp.SetText($"{num}");
            num++;
        }
    }

}
