using UnityEngine;

public class NearElevatorTriggerScript : MonoBehaviour
{
	public void closeExitStuff()
	{
		GameControllerScript.Instance.ExitReached(EntranceID);
		elvDoor.Close();
		gameObject.SetActive(false);
		exitClosed = true;
	}
	private void OnTriggerStay(Collider other)
	{
		if (GameControllerScript.Instance.exitsReached < GameControllerScript.Instance.maxExits - 1 && other.CompareTag("Player"))
		{
			if (GameControllerScript.Instance.finaleMode || GameControllerScript.Instance.mode == "LappingOfAsylum" && GameControllerScript.Instance.LapManag.allowClosElev)
			{
				closeExitStuff();
				if (GameControllerScript.Instance.baldiScrpt.isActiveAndEnabled)
				{
					GameControllerScript.Instance.baldiScrpt.Hear(transform.position, 8f);
				}
				if (GameControllerScript.Instance.famishScrpt.isActiveAndEnabled)
				{
					GameControllerScript.Instance.famishScrpt.Hear(transform.position, 8f);
				}
				if (GameControllerScript.Instance.zerulscrpt.isActiveAndEnabled)
				{
					GameControllerScript.Instance.zerulscrpt.Hear(transform.position, 8f);
				}
				if (GameControllerScript.Instance.muchoing.isActiveAndEnabled)
				{
					GameControllerScript.Instance.muchoing.Hear(transform.position, 8f);
				}
			}
		}
	}

	[Header("References")]
	public ElvDoorScript elvDoor;
	public int EntranceID;
	public bool exitClosed;
}