using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyGenerator
{
    public static CharacterDoll GenerateNewEnemyDoll(CharacterDoll doll)
    {
        return doll.Copy();
    }
}
