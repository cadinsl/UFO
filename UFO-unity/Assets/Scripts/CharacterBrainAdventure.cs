using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBrainAdventure : MonoBehaviour
{
    [HideInInspector] public CharacterMovement characterMovement;
    [HideInInspector] public PlayerAdventureInput playerAdventureInput;

    private NPCAdventure npcInArea;

    public void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
        playerAdventureInput = GetComponent<PlayerAdventureInput>();
        playerAdventureInput.characterBrain = this;
    }


    public void activateNPCAreaTalk(NPCAdventure npcAdventure)
    {
        npcInArea = npcAdventure;
        playerAdventureInput.canTalkToNPC = true;
    }
    public void disactivateNPCAreaTalk()
    {
        npcInArea = null;
        playerAdventureInput.canTalkToNPC = false;
    }
    public void TalkToNPC()
    {
        lookAtNPC(npcInArea.transform);
        StopMovement();
        playerAdventureInput.DisableInput();
        npcInArea.AnswerPlayer(this);
    }

    public void EndTalkToNPC()
    {
        StartMovement();
        playerAdventureInput.EnableInput();
    }

    public void StopMovement()
    {
        characterMovement.enabled = false;
    }

    public void StartMovement()
    {
        characterMovement.enabled = true;
    }

    public void DesactivatePlayer()
    {
        StopMovement();
        playerAdventureInput.DisableInput();
    }

    public void ActivatePlayer()
    {
        StartMovement();
        playerAdventureInput.EnableInput();
    }

    private void lookAtNPC(Transform targetTransform)
    {
        this.transform.LookAt(targetTransform);
    }
}
