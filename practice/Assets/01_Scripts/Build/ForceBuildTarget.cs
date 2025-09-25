using UnityEditor;

[InitializeOnLoad]
public class ForceBuildTarget
{
    static ForceBuildTarget()
    {
        // [InitializeOnLoad]가 실행되면 즉시 로직을 실행하지 않고,
        // EditorApplication.update 이벤트에 실행할 메서드를 등록만 합니다.
        EditorApplication.update += RunOnFirstUpdate;
    }

    private static void RunOnFirstUpdate()
    {
        // 이 메서드가 처음 실행되면, 가장 먼저 update 이벤트에서 자신을 제거(구독 해지)합니다.
        // 이렇게 해야 매 프레임마다 실행되는 것을 막고 "딱 한 번만" 실행되게 할 수 있습니다.
        EditorApplication.update -= RunOnFirstUpdate;


        // 세션 당 한 번만 체크하는 로직
        const string BuildTargetCheckedKey = "BuildTargetCheckedForThisSession";
        if (SessionState.GetBool(BuildTargetCheckedKey, false))
        {
            return;
        }
        SessionState.SetBool(BuildTargetCheckedKey, true);

        // 빌드 타겟 확인 및 전환
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        }
    }
}