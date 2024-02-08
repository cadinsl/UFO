using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerDecisionController : MonoBehaviour
{
    #region Field Declarations
    [HideInInspector] public CharacterFightController character;

    [HideInInspector] public List<CharacterFightController> enemyParty;

    [HideInInspector] public List<CharacterFightController> playerParty;

    public GameObject decisionPanel;
    public GameObject namePanel;
    private TurnController turnManager;

    private bool decisionMade = false;
    private CharacterDecision finalDecision;

    public GameObject bagManagerPanel;

    public GameObject targetManagerPrefab;

    public GameObject magicManagerPanel;

    private bool active = true;
    #endregion


    public void Setup(CharacterFightController _character, TurnController _turnManager, List<CharacterFightController>  _enemyParty, List<CharacterFightController>  _playerParty)
    {
        character = _character;
        turnManager = _turnManager;
        enemyParty = _enemyParty;
        playerParty = _playerParty;
        GameObject canvas = GameObject.Find("Canvas");
        decisionPanel.transform.SetParent(canvas.transform);
        RectTransform rectTransform = (RectTransform)decisionPanel.transform;
        rectTransform.anchoredPosition = Vector3.zero;
        decisionPanel.SetActive(true);
        decisionMade = false;
        UpdateNamePanel();
        SetInactive();
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
        CharacterDecision turn = new CharacterDecision(character, Decision.Fight, null);

        targetManagerPrefab.SetActive(true);
        TargetController targetController = targetManagerPrefab.GetComponent<TargetController>();
        targetController.Setup(enemyParty, this, turn);
        targetController.SetDecisionPanel(decisionPanel);
        targetController.Display();
        targetController.SetActive();
        SetInactive();
    }
    
    public void Bag()
    {
        bagManagerPanel.SetActive(true);
        BagPanelController bagPanelController = bagManagerPanel.GetComponent<BagPanelController>();
        bagPanelController.Setup(character, this, enemyParty);
        bagPanelController.Display();
        bagPanelController.SetActive();
        SetInactive();
    }

    public void Magic()
    {
        magicManagerPanel.SetActive(true);
        MagicPanelController magicPanelController = magicManagerPanel.GetComponent<MagicPanelController>();
        magicPanelController.Setup(character, this, enemyParty, playerParty);
        magicPanelController.Display();
        magicPanelController.SetActive();
        SetInactive();
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
        //decisionPanel.interactable = true;
        active = true;
        decisionPanel.GetComponent<Panel>().SetActive();
    }

    public void SetInactive()
    {
        //decisionPanel.interactable = false;
        active = false;
        decisionPanel.GetComponent<Panel>().SetInActive();
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

}
