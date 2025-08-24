using GoogleSheet.Core.Type;
using System.Collections;
using UnityEngine;

[UGS(typeof(ESpawnPattern))]
public enum ESpawnPattern
{
    Boss,          // 보스 위치
    LastBoss,     // 마지막 보스
    Random,         // 랜덤 위치
    Circle,         // 원형 패턴
    Line,          // 직선 패턴
    RandomInsideDisk,   // 원판 내부 균일(Random.Range^0.5 사용)
    SemiCircleForward,  // +Z 방향 반원
    SemiCircleBackward, // -Z 방향 반원
    Arc120,             // +Z 중심 120도 호(전방 부채꼴)
    SpiralOut,          // 바깥으로 뻗는 나선(아르키메데스)
    Phyllotaxis,        // 골든앵글 분포(균일하게 퍼지는 원판)
    Rings2,             // 이중 링(내/외반지)
    Cluster3,           // 3개 군집(클러스터) 생성
    Lanes4,             // 상/하/좌/우 4개 레인으로 퍼짐
    Ellipse,            // 타원(가로/세로 반경 다름)
    NoisyCircle,        // 노이즈가 섞인 원(살짝 흐트러짐)
    Grid                // 정사각 격자(사각형 범위에 골고루)
                        // 추가적인 패턴이 필요하면 여기에 정의

}
