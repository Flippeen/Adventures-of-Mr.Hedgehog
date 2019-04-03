using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    DontDestroy[] dontDestroyArr;
    List<GameObject> removeThese = new List<GameObject>();
    public void ChangeName()
    {
        dontDestroyArr = FindObjectsOfType<DontDestroy>();
        if(dontDestroyArr.Length > 1)
        {
            foreach (DontDestroy dontDest in dontDestroyArr)
            {
                if(dontDest != this)
                {
                    removeThese.Add(dontDest.gameObject);
                }
            }
            while (removeThese.Count > 0)
            {
                GameObject dontDestroy = removeThese[0];
                removeThese.Remove(dontDestroy);
                Destroy(dontDestroy.gameObject);
            }
        }
        Scene currSceneName = SceneManager.GetActiveScene();
        gameObject.name = currSceneName.name;
    }
}
