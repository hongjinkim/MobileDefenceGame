using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneInitializer : MonoBehaviour
{
    private static bool raisedThisLoad = false;

    private void OnEnable()
    {
        // �� ��ε�/��巹���� �ε� �� �ߺ� ����
        raisedThisLoad = false;
    }

    private IEnumerator Start()
    {
        // �� ������ ���: ��� Start�� ���� ��
        yield return null;

        if (!raisedThisLoad)
        {
            raisedThisLoad = true;
            EventManager.Raise(EEventType.MainSceneOpened);
        }
    }
}