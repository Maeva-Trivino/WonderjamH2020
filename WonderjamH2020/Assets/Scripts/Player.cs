using Rewired;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 2;
    [SerializeField]
    private int playerID = 0;
    [SerializeField]
    private GameObject popUp;
    private bool inMenu;
    private bool inQTE;

    private Rewired.Player inputManager;
    private Vector2 input, direction = Vector2.down; // direction will be used for animations
    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        inputManager = ReInput.players.GetPlayer(playerID);
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        input = new Vector2(inputManager.GetAxis("Horizontal"), inputManager.GetAxis("Vertical"));
        if (inQTE || inMenu || input.magnitude < .1f) input = Vector2.zero;

        if( input != Vector2.zero)
        {
            direction = input.normalized;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + speed * input * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(DisplayPopUp());
    }

    private IEnumerator DisplayPopUp()
    {
        inMenu = true;
        //popUp.SetActive(true); 
        bool choiceMade = false;
        while(!choiceMade)
        {
            float input = inputManager.GetAxis("Horizontal");
            if (inputManager.GetButton("Validate"))
            {
                Debug.Log("Validé !");
                choiceMade = true;
            }
            yield return true;
        }
        inMenu = false;
    }
}
