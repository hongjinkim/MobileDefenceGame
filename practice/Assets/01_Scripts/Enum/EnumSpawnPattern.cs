using GoogleSheet.Core.Type;
using System.Collections;
using UnityEngine;

[UGS(typeof(ESpawnPattern))]
public enum ESpawnPattern
{
    Boss,          // 보스 위치
    Random,         // 랜덤 위치
    Circle,         // 원형 패턴
    Line,          // 직선 패턴
    Spiral,        // 나선형 패턴
    Grid,          // 격자 패턴
    RandomGrid,    // 랜덤 격자 패턴
    RandomCircle,  // 랜덤 원형 패턴
    RandomLine,    // 랜덤 직선 패턴
    RandomSpiral,  // 랜덤 나선형 패턴
    RandomSpiralCircle, // 랜덤 나선형 원형 패턴
    RandomSpiralLine,   // 랜덤 나선형 직선 패턴
    RandomSpiralGrid,   // 랜덤 나선형 격자 패턴
    RandomSpiralRandom, // 랜덤 나선형 랜덤 패턴
    RandomSpiralRandomCircle, // 랜덤 나선형 랜덤 원형 패턴
    RandomSpiralRandomLine,   // 랜덤 나선형 랜덤 직선 패턴
    RandomSpiralRandomGrid,   // 랜덤 나선형 랜덤 격자 패턴
    RandomSpiralRandomAroundTarget // 랜덤 나선형 랜덤 타겟 주위 패턴
                                   // 추가적인 패턴이 필요하면 여기에 정의

}
