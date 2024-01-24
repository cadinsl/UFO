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
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAdventureInput.isInputEnabled())
        {
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
        adventurePlayerAnimation.SetSpeed(0);

    }
}
