using System.Collections;
using System.Collections.Generic;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeStartText : MonoBehaviour
{
    #region TextMeshes
    [SerializeField]
    private TextMeshProUGUI startGameText;

    [SerializeField]
    private TextMeshProUGUI creditsText;

    [SerializeField]
    private TextMeshProUGUI backToMenuText;
    #endregion

    //InputManager
    private Rewired.Player inputManager;

    //Detect Joysticks
    private Joystick joystick;

    //Bool to check if we're in creditView
    private bool isCredit;
    // Start is called before the first frame update
    void Start()
    {
        inputManager = ReInput.players.GetPlayer(0);
        isCredit = false;

        transform.GetChild(3).GetComponent<Image>().enabled = false;
        transform.GetChild(4).GetComponent<TextMeshProUGUI>().enabled = false;
        transform.GetChild(5).GetComponent<TextMeshProUGUI>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        joystick = ReInput.controllers.GetJoystick(0);
        ChangeTextsMenu(startGameText, "Interact", "Start the game");
        ChangeTextsMenu(creditsText, "Credits", "Credits");
        ChangeTextsMenu(backToMenuText, "Credits", "Back on menu");

        if(!isCredit && inputManager.GetButtonDown("Interact"))
        {
            SceneManager.LoadScene("DirectivesMenu");
        }
        else if (!isCredit && inputManager.GetButtonDown("Credits"))
        {
            isCredit = true;
            transform.GetChild(3).GetComponent<Image>().enabled = true;
            transform.GetChild(4).GetComponent<TextMeshProUGUI>().enabled = true;
            transform.GetChild(5).GetComponent<TextMeshProUGUI>().enabled = true;
        }
        else if(isCredit && inputManager.GetButtonDown("Credits"))
        {
            isCredit = false;
            transform.GetChild(3).GetComponent<Image>().enabled = false;
            transform.GetChild(4).GetComponent<TextMeshProUGUI>().enabled = false;
            transform.GetChild(5).GetComponent<TextMeshProUGUI>().enabled = false;
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
