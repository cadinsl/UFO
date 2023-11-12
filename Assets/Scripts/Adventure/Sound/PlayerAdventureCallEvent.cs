using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAdventureCallEvent : MonoBehaviour
{
    public void Callevent(string s)
    {
        AkSoundEngine.PostEvent(s, gameObject);
        Debug.Log("Print Event: " + s + "Calledat: " + Time.time);
    }
}
