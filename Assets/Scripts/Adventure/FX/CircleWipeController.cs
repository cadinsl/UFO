using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleWipeController : MonoBehaviour
{
    private const float RADIUS = 2f;

    public Shader shader;

    private Material material;


    [Range(0, RADIUS)]
    public float radius = 0f;

    public float horizontal = 16;

    public float vertical = 9;

    public float duration = 1f;

    void Awake()
    {
        material = new Material(shader);
        UpdateShader();
    }

    void OnValidate()
    {
        material = material == null ? new Material(shader) : material;
        UpdateShader();
    }

    void Start()
    {
        FadeIn();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }

    public void FadeOut()
    {
        StartCoroutine(DoFade(RADIUS, 0f));
    }

    public void FadeIn()
    {
        StartCoroutine(DoFade(0f, RADIUS));
    }

    IEnumerator DoFade(float start, float end)
    {
        radius = start;
        UpdateShader();

        var time = 0f;
        while (time < 1f)
        {
            radius = Mathf.Lerp(start, end, time);
            time += Time.deltaTime / duration;
            UpdateShader();
            yield return null;
        }

        radius = end;
        UpdateShader();
        //callback?.Invoke();
    }

    public void UpdateShader()
    {
        var radiusSpeed = Mathf.Max(horizontal, vertical);
        material.SetFloat("_Horizontal", horizontal);
        material.SetFloat("_Vertical", vertical);
        material.SetFloat("_RadiusSpeed", radiusSpeed);
        material.SetFloat("_Radius", radius);
    }
}
