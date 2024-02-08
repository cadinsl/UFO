using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PausedDecisionController : APausedMenu
{
    private CharacterDoll characterDoll;
    public PausedInventoryController pausedInventoryController;
    public PausedMagicSkillsController pausedMagicSkillsController;

    public UnityEvent unpauseGameEvent;

    public Stack<APausedMenu> pausedMenus = new Stack<APausedMenu>();
    public Panel panel;
    public Panel pickCharacterPanel;

    public void Start()
    {
        unpauseGameEvent.AddListener(AdventureManager.Instance.UnpauseGame);
        pausedMenus.Push(pausedInventoryController);
        pausedMenus.Push(pausedMagicSkillsController);
    }
    public override void Display(CharacterDoll characterDoll)
    {
        this.characterDoll = characterDoll;
        this.gameObject.SetActive(true);
        panel.SetActive();
        pickCharacterPanel.SetActive();
    }
    public void GoBack()
    {
        unpauseGameEvent.Invoke();
    }

    public void Inventory()
    {
        pausedMagicSkillsController.Close();
        pausedInventoryController.Display(this.characterDoll);
        panel.SetInActive();
        pickCharacterPanel.SetInActive();
    }

    public void Magic()
    {
        pausedInventoryController.Close();
        pausedMagicSkillsController.Display(this.characterDoll);
        panel.SetInActive();
        pickCharacterPanel.SetInActive();
    }

    public override void Close()
    {
        foreach(APausedMenu menu in pausedMenus)
        {
            menu.Close();
        }
        pausedMenus.Clear();
        this.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
