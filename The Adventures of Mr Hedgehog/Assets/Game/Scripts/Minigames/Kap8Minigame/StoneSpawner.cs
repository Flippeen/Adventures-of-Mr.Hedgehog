using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpawner : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SpawnStones());
    }

    [SerializeField] GameObject rollingStone;
    IEnumerator SpawnStones()
    {
        GameObject newStone = Instantiate(rollingStone, transform.position, transform.rotation);
        Destroy(newStone, 10);
        yield return new WaitForSeconds(15);
        StartCoroutine(SpawnStones());
    }
}
