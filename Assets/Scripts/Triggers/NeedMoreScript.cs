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
	private void OnTriggerExit(Collider other)
	{
		if (gc.StoryPreSpoop & other.CompareTag("Player") & gc.notebooks >= gc.UnlockAmount)
		{
			Debug.Log("it have started");
			if (!LearningGameManager.Instance.Tutor.IsLolbit && gc.failedNotebooks <= 1) LearningGameManager.Instance.Tutor.PlayCountdownheh();
			else if (gc.failedNotebooks == 2) gc.StorySpoop();
			gc.StoryPreSpoop = false;
			return;
		}
	}

	[Header("Game Controller")]
	[SerializeField] private GameControllerScript gc;

	[Header("Audio")]
	[SerializeField] private AudioManagerLiveReaction audioDevice;
    [SerializeField] private AudioObjectyeah baldiDoor;
}