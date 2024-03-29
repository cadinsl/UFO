using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetController : MonoBehaviour
{
    [HideInInspector] public List<CharacterFightController>  enemyParty;
    
    [HideInInspector] public PlayerDecisionController playerDecisionController;

    [HideInInspector] public CharacterDecision characterDecision;

    [HideInInspector] public GameObject optionalBagPanel;

    public GameObject targetPanel;

    private GameObject decisionPanel;

    private Panel panel;


    public void Setup(List<CharacterFightController> _enemyParty, PlayerDecisionController _playerDecisionController, CharacterDecision _characterDecision)
    {
        enemyParty = _enemyParty;
        playerDecisionController = _playerDecisionController;
        characterDecision = _characterDecision;
        panel = GetComponent<Panel>();
    }

    public void Display()
    {
        resetButtons();
        displayButtons();
    }

    private void displayButtons()
    {
        Transform panel = this.transform;
        for(int i = 0; i < enemyParty.Count; i++)
                {
                    CharacterFightController enemy = enemyParty[i];
                    Transform button = panel.GetChild(i);
                    TextMeshProUGUI buttonText = button.GetChild(0).GetComponent<TextMeshProUGUI>();
                    //Debug.Log(button.GetChild(0).gameObject);
                    buttonText.SetText(enemy.name);

                    Button buttonComponent = button.GetComponent<Button>();
                    int x = i;
                    buttonComponent.onClick.RemoveAllListeners();
                    buttonComponent.onClick.AddListener(delegate{ChosenEnemy(x);});

                    button.gameObject.SetActive(true);
                }
    }

    private void resetButtons(){
        for(int i = 0; i < 3; i++){
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void ChosenEnemy(int index)
    {
        CharacterFightController target = enemyParty[index];
        characterDecision.target = target;
        playerDecisionController.InputDecision(characterDecision);
        this.gameObject.SetActive(false);
    }

    public void GoBack()
    {
        this.gameObject.SetActive(false);
        decisionPanel.GetComponent<Panel>().SetActive();
    }

    public void SetDecisionPanel(GameObject _decisionPanel)
    {
        decisionPanel = _decisionPanel;
    }

    public void SetActive()
    {
        panel.SetActive();
    }

    public void SetInActive()
    {
        panel.SetInActive();
    }

}
