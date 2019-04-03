using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrevScene : MonoBehaviour
{
    DontDestroy CameFromScene;
    ChapterSelect chSelect;
    [SerializeField] Transform[] scenes;
    [SerializeField] GameObject mainMenu;
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        CameFromScene = FindObjectOfType<DontDestroy>();
        chSelect = GetComponent<ChapterSelect>();
        print(CameFromScene.gameObject.name);
        switch (CameFromScene.gameObject.name)
        {
            case "Prolog":
                chSelect.SwitchToScene(scenes[0].gameObject);
                print("Prolog");
                break;
            case "Kapitel 1":
                chSelect.SwitchToScene(scenes[1].gameObject);
                print("Kaptiel 1");
                break;
            case "Kapitel 2":
                chSelect.SwitchToScene(scenes[2].gameObject);
                print("Kapitel 2");
                break;
            default:
                print("None of the above");
                chSelect.SwitchToScene(mainMenu);
                break;
        }
    }
}
