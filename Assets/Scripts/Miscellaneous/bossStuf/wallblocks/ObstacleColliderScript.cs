using UnityEngine;

public class ObstacleColliderScript : MonoBehaviour
{
	[SerializeField] private BlockageScript obstacle;
	
	[SerializeField] private int id;
	
    private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !obstacle.active)
		{
			obstacle.colliderId = id;
		}
	}
}
