using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudioManager : MonoBehaviour
{
    [SerializeField]
    private AK.Wwise.Event footstepsEvent;

    private void PlayFootsteps()
    {
        AkSoundEngine.PostEvent(footstepsEvent.Id, this.gameObject);    
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
