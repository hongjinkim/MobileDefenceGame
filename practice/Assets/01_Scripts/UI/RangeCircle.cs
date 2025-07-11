using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCircle : MonoBehaviour
{
    private Transform Center;

    private void Awake()
    {
        Center = PositionInfo.Instance.MapCenter;
    }

    private void Start()
    {
        transform.position = Center.position;
    }
}
