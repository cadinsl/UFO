using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    #region Field Declarations
    //public List<CharacterController> mainParty;
    //public List<CharacterController> enemyParty;
    [HideInInspector] public List<CharacterFightController> playerParty;
    [HideInInspector] public List<CharacterFightController>  enemyParty;


    public GameObject turnControllerPrefab;

    List<IBattleObserver> observers = new List<IBattleObserver>();

    public bool goToNextDecision;

    public DialogController dialogController;

    private List<CharacterDecision> allDecisions;

    private int currentIndex;
    private bool canOpenDialog = true;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //DebugTestParties();
        //StartTurn();
    }

    public void SetupParties(List<CharacterFightController> _playerParty, List<CharacterFightController> _enemyParty)
    {
        playerParty = _playerParty;
        enemyParty = _enemyParty;
        allDecisions = new List<CharacterDecision>();
    }

    public void StartTurn(){
        updateCharactersTurn();
        currentIndex = 0;
        turnControllerPrefab.SetActive(true);
        TurnController turnController = turnControllerPrefab.GetComponent<TurnController>();
        removeDeadCharacters(this.playerParty);
        turnController.Setup(this,playerParty, enemyParty);
        turnController.turn();
    }

    //NO NULL ALLOWED
    public void getAllDecision(CharacterDecision[] playerDecisions, CharacterDecision[] enemyDecisions){
        //Merging 2 arrays
        allDecisions = playerDecisions.Concat(enemyDecisions).ToList<CharacterDecision>();
        allDecisions.OrderBy(dec => dec.from.doll.stats.speed);
        //allDecisions.ForEach(displayDecisionAuthor);
        goToNextDecision = false;
        currentIndex = 0;
        dialogController.Display(dialogController.GetTextForDecisionAboutToAct(allDecisions[0]), ContinueOnDecision);
    }

    private void ContinueOnDecision()
    {
        
        if(allDecisions[currentIndex].from.doll.status == Status.Dead)
        {
            //Player Dead
            //currentIndex++;
            ContinueToNextDecision();
            return;
        }
        else if(allDecisions[currentIndex].target != null)
        {
            if(allDecisions[currentIndex].target.doll.status == Status.Dead)
            {
                //currentIndex++;
                dialogController.Display(DialogController.GetText("The Enemy already fainted!"), ContinueToNextDecision);
                return;
            }
            
        }
        ActOnDecision(allDecisions[currentIndex]);
    }

    private void ContinueToNextDecision()
    {
        if(allDecisions[currentIndex].target != null)
        {
            if(allDecisions[currentIndex].target.doll.status == Status.Dead)
            {   
                CharacterFightController deceasedCharacter = allDecisions[currentIndex].target;             
                CollectXP(allDecisions[currentIndex].from, deceasedCharacter);//Probably distribute XP
                //dialogController.Display(dialogController.GetTextForDeceasedPlayer(deceasedCharacter.doll), null);
                if (enemyParty.Contains(deceasedCharacter))
                {
                    enemyParty.Remove(deceasedCharacter);
                }
                else
                {

                    playerParty.Remove(deceasedCharacter);
                }
                //Destroy(allDecisions[currentIndex].target.gameObject);
            }
        }
        if(checkIfPartyDead(playerParty) || checkIfAngelDead(playerParty))
        {
            EndFight(new BattleEndResult(BattleEndResult.Result.DEFEATED));
            return;
        }
        else if(checkIfPartyDead(enemyParty))
        {
            EndFight(new BattleEndResult(BattleEndResult.Result.WON));
            return;
        }

        currentIndex++;
        while(currentIndex < allDecisions.Count && allDecisions[currentIndex].from.doll.status == Status.Dead)
        {
            currentIndex++;
        }
        if(currentIndex < allDecisions.Count)
        {
            
            dialogController.Display(dialogController.GetTextForDecisionAboutToAct(allDecisions[currentIndex]), ContinueOnDecision);
        }
        else
        { //GO TO NEXT ROUN
            StartTurn();
        }
    }


    public void ActOnDecision(CharacterDecision characterDecision)
    {
        Debug.Log("Got to act on decision");
        //CharacterDecisionResult result = null;
        switch(characterDecision.decision)
        {
            case Decision.Fight:
                characterDecision.from.Attack(characterDecision, DisplayDecisionResult);
            break;
            case Decision.Bag:
                Item item = (Item)characterDecision.decisionObject;
                if(item is AttackItem)
                {
                    characterDecision.from.Attack(characterDecision, (AttackItem)item, DisplayDecisionResult);
                }
            break;
            case Decision.Magic:
                Spell spell = (Spell)characterDecision.decisionObject;
                if(spell is DamageSpell)
                {
                    characterDecision.from.Attack(characterDecision, (DamageSpell)spell, DisplayDecisionResult);
                }
                else if(spell is HealSpell)
                {
                    characterDecision.from.Heal(characterDecision, (HealSpell) spell, DisplayDecisionResult);
                }
            break;
            case Decision.Guard:
                Buff guardBuff = new Buff(Buff.Type.Defense, 50, 1);
                characterDecision.from.applyBoost(guardBuff);
                DisplayDecisionResult(new CharacterDecisionResult(characterDecision, true, 0));
            break;
            case Decision.Run:
                bool success = runSuccesful(characterDecision.from);
                Debug.Log("Succesful run: "+ success);
                DisplayDecisionResult(new CharacterDecisionResult(characterDecision, success, 0));               
            break;
        }
        /*
        if (result != null)
        {
            return result;
        }
        return new CharacterDecisionResult(characterDecision, true, 0);
        */

    }

    public void DisplayDecisionResult(CharacterDecisionResult result)
    {
        if(result.characterDecision.decision == Decision.Run && result.successful)
        {
            EndFight(new BattleEndResult(BattleEndResult.Result.RAN));
            return;
        }

        dialogController.Display(dialogController.GetTextForDecisionResult(result), ContinueToNextDecision);
    }

    public void AddObserver(IBattleObserver observer)
    { 
        if(!observers.Contains(observer))
        { 
            observers.Add(observer);
        }
    }

    public void Reset(){
        if(allDecisions != null){
            allDecisions.Clear();
        }
    }

    private void EndFight(BattleEndResult result)
    {
        //StopAllCoroutines();
        notifyObserversBattleEnded(result);
        //SceneController.Instance.LoadAdventureScene(encouterTranslator.SetupGoBackAdventure);
        return;
    }

    private void CollectXP(CharacterFightController from, CharacterFightController to)
    {
            from.doll.stats.xp += to.doll.stats.xp;
    }

    private bool checkIfPartyDead(List<CharacterFightController> party)
    {
        return (party.Count == 0);
        /*
        foreach(CharacterFightController character in party)
        {
            if(character.status != Status.Dead)
            {
                return false;
            }
        }
        return true;
        */
    }

    private bool checkIfAngelDead(List<CharacterFightController> party)
    {
        var angel = party.FirstOrDefault(character => character.doll.id == 1);
        return angel == null;
    }

    private void updateCharactersTurn()
    {
        foreach(CharacterFightController c in playerParty)
        {
            c.TurnUpdate();
        }
        foreach(CharacterFightController e in enemyParty)
        {
            e.TurnUpdate();
        }
    }

    private bool runSuccesful(CharacterFightController character)
    {
        int enemyMaxSpeed = 0;
        foreach(CharacterFightController enemy in enemyParty)
        {
            if(enemy.doll.status != Status.Dead && enemy.doll.stats.speed > enemyMaxSpeed)
            {
                enemyMaxSpeed = enemy.doll.stats.speed;
            }
        }
        if(character.doll.stats.speed >= enemyMaxSpeed)
        {
            //Random probability 90% chance of succesful run
            int randomNumber = Random.Range(0, 10);
            if(randomNumber == 9)
            {
                return false;
            }
            return true;
        }
        else
        {
            int randomNumber = Random.Range(0, 10);
            if(randomNumber == 9)
            {
                return true;
            }
            return false;
        }
    }


    private void notifyObserversBattleEnded(BattleEndResult result)
    {
        foreach(IBattleObserver observer in observers)
        {
            observer.NotifyBattleEnded(result);
        }
    }

    private void displayEnemiesModel()
    {
    }

    private void displayDecisionAuthor(CharacterDecision decision){
        Debug.Log(decision.from.doll.name);
    }
    
    private void waitForCharacterAnimationToBeOver(CharacterFightController character)
    {
        
    }

    private void removeDeadCharacters(List<CharacterFightController> party){
        List<CharacterFightController> deceasedCharacters = new List<CharacterFightController>();
        foreach(CharacterFightController character in party){
            if(character.doll.status == Status.Dead){
                deceasedCharacters.Add(character);
            }
        }
        foreach(CharacterFightController character in deceasedCharacters){
            party.Remove(character);
        }
    }

    #region DEBUG FUNCTIONS
    /*private void DebugTestParties()
    {
        GameObject player = GameObject.Find("Player");
        GameObject enemy = GameObject.Find("Enemy");

        playerParty = new CharacterFightController[] {player.GetComponent<CharacterFightController>()};
        enemyParty = new CharacterFightController[] {enemy.GetComponent<CharacterFightController>()};
    }*/
    #endregion
}
