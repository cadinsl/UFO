using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDecisionController : MonoBehaviour
{
    #region Field Declarations
    [HideInInspector] public CharacterFightController character;

    [HideInInspector] public List<CharacterFightController> enemyParty;
    public GameObject decisionPanel;
    public GameObject namePanel;
    private TurnController turnManager;

    private bool decisionMade = false;
    private CharacterDecision finalDecision;

    public GameObject bagManagerPrefab;

    public GameObject targetManagerPrefab;

    public GameObject magicManagerPrefab;

    private bool active = true;
    #endregion


    public void Setup(CharacterFightController _character, TurnController _turnManager, List<CharacterFightController>  _enemyParty)
    {
        character = _character;
        turnManager = _turnManager;
        enemyParty = _enemyParty;
        GameObject canvas = GameObject.Find("Canvas");
        decisionPanel.transform.SetParent(canvas.transform);
        RectTransform rectTransform = (RectTransform)decisionPanel.transform;
        rectTransform.anchoredPosition = Vector3.zero;
        decisionPanel.SetActive(true);
        decisionMade = false;
        UpdateNamePanel();
    }
    //We get decision turn it to turn manager and destroy this object.
    public void InputDecision(CharacterDecision decisionReceived)
    {
        switch(decisionReceived.decision)
        {
            case Decision.Magic:
            case Decision.Bag:
                if(decisionReceived.decisionObject == null)
                {
                    Debug.Log("NULL ITEM");
                }
                else
                {
                    turnManager.decisionMade(decisionReceived);
                }
            break;
            default:
            turnManager.decisionMade(decisionReceived);
            break;
        }
        Hide();
    }
    //Function called by the fight button
    public void Fight()
    {
        if(active)
        {
            CharacterDecision turn = new CharacterDecision(character, Decision.Fight, null);
            //InputDecision(turn);

            targetManagerPrefab.SetActive(true);
            TargetController targetController = targetManagerPrefab.GetComponent<TargetController>();
            targetController.Setup(enemyParty, this, turn);
            targetController.Display();
        }
    }
    
    public void Bag()
    {
        if(active)
        {
            GameObject bagManagerInstance = Instantiate(bagManagerPrefab);
            BagPanelController bagPanelController = bagManagerInstance.GetComponent<BagPanelController>();
            bagPanelController.Setup(character, this, enemyParty);
            bagPanelController.Display();
            active = false;
        }
    }

    public void Magic()
    {
        if(active)
        {
            GameObject magicManagerInstance = Instantiate(magicManagerPrefab);
            MagicPanelController magicPanelController = magicManagerInstance.GetComponent<MagicPanelController>();
            magicPanelController.Setup(character, this, enemyParty);
            magicPanelController.Display();
            active = false;
        }
    }

    public void Guard()
    {
        CharacterDecision turn = new CharacterDecision(character, Decision.Guard, null);
        InputDecision(turn);
    }

    public void Run()
    {
        CharacterDecision turn = new CharacterDecision(character, Decision.Run, null);
        InputDecision(turn);
    }

    public void SetActive()
    {
        active = true;
    }

    public void SetInactive()
    {
        active = false;
    }

    public void Hide(){
        HidePanel();
        this.gameObject.SetActive(false);
    }
    public void HidePanel(){
        decisionPanel.SetActive(false);
    }

    private void UpdateNamePanel()
    {
        namePanel.GetComponent<TextMeshProUGUI>().SetText(character.doll.name);
    }

    /*private IEnumerator waitForDecision()
    {
        while(!decisionMade)
        {
            yield return null;
        }
    }*/
}
