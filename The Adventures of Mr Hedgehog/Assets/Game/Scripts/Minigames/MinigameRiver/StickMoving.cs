using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickMoving : MonoBehaviour
{
    Vector3 top;
    Vector3 bottom;
    void Start()
    {
        top = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        bottom = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(top, bottom, Mathf.PingPong(Time.time * 1.3f, 1.0f));
        Quaternion rotateTo = Quaternion.Euler(45, Time.time * 90, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotateTo, 9);
    }
}
