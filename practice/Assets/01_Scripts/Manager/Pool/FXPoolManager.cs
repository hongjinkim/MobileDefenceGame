using UnityEngine;

public class FXPoolManager : PoolManager<FXPoolManager, EFXPoolType>
{
    public GameObject PopDamageText(Vector3 position, AttackInfo attackInfo)
    {
        var obj = Pop(EFXPoolType.DamageText);
        Debug.Log($"PopDamageText: {obj.name} at {position}");
        obj.transform.position = position;
        obj.GetComponent<DamageTextEffect>().SetDamage(attackInfo);
        return obj;
    }
}
