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

    public void Start()
    {
        unpauseGameEvent.AddListener(AdventureManager.Instance.UnpauseGame);
        pausedMenus.Push(pausedInventoryController);
    }
    public override void Display(CharacterDoll characterDoll)
    {
        this.characterDoll = characterDoll;
        this.gameObject.SetActive(true);
        panel.SetActive();
    }
    public void GoBack()
    {
        unpauseGameEvent.Invoke();
    }

    public void Inventory()
    {
        pausedInventoryController.Display(this.characterDoll);
    }

    public void Magic()
    {
        pausedMagicSkillsController.Display(this.characterDoll);
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
}
