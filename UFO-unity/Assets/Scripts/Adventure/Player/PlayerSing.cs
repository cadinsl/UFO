using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSing : MonoBehaviour{
    public Transform mouthPosition;
    public GameObject soundPrefab;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Sing"))
        {
            GameObject soundInstance = Instantiate(soundPrefab, mouthPosition.position, mouthPosition.rotation);
            ParticleSystem singParticles = soundInstance.GetComponent<ParticleSystem>();
            singParticles.Emit(1);
            singParticles.Play();
        }
    }
}
