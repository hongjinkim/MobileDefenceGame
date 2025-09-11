using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraMode
{
    Stage,  // �������� ���
    Lobby   // �κ� ���
}
public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float cameraAngleX = 60f;  // X�� ȸ�� ����
    [Range(5f, 20f)] public float cameraDistance = 11.18f;  // ī�޶�� ��ǥ ������ �Ÿ�
    public CameraMode cameraMode;  // ���� ī�޶� ���
    [SerializeField] private Transform targetPositionStage;     // �ٶ󺸴� ��ǥ ��ġ
    [SerializeField] private Transform targetPositionLobby;     // �ٶ󺸴� ��ǥ ��ġ
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
        // �������� ��ȯ
        float angleInRadian = cameraAngleX * Mathf.Deg2Rad;

        // ������ �Ÿ��� �̿��� Y�� Z ��ġ ���
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
            

        // ī�޶� ��ġ ����
        transform.position = new Vector3(targetPosition.x, yOffset, targetPosition.z + zOffset);

        // ī�޶� ��ǥ���� �ٶ󺸵��� ȸ��
        transform.rotation = Quaternion.Euler(cameraAngleX, 0f, 0f);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateCamera(cameraMode);
    }
#endif
}
