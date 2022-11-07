using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAdventureInput : MonoBehaviour
{
    public CharacterBrainAdventure characterBrain;
    public UnityEvent pausedGame;

    private bool enableInput = true;

    public bool canTalkToNPC = false;

    private NPCAdventure npcInArea;
    void Start()
    {
        pausedGame.AddListener(AdventureManager.Instance.PauseGame);
    }
    // Update is called once per frame
    void Update()
    {
        if(enableInput)
        {
            if(Input.GetButton("Pause"))
            {
                pausedGame.Invoke();
            }
            else if(canTalkToNPC && Input.GetButton("Action"))
            {
                characterBrain.TalkToNPC();
            }
        }
    }

    public void DisableInput()
    {
        enableInput = false;
    }

    public void EnableInput()
    {
        enableInput = true;
    }
}
