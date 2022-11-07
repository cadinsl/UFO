using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : MonoBehaviour
{
    public GameObject player;
    private NavMeshAgent agent;
    private AdventurePlayerAnimation adventurePlayerAnimation;
    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        adventurePlayerAnimation = this.GetComponent<AdventurePlayerAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position);
        RotateTowards(player.transform);
        if(adventurePlayerAnimation != null)
        {
            adventurePlayerAnimation.SetSpeed(agent.velocity.magnitude);
        }
    }
    /*
    private bool IsInMeleeRangeOf (Transform target) {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance < meleeRange;
     }*/
    
    private void RotateTowards (Transform target) {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;
    }
}
