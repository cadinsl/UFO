using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Turn Controller Workflow:
Turn -> getPlayerCharacterDecision -> PlayerDecisionController => DecisionMade -> Go Next -> getPlayerCharacterDecision... -> enemyDecision -> return all Decision
*/
public class TurnController : MonoBehaviour
{
    private BattleController battleController;
    [HideInInspector] public List<CharacterFightController> playerParty;
    private CharacterDecision[] charactersDecision;
    private int characterDecisionIndex = 0;

    [HideInInspector] public List<CharacterFightController> enemyParty;
    private CharacterDecision[] enemiesDecision;

    public GameObject playerDecisionController;

 //#TODO add speed which one gets to go first
   

    public void Setup(BattleController _battleController, List<CharacterFightController> _playerParty, List<CharacterFightController>  _enemyParty)
    {
        battleController = _battleController;
        playerParty = _playerParty;
        enemyParty = _enemyParty;
        charactersDecision = new CharacterDecision[playerParty.Count];
        enemiesDecision = new CharacterDecision[enemyParty.Count];
    }
    public void turn()
    {
        for(int i = playerParty.Count - 1; i >= 0; i--)
        {
            getPlayerCharacterDecision(playerParty[i]);
        }
    }

    public void getPlayerCharacterDecision(CharacterFightController character)
    {
        playerDecisionController.SetActive(true);
        PlayerDecisionController characterDecisionController = playerDecisionController.GetComponent<PlayerDecisionController>();
        characterDecisionController.Setup(character, this, enemyParty);
    }

    public void decisionMade(CharacterDecision turn)
    {
        charactersDecision[characterDecisionIndex] = turn;
        goToNext();
    }

    private void goToNext()
    {
        characterDecisionIndex++;
        if(characterDecisionIndex >= charactersDecision.Length)
        {
            enemyDecision();
        }
        else
        {
            CharacterFightController character = playerParty[characterDecisionIndex];
        }
    }

    private void enemyDecision()
    {
        for(int i = 0; i < enemyParty.Count; i++)
        {
            enemiesDecision[i] = new CharacterDecision(enemyParty[i], Decision.Fight, null);
            enemiesDecision[i].SetTarget(playerParty[0]);
        }
        sendDecisions();
    }

    private void sendDecisions()
    {
        battleController.getAllDecision(charactersDecision, enemiesDecision);
        Destroy(this.gameObject);
    }
}
