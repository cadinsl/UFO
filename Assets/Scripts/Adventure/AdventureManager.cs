using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdventureManager : MonoBehaviour
{
    #region Singleton
    public static AdventureManager Instance { get; private set;}
    [HideInInspector] public UnityEvent<NPCAdventure> playerTalksToNPC;

    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }
        playerInput = new PlayerInput();
    }
    #endregion

    public CharacterAdventureController[] playerParty;

    public GameObject canvas;

    public GameObject currentLeader;

    public PausedController pausedController;

    public PlayerInput playerInput;

    public bool isPaused;

    private bool isAdventure;

    private void OnEnable()
    {
        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }

    public void Start()
    {
        isPaused = false;
        isAdventure = true;
    }
    public void Update()
    {
        if ( playerInput.Player.Pause.triggered && isAdventure)
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                if (pausedController.CanUnpauseGame())
                {
                    UnpauseGame();
                }

            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pausedController.DisplayPauseSettings(playerParty);
        currentLeader.GetComponent<CharacterBrainAdventure>().DesactivatePlayer();
        isPaused = true;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        pausedController.ClosePauseMenu();
        currentLeader.GetComponent<CharacterBrainAdventure>().ActivatePlayer();
        isPaused = false;
    }

    public void SetupBackFromEncounter()
    {
    }

    public void DesactivateAdventure()
    {
        canvas.SetActive(false);
        currentLeader.GetComponent<CharacterBrainAdventure>().DesactivatePlayer();
        isAdventure = false;
    }

    public void ActivateAdventure()
    {
        canvas.SetActive(true);
        currentLeader.GetComponent<CharacterBrainAdventure>().ActivatePlayer();
        isAdventure = true;
    }
}
