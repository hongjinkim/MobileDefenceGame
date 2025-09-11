using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ProloguePage
{
    public GameObject Page;
    public PrologueScene[] Scenes;
}

public class PrologueDisplay : MonoBehaviour
{
    public static event Action OnFinish = delegate { };

    [SerializeField] private ProloguePage[] Page;
    [SerializeField] private FadeOutEvent LobbyFadeOut;
    [SerializeField] private FadeOutEvent PrologueFadeOut;
    [SerializeField] private EventButton SkipButton;
    [SerializeField] private EventButton NextButton;
    [SerializeField] private GameObject nextTextImage;
    [SerializeField] private EventButton StartButton;
    [SerializeField] private GameObject startTextImage;
    public AudioPlayerDissolve PrologueSound;

    private int pageIndex = 0;
    private int sceneIndex = 0;
    private float skipTimer = 0f;
    //private float skipTerm = 0.1f;

    private void OnEnable()
    {
        PrologueScene.OnPrologueFinish += PrologueSceneFinish;
        NextButton.OnClick += DoNextButton;
        StartButton.OnClick += DoStartButton;
    }

    private void OnDisable()
    {
        PrologueScene.OnPrologueFinish -= PrologueSceneFinish;
        NextButton.OnClick -= DoNextButton;
        StartButton.OnClick -= DoStartButton;
    }

    // 프롤로그 한 장면 끝
    private void PrologueSceneFinish(int index)
    {
        sceneIndex++;
        if (sceneIndex < Page[pageIndex].Scenes.Length)
            Page[pageIndex].Scenes[sceneIndex].gameObject.SetActive(true);
        else
        {
            SkipButton.gameObject.SetActive(false);
            if (pageIndex == 0)
            {
                NextButton.gameObject.SetActive(true);
                nextTextImage.gameObject.SetActive(true);
            }
            else
            {
                StartButton.gameObject.SetActive(true);
                startTextImage.gameObject.SetActive(true);
            }
        }
    }

    // 넥스트 버튼
    private void DoNextButton()
    {
        pageIndex++;
        ContinuePrologue();
    }

    // 게임 시작 버튼
    public void DoStartButton()
    {
        StartButton.gameObject.SetActive(false);
        //AllClearScene();
        PrologueFadeOut.gameObject.SetActive(true);
        float duration = PrologueFadeOut.StartFadeOut();
        Invoke(nameof(Finish), duration);
    }

    private void Finish() => OnFinish?.Invoke();

    // 프롤로그 시작
    public void StartPrologue()
    {
        PrologueSound.Play();
        ContinuePrologue();
    }
    private void ContinuePrologue()
    {
        SkipButton.gameObject.SetActive(true);
        sceneIndex = 0;
        LobbyFadeOut.DisableObject();
        AllClearScene();
        Page[pageIndex].Page.SetActive(true);
        Page[pageIndex].Scenes[sceneIndex].gameObject.SetActive(true);
    }

    // 화면 전부 지우기
    private void AllClearScene()
    {
        NextButton.gameObject.SetActive(false);
        nextTextImage.gameObject.SetActive(false);
        StartButton.gameObject.SetActive(false);
        startTextImage.gameObject.SetActive(false);
        for (int i = 0; i < Page.Length; i++)
        {
            Page[i].Page.SetActive(false);
            foreach (var scene in Page[i].Scenes)
                scene.gameObject.SetActive(false);
        }

    }

    public void ForceStart() => DoStartButton();

    private void Update()
    {
        skipTimer += Time.deltaTime;
    }
}

