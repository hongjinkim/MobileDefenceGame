using System;

public struct FinalHeroStats
{
    public BigNum Attack;
    public BigNum Health;
}

public static class HeroStatCalculator
{
    // ���� � �Ķ����(���ð�) ? ���� ���̺� ������ �E ����
    private const double LEVEL_ATK_PER_LV = 0.08; // ������ +8%
    private const double LEVEL_HP_PER_LV = 0.10; // ������ +10%
    private const double STAR_ATK_PER = 0.20;  // ��Ÿ�� +20%
    private const double STAR_HP_PER = 0.25;  // ��Ÿ�� +25%

    /// ���� ���� �ռ�
    /// - resolveEquip: equipmentId -> ��� �����͸� ã���ִ� �Լ�
    public static FinalHeroStats Compose(
        HeroValue master,
        PlayerHeroState state,
        Func<string, EquipmentValue> resolveEquip // null ���
    )
    {
        var atk = master.AttackPower;
        var hp = master.Health;
        var aspd = master.AttackSpeed;

        int lv = Math.Max(1, state.Level);
        int star = Math.Max(1, state.Star);

        // 1) ����/��� ���� �ݿ�
        double atkLvMul = 1.0 + LEVEL_ATK_PER_LV * (lv - 1);
        double hpLvMul = 1.0 + LEVEL_HP_PER_LV * (lv - 1);
        double atkStarMul = 1.0 + STAR_ATK_PER * (star - 1);
        double hpStarMul = 1.0 + STAR_HP_PER * (star - 1);

        atk = atk * atkLvMul * atkStarMul;
        hp = hp * hpLvMul * hpStarMul;

        // 2) ��� �ռ�
       

        return new FinalHeroStats { Attack = atk, Health = hp };
    }

}
