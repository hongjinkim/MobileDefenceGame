using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageFloating : MonoBehaviour
{
    private Animator Anim;
    [SerializeField] private TextMeshPro DamageText;
    [SerializeField] private Material enemyMt;
    [SerializeField] private Material normalMt;
    [SerializeField] private Material criticalMt;
    [SerializeField] private Material strikeMt;

    private void OnEnable()
    {
        this.Anim = this.GetComponent<Animator>();
        Anim.Play("DamageText");
    }

    // 데미지 및 데미지 타입에 따라 텍스트 설정
    public void SetDamage(AttackInfo attackInfo)
    {
        switch (attackInfo.AttackType)
        {
            case EAttackType.Enemy:
                DamageText.fontSharedMaterial = enemyMt;
                break;
            case EAttackType.Normal:
                DamageText.fontSharedMaterial = normalMt;
                break;
            case EAttackType.Critical:
                DamageText.fontSharedMaterial = criticalMt;
                break;
        }

        if (attackInfo.IsMiss == true)
            DamageText.text = $"Miss";
        else
            DamageText.text = $"{attackInfo.Damage.ToABC()}";
    }

    // 애니메이션 마지막 부분에서 호출
    // 플로팅 애니메이션 끝나면 풀에서 제거
    // 해당 스크립트의 호출 위치는 프리팹의 중간이므로 부모의 위치를 전달하도록 한다.
    public void AnimtionEnd()
    {
        FXPoolManager.Instance.Push(gameObject, EFXPoolType.DamageText);
    }
}
