using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAdventure : MonoBehaviour
{
    private NPCDialog npcDialog;
    void Start()
    {
        npcDialog = GetComponent<NPCDialog>();
    }
    public void AnswerPlayer(CharacterBrainAdventure characterBrainAdventure)
    {
        npcDialog.DisplayDialog(characterBrainAdventure.EndTalkToNPC);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CharacterBrainAdventure characteBrain = other.gameObject.GetComponent<CharacterBrainAdventure>();
            characteBrain.activateNPCAreaTalk(this);
        }
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
