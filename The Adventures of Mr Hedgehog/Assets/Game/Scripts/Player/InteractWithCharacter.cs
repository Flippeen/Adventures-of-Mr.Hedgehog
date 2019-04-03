using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class InteractWithCharacter : MonoBehaviour
{
	public static InteractWithCharacter Instance = null;
    private ReceiveTypeSuperClass receiveTypeSC;

    [SerializeField]
    private List<GameObject> characters = new List<GameObject>();
    private GameObject currentCharacter = null;

    [SerializeField]
    private GameObject tipCanvas_RightClick, tipsCanvas_E;

    [SerializeField]
    private float tipDist_X = 1.5f, tipDist_Y = 0.5f;

    [SerializeField]
    private Transform playerCamera;

    [SerializeField]
    private bool TESTMODE = false;
	private bool scriptedDialogueInProgress = false;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}
	private void Start()
    {
        if (TESTMODE)
            characters.AddRange(GameObject.FindGameObjectsWithTag("[TEST]_Character"));            
    }

    private void OnTriggerEnter(Collider other)
    {
        if (characters.Contains(other.gameObject) && !DialogueManager.Instance.GetTalking() && !scriptedDialogueInProgress)
        {                      
            if ((other.gameObject.GetComponent<ReceiveTypeSuperClass>() == null) && (other.gameObject.GetComponent<ScriptedDialogue>() == null))
                return;

			currentCharacter = other.gameObject;
			receiveTypeSC = currentCharacter.GetComponent<ReceiveTypeSuperClass>() ?? null;

			if (receiveTypeSC != null)
			{			
				if (!receiveTypeSC.GetMute())
					ToggleECanvas(true);

				if (PickUp.Instance.GetCurrentObject() != null &&
					receiveTypeSC.GetHasTalked() && !receiveTypeSC.GetHasTurnedIn() && !DialogueManager.Instance.GetTalking()) // Visa tips för att kunna ge föremål.
						ToggleRightClickCanvas(true);
			}                                                           
        }
    }

    private void Update()
    {
        // Tala med karaktären.
        if (Input.GetKeyDown(KeyCode.E) && currentCharacter != null && !DialogueManager.Instance.GetTalking()) 
            StartTalking();

        // Ge föremål.
        if (Input.GetMouseButtonDown(1) && PickUp.Instance.GetCurrentObject() != null && currentCharacter != null && !receiveTypeSC.GetHasTurnedIn() && 
            receiveTypeSC.GetHasTalked() && !DialogueManager.Instance.GetTalking()) 
        {
            ToggleECanvas(false);
            ToggleRightClickCanvas(false);
			PickUp.Instance.ConfirmItemGiven(receiveTypeSC.ReceiveItem(PickUp.Instance.GetCurrentObject()));
        }                  
            
        if (tipCanvas_RightClick.activeSelf)
            tipCanvas_RightClick.transform.LookAt(playerCamera.transform); 
        if (tipsCanvas_E.activeSelf)
            tipsCanvas_E.transform.LookAt(playerCamera.transform);        
    }

    private void LateUpdate()
    {     
        // Om spelaren plockar upp ett föremål framför en karaktär bör rightclickCanvas visas.
        if (Input.GetMouseButtonDown(0) && currentCharacter != null &&  receiveTypeSC != null && !receiveTypeSC.GetHasTurnedIn() && !tipCanvas_RightClick.activeSelf &&
           receiveTypeSC.GetHasTalked() && !DialogueManager.Instance.GetTalking() && PickUp.Instance.GetCurrentObject() != null)
                ToggleRightClickCanvas(true);           
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentCharacter)
        {
            receiveTypeSC = null;
            currentCharacter = null;
            tipCanvas_RightClick.SetActive(false);
            ToggleECanvas(false);
        }                
    }

    private void StartTalking()
    {
        ToggleRightClickCanvas(false);
        if (currentCharacter.GetComponent<ScriptedDialogue>() != null)
		{
			SetScriptedDialogueStatus(true);
			currentCharacter.GetComponent<ScriptedDialogue>().StartCoroutine("StartConversation");
		}            
        else if (currentCharacter.GetComponent<ReceiveTypeSuperClass>() != null)
            currentCharacter.GetComponent<ReceiveTypeSuperClass>().SayStandardLine(ReceiveTypeSuperClass.Line.APPROACHLINE);          
    }

    public void FinishedTalking()
    {        
        if (currentCharacter != null && receiveTypeSC != null)
        {
            ToggleECanvas(true);
            if (PickUp.Instance.GetCurrentObject() != null && !receiveTypeSC.GetHasTurnedIn())
                ToggleRightClickCanvas(true);
        }
        else
        {
            currentCharacter = null;
            ToggleECanvas(false);
        }
    }

	public void SetScriptedDialogueStatus(bool active)
	{
		scriptedDialogueInProgress = active;
	}

    private void ToggleRightClickCanvas(bool active)
    {
        if (active && currentCharacter != null)
        {
            TipsCanvasPosition TCP = currentCharacter.GetComponent<TipsCanvasPosition>() ?? null;
            Vector2 position = (TCP != null) ? TCP.Get_Tip_RightClick() : new Vector2(1, 1);

            tipCanvas_RightClick.transform.SetPositionAndRotation
                (currentCharacter.transform.localPosition + transform.right * position.x +
                new Vector3(0, position.y, 0), Quaternion.identity);
        }            
        tipCanvas_RightClick.SetActive(active);
    }

    private void ToggleECanvas(bool active)
    {
        if (active && currentCharacter != null)
        {
            TipsCanvasPosition TCP = currentCharacter.GetComponent<TipsCanvasPosition>() ?? null;
            Vector2 position = (TCP != null) ? TCP.Get_Tip_E() : new Vector2(1, 1);

            tipsCanvas_E.transform.SetPositionAndRotation
                (currentCharacter.transform.localPosition - transform.right * position.y +
                new Vector3(0, position.x, 0), Quaternion.identity);
        }            
        tipsCanvas_E.SetActive(active);
    }
}
