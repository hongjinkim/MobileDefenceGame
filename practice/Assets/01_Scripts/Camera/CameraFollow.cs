using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraMode
{
    Stage,  // 스테이지 모드
    Lobby   // 로비 모드
}
public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float cameraAngleX = 60f;  // X축 회전 각도
    [Range(5f, 20f)] public float cameraDistance = 11.18f;  // 카메라와 목표 사이의 거리
    public CameraMode cameraMode;  // 현재 카메라 모드
    [SerializeField] private Transform targetPositionStage;     // 바라보는 목표 위치
    [SerializeField] private Transform targetPositionLobby;     // 바라보는 목표 위치
    [SerializeField] private Camera cam;


    private void OnEnable()
    {
        EventManager.Subscribe<CameraMode>(EEventType.CameraChange, UpdateCamera);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<CameraMode>(EEventType.CameraChange, UpdateCamera);
    }

    private void Start()
    {
        UpdateCamera(cameraMode);
    }


    private void UpdateCamera(CameraMode cameraMode)
    {
        // 라디안으로 변환
        float angleInRadian = cameraAngleX * Mathf.Deg2Rad;

        // 각도와 거리를 이용해 Y와 Z 위치 계산
        float yOffset = cameraDistance * Mathf.Sin(angleInRadian);
        float zOffset = -cameraDistance * Mathf.Cos(angleInRadian);

        Vector3 targetPosition;

        switch (cameraMode)
        {
            case CameraMode.Stage:
                targetPosition = targetPositionStage.position;
                cam.orthographic = true;
                cameraDistance = 20f;
                break;
            case CameraMode.Lobby:
                targetPosition = targetPositionLobby.position;
                cam.orthographic = false;
                break;
            default:
                targetPosition = Vector3.zero;
                break;
        }
            

        // 카메라 위치 설정
        transform.position = new Vector3(targetPosition.x, yOffset, targetPosition.z + zOffset);

        // 카메라가 목표점을 바라보도록 회전
        transform.rotation = Quaternion.Euler(cameraAngleX, 0f, 0f);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateCamera(cameraMode);
    }
#endif
}
