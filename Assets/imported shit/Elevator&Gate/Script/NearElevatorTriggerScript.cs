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
				GameControllerScript.Instance.HearingShit(8f, this.transform, new Vector3(0f,0f,0f), "all",false);
			}
		}
	}

	[Header("References")]
	public ElvDoorScript elvDoor;
	public int EntranceID;
	public bool exitClosed;
}