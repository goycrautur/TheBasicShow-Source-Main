using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioQueueScript : MonoBehaviour
{
    #region Initialization
    private void Awake()
    {
        if (audioDevice == null)
        {
            audioDevice = GetComponent<AudioSource>();
        }
    }
    #endregion

    #region PlaybackControl
    private void Update()
    {
        if (queuedAudios.Count > 0 && !audioDevice.isPlaying)
        {
            if (baldiFeed && GameControllerScript.Instance.spoopMode)
            {
                return;
            }
            else if (!baldiFeed && Time.timeScale == 0)
            {
                return;
            }
            PlayNext();
        }
    }
    public void QueueAudio(AudioClip sound, subsScriptableObject subtitlesObje)
    {
        if (sound != null)
        {
            queuedAudios.Add(sound);
        }
        if (subtitlesObje != null)
        {
            queuedSubtitles.Add(subtitlesObje);
        }
    }

    public void ClearQueue()
    {
        queuedAudios.Clear();
        queuedSubtitles.Clear();
        audioDevice.Stop();
    }

    private void PlayNext()
    {
        audioDevice.PlayOneShot(queuedAudios[0]);
        GameControllerScript.Instance.SubsManager.summonLeSubtitle(queuedSubtitles[0].subtitleOption, queuedSubtitles[0], 0f, audioDevice);
        queuedSubtitles.RemoveAt(0);
        queuedAudios.RemoveAt(0);
    }
    #endregion

    #region QueueManagement
    
    #endregion

    #region AudioEffects
    public IEnumerator FadeOut(AudioSource audioDevice, float duration)
    {
        float startVolume = audioDevice.volume;
        while (audioDevice.volume > 0)
        {
            audioDevice.volume -= startVolume * Time.unscaledDeltaTime / duration;
            yield return null;
        }
        audioDevice.Stop();
        audioDevice.volume = startVolume;
        yield break;
    }

    public void SetLoop(bool toggle)
    {
        audioDevice.loop = toggle;
        if (queuedAudios.Count == 0 && toggle)
        {
            audioDevice.loop = true;
            return;
        }
        audioDevice.loop = false;
    }
    #endregion

    #region References
    public AudioSource audioDevice;
    public List<AudioClip> queuedAudios = new List<AudioClip>();
    public List<subsScriptableObject> queuedSubtitles = new List<subsScriptableObject>();
    [SerializeField] private bool baldiFeed;
    #endregion
}