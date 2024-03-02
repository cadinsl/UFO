using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneEncounterZone : MonoBehaviour
{
    [SerializeField]
    private EncounterManager encounterManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            encounterManager.canEncounter = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            encounterManager.canEncounter = true;
    }
}
