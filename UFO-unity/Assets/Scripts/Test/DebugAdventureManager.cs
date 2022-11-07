using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAdventureManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(Application.isEditor)
        {
            resetPlayerPartyStats(AdventureManager.Instance.playerParty);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void resetPlayerPartyStats(CharacterAdventureController[] playerParty)
    {
        foreach(CharacterAdventureController playerCharacter in playerParty)
        {
            playerCharacter.doll.resetToNormal();
        }
    }
}
