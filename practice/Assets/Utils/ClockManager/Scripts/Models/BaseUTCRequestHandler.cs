using System;
using UnityEngine;


public abstract class BaseUTCRequestHandler : MonoBehaviour
{
    protected Action<DateTimeOffset> OnSuccess;
    protected Action OnFailure;

    [SerializeField]
    protected bool printLog = false;


    public void Request(Action<DateTimeOffset> onSuccess, Action onFailed)
    {
        OnSuccess = onSuccess;
        OnFailure = onFailed;

        OnRequest();
    }


    protected void ClearCallbacks()
    {
        OnSuccess = null;
        OnFailure = null;
    }

    protected abstract void OnRequest();

}


