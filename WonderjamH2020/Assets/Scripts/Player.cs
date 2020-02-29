using Rewired;
using UnityEngine;
using System.Collections;
using UI.ChoicePopup;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    #region Variables
    #region Editor
    [Header("General")]
    [SerializeField]
    private float speed = 2;
    [SerializeField]
    private int playerID = 0;

    [Header("UI")]
    [SerializeField]
    private ChoicePopup choicePopup;
    #endregion

    #region Public
    #endregion

    #region Private
    private bool inMenu;
    private bool inQTE;

    private Rewired.Player inputManager;
    private Vector2 input, direction = Vector2.down; // direction will be used for animations
    private Rigidbody2D _rigidbody2D;
    #endregion
    #endregion

    #region Methods
    private void Start()
    {
        inputManager = ReInput.players.GetPlayer(playerID);
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        input = new Vector2(inputManager.GetAxis("Horizontal"), inputManager.GetAxis("Vertical"));
        if (input.magnitude < .1f) input = Vector2.zero;

        if (input != Vector2.zero)
        {
            direction = input.normalized;
        }
    }

    private void FixedUpdate()
    {
        if (!inQTE && !inMenu)
        {
            _rigidbody2D.MovePosition(_rigidbody2D.position + speed * input * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Test
        bool lol = true;
        choicePopup.SetChoices(new List<Choice>() {
                new Choice("Say hello", () => Debug.Log("hello"), () => true),
                new Choice("Disable this button", () => lol = false, () => lol),
            });
        StartCoroutine(DisplayChoicePopup());
    }

    private IEnumerator DisplayChoicePopup()
    {
        choicePopup.Display(transform.position);
        inMenu = true;
        while (choicePopup.IsVisible)
        {
            if (inputManager.GetButtonDown("Cancel"))
            {
                Debug.Log("Cancel");
                choicePopup.Hide();
            }
            else if (inputManager.GetButtonDown("MenuLeft"))
            {
                Debug.Log("Gauche");
                choicePopup.GoLeft();
            }
            else if (inputManager.GetButtonDown("MenuRight"))
            {
                Debug.Log("Droite");
                choicePopup.GoRight();
            }
            if (inputManager.GetButton("Validate"))
            {
                Debug.Log("Validé !");
                choicePopup.Validate();
            }
            yield return true;
        }
        inMenu = false;
    }
    #endregion
}
