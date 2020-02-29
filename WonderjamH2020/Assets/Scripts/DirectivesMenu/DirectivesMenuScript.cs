using System.Collections;
using System.Collections.Generic;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DirectivesMenuScript : MonoBehaviour
{
    //InputManager
    private Rewired.Player inputManager;

    //Detect Joysticks
    private Joystick joystick;

    //TextGoOnGame
    [SerializeField]
    private TextMeshProUGUI goOnGameText;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = ReInput.players.GetPlayer(0);
    }

    // Update is called once per frame
    void Update()
    {
        joystick = ReInput.controllers.GetJoystick(0);
        ChangeTextsMenu(goOnGameText, "Validate", "Let's play !");

        if (inputManager.GetButtonDown("Validate"))
        {
            SceneManager.LoadScene("MainScene");
        }
    }

    private void ChangeTextsMenu(TextMeshProUGUI textMeshToChange, string nameAction, string descriptionAction)
    {
        if (joystick == null)
        {
            textMeshToChange.text = inputManager.controllers.maps.GetFirstButtonMapWithAction(ControllerType.Keyboard, nameAction, true).elementIdentifierName + " : " + descriptionAction;
        }
        else
        {
            textMeshToChange.text = inputManager.controllers.maps.GetFirstButtonMapWithAction(ControllerType.Joystick, nameAction, true).elementIdentifierName + " : " + descriptionAction;
        }
    }
}
