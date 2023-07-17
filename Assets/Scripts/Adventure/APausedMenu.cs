using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class APausedMenu : MonoBehaviour
{
    
    public abstract void Close();

    public abstract void Display(CharacterDoll doll);
}
