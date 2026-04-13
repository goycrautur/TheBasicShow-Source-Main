using UnityEngine;

public class NeedMoreScript : MonoBehaviour
{
	private void OnTriggerEnter(Collider other) 
	{
		if (gc.notebooks < gc.UnlockAmount & other.CompareTag("Player")) 
		{
			if (!audioDevice.audioDevice.isPlaying) 
			{
				audioDevice.ClearQueue(true);
				if (baldiDoor !=null) audioDevice.PlaySingleClip(baldiDoor);
			}
		}
	}

	[Header("Game Controller")]
	[SerializeField] private GameControllerScript gc;

	[Header("Audio")]
	[SerializeField] private AudioManagerLiveReaction audioDevice;
    [SerializeField] private AudioObjectyeah baldiDoor;
}