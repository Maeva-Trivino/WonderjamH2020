using System.Collections;
using System.Collections.Generic;
using Rewired;
using TMPro;
using UnityEngine;

public class ChangeStartText : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro startGameText;

    private Rewired.Player inputManager;
    // Start is called before the first frame update
    void Start()
    {
        inputManager = ReInput.players.GetPlayer(0);
        Debug.Log(inputManager.controllers.maps.GetFirstButtonMapWithAction("Validate", true).elementIdentifierName);

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(inputManager.GetButton("Validate").ToString());
    }
}
