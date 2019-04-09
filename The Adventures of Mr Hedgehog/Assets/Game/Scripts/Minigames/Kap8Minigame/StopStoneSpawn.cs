using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopStoneSpawn : MonoBehaviour
{
    StoneSpawner sS;
    void Start()
    {
        sS = FindObjectOfType<StoneSpawner>();
    }

    private void OnTriggerEnter(Collider c)
    {
        if(c.transform.tag == "Player")
        {
            sS.dontSpawnStones = true;
        }
    }
}
