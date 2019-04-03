using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedDialogue : MonoBehaviour
{
    [SerializeField] private Dialogue[] talkOrder;
    [SerializeField] private bool freezePlayerMovement = true, continueWithApproachLine = true;
   IEnumerator StartConversation()
    {
        foreach (var dialogue in talkOrder)
        {
            DialogueManager.Instance.StartDialogue(dialogue, freezePlayerMovement);
            yield return new WaitUntil(() => DialogueManager.Instance.GetTalking() == false);
        }
        EndConversation();
    }

	[SerializeField] private GameObject blockade;
    public void EndConversation()
    {
        if (continueWithApproachLine)
            GetComponent<ReceiveTypeSuperClass>().SayStandardLine(ReceiveTypeSuperClass.Line.APPROACHLINE);
		if (blockade != null)
			Destroy(blockade);

		InteractWithCharacter.Instance.SetScriptedDialogueStatus(false);
        Destroy(this);
    }
}
