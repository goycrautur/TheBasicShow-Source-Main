using UnityEngine;

public class AmbienceScript : MonoBehaviour
{
    #region AmbiencePlaybackLogic
    public void PlayAudio()
    {
        int num = Mathf.RoundToInt(Random.Range(0f, 49f)); 
        if (!audioDevice.audioDevice.isPlaying & num == 0)
        {
            transform.position = aiLocation.position;
            int num2 = Mathf.RoundToInt(Random.Range(0f, sounds.Length - 1));
            audioDevice.PlaySingleClip(sounds[num2]);
        }
    }
    #endregion

    #region SerializedConfiguration
    [Header("Audio Settings")]
    [SerializeField] private AudioObjectyeah[] sounds;
    [SerializeField] private AudioManagerLiveReaction audioDevice;

    [Header("Location Settings")]
	[SerializeField] private Transform aiLocation;
    #endregion
}