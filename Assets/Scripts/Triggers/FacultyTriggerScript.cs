using UnityEngine;

public class FacultyTriggerScript : MonoBehaviour
{
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (!ps.outdoorsfr)
			{
				if (ps.door.lockTime <= 0f)
				{
				ps.ResetGuilt("faculty", 1f);
				}
			}
		}
	}

	[Header("References")]
	[SerializeField] private PlayerScript ps;
}