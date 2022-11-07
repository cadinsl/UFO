using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public enum Type{
        Speed,
        Defense,
        Attack
    }

    public Type type;
    public int boost;

    public int turns;

    public Buff(Type _type, int _boost, int _turns)
    {
        type = _type;
        boost = _boost;
        turns = _turns;
    }
}
