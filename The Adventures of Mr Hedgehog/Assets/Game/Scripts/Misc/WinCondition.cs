using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    public static WinCondition Instance { get; private set; }
    Meny meny;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        meny = FindObjectOfType<Meny>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            EndingOfLevel();
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.tag == "Player")
        {
            EndingOfLevel();
        }
    }
    
    [SerializeField] Canvas endingCanvas;
    float timePassed;
    public void EndingOfLevel()
    {
        Image blackFade = endingCanvas.GetComponentInChildren<Image>();
        blackFade.enabled = true;
        blackFade.color = new Vector4(0,0,0,0);

        StartCoroutine(FadeScreen(blackFade));
    }

    [SerializeField] string mainMenu;
    DontDestroy dontDestroyObject;
    IEnumerator FadeScreen(Image blackFade)
    {
        while (blackFade.color.a < 1)
        {
            blackFade.color = new Vector4(0, 0, 0, blackFade.color.a + 0.02f);

            yield return new WaitForSeconds(0.05f);

            if (blackFade.color.a >= 0.95f)
            {

                dontDestroyObject = FindObjectOfType<DontDestroy>();
                if (dontDestroyObject != null)
                {
                    dontDestroyObject.ChangeName();
                }
                meny.SwitchScene(mainMenu);
            }
        }
    }
}
