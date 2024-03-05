using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainRenderer : MonoBehaviour
{
    float speed = 0.2f;
    Renderer fountainRenderer;

    void Start()
    {
        fountainRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        float offset = Time.time * speed;
        fountainRenderer.material
            .mainTextureOffset =  new Vector2(0, offset);
    }
}
