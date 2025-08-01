using System.Collections;
using UnityEngine;
using System;

public class StageEnemyValue
{
    public EnemyStatValue EnemyHP = new EnemyStatValue();
    public EnemyStatValue EnemyAttack = new EnemyStatValue();
    public EnemyStatValue EnemyGold = new EnemyStatValue();
    public DoubleEnemyStatValue BossAttackMultiplier = new DoubleEnemyStatValue();
    public DoubleEnemyStatValue BossHPMultiplier = new DoubleEnemyStatValue();
}

public class EnemyStatValue
{
    public BigNum Start;
    public BigNum Constant; // 상수곱
    public double Exponent;
    public BigNum Stat;

    public void SetEnemyStat(int stage) => Stat = Start + Constant * (BigNum.Exp(Exponent * stage) - 1);
}

public class DoubleEnemyStatValue
{
    public BigNum Start;
    public BigNum Constant;   // 상수곱
    public double Exponent;
    public double Stat;

    public void SetEnemyStat(int stage)
    {
        Stat = (double)(Start + Constant * (double)(BigNum.Exp(Exponent * stage) - 1));
    }
}