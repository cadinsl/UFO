using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class DialogController : MonoBehaviour
{
    
    public GameObject dialogPanelPrefab;

    private GameObject dialogPanelInstance;

    List<IDialogObserver> dialogObservers = new List<IDialogObserver>();

    public List<string> texts;

    private int currentIndex;

    private TextMeshProUGUI dialogText;

    private UnityAction action;

    public void Display(List<string> texts, UnityAction action)
    {
        if(texts.Count == 0)
        {
            action();
            return;
        }
        GameObject canvas = GameObject.Find("Canvas");
        dialogPanelInstance = Instantiate(dialogPanelPrefab, canvas.transform);
        //dialogPanelInstance.transform.localPosition = Vector3.zero;
        dialogText = dialogPanelInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        this.texts = texts;
        dialogText.SetText(texts[0]);
        currentIndex = 0;
        this.action = action;
        addNextEventToButton(action);
        dialogPanelInstance.transform.SetAsLastSibling();
    }

    public void Next()
    {
        currentIndex++;
        if(currentIndex >= texts.Count)
        {
            action();
            Destroy(dialogPanelInstance);
        }
        else
        {
            Debug.Log("NEXT");
            dialogText.SetText(texts[currentIndex]);
        }
    }

    public void NextDecision()
    {
        currentIndex++;
        if(currentIndex >= texts.Count)
        {
            Destroy(dialogPanelInstance);
            notifyEndOfDialogDecision();
        }
    }

    public void NextEndBattle()
    {
        currentIndex++;
        if(currentIndex >= texts.Count)
        {
            Destroy(dialogPanelInstance);
            notifyEndOfDialogDecision();
        }
    }

    public void AddObserver(IDialogObserver observer)
    {
        if(!dialogObservers.Contains(observer))
        {
            dialogObservers.Add(observer);
            notifyEndOfDialog();
        }
    }

    public List<string> GetTextForDecisionAboutToAct(CharacterDecision characterDecision)
    {
        List<string> texts = new List<string>();
        switch(characterDecision.decision)
        {
            case Decision.Fight:
                texts.Add(characterDecision.from.name + " attacks " + characterDecision.target.name);
            break;
            case Decision.Bag:
                texts.Add(characterDecision.from.name+ " is going to use " + characterDecision.decisionObject.name + " on " + characterDecision.target.name);
            break;
            case Decision.Magic:
                if(characterDecision.decisionObject is DamageSpell)
                {
                    texts.Add(characterDecision.from.name+ " casted " + characterDecision.decisionObject.name + " on " + characterDecision.target.name);
                }
            break;
            case Decision.Guard:
                texts.Add(characterDecision.from.name + " is guarding");
            break;
            case Decision.Run:
                texts.Add(characterDecision.from.name + " tried to run!");
            break;
        }
        return texts;
    }

    public static List<string> GetText(string text)
    {
        List<string> texts = new List<string>();
        texts.Add(text);
        return texts;
    }

    public List<string> GetTextForDecisionResult(CharacterDecisionResult result)
    {
        List<string> dialogTexts = new List<string>();
        CharacterDecision characterDecision = result.characterDecision;
        switch(characterDecision.decision){
            case Decision.Fight:
                if(result.successful)
                {
                    if(result.critical)
                    {
                    dialogTexts.Add("IS A CRITICAL ATTACK");
                    }
                    
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
            break;
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
        return dialogTexts;
    }

    public List<string> GetTextItemPickup(string itemName){
        List<string> texts = new List<string>();
        texts.Add("You have picked up "+ itemName + ".");
        return texts;
    }

    public void DeleteAllDialogs(){
        GameObject[] dialogs = GameObject.FindGameObjectsWithTag("DialogFight");
        foreach(GameObject dialog in dialogs){
            Destroy(dialog);
        }
    }

    private void addNextEventToButton(UnityAction action)
    {
        Button button = dialogPanelInstance.transform.GetChild(1).GetComponent<Button>();
        button.onClick.AddListener(delegate{nextEvent(action);});
    }

    private void nextEvent(UnityAction action)
    {
        currentIndex++;
        if(currentIndex >= texts.Count)
        {
            Destroy(dialogPanelInstance);
            action();
        }
        else
        {
            dialogText.SetText(texts[currentIndex]);
        }
    }


    private void notifyEndOfDialog()
    {
        foreach(IDialogObserver dialogObserver in dialogObservers)
        {
            dialogObserver.NotifyEnd();
        }
    }

    private void notifyEndOfDialogDecision()
    {
        foreach(IDialogObserver dialogObserver in dialogObservers)
        {
            dialogObserver.NotifyEndDecision();
        }
    }

    private void notifyEndOfBattleDialog()
    {
        foreach(IDialogObserver dialogObserver in dialogObservers)
        {
            dialogObserver.NotifyEndOfBattle();
        }
    }

}
