using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongSpecialArea : MonoBehaviour
{
    private bool playerIsOn;
    private CharacterBrainAdventure _characterBrainAdventure;
    public Item item;

    private void Start()
    {
        SingManager.Instance.specialAreas.Add(this);
    }

    public void GiveItemToPlayer()
    {
        if (playerIsOn)
        {
            addItemToPlayerInventory();
            DisplayDialog();
            Destroy(this.gameObject);
        }
    }

    public void DisplayDialog()
    {
        DialogController dialogController = GameObject.Find("Dialog Manager").GetComponent<DialogController>();
        dialogController.Display(dialogController.GetTextItemPickup(item.name), null);
    }

    private void addItemToPlayerInventory()
    {
        _characterBrainAdventure.gameObject.GetComponent<CharacterAdventureController>().doll.inventory.addItem(item);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerIsOn = true;
            _characterBrainAdventure = other.GetComponent<CharacterBrainAdventure>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsOn = false;
        }
    }
}


