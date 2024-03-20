using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton
{
    public void OnClick()
    {
        AkSoundEngine.PostEvent("UIClick", gameObject);
    }

    public void GotFocus()
    {
        AkSoundEngine.PostEvent("UIGotFocus", gameObject);
    }
}
