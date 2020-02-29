using Rewired;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ChoicePopup;

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
    private ChoicePopup.ChoicePopup choicePopup;
    #endregion

    #region Public
    #endregion

    #region Private
    private bool inMenu;
    private bool inQTE;

    private Rewired.Player inputManager;
    private Vector2 input;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private bool isRunning;
    #endregion
    #endregion

    #region Methods
    private void Start()
    {
        inputManager = ReInput.players.GetPlayer(playerID);
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }


    private void Update()
    {
        input = new Vector2(inputManager.GetAxis("Horizontal"), inputManager.GetAxis("Vertical"));
        bool isMoving = input.magnitude > .2f;

        if (!isMoving)
        {
            input = Vector2.zero;
        }

        _animator.SetBool("Walking", !isRunning && isMoving);
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
    }

    private IEnumerator DisplayChoicePopup()
    {
        choicePopup.Display();
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
