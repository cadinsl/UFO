using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDecisionResult 
{
    public CharacterDecision characterDecision;

    public bool successful;

    public int points;

    public bool critical;

    public CharacterDecisionResult(CharacterDecision _characterDecision, bool _successful, int _points)
    {
        characterDecision = _characterDecision;
        successful = _successful;
        points = _points;
    }
}
