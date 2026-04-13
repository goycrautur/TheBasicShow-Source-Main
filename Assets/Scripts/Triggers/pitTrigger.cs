using UnityEngine;

public class pitTrigger : MonoBehaviour
{
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (!huh) pitHole.Instance.switchmodeStuff();
			else
            {
				pitManager.Instance.PitAudSourc.ClearQueue(true);
				pitManager.Instance.PitAudSourc.SetLoop(true);
				pitManager.Instance.PitAudSourc.QueueAudio(pitManager.Instance.Pitpeaksound);
            }
		}
	}
	public bool huh;
}