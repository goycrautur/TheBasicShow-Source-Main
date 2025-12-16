using UnityEngine;

public class pitTrigger : MonoBehaviour
{
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (!huh)
			{
				pitHole.Instance.switchmodeStuff();
			}
			else
            {
				pitManager.Instance.PitAudSourc.clip = pitManager.Instance.Pitpeaksound;
				pitManager.Instance.PitAudSourc.loop = true;
                pitManager.Instance.PitAudSourc.Play();
            }
		}
	}
	public bool huh;
}