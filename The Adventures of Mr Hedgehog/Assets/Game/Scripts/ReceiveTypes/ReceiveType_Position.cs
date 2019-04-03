using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ReceiveType_Position : ReceiveTypeSuperClass
{
    [SerializeField]
    private List<GameObject> targetPosition = new List<GameObject>();
	[SerializeField]
	private GameObject parentObj;

    [SerializeField]
    private int groundLayer = 10;

    protected override void Start()
    {
        if (TESTMODE)
        {
            wantedItems.AddRange(GameObject.FindGameObjectsWithTag("PickUpable").Where(obj => obj.name == "[TEST] PickUpable2_Red(Clone)"));
            targetPosition.AddRange(GameObject.FindGameObjectsWithTag("[TEST]_Item_Position"));
        }
    }

    protected override bool HandleItem(GameObject item)
    {
        if (targetPosition.Count == 0)
        {
            Debug.LogError("ReceiveType is set to POSITION but targetPosition is not defined!");
            return false;
        }
		item.GetComponent<Collider>().enabled = true;
        Transform t = item.transform;
        t.gameObject.layer = groundLayer;
        t.position = targetPosition[0].transform.position;
        t.rotation = targetPosition[0].transform.rotation;
        targetPosition.RemoveAt(0);
        return true;
    }
}
