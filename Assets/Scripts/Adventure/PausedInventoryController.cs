using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PausedInventoryController : APausedMenu
{
    public PausedTargetController targetController;
    public Panel panel;
    private Inventory inventory;
    private int maxButtons = 8;

    private Item chosenItem;

    public override void Display(CharacterDoll doll)
    {
        inventory = doll.inventory;
        updateItems();
        this.gameObject.SetActive(true);
        panel.SetActive();
    }

    public void ChosenItem(int index)
    {
        Item item = inventory.items[index];
        if(!(item is HealItem))
        {
            List<string> texts = new List<string>();
            texts.Add("Cannot use this item rn </3");
            PausedController.Instance.DisplayDialog(texts, delegate{ this.panel.SetActive(); });
        }
        else
        {
            chosenItem = item;
            displayTargets();
        }
    }

    public void UseItem(CharacterDoll target)
    {
        if(chosenItem is HealItem)
        {
            target.Heal(((HealItem)chosenItem).Heal);
            inventory.items.Remove(chosenItem);
        }
        updateItems();
    }

    private void displayTargets()
    {
        targetController.DisplayTargets(PausedController.Instance.playerPartyDolls, UseItem);
        panel.SetInActive();
    }

    private void updateItems()
    {
        for(int i = 0; i < maxButtons; i++)
        {
            GameObject buttonObject = this.transform.GetChild(i).gameObject;
            if(i < inventory.items.Count)
            {
                Button button = buttonObject.GetComponent<Button>();
                buttonObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(inventory.items[i].name);
                int x = i;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(delegate{ChosenItem(x);});
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
