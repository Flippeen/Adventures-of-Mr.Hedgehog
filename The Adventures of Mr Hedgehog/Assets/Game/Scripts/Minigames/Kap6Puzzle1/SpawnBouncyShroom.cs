using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBouncyShroom : MonoBehaviour
{
    [SerializeField] private GameObject bouncyShroom;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bouncyShroom.SetActive(true);
            Destroy(gameObject);
        }
    }
}
