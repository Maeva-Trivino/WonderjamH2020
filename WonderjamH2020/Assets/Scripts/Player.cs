using Rewired;
using UnityEngine;
using System.Collections;
using UI.ChoicePopup;
using QTE;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 2;
    [SerializeField]
    private int playerID = 0;

    private bool inMenu;
    private bool inQTE;

    private HashSet<GameObject> actionsInRange;
    private UserAction currentAction;
    private GameObject selected;
    [SerializeField]
    private GameObject QTEPopup;


    public Rewired.Player inputManager;
    private Vector2 input, direction = Vector2.down; // direction will be used for animations
    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        inputManager = ReInput.players.GetPlayer(playerID);
        _rigidbody2D = GetComponent<Rigidbody2D>();
        actionsInRange = new HashSet<GameObject>();
        updateQTEPopup(null);

    }


    private void Update()
    {
        if(!inQTE)
        {
            input = new Vector2(inputManager.GetAxis("Horizontal"), inputManager.GetAxis("Vertical"));
            if (input.magnitude < .1f) input = Vector2.zero;

            if( input != Vector2.zero)
            {
                direction = input.normalized;
            }
        }

        HandleQTEAction();
        UpdateQTESelection();

    }

    private void FixedUpdate()
    {
        if(!inQTE && !inMenu)
        {
            _rigidbody2D.MovePosition(_rigidbody2D.position + speed * input * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ChoicePopup choicePopUp = collision.GetComponent<ChoicePopup>();
        if(choicePopUp != null)
        {
            StartCoroutine(DisplayPopUp(choicePopUp));
        }
        Interactive interactive = collision.GetComponent<Interactive>();
        if (interactive != null)
        {
            actionsInRange.Add(collision.gameObject);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Interactive interactive = collision.GetComponent<Interactive>();
        if (interactive != null)
        {
            actionsInRange.Remove(collision.gameObject);
        }

    }

    private IEnumerator DisplayPopUp(ChoicePopup choicePopUp)
    {
        choicePopUp.Display(transform);
        inMenu = true;
        bool choiceMade = false;
        while(!choiceMade)
        {
            if (inputManager.GetButtonDown("Cancel"))
            {
                Debug.Log("Cancel");
                choiceMade = true;
                break;
            }
            float input = inputManager.GetAxis("Horizontal");
            if(inputManager.GetButtonDown("MenuLeft"))
            {
                Debug.Log("Gauche");
                choicePopUp.GoLeft();
            }
            if(inputManager.GetButtonDown("MenuRight"))
            {
                Debug.Log("Droite");
                choicePopUp.GoRight();
            }
            if (inputManager.GetButton("Validate"))
            {
                Debug.Log("Validé !");
                choiceMade = true;
                choicePopUp.Validate();
            }
            yield return true;
        }
        inMenu = false;
        choicePopUp.Hide();
    }

    #region 
    public void HandleQTEAction()
    {
        if (currentAction != null)
        {
            if (inputManager.GetButtonDown("Interact"))
            {
                inQTE = true;
                updateQTEPopup(currentAction);
            }
            else if(inputManager.GetButtonUp("Interact"))
            {
                inQTE = false;
                currentAction = null;
                Debug.Log("Set QTE to false (button interact up)");

            }
            if (inQTE)
            {
                currentAction.Do();
                if (currentAction.IsDone())
                {
                    currentAction = null;
                    inQTE = false;
                    Debug.Log("Set QTE to false (isdone)");
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
                    UserAction action = o.GetComponent<Interactive>().GetAction(inputManager);
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

            if (inQTE)
            {
                if (! (action is ComboAction))
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
}

