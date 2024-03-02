using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public bool canEncounter = false;
    public Transform player;

    public Grid grid;

    public GameObject encounterTranslator;

    public GameObject fightCamera;

    private Vector3Int lastCellPosition;

    private RegionMonsterProvider regionMonsterProvider;

    [SerializeField]
    private FF7Transition ff7Transition;


    // Start is called before the first frame update
    void Start()
    {
        regionMonsterProvider = GetComponent<RegionMonsterProvider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canEncounter)
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
                StartCoroutine(TransitionToFight());
            }
        }
    }

    IEnumerator TransitionToFight()
    {
        player.GetComponent<CharacterBrainAdventure>().DesactivatePlayer();
        ff7Transition.enabled = true;

        yield return new WaitForSeconds(1f);
        ff7Transition.enabled = false;
        startEncounter();
    }

    private void startEncounter()
    {
        Instantiate(encounterTranslator);
        EncouterTranslator translator = encounterTranslator.GetComponent<EncouterTranslator>();
        List<CharacterDoll> dolls = new List<CharacterDoll>();
        //#TODO IMPLEMENT IT WITHOUT BEING DEPENDANT ON ANGEL
        player.GetComponent<CharacterBrainAdventure>().DesactivatePlayer();
        CharacterAdventureController[] playerParty = AdventureManager.Instance.playerParty;
        foreach(CharacterAdventureController adventureController in playerParty)
        {
            dolls.Add(adventureController.doll);
        }
        translator.StartEncounter(dolls.ToArray(), regionMonsterProvider.GetMonsterForEncounter());
    }

    public void startPlannedEncounter(CharacterDoll[] enemies)
    {
        player.GetComponent<CharacterBrainAdventure>().disactivateNPCAreaTalk();
        Instantiate(encounterTranslator);
        EncouterTranslator translator = encounterTranslator.GetComponent<EncouterTranslator>();
        List<CharacterDoll> dolls = new List<CharacterDoll>();
        CharacterAdventureController[] playerParty = AdventureManager.Instance.playerParty;
        foreach (CharacterAdventureController adventureController in playerParty)
        {
            dolls.Add(adventureController.doll);
        }
        translator.StartEncounter(dolls.ToArray(), enemies);
    }
}
