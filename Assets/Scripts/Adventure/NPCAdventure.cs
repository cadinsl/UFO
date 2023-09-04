using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAdventure : MonoBehaviour
{
    public Animator controller;
    private NPCDialog npcDialog;
    public bool allowLookAtCharacter;

    private CharacterBrainAdventure _characterBrainAdventure;
    void Start()
    {
        npcDialog = GetComponent<NPCDialog>();
    }
    public void AnswerPlayer(CharacterBrainAdventure characterBrainAdventure)
    {
        _characterBrainAdventure = characterBrainAdventure;
        if(controller != null)
            controller.SetBool("Talking", true );
        if(allowLookAtCharacter)
            lookAtCharacter(characterBrainAdventure);
        if (npcDialog is NPCFightDialog)
        {
            ((NPCFightDialog)npcDialog).DisplayDialog(EndDialog);
        }
        else
        {
            npcDialog.DisplayDialog(EndDialog);
        }
    }

    public void EndDialog(){
        _characterBrainAdventure.EndTalk();
        if (controller != null)
            controller.SetBool("Talking", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CharacterBrainAdventure characteBrain = other.gameObject.GetComponent<CharacterBrainAdventure>();
            characteBrain.activateNPCAreaTalk(this);
        }
    }

    private void lookAtCharacter(CharacterBrainAdventure characterBrainAdventure){
        Transform target = characterBrainAdventure.transform;
        Vector3 targetPostition = new Vector3( target.position.x, this.transform.position.y, target.position.z );
        this.transform.LookAt(targetPostition);
    }

    private void OnTriggerExit(Collider other)
    {
         if(other.gameObject.CompareTag("Player"))
        {
            CharacterBrainAdventure characteBrain = other.gameObject.GetComponent<CharacterBrainAdventure>();
            characteBrain.disactivateNPCAreaTalk();
        }
    }
}
