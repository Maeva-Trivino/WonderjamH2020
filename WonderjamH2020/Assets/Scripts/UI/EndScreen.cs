using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
        if (waitingForReturnToMenu && inputManager.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }


    public void Show(int winnerId)
    {
        winnerMessage.SetText(string.Format("Congratulations ! Player {0} has won !", winnerId));
        container.SetActive(true);
        waitingForReturnToMenu = true;
    }
}
