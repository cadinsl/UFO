using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAdventureCallEvent : MonoBehaviour
{
    public void CallEvent(string s)
    {
        AkSoundEngine.PostEvent(s, gameObject);
    }
}
