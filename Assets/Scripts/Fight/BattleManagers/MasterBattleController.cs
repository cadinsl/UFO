using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MasterBattleController : MonoBehaviour, IDialogObserver, IMagicObserver, IBattleObserver
{
    public GameObject battleManager;

    private BattleController battleController;

    [HideInInspector] public CharacterFightController[] playerParty;

    [HideInInspector] public CharacterFightController[] enemyParty;

    public GameObject[] playerPartyObjects;

    public GameObject[] enemyPartyObjects;

    public DialogController dialogController;

    public PlayerDisplayController playerDisplayController;

    public GameObject virtualCamera;

    public GameObject canvas;

    public void Start()
    {
    }

    public void setupParties(CharacterDoll[] playerPartyDolls, CharacterDoll[] enemyPartyDolls)
    {
        playerParty = new CharacterFightController[playerPartyDolls.Length];
        for(int i = 0; i < playerPartyDolls.Length; i++)
        {
            CharacterFightController fightController = playerPartyObjects[i].GetComponent<CharacterFightController>();
            fightController.doll = playerPartyDolls[i];
            playerPartyObjects[i].SetActive(true);
            playerParty[i] = fightController;
        }

        enemyParty = new CharacterFightController[enemyPartyDolls.Length];
        for(int i = 0; i < enemyPartyDolls.Length; i++)
        {
            CharacterFightController fightController = enemyPartyObjects[i].GetComponent<CharacterFightController>();
            fightController.doll = enemyPartyDolls[i];
            fightController.DisplayModel();
            setupEnemyStats(fightController);
            enemyPartyObjects[i].SetActive(true);
            enemyParty[i] = fightController;
        }
    }

    public void startBattle()
    {
        dialogController.gameObject.SetActive(true);
        playerDisplayController.DisplayCharacter(playerParty);
        battleController = battleManager.GetComponent<BattleController>();
        battleController.SetupParties(playerParty.ToList(), enemyParty.ToList());
        battleController.AddObserver(this);
        virtualCamera.SetActive(true);
        canvas.SetActive(true);
        battleController.Reset();
        dialogController.DeleteAllDialogs();
        battleController.StartTurn();
    }

    public void endBattle()
    {
        dialogController.gameObject.SetActive(false);
        virtualCamera.SetActive(false);
        canvas.SetActive(false);
        removeEnemyModels();
        EncouterTranslator encounterTranslator = GameObject.FindGameObjectWithTag("Encounter Translator").GetComponent<EncouterTranslator>();
        encounterTranslator.SetupGoBackAdventure();
    }

    public void NotifyEnd()
    {

    }

    public void NotifyEndDecision()
    {
        battleController.goToNextDecision = true;
    }

    public void NotifyEndOfBattle()
    {
        Debug.Log("Voila");
    }

    public void NotifyNoMana(string spellname)
    {
        List<string> dialogTexts = new List<string>();
        dialogTexts.Add("Not enough Mana for "+spellname);
        dialogController.AddObserver(this);
        dialogController.Display(dialogTexts, dialogController.Next);
    }

    public void NotifyAboutToAct(CharacterDecision characterDecision)
    {
        List<string> dialogTexts = new List<string>();
        switch(characterDecision.decision)
        {
            case Decision.Fight:
                dialogTexts.Add(characterDecision.from.name + " attacks " + characterDecision.target.name);
            break;
            case Decision.Bag:
                dialogTexts.Add(characterDecision.from.name+ " is going to use " + characterDecision.decisionObject.name + " on " + characterDecision.target.name);
            break;
            case Decision.Magic:
                if(characterDecision.decisionObject is DamageSpell)
                {
                    dialogTexts.Add(characterDecision.from.name+ " casted " + characterDecision.decisionObject.name + " on " + characterDecision.target.name);
                }
            break;
            case Decision.Guard:
                dialogTexts.Add(characterDecision.from.name + " is guarding");
            break;
            case Decision.Run:
                dialogTexts.Add(characterDecision.from.name + " tried to run!");
            break;
        }
        dialogController.AddObserver(this);
        dialogController.Display(dialogTexts, dialogController.NextDecision);
    }

    public void NotifyDecisionActed(CharacterDecisionResult result)
    {
        List<string> dialogTexts = new List<string>();
        CharacterDecision characterDecision = result.characterDecision;
        switch(characterDecision.decision){
            case Decision.Fight:
                if(result.successful)
                {
                    dialogTexts.Add("It deals " + result.points + " in Damage");
                }
                else
                {
                    dialogTexts.Add("The Attack Misses!");
                }
            break;
            case Decision.Bag:
                if(result.successful)
                {
                    if(result.characterDecision.decisionObject is AttackItem)
                    {
                        dialogTexts.Add("It deals " + result.points + " in Damage");
                    }
                }
                else
                {
                    dialogTexts.Add("It Misses!");
                }
            break;
            case Decision.Magic:
                if(result.successful)
                {
                    if(result.characterDecision.decisionObject is DamageSpell)
                    {
                        dialogTexts.Add("It deals " + result.points + " in Damage");
                    }
                }
                else
                {
                    dialogTexts.Add("It Misses!");
                }

            break;
            case Decision.Guard:
            NotifyEndDecision();
            return;
            case Decision.Run:
                if(result.successful)
                {
                    dialogTexts.Add("You were able to escape");
                }
                else
                {
                    dialogTexts.Add("You weren't able to escape");
                }
            break;
        }
        dialogController.AddObserver(this);
        dialogController.Display(dialogTexts, dialogController.NextDecision);
    }

    public void NotifyBattleEnded(BattleEndResult result)
    {
        List<string> dialogTexts = new List<string>();
        switch(result.result)
        {
            case BattleEndResult.Result.WON:
                dialogTexts.Add("YOU WON!");
            break;
            case BattleEndResult.Result.DEFEATED:
                dialogTexts.Add("you have been defeated :(");
            break;
            case BattleEndResult.Result.RAN:
                dialogTexts.Add("You got away!");
            break;
        }
        dialogController.AddObserver(this);
        dialogController.Display(dialogTexts, this.endBattle);
    }

    private void removeEnemyModels()
    {
        foreach(CharacterFightController enemyFight in enemyParty)
        {
            enemyFight.RemoveModel();
        }
    }

    private void setupEnemyStats(CharacterFightController characterFightController)
    {
        characterFightController.doll.status = Status.Normal;
        characterFightController.doll.stats.hp = characterFightController.doll.stats.maxHp;
        characterFightController.doll.stats.mana = characterFightController.doll.stats.maxMana;
    }
}
