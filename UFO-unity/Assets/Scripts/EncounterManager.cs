using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public Transform player;

    public Grid grid;

    public GameObject encounterTranslator;

    public GameObject fightCamera;

    private Vector3Int lastCellPosition;

    private RegionMonsterProvider regionMonsterProvider;


    // Start is called before the first frame update
    void Start()
    {
        regionMonsterProvider = GetComponent<RegionMonsterProvider>();
    }

    // Update is called once per frame
    void Update()
    {
        checkForEncounters();
    }

    private void checkForEncounters()
    {
        Vector3Int currentCellPosition = grid.LocalToCell(player.position);
        if(currentCellPosition != lastCellPosition)
        {
            lastCellPosition = currentCellPosition;
            if(Random.Range(1, 101)  <= WorldConstants.EncounterChance)
            {
                startEncounter();
            }
        }
    }

    private void startEncounter()
    {
        Instantiate(encounterTranslator);
        EncouterTranslator translator = encounterTranslator.GetComponent<EncouterTranslator>();
        List<CharacterDoll> dolls = new List<CharacterDoll>();
        CharacterAdventureController[] playerParty = AdventureManager.Instance.playerParty;
        foreach(CharacterAdventureController adventureController in playerParty)
        {
            dolls.Add(adventureController.doll);
        }
        translator.StartEncounter(dolls.ToArray(), regionMonsterProvider.GetMonsterForEncounter());
    }

}
