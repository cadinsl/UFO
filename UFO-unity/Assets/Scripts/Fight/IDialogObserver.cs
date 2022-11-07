using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogObserver
{
    public void NotifyEnd();

    public void NotifyEndDecision();

    public void NotifyEndOfBattle();
}
