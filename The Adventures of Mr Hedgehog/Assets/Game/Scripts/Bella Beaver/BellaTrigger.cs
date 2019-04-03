using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellaTrigger : MonoBehaviour
{  
    [SerializeField] private Collider playerCollider;
    [SerializeField] private FollowPlayer followPlayer;

    [SerializeField] private bool beginFollowingPlayer = true;
    [SerializeField] private Transform destination;
    
    void OnTriggerEnter(Collider collider)
    {
        if (collider == playerCollider)
        {
            if (beginFollowingPlayer)
                followPlayer.StartFollowing();
            else
                followPlayer.WaitAt(destination);
            Destroy(gameObject);
        }            
    }
}
