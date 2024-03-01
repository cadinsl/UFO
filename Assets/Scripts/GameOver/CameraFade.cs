using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFade : MonoBehaviour
{
    public PSXEffects pSXEffects;
    public float fadeSpeed;

    private void Start()
    {
        pSXEffects.subtractFade = 60;
    }
    private void Update()
    {
        pSXEffects.subtractFade =  (int)Mathf.Lerp(60, 0, Time.time * fadeSpeed);
    }
}
