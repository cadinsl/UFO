using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDisplayController : MonoBehaviour
{
    [HideInInspector] public CharacterFightController[] playerParty;

    public GameObject characterPanel;

    public GameObject[] characterPlacements;
    private bool itIsDisplaying;

    public void Update()
    {
        if(itIsDisplaying)
        {
            UpdateCharacters();
        }
    }

    public void DisplayCharacter(CharacterFightController[] playerParty)
    {
        this.playerParty = playerParty;
        itIsDisplaying = true;
        characterPanel.SetActive(true);
    }

    public void UpdateCharacters()
    {
        for(int i = 0; i < playerParty.Length; i++)
        {
            CharacterFightController character = playerParty[i];
            GameObject characterPlacement = characterPlacements[i];
            characterPlacement.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(character.doll.name);
            characterPlacement.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(character.doll.stats.hp +"");
            characterPlacement.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(character.doll.stats.mana +"");
            characterPlacement.transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText(character.doll.stats.xp +"");
        }
    }

    public void HidePanel()
    {
        itIsDisplaying = false;
        characterPanel.SetActive(false);
    }
}
