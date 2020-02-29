using System;
using Rewired;
using UnityEngine;
using ChoicePopup;
using QTE;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using Gameplay.Delivery;
using UnityEditor.PackageManager;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    #region Enum
    public enum Mode { MOVING, CHOOSING, QTEING }
    #endregion

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
    private ChoicePopup.ChoicePopup choicePopup;
    #endregion

    #region Public
    public Rewired.Player inputManager;

    public int Lemons
    {
        get { return lemons; }
        set { lemons = value; }
    }
    #endregion

    #region Private
    private bool inMenu;
    // private bool inQTE; Si vous voyez cette ligne vous pouvez la supprimeer
    private bool isPickingUpItem;

    private HashSet<GameObject> actionsInRange;
    private UserAction currentAction;
    private GameObject selected;
    [SerializeField]
    private GameObject QTEPopup;


    private int money = 100;
    [SerializeField]
    private int lemons = 0;


    private Vector2 input, direction = Vector2.down; // direction will be used for animations
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private bool isRunning;
    private Mode mode;
    #endregion
    #endregion

    #region Methods
    #region Unity
    private void Start()
    {
        inputManager = ReInput.players.GetPlayer(playerID);
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        choicePopup.onClose.AddListener(() => mode = Mode.MOVING);
        actionsInRange = new HashSet<GameObject>();
        updateQTEPopup(null);
    }


    private void Update()
    {
        switch (mode)
        {
            case Mode.MOVING:
                PlayerMove();
                PlayerQTE();
                break;
            case Mode.CHOOSING:
                PlayerChoose();
                break;
            case Mode.QTEING:
                PlayerQTE();
                break;
        }
    }

    private void PlayerQTE()
    {
        HandleQTEAction();
        UpdateQTESelection();
    }

    private void FixedUpdate()
    {
        if (mode == Mode.MOVING)
        {
            _rigidbody2D.MovePosition(_rigidbody2D.position + speed * input * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<ChoicesSenderBehaviour>())
        {
            choicePopup.SetChoices(collision.transform.GetComponent<ChoicesSenderBehaviour>().GetChoices(this));
            choicePopup.Display();

            mode = Mode.CHOOSING;

            _animator.SetBool("IsWalkingUp", false);
            _animator.SetBool("IsWalkingRight", false);
            _animator.SetBool("IsWalkingDown", false);
            _animator.SetBool("IsWalkingLeft", false);
            _animator.SetBool("IsRunningLeft", false);
            _animator.SetBool("IsRunningRight", false);
        }

        if (collision.transform.GetComponent<ItemBox>())
        {
            ItemBox itemBox = collision.transform.GetComponent<ItemBox>();
            StartCoroutine(PickUpItemBox(itemBox));
        }
        Interactive interactive = collision.GetComponent<Interactive>();
        if (interactive != null)
        {
            currentAction = null;
            actionsInRange.Add(collision.gameObject);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {

        if (other.transform.GetComponent<ItemBox>())
        {
            isPickingUpItem = false;
        }
        Interactive interactive = other.GetComponent<Interactive>();
        if (interactive != null)
        {
            currentAction = null;
            actionsInRange.Remove(other.gameObject);
        }

    }
    #endregion

    #region Private
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

    private void PlayerChoose()
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
        if (inputManager.GetButtonDown("Validate"))
        {
            Debug.Log("Validé !");
            choicePopup.Validate();
        }
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
        Lemons += lemonsCount;
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

    #region QTE
    public void HandleQTEAction()
    {
        if (currentAction != null)
        {
            if (inputManager.GetButtonDown("Interact"))
            {
                mode = Mode.QTEING; // inQTE = true;
                updateQTEPopup(currentAction);
            }
            else if (inputManager.GetButtonUp("Interact"))
            {
                mode = Mode.MOVING; // = false;
                currentAction = null;
            }
            if (mode == Mode.QTEING)
            {
                currentAction.Do();
                if (currentAction.IsDone())
                {
                    currentAction = null;
                    mode = Mode.MOVING; //inQTE = false;
                }
                updateQTEPopup(currentAction);

            }
        }
    }
    public void UpdateQTESelection()
    {
        if (currentAction == null)
        {
            if (actionsInRange.Count > 0)
            {
                // On cherche l'objet intéractif le plus proche
                UserAction bestAction = null;
                float distanceMin = float.PositiveInfinity;
                GameObject nearest = null;

                foreach (GameObject o in actionsInRange)
                {
                    UserAction action = o.GetComponent<Interactive>().GetAction(this);
                    float distance = (o.transform.position - transform.position).magnitude;
                    if (action != null && distance < distanceMin)
                    {
                        distanceMin = distance;
                        nearest = o;
                        bestAction = action;
                    }
                }

                // On désélectionne l'objet sélectionné auparavant
                if (selected != null)
                {
                    selected.GetComponent<Interactive>().Deselect();
                }

                selected = nearest;
                // Si l'interactive le plus proche a une action disponible, alors on la selectionne
                if (nearest != null)
                {
                    Interactive interactive = selected.GetComponent<Interactive>();
                    interactive.Select();
                }

                //L'action de l'object selectionner devient notre nouvelle action courante
                currentAction = bestAction;

                updateQTEPopup(currentAction);
            }
            else
            {

                updateQTEPopup(null);
                if (selected != null)
                {
                    selected.GetComponent<Interactive>().Deselect();
                    selected = null;
                }
            }
        }
    }

    //Display of the popup
    private void updateQTEPopup(UserAction action)
    {
        if (action != null)
        {
            Text text = QTEPopup.GetComponentsInChildren<Text>()[0];
            Text button = QTEPopup.GetComponentsInChildren<Text>()[1];
            Text combos = QTEPopup.GetComponentsInChildren<Text>()[2];
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, transform.GetComponentInChildren<Renderer>().bounds.size.y));

            if (mode == Mode.QTEING)
            {
                if (!(action is ComboAction))
                {
                    text.text = "";
                    button.text = "";
                    combos.text = "";
                }
                else
                {
                    ComboAction comboAction = (ComboAction)action;
                    text.text = "";
                    combos.text = "";
                    foreach (string s in comboAction.expectedCombos)
                    {
                        combos.text += s + " ";
                    }

                    combos.text = combos.text.Remove(combos.text.Length - 1);
                    button.text = "";
                }

                Slider slider = QTEPopup.GetComponentInChildren<Slider>();
                slider.transform.localScale = new Vector3(1, 1, 1);
                slider.value = action.progression;
                slider.gameObject.SetActive(true);
                QTEPopup.gameObject.SetActive(true);

            }
            else
            {
                text.text = action.name;
                text.fontSize = 15;
                button.text = "F";
                combos.text = "";
                QTEPopup.GetComponentInChildren<Slider>().gameObject.transform.localScale = new Vector3(0, 0, 0);
                QTEPopup.gameObject.SetActive(false);

            }

            QTEPopup.transform.position = screenPos;
        }
        else
        {
            if (QTEPopup != null && QTEPopup.activeSelf)
            {
                QTEPopup.GetComponentInChildren<Slider>().gameObject.transform.localScale = new Vector3(0, 0, 0);
                QTEPopup.gameObject.SetActive(false);
            }
        }
    }
    #endregion
    #endregion
    #endregion
}