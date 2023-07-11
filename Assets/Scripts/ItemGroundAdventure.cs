using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemGroundAdventure : MonoBehaviour
{

    public Item item;
    public GameObject model;

    private CharacterBrainAdventure _characterBrainAdventure;
    void Start()
    {
    }
    public void AnswerPlayer(CharacterBrainAdventure characterBrainAdventure)
    {
        _characterBrainAdventure = characterBrainAdventure;
        addItemToPlayerInventory();
        model.SetActive(false);
        DisplayDialog(EndDialog);
    }

    public void EndDialog(){
        _characterBrainAdventure.EndTalk();
    }

    public void DisplayDialog(UnityAction action)
    {
        DialogController dialogController = GameObject.Find("Dialog Manager").GetComponent<DialogController>();
        dialogController.Display(dialogController.GetTextItemPickup(item.name), action);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            CharacterBrainAdventure characteBrain = other.gameObject.GetComponent<CharacterBrainAdventure>();
            characteBrain.activateItemArea(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
         if(other.gameObject.CompareTag("Player"))
        {
            CharacterBrainAdventure characteBrain = other.gameObject.GetComponent<CharacterBrainAdventure>();
            characteBrain.disactivateItemArea();
        }
    }

    private void addItemToPlayerInventory(){
        _characterBrainAdventure.gameObject.GetComponent<CharacterAdventureController>().doll.inventory.addItem(item);
    }
}
