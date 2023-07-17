using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCDialog : MonoBehaviour
{
    public string[] dialogTexts;

    public void DisplayDialog(UnityAction action)
    {
        DialogController dialogController = GameObject.Find("Dialog Manager").GetComponent<DialogController>();
        dialogController.Display(new List<string>(dialogTexts), action);
    }
}
