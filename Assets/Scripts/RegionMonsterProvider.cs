using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionMonsterProvider : MonoBehaviour
{
    public int currentRegionIndex;
    public RegionMonsters[] monstersForRegion;


    public CharacterDoll[] GetMonsterForEncounter()
    {
        List<CharacterDoll> dolls = new List<CharacterDoll>();
        RegionMonsters monsters = monstersForRegion[currentRegionIndex];
        int monsterCount = 1;
        dolls.Add(EnemyGenerator.GenerateNewEnemyDoll(pickAnEnemy(monsters)));
        while(Chancer.getChance(WorldConstants.ChanceForAnotherEnemyToJoinEncounter) && monsterCount < WorldConstants.maxEnemiesInFight)
        {
            dolls.Add(EnemyGenerator.GenerateNewEnemyDoll(pickAnEnemy(monsters)));
            monsterCount++;
        }
        return dolls.ToArray();
    }

    private CharacterDoll pickAnEnemy(RegionMonsters regionMonsters)
    {
        int randomIndex = Random.Range(0, regionMonsters.monsters.Length);
        return regionMonsters.monsters[randomIndex];
    }
}
