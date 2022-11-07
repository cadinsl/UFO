using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Chancer 
{
    public static bool getChance(int probability)
    {
        int roll = Random.Range(1, 101);
        
        if(roll <= probability)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
}
