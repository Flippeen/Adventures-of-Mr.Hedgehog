using System.Collections.Generic;
using UnityEngine;

public abstract class ReceiveTypeSuperClass : MonoBehaviour
{
    protected enum ReceiveType { DESTROY, POSITION, EQUIP_AS_HAT }
    [SerializeField]
    protected ReceiveType receiveType = ReceiveType.EQUIP_AS_HAT;

    public enum Line { APPROACHLINE, RECEIVELINE, REJECTLINE, COMPLETEDLINE }

    [SerializeField]
    protected List<GameObject> wantedItems = new List<GameObject>();

    [SerializeField]
    [TextArea(3, 10)]
    protected string[] approachLine, receiveLine, rejectLine, completedLine;

    [SerializeField]
    protected bool TESTMODE = false, mute = false, inOrder = false, hasTalked = false;
	protected bool hasTurnedIn = false;

	protected virtual void Start(){}

    public bool GetHasTurnedIn(){ return hasTurnedIn; }

    public bool ReceiveItem(GameObject item)
    {
        if (item == null)
            return false;

        if (wantedItems.Contains(item))
        {
            if (inOrder && wantedItems.IndexOf(item) != 0)
            {
                SayStandardLine(Line.REJECTLINE);
                return false;
            }
			
			wantedItems.Remove(item);            
            SayStandardLine(Line.RECEIVELINE);
              
            Destroy(item.GetComponent<Rigidbody>());
            item.tag = "Untagged";
			PickUp.Instance.RemoveGivenItem(item);

			if (wantedItems.Count == 0)
            {
                hasTurnedIn = true;
                SayStandardLine(Line.COMPLETEDLINE);
                CompletedAction();
            }
            return HandleItem(item);            
        }
        else
        {
            SayStandardLine(Line.REJECTLINE);
            return false;
        }        
    }

    protected virtual bool HandleItem(GameObject item){ return true; }

    protected virtual bool CompletedAction() { return true;  }

    [SerializeField] protected bool freezeMovementDuringTalk = false;
    [SerializeField] private float ChatBubbleDistanceAboveCharacter = 2f;
    public bool SayStandardLine(Line line)
    {
        if (mute)
            return false;

        switch (line)
        {
            case Line.APPROACHLINE:
                hasTalked = true;
                DialogueManager.Instance.StartDialogue(new Dialogue() { id = 1, character = gameObject, sentences = approachLine, dialogueboxHeight = ChatBubbleDistanceAboveCharacter }, freezeMovementDuringTalk);
                return true;

            case Line.RECEIVELINE:
                DialogueManager.Instance.StartDialogue(new Dialogue() { id = 2, character = gameObject, sentences = receiveLine, dialogueboxHeight = ChatBubbleDistanceAboveCharacter }, freezeMovementDuringTalk);
                return true;

            case Line.REJECTLINE:
                DialogueManager.Instance.StartDialogue(new Dialogue() { id = 3,  character = gameObject, sentences = rejectLine, dialogueboxHeight = ChatBubbleDistanceAboveCharacter }, freezeMovementDuringTalk);
                return true;

            case Line.COMPLETEDLINE:
                DialogueManager.Instance.StartDialogue(new Dialogue() { id = 4, character = gameObject, sentences = completedLine, dialogueboxHeight = ChatBubbleDistanceAboveCharacter }, freezeMovementDuringTalk);
                return true;

            default:
                return false;
        }             
    }

    public bool SaySpecialLine(string[] lines, bool freezePlayerMovement)
    {
        if (lines.Length < 1)
            return false;

        DialogueManager.Instance.StartDialogue(new Dialogue() { id = 5, character = gameObject, sentences = lines, dialogueboxHeight = ChatBubbleDistanceAboveCharacter }, freezeMovementDuringTalk);
        return true;
    }

    public bool UpdateApproachLine(string[] line)
    {
        if (line.Length < 1)
            return false;

        approachLine = line;
        return true;
    }

    public bool GetHasTalked()
    {
        return hasTalked;
    }

	public bool GetMute()
	{
		return mute;
	}
}
