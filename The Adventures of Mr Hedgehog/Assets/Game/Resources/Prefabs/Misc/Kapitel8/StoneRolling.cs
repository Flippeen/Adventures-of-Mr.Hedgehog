using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneRolling : MonoBehaviour
{
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * 1000 * Time.deltaTime, ForceMode.VelocityChange);
    }
}
