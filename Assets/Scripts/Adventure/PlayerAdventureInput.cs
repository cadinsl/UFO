using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerAdventureInput : MonoBehaviour
{
    public CharacterBrainAdventure characterBrain;
    public UnityEvent pausedGame;
    public UnityEvent sing;
    public UnityEvent singEnd;

    private bool enableInput = true;

    public bool canTalkToNPC = false;

    public bool canPickupItem = false;
    [SerializeField]
    private PlayerSing playerSing;
    public DogBark dogBark;

    private bool pauseMenuUp;

    public PlayerInput playerInput;

    private NPCAdventure npcInArea;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }


    void Start()
    {
        pausedGame.AddListener(AdventureManager.Instance.PauseGame);
        playerSing = this.GetComponent<PlayerSing>();
    }
    // Update is called once per frame
    void Update()
    {
        if(enableInput)
        {
            if (playerInput.Player.Sing.IsPressed())
            {
                playerSing.Sing(playerInput);
                sing.Invoke();
            }
            else if (canTalkToNPC && (playerInput.Player.Interact.WasPressedThisFrame()))
            {
                characterBrain.TalkToNPC();
            }
            else if(canPickupItem && (playerInput.Player.Interact.WasPressedThisFrame())){
                characterBrain.PickUpItem();
            }
            else if (playerInput.Player.SpecialAction.WasPressedThisFrame())
            {
                if (dogBark != null)
                    dogBark.Bark();
            }

            if(!playerInput.Player.Sing.IsPressed())
            {
                playerSing.Clear();
                singEnd.Invoke();
            }
        }
    }

    public void DisableInput()
    {
        enableInput = false;
    }

    public void EnableInput()
    {
        enableInput = true;
    }

    public bool isInputEnabled()
    {
        return enableInput;
    }

    public bool canPlayerMove()
    {
        return !playerInput.Player.Sing.IsPressed();
    }
}
