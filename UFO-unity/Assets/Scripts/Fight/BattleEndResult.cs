using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEndResult : MonoBehaviour
{
    public enum Result{
        WON,
        DEFEATED,
        RAN
    }

    public Result result;

    public BattleEndResult(Result result)
    {
        this.result = result;
    }
}
