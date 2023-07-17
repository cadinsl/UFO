using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Decision
{
    Fight,
    Magic,
    Bag,
    Guard,
    Run
}
public class CharacterDecision 
{
    private Decision _decision;

    public Decision decision
    {
        get => _decision;
        set
        {
            _decision = value;
        }
    }

    private DecisionObject _decisionObject;

    public DecisionObject decisionObject
    {
        get => _decisionObject;
        set
        {
            _decisionObject = value;
        }
    }

    private CharacterFightController _target;

    public CharacterFightController target
    {
        get => _target;
        set
        {
            _target = value;
        }
    }

    private CharacterFightController _from;

    public CharacterFightController from
    {
        get => _from;
        set
        {
            _from = value;
        }
    }

    public CharacterDecision (CharacterFightController _from, Decision _decision, DecisionObject _decisionObject)
    {
        from = _from;
        decision = _decision;
        decisionObject = _decisionObject;
    }

    public void SetTarget(CharacterFightController target)
    {
        this.target = target;
    }
}
