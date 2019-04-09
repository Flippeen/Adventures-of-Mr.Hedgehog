using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ReceiveType_Position : ReceiveTypeSuperClass
{
    [SerializeField] private List<GameObject> targetPosition = new List<GameObject>();

    [SerializeField] private int groundLayer = 10, itemLayer = 12;

	[SerializeField] private bool placedItemsAreReuseable = false;

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

        Transform t = item.transform;
        t.gameObject.layer = groundLayer;
        t.position = targetPosition[0].transform.position;
        t.rotation = targetPosition[0].transform.rotation;

		item.GetComponent<Collider>().enabled = true;

		if (placedItemsAreReuseable)
		{
            item.tag = "PickUpable";
            t.gameObject.layer = itemLayer;
            wantedItems.Add(item);
            hasTurnedIn = false;
		}			
		else
			targetPosition.RemoveAt(0);
        return true;
    }
}
