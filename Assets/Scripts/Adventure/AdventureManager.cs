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

    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void Update()
    {
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pausedController.DisplayPauseSettings(playerParty);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        pausedController.ClosePauseMenu();
    }

    public void SetupBackFromEncounter()
    {
    }

    public void DesactivateAdventure()
    {
        canvas.SetActive(false);
        currentLeader.GetComponent<CharacterBrainAdventure>().DesactivatePlayer();
    }

    public void ActivateAdventure()
    {
        canvas.SetActive(true);
        currentLeader.GetComponent<CharacterBrainAdventure>().ActivatePlayer();
    }
}
