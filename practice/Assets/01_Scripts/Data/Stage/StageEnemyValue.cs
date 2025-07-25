using System.Collections;
using UnityEngine;
using System;

public class StageEnemyValue
{
    public PoomEnemyStatValue EnemyHP = new PoomEnemyStatValue();
    public PoomEnemyStatValue EnemyAttack = new PoomEnemyStatValue();
    public PoomEnemyStatValue EnemyGold = new PoomEnemyStatValue();
    public DoubleEnemyStatValue BossAttackMultiplier = new DoubleEnemyStatValue();
    public DoubleEnemyStatValue BossHPMultiplier = new DoubleEnemyStatValue();
}

public class PoomEnemyStatValue
{
    public BigNum Start { get; set; }
    public BigNum Constant { get; set; }       // 상수곱
    public double Exponent { get; set; }
    public BigNum Stat { get; private set; }

    public void SetEnemyStat(int stage) => Stat = Start + Constant * (BigNum.Exp(Exponent * stage) - 1);
}

public class DoubleEnemyStatValue
{
    public BigNum Start { get; set; }
    public BigNum Constant { get; set; }       // 상수곱
    public double Exponent { get; set; }
    public double Stat { get; private set; }

    public void SetEnemyStat(int stage)
    {
        Stat = (double)(Start + Constant * (double)(BigNum.Exp(Exponent * stage) - 1));
    }
}