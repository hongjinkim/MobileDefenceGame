using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPush : MonoBehaviour
{
    [SerializeField] protected EFXPoolType FxPoolType;
    [SerializeField] private Vector3 initPos;
    [SerializeField] private Quaternion initRot;
    [SerializeField] private Vector3 initScale = new Vector3(1f, 1f, 1f);
    public float deathtimer = 5;

    protected virtual void OnEnable()
    {
        CancelInvoke();
        Invoke("PoolPush", deathtimer);
    }

    public void InitTransform()
    {
        transform.localPosition = initPos;
        transform.localRotation = initRot;
        transform.localScale = initScale;
    }

    public virtual void PoolPush()
    {
        if (gameObject.activeSelf)
        {
            FXPoolManager.Instance.Push(gameObject, FxPoolType);
        }
    }

    public void PoolPush(GameObject o)
    {
        if (o.activeSelf)
            FXPoolManager.Instance.Push(o, FxPoolType);
    }
}
