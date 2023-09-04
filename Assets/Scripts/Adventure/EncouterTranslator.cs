using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class EncouterTranslator : MonoBehaviour
{

    public static EncouterTranslator Instance { get; private set;}
    public UnityEvent postFightEvent;

    private CharacterDoll[] playerParty;
    private CharacterDoll[] enemyParty;

    private Vector3[] playerPositions;
    private void Awake() 
{ 
    // If there is an instance, and it's not me, delete myself.
    
    if (Instance != null && Instance != this) 
    { 
        Destroy(this); 
    } 
    else 
    { 
        Instance = this; 
    } 
}
    // Start is called before the first frame update

    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void StartEncounter(CharacterDoll[] playerParty, CharacterDoll[] enemyParty)
    {
        //Debug.Log(enemyParty[0]);
        this.playerParty = playerParty;
        this.enemyParty = enemyParty;
        AdventureManager.Instance.DesactivateAdventure();
        SetupAfterFightLoaded();
        //((SceneController)SceneController.Instance).LoadFightScene(SetupAfterFightLoaded);
        
    }


    public void SetupAfterFightLoaded()
    {
        GameObject masterManagerObject = GameObject.Find("Master Manager");
        MasterBattleController master = masterManagerObject.GetComponent<MasterBattleController>();
        master.setupParties(playerParty, enemyParty);
        master.startBattle();
    }

    public void SetupGoBackAdventure()
    {
        AdventureManager.Instance.ActivateAdventure();
        Destroy(this.gameObject);
        postFightEvent.Invoke();
        postFightEvent.RemoveAllListeners();
    }


}
