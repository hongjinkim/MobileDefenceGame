using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneInitializer : MonoBehaviour
{
    private static bool raisedThisLoad = false;

    private void OnEnable()
    {
        // 씬 재로드/어드레서블 로드 중 중복 방지
        raisedThisLoad = false;
    }

    private IEnumerator Start()
    {
        // 한 프레임 대기: 모든 Start가 끝난 뒤
        yield return null;

        if (!raisedThisLoad)
        {
            raisedThisLoad = true;
            EventManager.Raise(EEventType.MainSceneOpened);
        }
    }
}