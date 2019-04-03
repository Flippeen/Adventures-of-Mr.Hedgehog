using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float rotateSpeed = 3.0f;
    [SerializeField] private bool following = false;

    private Animator animator;        
    private NavMeshAgent agent;
    private RaycastHit shot;
    private Quaternion rotationUponDestinationReached;
    private float stoppingDistance_default;

    private void Start()
    {
        animator = GetComponent<Animator>() ?? null;
        agent = GetComponent<NavMeshAgent>() ?? null;

        if (player == null || agent == null)
        {
            Debug.LogError("FollowPlayer script on " + gameObject.name + " is missing player and/or agent reference.");
            Destroy(this);
        }
        stoppingDistance_default = agent.stoppingDistance;
        rotationUponDestinationReached = transform.rotation;
    }        
    
    private void FixedUpdate()
    {
        if (following)
            agent.SetDestination(player.position);
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            animator.SetBool("Walking", false);
            if (!following && (rotationUponDestinationReached.eulerAngles - transform.rotation.eulerAngles).sqrMagnitude > 1)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationUponDestinationReached, rotateSpeed);
        }
        else if (animator.GetBool("Walking") == false)    
            animator.SetBool("Walking", true);
    }

    public void WaitAt(Transform t)
    {
        following = false;
        animator.SetBool("Walking", true);
        agent.stoppingDistance = 0.1f;
        agent.SetDestination(t.position);
        rotationUponDestinationReached = t.rotation;
    }

    public void StartFollowing()
    {
        agent.stoppingDistance = stoppingDistance_default;
        following = true;
        animator.SetBool("Walking", true);
    }
}
