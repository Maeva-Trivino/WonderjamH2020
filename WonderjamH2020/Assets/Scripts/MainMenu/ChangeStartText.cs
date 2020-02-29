using System.Collections;
using System.Collections.Generic;
using Rewired;
using TMPro;
using UnityEngine;

public class ChangeStartText : MonoBehaviour
{
    #region TextMeshes
    [SerializeField]
    private TextMeshProUGUI startGameText;

    [SerializeField]
    private TextMeshProUGUI creditsText;
    #endregion

    //InputManager
    private Rewired.Player inputManager;

    //Detect Joysticks
    private Joystick joystick;
    // Start is called before the first frame update
    void Start()
    {
        inputManager = ReInput.players.GetPlayer(0);
    }

    // Update is called once per frame
    void Update()
    {
        joystick = ReInput.controllers.GetJoystick(0);
        ChangeTextsMenu(startGameText, "Validate", "Start the game");
        ChangeTextsMenu(creditsText, "Credits", "Credits");
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
