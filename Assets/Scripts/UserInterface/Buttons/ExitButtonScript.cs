using UnityEngine;
using System.Collections;

public class ExitButtonScript : MonoBehaviour
{
	public void ExitGame()
	{
		BaldiSource.ClearQueue();
		BaldiSource.PlaySingleClip(aud_Thanks);
		Cursor.LockCursor();
		StartCoroutine(WaitForAudio(aud_Thanks.audClip.length));
	}

	private IEnumerator WaitForAudio(float time)
	{
		while (time > 0f)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		Application.Quit();
		yield break;
	}

	[Header("References")]
	[SerializeField] private CursorControllerScript Cursor;
    [SerializeField] private AudioManagerLiveReaction BaldiSource;
	[SerializeField] private AudioObjectyeah aud_Thanks;
}