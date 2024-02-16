using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public int movementSpeed;

    private CharacterController characterController;
    private AdventurePlayerAnimation adventurePlayerAnimation;
    private PlayerAdventureInput playerAdventureInput;

    public PlayerInput playerInput;
    private InputAction moveInput;
    private Rigidbody rb;
    public float neilPushForce;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        moveInput = playerInput.Player.Move;
        moveInput.Enable();
    }

    private void OnDisable()
    {
        moveInput.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
        adventurePlayerAnimation = gameObject.GetComponent<AdventurePlayerAnimation>();
        playerAdventureInput = gameObject.GetComponent<PlayerAdventureInput>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAdventureInput.isInputEnabled())
        {
            characterController.enabled = true;
            Vector3 move = new Vector3(moveInput.ReadValue<Vector2>().x, 0, moveInput.ReadValue<Vector2>().y);
            move = Vector3.ClampMagnitude(move, 1);
            if (adventurePlayerAnimation != null)
            {
                adventurePlayerAnimation.SetSpeed(move.magnitude);
            }
            characterController.SimpleMove(move * movementSpeed);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }
        }
    }

    public void StopMovement()
    {
        characterController.enabled = false;
        adventurePlayerAnimation.SetSpeed(0);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.position = this.gameObject.transform.position;
        rb.rotation = this.gameObject.transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Equals("Neil"))
        {
            var neil = collision.gameObject;
            neil.GetComponent<Rigidbody>().velocity = -neil.transform.forward * neilPushForce;
        }
    }
}
