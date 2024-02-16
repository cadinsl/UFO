using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PausedMagicSkillsController : APausedMenu
{
    public PausedTargetController targetController;
    public Panel panel;
    private CharacterDoll doll;
    private int maxButtons = 8;

    private Spell chosenSpell;

    public override void Display(CharacterDoll doll)
    {
        this.doll = doll;
        updateSpells();
        this.gameObject.SetActive(true);
        panel.SetActive();
    }

    public void ChosenSpell(int index)
    {
        Spell spell = doll.magicSkills.spells[index];
        if(!(spell is HealSpell))
        {
            List<string> texts = new List<string>();
            texts.Add("Cannot use this spell rn </3");
            this.panel.SetInActive();
            PausedController.Instance.DisplayDialog(texts, delegate{ this.Display(doll); });
        }
        else
        {
            if(doll.stats.mana < spell.mana)
            {
                List<string> texts = new List<string>();
                texts.Add("not enough mana </3");
                this.panel.SetInActive();
                PausedController.Instance.DisplayDialog(texts, delegate{ this.Display(doll); });
            }
            else
            {
                chosenSpell = spell;
                displayTargets();
            }
        }
    }

    public void UseSpell(CharacterDoll target)
    {
        if(chosenSpell is HealSpell)
        {
            List<string> texts = new List<string>();
            texts.Add("You are now healing <3");
            PausedController.Instance.DisplayDialog(texts, delegate { this.Display(doll); });
            target.Heal(((HealSpell)chosenSpell).heal);
            doll.stats.mana -= chosenSpell.mana;
        }
    }

    private void displayTargets()
    {
        targetController.DisplayTargets(PausedController.Instance.playerPartyDolls, UseSpell);
        panel.SetInActive();
    }

    private void updateSpells()
    {
        for(int i = 0; i < maxButtons; i++)
        {
            GameObject buttonObject = this.transform.GetChild(i).gameObject;
            if(i < doll.magicSkills.spells.Count)
            {
                Button button = buttonObject.GetComponent<Button>();
                buttonObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(doll.magicSkills.spells[i].name);
                int x = i;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(delegate{ChosenSpell(x);});
                buttonObject.SetActive(true);
            }
            else
            {
                buttonObject.SetActive(false);
            }
        }
    }

    public override void Close()
    {
        for(int i = 0; i < maxButtons; i++)
        {
            GameObject buttonObject = this.transform.GetChild(i).gameObject;
            buttonObject.SetActive(false);
        }
        targetController.Close();
        this.gameObject.SetActive(false);
    }
}
