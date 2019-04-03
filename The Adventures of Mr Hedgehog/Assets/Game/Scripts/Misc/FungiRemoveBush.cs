using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungiRemoveBush : MonoBehaviour
{
    [SerializeField] GameObject bushToDestroy;
    public void DestroyTheBush()
    {
        Destroy(bushToDestroy, 1);
    }
}
