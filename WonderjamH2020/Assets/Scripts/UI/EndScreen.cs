using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    public TextMeshProUGUI winnerMessage;

    public GameObject container;

    private Rewired.Player inputManager;

    private bool waitingForReturnToMenu = false;

    public void Start()
    {
        inputManager = ReInput.players.GetPlayer(0);
        container.SetActive(false);
    }

    private void Update()
    {
        if (waitingForReturnToMenu && inputManager.GetButtonDown("Return_to_menu"))
        {
            Debug.Log("RETURN !!");
        }
    }


    public void Show(string winner)
    {
        winnerMessage.SetText(string.Format("Congratulations ! {0} has won !", winner));
        container.SetActive(true);
        waitingForReturnToMenu = true;
    }
}
