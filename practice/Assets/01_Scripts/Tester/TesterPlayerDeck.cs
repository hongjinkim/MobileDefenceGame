using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerManager))]
public class TesterPlayerDeck : MonoBehaviour
{
    private PlayerManager playerManager;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    private void Start()
    {
        playerManager.MakeTestDeck();
    }
}
