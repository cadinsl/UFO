using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurePlayerAnimation : MonoBehaviour
{
    public Animator animator;
    

    public void SetSpeed(float speed)
    {
        animator.SetFloat("Speed", speed);
    }
}
