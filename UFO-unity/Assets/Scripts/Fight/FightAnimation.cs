using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightAnimation : MonoBehaviour
{
    public Animator animator;

    public void SetAnimator(Animator animator)
    {
        this.animator = animator;
    }
    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void Death()
    {
        animator.SetTrigger("Death");
    }

    public bool isInAction()
    {
        //If not idle or dead
        bool finishedTransition = this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
        bool isIdle = this.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && finishedTransition;
        bool isDead = this.animator.GetCurrentAnimatorStateInfo(0).IsName("Death") && finishedTransition;
        
        bool isInAction = (!isIdle && !isDead);
        Debug.Log(this.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        return isInAction;
    }
}
