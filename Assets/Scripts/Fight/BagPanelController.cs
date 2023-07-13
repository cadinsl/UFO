using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BagPanelController : MonoBehaviour
{
    [HideInInspector] public CharacterFightController character;
    [HideInInspector] public PlayerDecisionController decisionController;

    [HideInInspector] public List<CharacterFightController> enemyParty;

    public GameObject bagPanel;

    public GameObject buttonPrefab;

    public GameObject targetManagerPanel;


    public void Setup(CharacterFightController _character, PlayerDecisionController _decisionController, List<CharacterFightController> _enemyParty)
    {
        character = _character;
        decisionController = _decisionController;
        enemyParty = _enemyParty;
    }

    public void Display()
    {
        resetButtons();
        displayItems();
    }

    private void displayItems()
    {
        int yPosition = 120;
        if(character.doll.inventory == null)
        {
            return;
        }
        Transform panel = this.transform.GetChild(0);
        for(int i = 0; i < character.doll.inventory.items.Count; i++)
        {
            Item item = character.doll.inventory.items[i];
            Transform button = panel.GetChild(i);
            TextMeshProUGUI buttonText = button.GetChild(0).GetComponent<TextMeshProUGUI>();
            //Debug.Log(button.GetChild(0).gameObject);
            buttonText.SetText(character.doll.inventory.items[i].name);

            Button buttonComponent = button.GetComponent<Button>();
            int x = i;
            buttonComponent.onClick.AddListener(delegate{ChosenItem(x);});

            button.gameObject.SetActive(true);
        }
    }

    #region ButtonFunctions
    public void GoBack()
    {
        decisionController.SetActive();
        this.gameObject.SetActive(false);
    }

    public void ChosenItem(int i)
    {
        
        CharacterDecision decision = new CharacterDecision(character, Decision.Bag, character.doll.inventory.items[i]);
        //decisionController.InputDecision(decision);
        //Destroy(this.gameObject);
        targetManagerPanel.SetActive(true);
        TargetController targetController = targetManagerPanel.GetComponent<TargetController>();
        targetController.Setup(enemyParty, decisionController, decision);
        targetController.optionalBagPanel = this.gameObject;
        targetController.Display();
        this.gameObject.SetActive(false);
    }

    private void resetButtons(){
        Transform panel = this.transform.GetChild(0);
        for(int i = 0; i < 6; i++){
            panel.GetChild(i).gameObject.SetActive(false);
        }
    }

    /*public void SetTarget(CharacterController target)
    {
        CharacterDecision decision = new CharacterDecision(Decision.Bag, savedItem, target);
        decisionController.InputDecision(decision);
        Destroy(this.gameObject);
    }*/

    #endregion
}
