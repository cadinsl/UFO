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

    public GameObject magicPanel;

    public GameObject buttonPrefab;

    public GameObject targetManagerPrefab;

    List<IMagicObserver> observers = new List<IMagicObserver>();


    public void Setup(CharacterFightController _character, PlayerDecisionController _decisionController, List<CharacterFightController> _enemyParty)
    {
        character = _character;
        decisionController = _decisionController;
        enemyParty = _enemyParty;

        //setup observers
        MasterBattleController masterBattleController =   GameObject.Find("Master Manager").GetComponent<MasterBattleController>();
        AddObserver(masterBattleController);
    }

    public void Display()
    {
        GameObject canvas = GameObject.Find("Canvas");
        this.transform.SetParent(canvas.transform);
        this.transform.localPosition = Vector3.zero;
        magicPanel.SetActive(true);
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

    #region ButtonFunctions
    public void GoBack()
    {
        decisionController.SetActive();
        Destroy(this.gameObject);
    }

    public void ChosenSpell(int i)
    {
        if(character.doll.stats.mana < character.doll.magicSkills.spells[i].mana)
        {
            NotifyObserversNoMana(character.doll.magicSkills.spells[i].name);
            return;
        }
        
        CharacterDecision decision = new CharacterDecision(character, Decision.Magic, character.doll.magicSkills.spells[i]);
        //decisionController.InputDecision(decision);
        //Destroy(this.gameObject);
        GameObject targetManagerInstance = Instantiate(targetManagerPrefab);
        TargetController targetController = targetManagerInstance.GetComponent<TargetController>();
        targetController.Setup(enemyParty, decisionController, decision);
        targetController.optionalBagPanel = this.gameObject;
        targetController.Display();
    }

    /*public void SetTarget(CharacterController target)
    {
        CharacterDecision decision = new CharacterDecision(Decision.Bag, savedItem, target);
        decisionController.InputDecision(decision);
        Destroy(this.gameObject);
    }*/

    #endregion
}
