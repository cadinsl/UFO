using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingManager : MonoBehaviour
{
    public static SingManager Instance { get; private set; }

    public List<SongSpecialArea> specialAreas = new List<SongSpecialArea>();

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

    public void ActivateSong(string code)
    {
        switch(code)
        {
            case WorldConstants.removeEncounterManagercheatCode:
                FindObjectOfType<EncounterManager>().gameObject.SetActive(false);
                break;
            case WorldConstants.specialAreaCode:
                ActivateSpecialArea();
                break;
        } 
    }

    private void ActivateSpecialArea()
    {
        foreach(SongSpecialArea area in specialAreas)
        {
            area.GiveItemToPlayer();
        }
    }
}
