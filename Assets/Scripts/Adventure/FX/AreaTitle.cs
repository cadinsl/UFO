using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        color = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Mathf.Abs(Vector3.Distance(this.transform.position, player.transform.position));
        if(distance > distanceThreshold)
        {
            spriteRenderer.color = new Color(color.r, color.g, color.b, color.a - ((distance - distanceThreshold)) * transparencyMultiplier);
        }
        else
        {
            spriteRenderer.color = color;
        }
    }
}
