using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAndTip_Positioning : MonoBehaviour
{
    [SerializeField] private Vector3 tipPosition;
    public Vector3 GetTipPosition() { return tipPosition; }

	[SerializeField] private Vector3 itemPosition;
	public Vector3 GetItemPosition() { return itemPosition; }

	[SerializeField] private Vector3 itemRotation;
	public Vector3 GetItemRotation() { return itemRotation; }

	[SerializeField] private bool placeInHand = false;
	public bool GetPlaceObjectInHand() { return placeInHand; }

}
