using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonesPushing : MonoBehaviour
{
    Rigidbody rb;
    private void OnCollisionEnter(Collision c)
    {
        if (c.transform.tag == "Player")
        {
            StartCoroutine(c.transform.GetComponent<MinigameController>().NoInputTimer(0.5f));
            Vector3 dir = c.transform.position - transform.position;
            dir = dir.normalized;
            rb = c.transform.GetComponent<Rigidbody>();
            if (c.transform.position.x > transform.position.x)
            {
                rb.AddForce(dir * 5 + Vector3.right * 5, ForceMode.VelocityChange);
            }
            else
            {
                rb.AddForce(dir * 5 + Vector3.left * 5, ForceMode.VelocityChange);
            }
        }
    }
}
