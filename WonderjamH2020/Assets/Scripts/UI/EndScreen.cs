using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI winnerMessage;

    [SerializeField]
    private TextMeshProUGUI returnToMainMenuText;

    public GameObject container;

    private Rewired.Player inputManager;

    private bool waitingForReturnToMenu = false;

    //Detect Joysticks
    private Joystick joystick;

    private bool isEndGame = false;


    public void Start()
    {
        inputManager = ReInput.players.GetPlayer(0);
        container.SetActive(false);
    }

    private void Update()
    {
        if (isEndGame)
        {
            joystick = ReInput.controllers.GetJoystick(0);
            ChangeTextsMenu(returnToMainMenuText, "Interact", " to return to menu");
            if (waitingForReturnToMenu && inputManager.GetButtonDown("Interact"))
            {
                SceneManager.LoadScene("MainMenu");
            }
        } 
    }


    public void Show(int winnerId)
    {
        winnerMessage.SetText(string.Format("Congratulations ! Player {0} has won !", winnerId));
        container.SetActive(true);
        waitingForReturnToMenu = true;
        isEndGame = true;
    }

    public void ShowDraw()
    {
        winnerMessage.SetText(string.Format("Draw ! None of you were fast enough !"));
        container.SetActive(true);
        waitingForReturnToMenu = true;
        isEndGame = true;
    }

    private void ChangeTextsMenu(TextMeshProUGUI textMeshToChange, string nameAction, string descriptionAction)
    {
        if (joystick == null)
        {
            textMeshToChange.text = "Press " + inputManager.controllers.maps.GetFirstButtonMapWithAction(ControllerType.Keyboard, nameAction, true).elementIdentifierName + descriptionAction;
        }
        else
        {
            textMeshToChange.text = "Press " + inputManager.controllers.maps.GetFirstButtonMapWithAction(ControllerType.Joystick, nameAction, true).elementIdentifierName + descriptionAction;
        }
    }
}
