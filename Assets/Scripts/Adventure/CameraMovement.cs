using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;

    private Vector3 offset; 

    // Start is called before the first frame update
    void Start()
    {
        offset = player.position - this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
