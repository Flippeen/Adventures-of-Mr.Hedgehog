using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Riddles : MonoBehaviour
{
    [SerializeField]
    private DialogueManager dialogueManager;
    private ReceiveType_Destroy receiveType_Destroy;

    [SerializeField]
    [TextArea(3, 10)]
    protected string[] riddle1, riddle2, riddle3;
    private int riddleNr = 0;
    private List<string[]> riddles;

    private void Start()
    {
        riddles = new List<string[]> { riddle1, riddle2, riddle3 };

        receiveType_Destroy = GetComponent<ReceiveType_Destroy>() ?? null;
        if (receiveType_Destroy == null)
            Debug.LogError("ReceiveType_Destroy refrence missing in Riddles script.");
    }

    public void NextRiddle()
    {
        if (++riddleNr < riddles.Count)
        {
            StopCoroutine("Wait");
            StartCoroutine("Wait");
        }
        else
            RiddlesCompletedActions();

    }
    [SerializeField]
    private PlayerMovement playerM;
    IEnumerator Wait()
    {
        receiveType_Destroy.UpdateApproachLine(riddles[riddleNr]);

        while (dialogueManager.GetTalking())
            yield return null;
       
        if (riddleNr <= riddles.Count)           
            receiveType_Destroy.SaySpecialLine(riddles[riddleNr], true);
    }

    [SerializeField] private GameObject bushToDestroy;
    [SerializeField] private float destroyDelay;
    [SerializeField] private FollowPlayer followPlayer;
    [SerializeField] private Transform waitPosition;
    private void RiddlesCompletedActions()
    {
        Destroy(bushToDestroy, destroyDelay); // Senare "Animator.SetTrigger("DestroyBush");
        followPlayer.WaitAt(waitPosition);
        Destroy(this);
    }
}
