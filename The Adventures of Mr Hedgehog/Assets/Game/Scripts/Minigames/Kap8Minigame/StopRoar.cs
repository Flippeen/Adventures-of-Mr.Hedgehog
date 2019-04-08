using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopRoar : MonoBehaviour
{
    BorisController sS;
    void Start()
    {
        sS = FindObjectOfType<BorisController>();
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.transform.tag == "Player")
        {
            sS.dontRoar = true;
        }
    }
}
