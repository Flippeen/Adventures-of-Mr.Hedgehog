using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyShroom : MonoBehaviour
{
    [SerializeField] private Vector3 bounceForce = new Vector3(0,12,0);
    private void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(bounceForce, ForceMode.Impulse);
        }
    }
}
