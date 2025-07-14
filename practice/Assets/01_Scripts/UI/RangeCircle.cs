using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCircle : MonoBehaviour
{
    private Transform Center;
    [SerializeField] private float radius = 5f; // Circle의 반지름
    []SerializeField] private ParticleSystem particleSystem;

    private void Awake()
    {
        Center = PositionInfo.Instance.StageCenter;
    }

    private void Start()
    {
        transform.position = Center.position;
    }
}
