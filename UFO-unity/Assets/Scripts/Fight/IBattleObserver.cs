using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleObserver 
{
    public void NotifyAboutToAct(CharacterDecision characterDecision);

    public void NotifyDecisionActed(CharacterDecisionResult result);

    public void NotifyBattleEnded(BattleEndResult result);
}
