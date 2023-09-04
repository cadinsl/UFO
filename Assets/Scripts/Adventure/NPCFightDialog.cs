using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCFightDialog : NPCDialog
{
    public EncounterManager encounterManager;
    public CharacterDoll enemy;

    public void DisplayDialog(UnityAction action)
    {
        Debug.Log("DISPLAY DIALGO");
        DialogController dialogController = GameObject.Find("Dialog Manager").GetComponent<DialogController>();
        dialogController.Display(new List<string>(dialogTexts), StartFight);
    }

    public void StartFight()
    {
        Debug.Log("GOT TO START FIGHT");
        encounterManager.startPlannedEncounter(new CharacterDoll[] { enemy });
    }
}
