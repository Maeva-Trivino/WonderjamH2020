using Rewired;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private int playerID = 0;

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
        Vector2 input = new Vector2(inputManager.GetAxis("Move Horizontal"), inputManager.GetAxis("Move Vertical"));
        if (input.magnitude < .1f) input = Vector2.zero;

        if( input != Vector2.zero)
        {
            direction = input.normalized;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + speed * input * Time.fixedDeltaTime);
    }
}
