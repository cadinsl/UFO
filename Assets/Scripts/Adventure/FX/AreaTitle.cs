using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AreaTitle : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float distanceThreshold;
    [SerializeField]
    private float transparencyMultiplier;
    private Color color;
    [SerializeField]
    private bool isNew;
    [SerializeField]
    private TMP_Text text;
    private bool increment = true;
    private bool alreadyCalled = false;
    private float t;

    private void Start()
    {
        t = 0;
        if (isNew)
        { 
            color = spriteRenderer.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isNew)
        {
            float distance = Mathf.Abs(Vector3.Distance(this.transform.position, player.transform.position));
            if (distance > distanceThreshold)
            {
                spriteRenderer.color = new Color(color.r, color.g, color.b, color.a - ((distance - distanceThreshold)) * transparencyMultiplier);
            }
            else
            {
                spriteRenderer.color = color;
            }
        }
        else
        {
            if (increment)
            {
                FadeIn();
                if (text.color.a >= 1 && !alreadyCalled)
                {
                    StartCoroutine(fadeInfadeOut());
                    alreadyCalled = true;
                }
            }
            else
            {
                FadeOut();
            }
        }
    }

    IEnumerator fadeInfadeOut()
    {
        yield return new WaitForSeconds(1f);
        t = 0;
        increment = false;
        
    }

    private void FadeIn()
    {
        float transperancy = Mathf.Lerp(0, 255, t)/255;
        t += transparencyMultiplier * Time.deltaTime;
        text.color = new Color(text.color.r, text.color.g, text.color.b, transperancy);
    }

    private void FadeOut()
    {
        float transperancy = Mathf.Lerp(255, 0, t)/255;
        t += transparencyMultiplier * Time.deltaTime;
        text.color = new Color(text.color.r, text.color.g, text.color.b, transperancy);
        if(transperancy <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
