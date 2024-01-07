using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PausedTargetController : MonoBehaviour
{
    [HideInInspector] public CharacterDoll[] targets;

    public GameObject targetPanel;

    public Button[] itemButtons;
    public Panel panel;
    private UnityAction<CharacterDoll> getBackAction;

    public void DisplayTargets(CharacterDoll[] targets, UnityAction<CharacterDoll> getBackAction)
    {
        this.targets = targets;
        this.getBackAction= getBackAction;
        targetPanel.SetActive(true);
        for(int i = 0; i < targets.Length; i++)
        {
            itemButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(targets[i].name);
            int x = i;
            itemButtons[i].onClick.RemoveAllListeners();
            itemButtons[i].onClick.AddListener(delegate {ChosenItem(x);});
            itemButtons[i].gameObject.SetActive(true);
        }
        panel.SetActive();
    }

    public void ChosenItem(int index)
    {
        CharacterDoll target = targets[index];
        getBackAction(target);
        Close();
    }

    public void Close()
    {
        foreach(Button button in itemButtons)
        {
            button.gameObject.SetActive(false);
        }
        targetPanel.SetActive(false);
    }
}
