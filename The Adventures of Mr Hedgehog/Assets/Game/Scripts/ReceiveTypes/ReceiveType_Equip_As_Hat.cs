using UnityEngine;
using System.Collections;

public class ReceiveType_Equip_As_Hat : ReceiveTypeSuperClass
{
    [SerializeField] private GameObject hatPosition;    

    protected override void Start()
    {
        if (TESTMODE)
            wantedItems.Add(GameObject.Find("[TEST] PickUpable1_Blue(Clone)"));        
    }

    protected override bool HandleItem(GameObject item)
    {
        if (hatPosition == null)
        {
            Debug.LogError("ReceiveType is set to EQUIP_AS_HAT but hatPosition is not assigned!");
            return false;
        }        
        Transform t = item.transform;
        t.SetParent(transform);
        t.localPosition = hatPosition.transform.localPosition;
        t.localRotation = Quaternion.identity;

        if (wantedItems.Count == 0)
            UpdateApproachLine(completedLine);
        return true;
    }    
}
