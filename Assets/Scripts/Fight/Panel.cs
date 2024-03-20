using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    public GameObject targetPanel;
    private Button[] buttons;

    void Start()
    {   
    }

    private void OnEnable()
    {
        if (targetPanel == null)
        {
            targetPanel = this.gameObject;
        }
    }
    public void SetActive()
    {
        buttons = getAllChildButtons();
        if (buttons != null)
        {
            foreach (Button button in buttons)
            {
                button.interactable = true;
            }
            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
        }
    }

    public void SetInActive()
    {
        buttons = getAllChildButtons();
        if (buttons != null)
        {
            foreach (Button button in buttons)
            {
                button.interactable = false;
            }
        }
    }

    private Button[] getAllChildButtons()
    {
        if (targetPanel == null)
            targetPanel = this.gameObject;
        Button[] _button = targetPanel.GetComponentsInChildren<Button>();
        AddClickSoundToAllButtons(_button);
        return _button;
    }

    private void AddClickSoundToAllButtons(Button[] buttons)
    {
        foreach(Button button in buttons)
        {
            button.onClick.AddListener(() => SoundManager.Instance.OnClick());
            if (Equals(button.gameObject.GetComponent<EventTrigger>(), null))
            {
                EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.Move;
                entry.callback.AddListener((data) => { SoundManager.Instance.OnMove(); });
                trigger.triggers.Add(entry);
            }
        }
    }
}
