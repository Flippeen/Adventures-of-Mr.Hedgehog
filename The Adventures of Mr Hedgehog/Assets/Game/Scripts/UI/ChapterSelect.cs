using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterSelect : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    Canvas[] canvases;
    public void SwitchToScene(GameObject setActiveCanvas)
    {
        canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            canvas.gameObject.SetActive(false);
        }
        setActiveCanvas.SetActive(true);

        //mainCam.transform.position = new Vector3(posOfScene.position.x, posOfScene.position.y, -1);
    }
}
