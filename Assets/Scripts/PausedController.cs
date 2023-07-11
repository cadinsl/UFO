using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PausedController : MonoBehaviour
{
    #region Singleton
    public static PausedController Instance { get; private set;}

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
    
    public APausedMenu[] pausedMenus;

    public CharacterDoll[] playerPartyDolls;
    public DialogController pauseDialog;

    private CharacterDoll currentCharacter;



    public void Start()
    {
    }

    public void DisplayPauseSettings(CharacterAdventureController[] playerPartyControllers)
    {
        this.playerPartyDolls = GetDolls(playerPartyControllers);
        currentCharacter = playerPartyDolls[0];
        DisplayPauseMenuForCharacter();
    }
    private void DisplayPauseMenuForCharacter()
    {
        foreach(APausedMenu pausedMenu in pausedMenus)
        {
            pausedMenu.Display(currentCharacter);
        }
    }

    public void ClosePauseMenu()
    {
        foreach(APausedMenu pausedMenu in pausedMenus)
        {
            pausedMenu.Close();
        }
    }

    public void ChangeCharacter(CharacterDoll doll)
    {
        currentCharacter = doll;

        Debug.Log(currentCharacter.name);
        ClosePauseMenu();
        foreach(APausedMenu pausedMenu in pausedMenus)
        {
            pausedMenu.Display(currentCharacter);
        }
    }

    public void DisplayDialog(List<string> texts, UnityAction goBackAction)
    {
        pauseDialog.Display(texts, goBackAction);
    }

    public CharacterDoll[] GetDolls(CharacterAdventureController[] controllers)
    {
        CharacterDoll[] dolls = new CharacterDoll[controllers.Length];
        for(int i = 0; i < dolls.Length; i++)
        {
            dolls[i] = controllers[i].doll;
        }
        return dolls;
    }
}
