using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneRolling : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 1000 * Time.deltaTime, ForceMode.VelocityChange);
    }

    void OnCollisionEnter(Collision c)
    {
        if(c.gameObject.tag == "RiverWalls")
            rb.AddForce((c.GetContact(0).normal + c.transform.forward).normalized * 500 * Time.deltaTime, ForceMode.VelocityChange);
    }
    [SerializeField] float gravityDown;
    [SerializeField] LayerMask ground;
    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * (transform.localScale.y / 2));
        if (!Physics.Raycast(transform.position, Vector3.down, transform.localScale.y / 2, ground))
        {
            Debug.Log("In Air");
            rb.velocity += Vector3.down * gravityDown * Time.deltaTime;
        }
    }
}
