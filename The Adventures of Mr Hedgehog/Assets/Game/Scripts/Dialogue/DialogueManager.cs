using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    private Queue<string> sentences;
    private bool talking = false, writing = false;

    [SerializeField]
    private GameObject chatBubble, playerCamera, playerPivot;
    private Transform talkingChar, talkingToChar;
    private Dialogue currentDia;

    [SerializeField]
    private TextMeshProUGUI nameText, dialogueText;

    [SerializeField]
    private Animator chatBubble_Animator, player_Animator;

    [SerializeField]
    private float talkSpeed = 0.03f, rotateSpeed = 0.005f;

    [SerializeField]
    private float distanceB4EndDialogue = 30;
    private Quaternion originalRotation;

    [SerializeField]
    private PlayerMovement playerM;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        sentences = new Queue<string>();
        talkingToChar = playerPivot.transform;
    }
    
    private void Update()
    {        
        if (talking)
        {
            chatBubble.transform.LookAt(playerCamera.transform);
            talkingChar.LookAt(new Vector3(talkingToChar.position.x, talkingChar.position.y, talkingToChar.position.z));
            talkingToChar.LookAt(new Vector3(talkingChar.position.x, talkingToChar.position.y, talkingChar.position.z));

            if ((talkingChar.position - playerPivot.transform.position).sqrMagnitude > distanceB4EndDialogue) // Stäng konversation om spelaren springer iväg.
                EndDialogue();
        }        
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E) && talking && !writing)
            DisplayNextSentence();
    }

    public void StartDialogue(Dialogue dialogue, bool freezeMovement)
    {
        if (currentDia != null && dialogue.id == currentDia.id)
            return;

        originalRotation = dialogue.character.transform.rotation;
        currentDia = dialogue;
        talkingChar = dialogue.character.transform;
        talkingToChar = (dialogue.talkingToCharacter != null) ? dialogue.talkingToCharacter : playerPivot.transform;
        chatBubble.transform.position = new Vector3(talkingChar.position.x, talkingChar.position.y + dialogue.dialogueboxHeight, talkingChar.position.z);
        nameText.text = dialogue.character.name;

       
        chatBubble_Animator.SetBool("IsOpen", true);
        talking = true;
        sentences.Clear();

        if (freezeMovement)
        {            
            playerM.FreezePlayerMovement(true, dialogue.character.transform);
            player_Animator.SetBool("Walking", false);
        }                  

        foreach (string sentence in dialogue.sentences)
            sentences.Enqueue(sentence);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        dialogueText.text = string.Empty;
        StopCoroutine("Write");
        StartCoroutine("Write", sentences.Dequeue());
    }

    [SerializeField]
    private GameObject tipsCanvas_E;

    private IEnumerator Write(string sentence)
    {
        writing = true;
        tipsCanvas_E.SetActive(false);
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(talkSpeed);
        }
        writing = false;
        tipsCanvas_E.SetActive(true);
    }

    private void EndDialogue()
    {
        chatBubble_Animator.SetBool("IsOpen", false);
        talking = false;
        StopCoroutine("Rotate");
        StartCoroutine("Rotate", talkingChar);
        talkingChar = null;
        currentDia = null;
        playerM.FreezePlayerMovement(false, null);   
		InteractWithCharacter.Instance.FinishedTalking();
    }
    
    private IEnumerator Rotate(Transform lerpTarget)
    {
        if (lerpTarget.GetComponent<FollowPlayer>() != null)
            StopCoroutine("Rotate");

        Quaternion startRotation = lerpTarget.rotation;
        float progress = 0.0f;

        while (progress < 0.99f)
        {
            progress += rotateSpeed;
            lerpTarget.rotation = Quaternion.Lerp(lerpTarget.rotation, originalRotation, progress);
            yield return new WaitForFixedUpdate();
        }        
    }

    public bool GetTalking()
    {
        return talking;
    }
}
