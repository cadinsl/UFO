using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBrainAdventure : MonoBehaviour
{
    [HideInInspector] public CharacterMovement characterMovement;
    [HideInInspector] public PlayerAdventureInput playerAdventureInput;

    private NPCAdventure npcInArea;
    private ItemGroundAdventure itemInArea;

    public void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
        playerAdventureInput = GetComponent<PlayerAdventureInput>();
        playerAdventureInput.characterBrain = this;
        playerAdventureInput.canTalkToNPC = false;
    }

    public void activateItemArea(ItemGroundAdventure itemGround){
        itemInArea = itemGround;
        playerAdventureInput.canPickupItem = true;
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
    public void disactivateItemArea(){
        itemInArea = null;
        playerAdventureInput.canPickupItem = false;
    }
    public void TalkToNPC()
    {
        lookAtNPC(npcInArea.transform);
        StopMovement();
        playerAdventureInput.DisableInput();
        npcInArea.AnswerPlayer(this);
    }

    public void EndTalk()
    {
        StartMovement();
        playerAdventureInput.EnableInput();
    }

    public void PickUpItem(){
        StopMovement();
        playerAdventureInput.DisableInput();
        itemInArea.AnswerPlayer(this);
    }

    public void StopMovement()
    {
        characterMovement.StopMovement();
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

    private void lookAtNPC(Transform target)
    {
        Vector3 targetPostition = new Vector3( target.position.x, this.transform.position.y, target.position.z );
        transform.LookAt(targetPostition);
    }
}
