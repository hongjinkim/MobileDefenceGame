using UnityEngine;

public class GameManager : BasicSingleton<GameManager>
{
    [Header("ÀÌº¥Æ®")]
    public VoidEventChannelSO DataLoadEvent;

    [SerializeField] private AudioPlayerDissolve bgmSound;
    [HideInInspector] public bool dataLoadComplete = false;

    void OnEnable()
    {
        if(DataLoadEvent != null)
		{
            DataLoadEvent.AddListener(OnDataLoaded);
		}
    }

    void OnDisable()
    {
        if (DataLoadEvent != null)
        {
            DataLoadEvent.RemoveListener(OnDataLoaded);
        }
    }

    private void OnDataLoaded(Void _)
    {
        
    }

    private void Start() => Init();

    private void Init()
	{
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        bgmSound.Play();

        dataLoadComplete = true;
        DataLoadEvent.RaiseEvent();
	}

    public void ResetData() => Init();

}
