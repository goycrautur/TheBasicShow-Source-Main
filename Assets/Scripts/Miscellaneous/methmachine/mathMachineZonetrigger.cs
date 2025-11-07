using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mathMachineZonetrigger : MonoBehaviour
{
	private void Start()
	{
		hitBox = base.GetComponent<BoxCollider>();
		foreach (NumBallScript numBallScript in Object.FindObjectsOfType<NumBallScript>())
		{
			if (this.IsInsideBoxCollider(numBallScript.transform))
			{
				numBallScript.RoomID = RoomID;
			}
		}
	}

	private void LateUpdate()
	{
		foreach (NumBallScript numBallScript in Object.FindObjectsOfType<NumBallScript>())
		{
            if (!this.IsInsideBoxCollider(numBallScript.transform) && numBallScript.RoomID == RoomID)
            {
                numBallScript.DownBall();
                numBallScript.die();
			}
		}
	}

	private bool IsInsideBoxCollider(Transform Object)
	{
		Vector3 position = Object.transform.position;
		return hitBox.bounds.Contains(position);
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			PlayerInTheRoom = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			PlayerInTheRoom = false;
		}
	}
    [SerializeField] private BoxCollider hitBox;
	public bool PlayerInTheRoom;
	public int RoomID;
}
