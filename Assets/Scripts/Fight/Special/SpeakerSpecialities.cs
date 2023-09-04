using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerSpecialities : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EncouterTranslator encounterTranslator = GameObject.FindGameObjectWithTag("Encounter Translator").GetComponent<EncouterTranslator>();
        encounterTranslator.postFightEvent.AddListener(ForestManager.Instance.DestroySpeaker);
    }

}
