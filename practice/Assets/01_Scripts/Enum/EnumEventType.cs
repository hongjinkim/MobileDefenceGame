public enum EEventType
{
    None = 0,
    MainSceneOpened, // �κ� -> ���� �� ���� �̺�Ʈ
    EnemyDied, // �� ��� �̺�Ʈ
    HeroDied, // ���� ��� �̺�Ʈ
    LevelUp, // ������ �̺�Ʈ
    EnemyGenerated, // ���� ���� �̺�Ʈ
    EnemyDead, // �� ��� �̺�Ʈ
    BossDead, // ���� ��� �̺�Ʈ
    EnergyUpdated, // ������ ������Ʈ �̺�Ʈ
    GoldUpdated, // ��� ������Ʈ �̺�Ʈ
    CrystalUpdated, //  ũ����Ż ������Ʈ �̺�Ʈ
    StageCleared, // �������� ���� �̺�Ʈ
    LastBossDead, // ������ ���� Ŭ���� �̺�Ʈ
    CameraChange, // ī�޶� ���� �̺�Ʈ
}