using Rewired;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using Gameplay.Delivery;
using Popup;
using Interactive.Base;

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
        get { return playerID; }
        set { playerID = value; }
    }

    [Header("UI")]
    [SerializeField]
    private ChoicePopup choicePopup;
    [SerializeField]
    private QTEPopup QTEPopup;
    #endregion

    #region Public
    public Rewired.Player inputManager;
    [Header("Inventory")]
    public int money = 100;
    public int lemons = 0;
    #endregion

    #region Private
    private bool isPickingUpItem;

    private HashSet<GameObject> actionsInRange;
    private GameObject selection;
    private Popup.Popup currentPopup;

    private Vector2 input;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private Canvas _canvas;
    private bool isRunning;
    private bool canMove = true;
    #endregion
    #endregion

    #region Methods
    #region Unity
    private void Start()
    {
        inputManager = ReInput.players.GetPlayer(playerID);
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _canvas = GetComponentInChildren<Canvas>();
        actionsInRange = new HashSet<GameObject>();
        choicePopup.SetInputManager(inputManager);
    }


    private void Update()
    {
        if (canMove)
        {
            PlayerMove();
            PlayerOrderInLayer();
        }

        ListenInteraction();
    }

    private void ListenInteraction()
    {
        // MISE A JOUR DE LA SELECTION
        // On cherche l'objet intéractif le plus proche
        float distanceMin = float.PositiveInfinity;
        GameObject nearest = null;

        foreach (GameObject go in actionsInRange)
        {
            float distance = (go.transform.position - transform.position).magnitude;
            if (distance < distanceMin)
            {
                distanceMin = distance;
                nearest = go;
            }
        }

        // On désélectionne l'objet sélectionné auparavant
        if (selection != null)
        {
            selection.GetComponent<Interactable>().Deselect();
        }

        selection = nearest;
        // Si l'interactive le plus proche a une action disponible, alors on la selectionne
        if (nearest != null)
        {
            Interactable interactive = selection.GetComponent<Interactable>();
            interactive.Select();
        }


        // ECOUTE DE L'INTERACTION
        if (selection != null)
        {
            if (inputManager.GetButtonDown("Interact") && canMove)
            {
                // Set static player
                canMove = false;
                _animator.SetBool("IsWalkingUp", false);
                _animator.SetBool("IsWalkingRight", false);
                _animator.SetBool("IsWalkingDown", false);
                _animator.SetBool("IsWalkingLeft", false);
                _animator.SetBool("IsRunningLeft", false);
                _animator.SetBool("IsRunningRight", false);

                // Start interaction
                Interactable script = selection.GetComponent<Interactable>();
                if (script is ChoicesSenderBehaviour)
                {
                    ChoicesSenderBehaviour iObj = script as ChoicesSenderBehaviour;
                    currentPopup = choicePopup;
                    choicePopup.SetChoices(iObj.GetChoices(this));
                }
                else if (script is QTEBehaviour)
                {
                    QTEBehaviour iObj = script as QTEBehaviour;
                    currentPopup = QTEPopup;
                    QTEPopup.SetAction(iObj.GetAction(this));
                }

                // Display popup
                currentPopup.Display();
                currentPopup.onClose.RemoveAllListeners();
                currentPopup.onClose.AddListener(() => canMove = true);
            }
            else if (inputManager.GetButtonDown("Cancel"))
            {
                currentPopup.Hide();
            }
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
            _rigidbody2D.MovePosition(_rigidbody2D.position + speed * input * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<ItemBox>())
        {
            ItemBox itemBox = collision.transform.GetComponent<ItemBox>();
            StartCoroutine(PickUpItemBox(itemBox));
        }

        Interactable interactive = collision.GetComponent<Interactable>();
        if (interactive != null && !actionsInRange.Contains(collision.gameObject))
        {
            actionsInRange.Add(collision.gameObject);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {

        if (other.transform.GetComponent<ItemBox>())
        {
            isPickingUpItem = false;
        }

        Interactable interactive = other.GetComponent<Interactable>();
        if (interactive != null)
        {
            actionsInRange.Remove(other.gameObject);
        }
    }
    #endregion

    #region Private
    private void PlayerOrderInLayer()
    {
        GetComponentInChildren<Renderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }
    private void PlayerMove()
    {
        input = new Vector2(inputManager.GetAxis("Horizontal"), inputManager.GetAxis("Vertical"));
        bool isMoving = input.magnitude > .2f;

        if (!isMoving)
        {
            input = Vector2.zero;
        }

        //_animator.SetBool("Walking", !isRunning && isMoving);
        _animator.speed = isMoving ? input.magnitude : 1;

        bool wu = false, wr = false, wd = false, wl = false;
        if (input != Vector2.zero && !choicePopup.IsVisible)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            { // X stringer than Y
                wl = !(wr = input.x > 0);
            }
            else
            {
                wd = !(wu = input.y > 0);
            }
        }

        _animator.SetBool("IsWalkingUp", wu);
        _animator.SetBool("IsWalkingRight", wr);
        _animator.SetBool("IsWalkingDown", wd);
        _animator.SetBool("IsWalkingLeft", wl);
        _animator.SetBool("IsRunningLeft", isRunning && wl);
        _animator.SetBool("IsRunningRight", isRunning && wr);
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

    public void SellLemonade(int lemonadePrice)
    {
        this.money += lemonadePrice;
        Debug.Log("Grandma now has $" + money);
    }

    public void HarvestLemons(int lemonsCount)
    {
        Debug.Log("LEMONS");
        lemons += lemonsCount;
    }

    private IEnumerator PickUpItemBox(ItemBox itemBox)
    {
        isPickingUpItem = true;
        while (isPickingUpItem)
        {
            if (inputManager.GetButtonDown("PickUp"))
            {
                //TODO GET ITEM
                isPickingUpItem = false;
                Destroy(itemBox.gameObject);
            }

            yield return null;
        }
    }
    #endregion
    #endregion

    #region Public
    public void DestroyInteractive(GameObject toDestroy)
    {
        actionsInRange.Remove(toDestroy);
        Destroy(toDestroy);
    }
    #endregion
}