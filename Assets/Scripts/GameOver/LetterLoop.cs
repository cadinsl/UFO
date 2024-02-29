using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterLoop : MonoBehaviour
{
    public float speed;
    public float distance;
    private float startY;
    private void Start()
    {
        startY = transform.position.y;
    }

    public void Update()
    {
        float y = Mathf.PingPong(Time.time * speed, distance);
        transform.position = new Vector3(transform.position.x, startY + y, transform.position.z);
    }
}
