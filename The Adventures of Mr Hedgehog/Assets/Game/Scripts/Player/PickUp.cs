using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class PickUp : MonoBehaviour
{
    public static PickUp Instance { get; private set; }

	[SerializeField] private Transform rightHand;

    [SerializeField] private GameObject tipCanvas_LeftClick;
    [SerializeField] private Transform playerCamera;

    [SerializeField] private float tipCanvasDistanceAboveTarget = 1;
    [SerializeField] private float defaultItemDistanceFromPlayer_Y = 0.25f, defaultItemDistanceFromPlayer_Z = 1;

    private List<GameObject> nearbyObjects = new List<GameObject>();
    private GameObject currentlyHoldingObject, closestObject;

    private Vector3 previousPlayerPosition;
    private bool pickUpAndDropReady = true;
    private GameObject player;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        player = transform.parent.gameObject;
    }

    private void Update()
    {
        if (nearbyObjects.Count < 1)
            HideTipCanvas();         
        else if (nearbyObjects.Count > 1 && previousPlayerPosition != player.transform.position)
            closestObject = FindNearestObject();

        if (Input.GetMouseButtonDown(0) && pickUpAndDropReady && nearbyObjects.Count > 0)
            PickUpObject(FindNearestObject());                

        previousPlayerPosition = player.transform.position;

        if (tipCanvas_LeftClick.activeSelf)
        {
            tipCanvas_LeftClick.transform.LookAt(playerCamera.transform);
            if (closestObject != null)
            {
				// Hämtar objektes egna inställning för positionen av vänsterklick-tipset. Om ingen inställning hittas anges ett default värde.
				ItemAndTip_Positioning pos = closestObject.GetComponent<ItemAndTip_Positioning>() ?? null;
                tipCanvas_LeftClick.transform.position = closestObject.transform.position + ((pos != null) ? pos.GetTipPosition() : new Vector3(0f, 1f, 0f));
            }                
            else
                tipCanvas_LeftClick.SetActive(false);
        }                  
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(1) && pickUpAndDropReady && !DialogueManager.Instance.GetTalking())
        {
            DropObject(false);
            closestObject = FindNearestObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUpable") && !nearbyObjects.Contains(other.gameObject) && other.gameObject != currentlyHoldingObject)
        {
            nearbyObjects.Add(other.gameObject);
            closestObject = FindNearestObject();
        }           
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PickUpable") && nearbyObjects.Contains(other.gameObject))
        {
            nearbyObjects.Remove(other.gameObject);
            if (nearbyObjects.Count > 0)
                closestObject = FindNearestObject();
            else
                HideTipCanvas();
        }     
    }    

    private bool PickUpObject(GameObject objectToPickUp)
    {
        if (objectToPickUp != null)
        {
            DropObject(true);
            currentlyHoldingObject = objectToPickUp;
            
            objectToPickUp.tag = "Untagged";
            nearbyObjects.Remove(objectToPickUp);
            closestObject = FindNearestObject();

            Rigidbody rb = objectToPickUp.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeAll;

			currentlyHoldingObject.GetComponent<Collider>().enabled = false;

			Transform t = objectToPickUp.transform;
			ItemAndTip_Positioning pos = t.gameObject.GetComponent<ItemAndTip_Positioning>() ?? null;
			//t.SetParent(pos != null ? rightHand : player.transform);	Objektet fryser i luften med denna?
			t.SetParent(player.transform);
            StartCoroutine(LerpObject(t, pos));

			ToggleGlitterEffect(t, false);
			return true;            
        }
        return false;      
    }

    [SerializeField] private float pickUpSpeed = 1.5f;
    IEnumerator LerpObject(Transform t, ItemAndTip_Positioning pos)
    {
        pickUpAndDropReady = false;
        bool lerping = true;

        Vector3 startPosition = t.localPosition;

		Vector3 endPosition = (pos != null) ? pos.GetItemPosition() : new Vector3(0, defaultItemDistanceFromPlayer_Y, defaultItemDistanceFromPlayer_Z);		
        Quaternion startRotation = t.localRotation;

        float journeyLength = Vector3.Distance(startPosition, endPosition);
        float startTime = Time.time;
        float distCovered;
        float fracJourney;        

        while (lerping)
        {
            yield return new WaitForFixedUpdate();
            distCovered = (Time.time - startTime) * pickUpSpeed;
            fracJourney = distCovered / journeyLength;
            t.localPosition = Vector3.Lerp(startPosition, endPosition, fracJourney);
            t.localRotation = Quaternion.Lerp(startRotation, Quaternion.identity, fracJourney);

            if (fracJourney > 0.99f)
                lerping = false;            
        }
		if (pos != null && pos.GetPlaceInHand())
			t.SetParent(rightHand);
        pickUpAndDropReady = true;
    }

    [SerializeField] private float dropForce;
    public GameObject DropObject(bool withForce)
    {
        GameObject returnValue = null;

        if (currentlyHoldingObject == null)
            return returnValue;

        currentlyHoldingObject.transform.SetParent(null);

        Rigidbody rb = currentlyHoldingObject.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        rb.isKinematic = false;

        if (withForce)
            rb.AddRelativeForce(Vector3.forward * dropForce, ForceMode.Impulse);
        else
            rb.AddRelativeForce(Vector3.forward * 0.3f, ForceMode.Impulse); // För att välta objektet.

		if (!nearbyObjects.Contains(currentlyHoldingObject))
            nearbyObjects.Add(currentlyHoldingObject);

        ToggleGlitterEffect(currentlyHoldingObject.transform, true);
        currentlyHoldingObject.tag = "PickUpable";
		currentlyHoldingObject.GetComponent<Collider>().enabled = true;
		returnValue = currentlyHoldingObject;
        currentlyHoldingObject = null;        
        return returnValue;
    }

	public bool ConfirmItemGiven(bool received)
	{
		if (received)
		{
			if (currentlyHoldingObject != null)
			{
				currentlyHoldingObject.transform.SetParent(null);
				Physics.IgnoreCollision(currentlyHoldingObject.GetComponent<Collider>(), player.GetComponentInParent<CapsuleCollider>(), false);
				currentlyHoldingObject = null;
			}						
		}
		return received;
	}

    private GameObject FindNearestObject()
    {
		Vector3 currentPosition = player.transform.position;		
		GameObject returnValue = null;

		if (nearbyObjects.Count > 0)
			if (nearbyObjects.Count == 1)
				returnValue = nearbyObjects.First((obj) => obj is GameObject);
			else
				returnValue = nearbyObjects.Aggregate((closestObj, next) =>
								 (closestObj.transform.position - currentPosition).sqrMagnitude <
								 (next.transform.position - currentPosition).sqrMagnitude ? closestObj : next);			
		ShowTipCanvas(returnValue);
		return returnValue;
	}

	private bool ShowTipCanvas(GameObject position)
    {
        if (position == null)
        {
            HideTipCanvas();
            return false;
        }
        tipCanvas_LeftClick.transform.SetPositionAndRotation(position.transform.position + new Vector3(0, tipCanvasDistanceAboveTarget, 0), Quaternion.identity);
        tipCanvas_LeftClick.SetActive(true);
        return true;
    }

    private void HideTipCanvas()
    {
        tipCanvas_LeftClick.SetActive(false);
    }    

    public GameObject GetCurrentObject()
    {
        return currentlyHoldingObject;
    }

    public void RemoveGivenItem(GameObject item)
    {
        if (item != null && nearbyObjects.Contains(item))
        {
            nearbyObjects.Remove(item);
            closestObject = FindNearestObject();
        }
    }

    private void ToggleGlitterEffect(Transform t, bool active)
    {
        Transform glitter = t.Find("Glitter") ?? null;
        if (glitter != null)
            glitter.gameObject.SetActive(active);
    }
}
