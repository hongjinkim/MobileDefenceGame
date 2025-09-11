using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueScene : MonoBehaviour
{
    public static event Action<int> OnPrologueFinish = delegate { };

    [SerializeField] private AudioPlayerSingle SceneSound;
    [SerializeField] private CanvasGroup CanvasGroup;
    [SerializeField] private LocalizedTMPConstant_Script[] scripts;

    private Animator Anim;
    private bool isFinish = false;

    private void Awake()
    {
        Anim = this.GetComponent<Animator>();
    }

    public void SceneFinish(int sceneIndex)
    {
        if (isFinish == true)
            return;

        isFinish = true;
        Anim.enabled = false;
        CanvasGroup.alpha = 1;
        OnPrologueFinish?.Invoke(sceneIndex);
    }

    public void PlaySound() => SceneSound.Play();

    public void StartScript(int index)
    {
        scripts[index].StartScripting();
    }
}

