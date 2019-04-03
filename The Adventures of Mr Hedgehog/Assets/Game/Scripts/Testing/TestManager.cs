using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{    
    private Transform characterSpawnPos, itemSpawnPos;

    [SerializeField]
    private int numberOfRedCubes = 2;
    [SerializeField]
    private string redCubeName = "[TEST] PickUpable2_Red";

    void Awake()
    {
        characterSpawnPos = GameObject.Find("CharacterSpawnPos").transform;
        itemSpawnPos = GameObject.Find("ItemSpawnPos").transform;

        SpawnCharacters();
        SpawnItems();        
    }   
    
    private void SpawnCharacters()
    {
        GameObject[] playerPref = Resources.LoadAll<GameObject>("Prefabs/Characters/Testing");
        Debug.Log(playerPref.Length + " characters found and spawned.");
        for (int i = 0; i < playerPref.Length; i++)
            Instantiate(playerPref[i], characterSpawnPos.position + new Vector3(i * 9, 0, 0), Quaternion.Euler(0,180,0));
    }

    private void SpawnItems()
    {
        GameObject[] itemPref = Resources.LoadAll<GameObject>("Prefabs/Items/PickUpables/Testing");
        Debug.Log(itemPref.Length + " items found and spawned.");
        for (int i = 0; i < itemPref.Length; i++)
        {            
            Instantiate(itemPref[i], itemSpawnPos.position + new Vector3(i * 9, 0, 0), Quaternion.Euler(0, 180, 0));

            if (itemPref[i].name == redCubeName)
                for (int j = 1; j <= numberOfRedCubes-1; j++)
                    Instantiate(itemPref[i], itemSpawnPos.position + new Vector3(i * 9, 0.75f * j, 0), Quaternion.Euler(0, 180, 0));
        }            
    }
}
