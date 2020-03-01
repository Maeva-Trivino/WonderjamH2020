using Rewired;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Popup;
using Interactive.Base;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    #region Variables
    #region Editor
    [Header("General")]
    [SerializeField] private float baseSpeed = 6;
    [SerializeField] private int playerID = 0;
    [Header("Dash")]
    [SerializeField] private float dashCoefficient = 2.5f;
    [SerializeField] private float dashDuration = 0.10f;
    [SerializeField] private int dashPrice = 1;

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
    [SerializeField]
    private LabelPopup LabelPopup;
    [SerializeField]
    private DialoguePopup DialoguePopup;

    [SerializeField]
    Timer timer;
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
    private bool IsRunning => speed > 10f;
    private bool canMove = true;
    private float speed;
    private float dashTimeRemaining = 0.0f;
    #endregion
    #endregion

    #region Methods
    #region Unity
    private void Start()
    {
        speed = baseSpeed;
        inputManager = ReInput.players.GetPlayer(playerID);
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        actionsInRange = new HashSet<GameObject>();
        choicePopup.SetInputManager(inputManager);
        if(playerID == 0)
        {
            Speak("I'm the one selling Lemonade on Sunday !", 1f,0f) ;
        } else
        {
            Speak("You wish, old cow !", 1f, -1);
        }
    }


    private void Update()
    {
        if (timer.TimeIsStopped())
        {
            return;
        }
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
                if (currentPopup != null)
                    currentPopup.Hide();

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

                    UserAction action = (script as QTEBehaviour).GetAction(this);
                    if (action == null)
                        return;

                    if (!(action is ComboAction))
                    {
                       action.DoAction();
                    }

                    currentPopup = QTEPopup;
                    QTEPopup.SetAction(action);
                }

                // Set static player
                canMove = false;
                _animator.SetBool("IsWalkingUp", false);
                _animator.SetBool("IsWalkingRight", false);
                _animator.SetBool("IsWalkingDown", false);
                _animator.SetBool("IsWalkingLeft", false);
                _animator.SetBool("IsRunningLeft", false);
                _animator.SetBool("IsRunningRight", false);

                if (currentPopup != null)
                {
                    // Display popup
                    currentPopup.Display();
                    currentPopup.onClose.RemoveAllListeners();
                    currentPopup.onClose.AddListener(() =>
                    {
                        canMove = true;
                        currentPopup = null;
                    });
                }
                else
                {
                    canMove = true;
                }
            }
            else if (inputManager.GetButtonDown("Cancel"))
            {
                currentPopup.Hide();
                currentPopup = null;
            }
            else if(currentPopup == null || currentPopup == LabelPopup)
            {
                string text = selection.GetComponent<Interactable>().GetDecription(this);
                if (string.IsNullOrWhiteSpace(text))
                {
                    if (currentPopup != null)
                    {
                        currentPopup.Hide();
                        currentPopup = null;
                    }
                    return;
                }

                LabelPopup.SetText(text);
                if (currentPopup == null)
                    LabelPopup.Display();
                currentPopup = LabelPopup;
            }
        }
        else if (currentPopup != null)
        {
            currentPopup.Hide();
            currentPopup = null;
        }
    }

    private void FixedUpdate()
    {
        if (timer.TimeIsStopped())
        {
            return;
        }
        if (canMove)
            _rigidbody2D.MovePosition(_rigidbody2D.position + speed * input * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Interactable interactive = collision.GetComponent<Interactable>();
        if (interactive != null && !actionsInRange.Contains(collision.gameObject))
        {
            actionsInRange.Add(collision.gameObject);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        Interactable interactive = other.GetComponent<Interactable>();
        if (interactive != null)
        {
            actionsInRange.Remove(other.gameObject);
        }
    }
    #endregion

    public void ChangeMood(float newPercentage)
    {
        speed = baseSpeed + (1 - newPercentage) * (baseSpeed * 1.4f);

        //Maj Color
        newPercentage = map(newPercentage, 0, 1, .2f, 1);
        float redValue = 30 * newPercentage + 225;
        float gbValue = newPercentage * 255;
        Color color = new Color(redValue / 255f, gbValue / 255f, gbValue / 255f, 1f);
        this.GetComponentInChildren<SpriteRenderer>().color = color;
    }
    float map(float value, float from1, float to1, float from2, float to2) => (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    #region Private
    private void PlayerOrderInLayer()
    {
        GetComponentInChildren<Renderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }

    #region Movement

    
    private void PlayerMove()
    {
        input = new Vector2(inputManager.GetAxis("Horizontal"), inputManager.GetAxis("Vertical"));
        bool isMoving = input.magnitude > .2f;

        if (!isMoving)
        {
            input = Vector2.zero;
        }
        else if(!IsDashing() && CanAfford(dashPrice))
        {
            if (input != Vector2.zero && inputManager.GetButtonDown("Dash"))
            {
                Pay(dashPrice);
                dashTimeRemaining = dashDuration;
            }
        }


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

        if (IsDashing())
        {
            input = input * dashCoefficient;
            dashTimeRemaining -= Time.deltaTime;
        }

        _animator.SetBool("IsWalkingUp", wu);
        _animator.SetBool("IsWalkingRight", wr);
        _animator.SetBool("IsWalkingDown", wd);
        _animator.SetBool("IsWalkingLeft", wl);
        _animator.SetBool("IsRunningLeft", IsRunning && wl);
        _animator.SetBool("IsRunningRight", IsRunning && wr);
        _animator.speed = isMoving ? input.magnitude : 1;
    }

    private bool IsDashing()
    {
        return dashTimeRemaining > 0;
    }
    #endregion


    public bool CanAfford(int price)
    {
        return money >= price;
    }

    // Returns true if successful
    public void Pay(int price)
    {
        money -= price;
    }

    public bool CanMakeLemonade(int lemonsAmount)
    {
        return lemons >= lemonsAmount;
    }

    public void SellLemonade(int lemonadePrice,int lemonsAmount)
    {
        this.money += lemonadePrice;
        this.lemons -= lemonsAmount;
        Debug.Log("Grandma now has $" + money);
    }

    public void HarvestLemons(int lemonsCount)
    {
        lemons += lemonsCount;
    }
    #endregion
    #endregion

    #region Public
    public void DestroyInteractive(GameObject toDestroy)
    {
        actionsInRange.Remove(toDestroy);
        Destroy(toDestroy);
    }

    public void Speak(string message, float baseTime= 1.5f, float delay=0f)
    {
        //DialoguePopup.SetText(message);
        if(delay == -1)
        {
            delay = baseTime;
        }
        StartCoroutine(DialoguePopup.PopupDeactivation(delay, baseTime, message));  ;
    }
    #endregion
}