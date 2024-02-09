using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogBark : MonoBehaviour
{
    public Transform mouthPosition;
    public GameObject soundPrefab;
    public AK.Wwise.Event BarkSound;

    public void Bark()
   {
        GameObject soundInstance = Instantiate(soundPrefab, mouthPosition.position, mouthPosition.rotation);
        ParticleSystem barkParticles = soundInstance.GetComponent<ParticleSystem>();
        barkParticles.Emit(1);
        barkParticles.Play();
        BarkSound.Post(gameObject);
    }
}
