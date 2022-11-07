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


    public void Setup(List<CharacterFightController> _enemyParty, PlayerDecisionController _playerDecisionController, CharacterDecision _characterDecision)
    {
        enemyParty = _enemyParty;
        playerDecisionController = _playerDecisionController;
        characterDecision = _characterDecision;
    }

    public void Display()
    {
        GameObject canvas = GameObject.Find("Canvas");
        this.transform.SetParent(canvas.transform);
        this.transform.localPosition = Vector3.zero;
        targetPanel.SetActive(true);

        displayButtons();
    }

    private void displayButtons()
    {
        Transform panel = this.transform.GetChild(0);
        for(int i = 0; i < enemyParty.Count; i++)
                {
                    CharacterFightController enemy = enemyParty[i];
                    Transform button = panel.GetChild(i);
                    TextMeshProUGUI buttonText = button.GetChild(0).GetComponent<TextMeshProUGUI>();
                    //Debug.Log(button.GetChild(0).gameObject);
                    buttonText.SetText(enemy.name);

                    Button buttonComponent = button.GetComponent<Button>();
                    int x = i;
                    buttonComponent.onClick.AddListener(delegate{ChosenEnemy(x);});

                    button.gameObject.SetActive(true);
                }
        }

    public void ChosenEnemy(int index)
    {
        CharacterFightController target = enemyParty[index];
        characterDecision.target = target;
        playerDecisionController.InputDecision(characterDecision);
        if(optionalBagPanel != null)
        {
            Destroy(optionalBagPanel);
        }
        Destroy(this.gameObject);
    }

    public void GoBack()
    {
        Destroy(this.gameObject);
    }
}
