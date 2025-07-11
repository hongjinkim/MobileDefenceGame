using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float cameraAngleX = 60f;  // X축 회전 각도
    [Range(5f, 20f)] public float cameraDistance = 11.18f;  // 카메라와 목표 사이의 거리
    [SerializeField] private Vector3 targetPosition = Vector3.zero;  // 바라보는 목표 위치

    private void Start()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        // 라디안으로 변환
        float angleInRadian = cameraAngleX * Mathf.Deg2Rad;

        // 각도와 거리를 이용해 Y와 Z 위치 계산
        float yOffset = cameraDistance * Mathf.Sin(angleInRadian);
        float zOffset = -cameraDistance * Mathf.Cos(angleInRadian);

        // 카메라 위치 설정
        transform.position = new Vector3(targetPosition.x, yOffset, targetPosition.z + zOffset);

        // 카메라가 목표점을 바라보도록 회전
        transform.rotation = Quaternion.Euler(cameraAngleX, 0f, 0f);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateCameraPosition();
    }
#endif
}
