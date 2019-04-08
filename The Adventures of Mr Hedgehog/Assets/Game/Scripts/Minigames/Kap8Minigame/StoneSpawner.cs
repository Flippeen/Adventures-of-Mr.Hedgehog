using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpawner : MonoBehaviour
{
    [SerializeField] GameObject rollingStone;
    public void SpawnStones()
    {
        GameObject newStone = Instantiate(rollingStone, transform.position, transform.rotation);
        Destroy(newStone, 10);
    }
}
