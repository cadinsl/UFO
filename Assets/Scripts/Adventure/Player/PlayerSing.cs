using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eSingNotes
{
    None,
    Do,
    Re,
    Mi,
    Fa
}

public class PlayerSing : MonoBehaviour{
    public Transform mouthPosition;
    public GameObject soundPrefab;
    public CharacterMovement characterMovement;

    public void Sing()
    {
        characterMovement.StopMovement();
        eSingNotes currentNote = GetNote();
        GameObject soundInstance = Instantiate(soundPrefab, mouthPosition.position, mouthPosition.rotation);
        ParticleSystem singParticles = soundInstance.GetComponent<ParticleSystem>();
        switch (currentNote)
        {
            case eSingNotes.Do:
                singParticles.Emit(1);
                singParticles.Play();
                break;
        }
    }

    private eSingNotes GetNote()
    {
        Vector2 moveVector = characterMovement.GetCharacterMovement();
        if(Equals(moveVector, Vector2.up) )
        {
            return eSingNotes.Do;
        }
        else if (Equals(moveVector, Vector2.down))
        {
            return eSingNotes.Re;
        }
        else if (Equals(moveVector, Vector2.left))
        {
            return eSingNotes.Mi;
        }
        else if (Equals(moveVector, Vector2.right))
        {
            return eSingNotes.Fa;
        }
        return eSingNotes.None;
    }

    
}
