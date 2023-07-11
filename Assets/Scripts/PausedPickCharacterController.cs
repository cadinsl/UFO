using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PausedPickCharacterController : APausedMenu
{
    public GameObject pickCharacterPanel;
    public Button[] buttons;

    private CharacterDoll[] dolls;

    private CharacterDoll currentDoll;

    public override void Display(CharacterDoll doll)
    {
        currentDoll = doll;
        dolls = PausedController.Instance.playerPartyDolls;
        setupCharacterButtons();
        this.gameObject.SetActive(true);
    }

    public override void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void ChosenCharacter(int index)
    {
        if(dolls[index] == currentDoll)
        {
           //DO NOTHING
        }
        else
        {
           currentDoll = dolls[index];
           PausedController.Instance.ChangeCharacter(currentDoll);
        }
    }

    private void setupCharacterButtons()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            if(i < dolls.Length)
            {
                buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(dolls[i].name);
                int x = i;
                buttons[i].onClick.AddListener(delegate{ChosenCharacter(x);});
                buttons[i].gameObject.SetActive(true);
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }
}
