using UnityEngine;

public class HeroControl : CharacterBase
{
	//[SerializeField] protected RectTransform HP_HUD;
	//[SerializeField] protected RectTransform HP_HUD_After;
	//[SerializeField] protected RoundedFillUI HP_HUD_Fill;
	//[SerializeField] protected RoundedFillUI HP_HUD_Fill_After;

	public bool IsHeroActive()
	{
		return true;
	}

	public override void Die()
	{
		base.Die();
		//HP_HUD.gameObject.SetActive(false);
		//HP_HUD_After.gameObject.SetActive(false);
	}

	protected override void InitHP(float maxHp, bool showHPWhenStart)
	{
		base.InitHP(maxHp, showHPWhenStart);
		UpdateHpBar();
	}

	// 체력바 갱신
	protected void UpdateHpBar()
	{
		//HP_HUD.gameObject.SetActive(true);
		//HP_HUD_After.gameObject.SetActive(true);
		//
		//if (State.CurrentHp >= State.MaxHp)
		//{
		//	HP_HUD_Fill.SetProgress(1f);
		//	HP_HUD_Fill_After.SetProgress(1f);
		//	HP_HUD_Fill_After.StopAfterSlide();
		//}
		//else if (State.CurrentHp <= 0 || State.CurrentHp * 1000000000 < State.MaxHp)
		//{
		//	HP_HUD_Fill.SetProgress(0f);
		//	HP_HUD_Fill_After.SetProgress_After(0f);
		//}
		//else
		//{
		//	float ratio = (float)(double)(State.CurrentHp / State.MaxHp);
		//	HP_HUD_Fill.SetProgress(ratio);
		//	HP_HUD_Fill_After.SetProgress_After(ratio);
		//}
	}
}