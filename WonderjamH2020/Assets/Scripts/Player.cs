using System;
using Rewired;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ChoicePopup;
using Gameplay.Delivery;

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

    public int PlayerId
    {
        get { return playerID;}
        set { playerID = value; }
    }

    [Header("UI")]
    [SerializeField]
    private ChoicePopup.ChoicePopup choicePopup;
    #endregion

    #region Public
    private int money = 100;
    #endregion

    #region Private
    private bool inMenu;
    private bool inQTE;
    private bool isPickingUpItem;

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
        if (collision.transform.GetComponent<ChoicesSenderBehaviour>())
        {
            choicePopup.SetChoices(collision.transform.GetComponent<ChoicesSenderBehaviour>().GetChoices());
            StartCoroutine(DisplayChoicePopup());
        }

        if (collision.transform.GetComponent<ChoicesSenderBehaviourWithContext>())
        {
            choicePopup.SetChoices(collision.transform.GetComponent<ChoicesSenderBehaviourWithContext>().GetChoices(this));
            StartCoroutine(DisplayChoicePopup());
        }

        if (collision.transform.GetComponent<ItemBox>())
        {
            ItemBox itemBox = collision.transform.GetComponent<ItemBox>();
            StartCoroutine(PickUpItemBox(itemBox));
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
       
        if (other.transform.GetComponent<ItemBox>())
        {
            isPickingUpItem = false;
        }
    }

    private IEnumerator PickUpItemBox(ItemBox itemBox)
    {
        isPickingUpItem = true;
        while (isPickingUpItem)
        {
            if (inputManager.GetButtonDown("Pickup Box"))
            {
                //TODO GET ITEM
                isPickingUpItem = false;
                Destroy(itemBox.gameObject);
            }

            yield return null;
        }
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

    public bool CanAffordMissile(MissileBlueprint blueprint)
    {
        return money >= blueprint.price;
    }

    // Returns true if successful
    public void PayForMissile(MissileBlueprint blueprint)
    {
        money -= blueprint.price;
    }
    #endregion
}
