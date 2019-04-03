using System.Collections;
using UnityEngine;
using Cinemachine;

public class CutScene_Riddles : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerM;
    [SerializeField] private PlayerCamera playerC;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private CinemachineVirtualCamera cutSceneCamera;
    [SerializeField] private Animator cutSceneAnimator;
    [SerializeField] private Transform bushT, player, newPlayerPos, playerCamera, newPlayerCameraPos;

    public void StartCutScene()
    {
        StartCoroutine("WaitUntilFinishedSpeaking");        
    }

    IEnumerator WaitUntilFinishedSpeaking()
    {
        while (dialogueManager.GetTalking())
            yield return null;

        playerM.FreezePlayerMovement(true, null);
        cutSceneCamera.Priority = int.MaxValue;
        cutSceneAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(6);
        player.SetPositionAndRotation(newPlayerPos.position, newPlayerPos.rotation);
        yield return new WaitForSeconds(0.5f);
        cutSceneAnimator.SetTrigger("Return");
        playerC.SetCameraRotation(newPlayerCameraPos, 20, -90);
        playerC.enabled = false;
        yield return new WaitForSeconds(4);        
        playerM.FreezePlayerMovement(false, bushT);        
        cutSceneCamera.Priority = 0;
        playerC.enabled = true;
    }
}
