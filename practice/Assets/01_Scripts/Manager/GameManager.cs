using UnityEngine;

public class GameManager : BasicSingleton<GameManager>
{
    [SerializeField] private AudioPlayerDissolve bgmSound;
    [HideInInspector] public bool dataLoadComplete = false;

    void OnEnable()
    {
        
    }

    void OnDisable()
    {

    }


    private void Start() => Init();

    private void Init()
	{
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        bgmSound.Play();
	}

    public void ResetData() => Init();

}
