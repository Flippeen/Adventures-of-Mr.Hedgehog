using UnityEngine;

public class ReceiveType_Destroy : ReceiveTypeSuperClass
{
    private Riddles riddles;

    protected override void Start()
    {
        if (TESTMODE)
            wantedItems.Add(GameObject.Find("[TEST] PickUpable3_Yellow(Clone)"));
        riddles = GetComponent<Riddles>() ?? null;
    }

    protected override bool HandleItem(GameObject item)
    {
        if (riddles != null)
            riddles.NextRiddle();
            
        Destroy(item);
        return true;
    }    

    [SerializeField] private CutScene_Riddles cutScene_Riddles;
    protected override bool CompletedAction()
    {
        UpdateApproachLine(completedLine);
        if (cutScene_Riddles != null)
            cutScene_Riddles.StartCutScene();
        return true;
    }    
}
