using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MagicPanelController : MonoBehaviour
{
    [HideInInspector] public CharacterFightController character;
    [HideInInspector] public PlayerDecisionController decisionController;

    [HideInInspector] public List<CharacterFightController> enemyParty;

    [HideInInspector] public List<CharacterFightController> playerParty;

    public GameObject magicPanel;

    public GameObject buttonPrefab;

    public GameObject targetManagerPanel;

    private Panel panel;

    List<IMagicObserver> observers = new List<IMagicObserver>();


    public void Setup(CharacterFightController _character, PlayerDecisionController _decisionController, List<CharacterFightController> _enemyParty, List<CharacterFightController> _playerParty)
    {
        character = _character;
        decisionController = _decisionController;
        enemyParty = _enemyParty;
        playerParty = _playerParty;
        panel = GetComponent<Panel>();
        //setup observers
        MasterBattleController masterBattleController =   GameObject.Find("Master Manager").GetComponent<MasterBattleController>();
        AddObserver(masterBattleController);
    }

    public void Display()
    {
        resetButtons();
        displayItems();
    }

    private void displayItems()
    {
        int yPosition = 120;
        if(character.doll.magicSkills == null)
        {
            return;
        }
        Transform panel = this.transform.GetChild(0);
        for(int i = 0; i < character.doll.magicSkills.spells.Count; i++)
        {
            Spell spell = character.doll.magicSkills.spells[i];
            Transform button = panel.GetChild(i);
            TextMeshProUGUI buttonText = button.GetChild(0).GetComponent<TextMeshProUGUI>();
            //Debug.Log(button.GetChild(0).gameObject);
            buttonText.SetText(spell.name);

            Button buttonComponent = button.GetComponent<Button>();
            int x = i;
            buttonComponent.onClick.AddListener(delegate{ChosenSpell(x);});

            button.gameObject.SetActive(true);
        }
    }

    public void AddObserver(IMagicObserver observer)
    {
        observers.Add(observer);
    }

    public void NotifyObserversNoMana(string spellname)
    {
        foreach(IMagicObserver observer in observers)
        {
            observer.NotifyNoMana(spellname);
        }
    }

    public void SetActive()
    {
        panel.SetActive();
    }

    public void SetInActive()
    {
        panel.SetInActive();
    }

    #region ButtonFunctions
    public void GoBack()
    {
        decisionController.SetActive();
        this.gameObject.SetActive(false);
    }

    public void ChosenSpell(int i)
    {
        this.gameObject.SetActive(false);
        if(character.doll.stats.mana < character.doll.magicSkills.spells[i].mana)
        {
            NotifyObserversNoMana(character.doll.magicSkills.spells[i].name);
            return;
        }
        CharacterDecision decision = new CharacterDecision(character, Decision.Magic, character.doll.magicSkills.spells[i]);
        //decisionController.InputDecision(decision);
        //Destroy(this.gameObject);
        if(character.doll.magicSkills.spells[i] is DamageSpell){
            targetManagerPanel.SetActive(true);
            TargetController targetController = targetManagerPanel.GetComponent<TargetController>();
            targetController.Setup(enemyParty, decisionController, decision);
            targetController.optionalBagPanel = this.gameObject;
            targetController.SetDecisionPanel(decisionController.decisionPanel);
            targetController.Display();
            targetController.SetActive();
        }
        else if(character.doll.magicSkills.spells[i] is HealSpell){
            targetManagerPanel.SetActive(true);
            TargetController targetController = targetManagerPanel.GetComponent<TargetController>();
            targetController.Setup(playerParty, decisionController, decision);
            targetController.optionalBagPanel = this.gameObject;
            targetController.SetDecisionPanel(decisionController.decisionPanel);
            targetController.Display();
            targetController.SetActive();
        }
    }

    private void resetButtons(){
        Transform panel = this.transform.GetChild(0);
        for(int i = 0; i < 6; i++){
            panel.GetChild(i).gameObject.SetActive(false);
        }
    }

    /*public void SetTarget(CharacterController target)
    {
        CharacterDecision decision = new CharacterDecision(Decision.Bag, savedItem, target);
        decisionController.InputDecision(decision);
        Destroy(this.gameObject);
    }*/

    #endregion
}
