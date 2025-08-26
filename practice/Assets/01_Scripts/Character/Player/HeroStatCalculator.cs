using System;

public struct FinalHeroStats
{
    public BigNum Attack;
    public BigNum Health;
}

public static class HeroStatCalculator
{
    // 성장 곡선 파라미터(예시값) ? 추후 테이블 값으로 뺼 예정
    private const double LEVEL_ATK_PER_LV = 0.08; // 레벨당 +8%
    private const double LEVEL_HP_PER_LV = 0.10; // 레벨당 +10%
    private const double STAR_ATK_PER = 0.20;  // 스타당 +20%
    private const double STAR_HP_PER = 0.25;  // 스타당 +25%

    /// 최종 스탯 합성
    /// - resolveEquip: equipmentId -> 장비 마스터를 찾아주는 함수
    public static FinalHeroStats Compose(
        HeroValue master,
        PlayerHeroState state,
        Func<string, EquipmentValue> resolveEquip // null 허용
    )
    {
        var atk = master.AttackPower;
        var hp = master.Health;
        var aspd = master.AttackSpeed;

        int lv = Math.Max(1, state.Level);
        int star = Math.Max(1, state.Star);

        // 1) 레벨/등급 성장 반영
        double atkLvMul = 1.0 + LEVEL_ATK_PER_LV * (lv - 1);
        double hpLvMul = 1.0 + LEVEL_HP_PER_LV * (lv - 1);
        double atkStarMul = 1.0 + STAR_ATK_PER * (star - 1);
        double hpStarMul = 1.0 + STAR_HP_PER * (star - 1);

        atk = atk * atkLvMul * atkStarMul;
        hp = hp * hpLvMul * hpStarMul;

        // 2) 장비 합성
       

        return new FinalHeroStats { Attack = atk, Health = hp };
    }

}
