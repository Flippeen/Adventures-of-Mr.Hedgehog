using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Meny : MonoBehaviour
{
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    [SerializeField] GameObject player;
    public void ResetToLastCheckpoint()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Checkpoint>().RespawnAtCheckpoint();
    }

    bool canvasVisable;
    [SerializeField] GameObject canvasPause;
    public void ShowCanvas()
    {
        canvasVisable = !canvasVisable;
        Cursor.visible = canvasVisable;
        Cursor.lockState = (canvasVisable) ? CursorLockMode.None : CursorLockMode.Locked;
        if (canvasPause == null)
            return;
        canvasPause.SetActive(canvasVisable);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ShowCanvas();
    }
}
