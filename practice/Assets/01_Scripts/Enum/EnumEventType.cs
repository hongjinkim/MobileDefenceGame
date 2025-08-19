public enum EEventType
{
    None = 0,
    DataLoaded, // 데이터 로드 완료 이벤트
    EnemyDied, // 적 사망 이벤트
    HeroDied, // 영웅 사망 이벤트
    LevelUp, // 레벨업 이벤트
    MonsterGenerated, // 몬스터 생성 이벤트
    EnergyUpdated, // 에너지 업데이트 이벤트
    GoldUpdated, // 골드 업데이트 이벤트
    CrystalUpdated, //  크리스탈 업데이트 이벤트
    StageChanged, // 스테이지 변경 이벤트
}