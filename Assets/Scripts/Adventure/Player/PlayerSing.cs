using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSing : MonoBehaviour{
    public Transform mouthPosition;
    public GameObject soundPrefab;
    public AudioClip[] notes;

    public AudioSource source;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Sing"))
        {
            GameObject soundInstance = Instantiate(soundPrefab, mouthPosition.position, mouthPosition.rotation);
            ParticleSystem singParticles = soundInstance.GetComponent<ParticleSystem>();
            singParticles.Emit(1);
            singParticles.Play();
            addRandomNoteToAudioSource();
            source.Play();
        }
    }

    private void addRandomNoteToAudioSource(){
        int randomIndex =  Random.Range(0, notes.Length);
        source.clip = notes[randomIndex];
    }
}
