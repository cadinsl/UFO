using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsSlips : MonoBehaviour
{
    [SerializeField]
    private AK.Wwise.Event footstepsslipsEvent;

    private void PlayFootstepsSlips()
    {
        AkSoundEngine.PostEvent(footstepsslipsEvent.Id, this.gameObject);
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