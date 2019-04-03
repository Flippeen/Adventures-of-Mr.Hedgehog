using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveMeshes : MonoBehaviour
{
    [SerializeField] GameObject[] parentsOfMeshes;
    List<MeshRenderer> childrenWithMeshes = new List<MeshRenderer>();
    void Start()
    {
        foreach (var parent in parentsOfMeshes)
        {
            if(parent.transform.childCount == 0)
            {
                parent.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                for (int i = 0; i < parent.transform.childCount; i++)
                {
                    childrenWithMeshes.Add(parent.transform.GetChild(i).GetComponentInChildren<MeshRenderer>());
                }
            }
        }
        HideMeshes();
    }

    void HideMeshes()
    {
        foreach (var childWithMesh in childrenWithMeshes)
        {
            childWithMesh.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
