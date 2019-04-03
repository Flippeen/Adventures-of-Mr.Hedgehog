using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] Transform[] saveUs;
    void Start()
    {
        foreach (var item in saveUs)
        {
            dict.Add(item, item.transform.position);
        }
    }

    int count;
    GameObject[] playerPref;
    Dictionary<Transform, Vector3> dict = new Dictionary<Transform, Vector3>();
    public bool AddToDictionary(Transform itemToAdd)
    {
        if (!dict.ContainsKey(itemToAdd))
        {
            dict.Add(itemToAdd, itemToAdd.transform.position);
            return true;
        }
        return false;
    }
    public bool RemoveItemForDictionary(Transform itemToAdd)
    {
        if (dict.ContainsKey(itemToAdd))
        {
            dict.Remove(itemToAdd);
            return true;
        }
        return false;
    }
    public void ResetItemsInDictionary()
    {
        foreach (KeyValuePair<Transform, Vector3> item in dict)
        {
            item.Key.position = item.Value;
        }
        dict.Clear();
    }

    public void RespawnAtCheckpoint()
    {
        transform.position = checkpointPos;
        ResetItemsInDictionary();
    }

    Vector3 checkpointPos;
    void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.tag == "Checkpoint")
        {
            checkpointPos = c.transform.position;
        }
        if(c.gameObject.tag == "Death")
        {
            RespawnAtCheckpoint();
        }
    }
}
