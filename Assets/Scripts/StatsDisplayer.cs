using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsDisplayer : APausedMenu
{
    public GameObject playerPanels;
    #region textHoldersForPanel
    private TextMeshProUGUI nameHolder;
    private TextMeshProUGUI levelHolder;
    private TextMeshProUGUI hpHolder;
    private TextMeshProUGUI manaHolder;
    private TextMeshProUGUI xpHolder;
    private TextMeshProUGUI strengthHolder;
    private TextMeshProUGUI defenseHolder;
    private TextMeshProUGUI speedHolder;
    private TextMeshProUGUI witHolder;
    private TextMeshProUGUI luckHolder;
    #endregion
    
    private bool isDisplaying = false;
    private CharacterStats playerStats;

    public void Awake()
    {
        setTextHolderFromPanel(playerPanels);
    }

    public void Update()
    {
        if(isDisplaying)
        {
            displayCharacterStats(playerStats);
        }
    }

    public override void Display(CharacterDoll playerDoll)
    {
        this.playerStats = playerDoll.stats;
        this.gameObject.SetActive(true);
        displayCharacterStats(playerStats);
        isDisplaying = true;
    }

    public override void Close()
    {
        isDisplaying = false;
        this.gameObject.SetActive(false);
    }

    public void ChangeCharacter(CharacterDoll doll)
    {
        Display(doll);
    }

    private void displayCharacterStats(CharacterStats stats)
    {
        nameHolder.SetText(stats.characterName);
        hpHolder.SetText(stats.hp +"/"+ stats.maxHp);
        manaHolder.SetText(stats.mana+"/"+stats.maxMana);
        xpHolder.SetText(stats.xp+"");
        strengthHolder.SetText(stats.strenght+"");
        defenseHolder.SetText(stats.defense+"");
        speedHolder.SetText(stats.speed+"");
        witHolder.SetText(stats.intelligence+"");
        luckHolder.SetText(stats.luck+"");
    }

    private void setTextHolderFromPanel(GameObject playerPanel)
    {
        nameHolder = playerPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        hpHolder = playerPanel.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        manaHolder = playerPanel.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        xpHolder = playerPanel.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>();
        strengthHolder = playerPanel.transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>();
        defenseHolder = playerPanel.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>();
        speedHolder = playerPanel.transform.GetChild(6).GetChild(1).GetComponent<TextMeshProUGUI>();
        witHolder = playerPanel.transform.GetChild(7).GetChild(1).GetComponent<TextMeshProUGUI>();
        luckHolder = playerPanel.transform.GetChild(8).GetChild(1).GetComponent<TextMeshProUGUI>();
    }
}
