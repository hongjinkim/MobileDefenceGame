using UnityEngine;

public class GameManager : BasicSingleton<GameManager>
{
    [Header("ÀÌº¥Æ®")]
    [SerializeField] private VoidEventChannelSO dataLoaded;

    [SerializeField] private AudioPlayerDissolve bgmSound;
    [HideInInspector] public bool dataLoadComplete = false;

    void OnEnable()
    {
        if(dataLoaded != null)
		{
            dataLoaded.AddListener(OnDataLoaded);
		}
    }

    void OnDisable()
    {
        if (dataLoaded != null)
        {
            dataLoaded.RemoveListener(OnDataLoaded);
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
        dataLoaded.RaiseEvent();
	}

    public void ResetData() => Init();

}
