using System;
using UnityEngine;

public class ShapedLockUnlocker : MonoBehaviour
{

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player")) //If it is a player
		{
			locker.OpenDoor();
		}
	}

	public ShapeLockerScript locker;
}
