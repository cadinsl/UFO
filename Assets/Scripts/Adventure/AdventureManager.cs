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
    }
    #endregion

    public CharacterAdventureController[] playerParty;

    public GameObject canvas;

    public GameObject currentLeader;

    public PausedController pausedController;

    public bool isPaused;

    private bool isAdventure;

    public void Start()
    {
        isPaused = false;
        isAdventure = true;
        DontDestroyOnLoad(this.gameObject);
    }
    public void Update()
    {
        if (Input.GetButtonDown("Pause") && isAdventure)
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                UnpauseGame();

            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pausedController.DisplayPauseSettings(playerParty);
        isPaused = true;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        pausedController.ClosePauseMenu();
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
