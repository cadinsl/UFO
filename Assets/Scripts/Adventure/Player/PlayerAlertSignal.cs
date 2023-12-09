using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAlertSignal : MonoBehaviour
{
    public PlayerAdventureInput playerInput;
    public GameObject AlertSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInput.canPickupItem || playerInput.canTalkToNPC)
        {
            AlertSprite.SetActive(true);
        }
        else
        {
            AlertSprite.SetActive(false);
        }
    }
}
