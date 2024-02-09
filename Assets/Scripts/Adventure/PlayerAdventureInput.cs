using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerAdventureInput : MonoBehaviour
{
    public CharacterBrainAdventure characterBrain;
    public UnityEvent pausedGame;

    private bool enableInput = true;

    public bool canTalkToNPC = false;

    public bool canPickupItem = false;
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
            if(canTalkToNPC && (playerInput.Player.Interact.WasPressedThisFrame()))
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
}
