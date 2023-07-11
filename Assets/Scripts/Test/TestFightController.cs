using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFightController : MonoBehaviour
{
    public CharacterDoll[] playerPartyDolls;

    public CharacterDoll[] enemyPartyDolls;

    private MasterBattleController master;

    public void Start()
    {
        master = GameObject.Find("Master Manager").GetComponent<MasterBattleController>();
        master.setupParties(playerPartyDolls, enemyPartyDolls);
        master.startBattle();
    }
    
}
