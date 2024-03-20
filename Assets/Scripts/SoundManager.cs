using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void OnClick()
    {
        AkSoundEngine.PostEvent("UIClick", gameObject);
    }

    public void OnMove()
    {
        AkSoundEngine.PostEvent("UIOnMove", gameObject);
    }
}
