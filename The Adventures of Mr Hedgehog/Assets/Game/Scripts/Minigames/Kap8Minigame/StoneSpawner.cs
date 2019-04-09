using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpawner : MonoBehaviour
{
    [SerializeField] GameObject rollingStone;
    public bool dontSpawnStones;
    public void SpawnStones()
    {
        if (dontSpawnStones)
            return;
        GameObject newStone = Instantiate(rollingStone, transform.position, transform.rotation);
        Destroy(newStone, 10);
    }
}
