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
    public GameObject[] soundPrefab;
    public CharacterMovement characterMovement;
    private GameObject soundInstance;
    private List<string> songs = new List<string>();
    private int[] indexes;
    private string currentCode;
    public AK.Wwise.Event RedSound;
    public AK.Wwise.Event BlueSound;
    public AK.Wwise.Event PinkSound;
    public AK.Wwise.Event YellowSound;
    private void Start()
    {
        songs.Add(WorldConstants.removeEncounterManagercheatCode);
        songs.Add(WorldConstants.specialAreaCode);
    }

    public void Sing(PlayerInput playerInput)
    {
        characterMovement.StopMovement();
        eSingNotes currentNote = GetNote(playerInput);
        ParticleSystem singParticles = null;
        switch (currentNote)
        {
            case eSingNotes.Do:
                soundInstance = Instantiate(soundPrefab[0], mouthPosition.position, mouthPosition.rotation);
                singParticles = soundInstance.GetComponent<ParticleSystem>();
                singParticles.Play();
                RedSound.Post(gameObject);
                currentCode += "d";
                CheckForCode();
                break;
            case eSingNotes.Re:
                soundInstance = Instantiate(soundPrefab[1], mouthPosition.position, mouthPosition.rotation);
                singParticles = soundInstance.GetComponent<ParticleSystem>();
                singParticles.Play();
                currentCode += "r";
                PinkSound.Post(gameObject);
                CheckForCode();
                break;
            case eSingNotes.Mi:
                soundInstance = Instantiate(soundPrefab[2], mouthPosition.position, mouthPosition.rotation);
                singParticles = soundInstance.GetComponent<ParticleSystem>();
                currentCode += "m";
                BlueSound.Post(gameObject);
                singParticles.Play();
                CheckForCode();
                break;
            case eSingNotes.Fa:
                soundInstance = Instantiate(soundPrefab[3], mouthPosition.position, mouthPosition.rotation);
                singParticles = soundInstance.GetComponent<ParticleSystem>();
                singParticles.Play();
                currentCode += "f";
                YellowSound.Post(gameObject);
                CheckForCode();
                break;
        }
    }

    private eSingNotes GetNote(PlayerInput playerInput)
    {
        Vector2 moveVector = characterMovement.GetCharacterMovement();
        if(playerInput.Player.UpSing.WasPressedThisFrame())
        {
            return eSingNotes.Do;
        }
        else if (playerInput.Player.RightSing.WasPressedThisFrame())
        {
            return eSingNotes.Re;
        }
        else if (playerInput.Player.LeftSing.WasPressedThisFrame())
        {
            return eSingNotes.Mi;
        }
        else if (playerInput.Player.DownSing.WasPressedThisFrame())
        {
            return eSingNotes.Fa;
        }
        return eSingNotes.None;
    }

    private void CheckForCode()
    {
        bool match = false;
        foreach(string song in songs)
        {
            if(song.StartsWith(currentCode))
            {
                match = true;
                if (Equals(song, currentCode))
                {
                    SingManager.Instance.ActivateSong(song);
                    Clear();
                }
                    
            }
        }
        if(!match)
        {
            Clear();
        }
    }

    public void Clear()
    {
        currentCode = string.Empty;
    }

    
}
