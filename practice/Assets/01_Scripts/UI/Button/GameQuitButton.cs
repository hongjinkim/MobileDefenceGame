using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameQuitButton : UIButton
{

    protected override void OnClicked()
    {
        Debug.Log("���� ����");
        Application.Quit();
    }
}

