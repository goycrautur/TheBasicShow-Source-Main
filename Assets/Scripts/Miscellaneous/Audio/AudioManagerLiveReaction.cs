using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AudioManagerLiveReaction : MonoBehaviour
{
    private void OnEnable()
    {
        InitialiUhh();
    }
    private void OnDestroy()
    {
        SubtitlesManagerAkaSubtitleSpawnOkSDIYBT.SubtitleTotalId = totalIds - 1;
        Array.Resize(ref SubtitlesManagerAkaSubtitleSpawnOkSDIYBT.Instance.subsObjectIdSon, totalIds - 1);
    }
    private void OnDisable()
    {
        SubtitlesManagerAkaSubtitleSpawnOkSDIYBT.SubtitleTotalId = totalIds - 1;
        Array.Resize(ref SubtitlesManagerAkaSubtitleSpawnOkSDIYBT.Instance.subsObjectIdSon, totalIds - 1);
    }
    #region Initialization
    private void Awake()
    {
        InitialiUhh();
    }
    #endregion

    #region PlaybackControl
    public void InitialiUhh()
    {
        sourceId = totalIds;
		totalIds++;
		if (totalIds >= SubtitlesManagerAkaSubtitleSpawnOkSDIYBT.SubtitleTotalId)
        {
            SubtitlesManagerAkaSubtitleSpawnOkSDIYBT.SubtitleTotalId = totalIds + 1;
            Array.Resize(ref SubtitlesManagerAkaSubtitleSpawnOkSDIYBT.Instance.subsObjectIdSon, totalIds + 1);
        }
        if (audioDevice == null) audioDevice = GetComponent<AudioSource>();

        if (PlayAtAwake && thoseWhoObject != null) PlaySingleClip(thoseWhoObject);
        SetLoop(StartingLoop);
    }
    
    public void PlaySingleClip(AudioObjectyeah obje)
    {
        if (obje != null)  
        {
            if (obje.ForcedSetAudiomixer != null) audioDevice.outputAudioMixerGroup = obje.ForcedSetAudiomixer;
            else audioDevice.outputAudioMixerGroup = GameControllerScript.Instance.MixerOverrideGlobalson[(int)obje.SoundTypeWahh != 0 ? ((int)obje.SoundTypeWahh)-1 : 0];
            audioDevice.volume = obje.volume;
            audioDevice.PlayOneShot(obje.audClip);
            if (obje.subti != null && obje.HasSubtitle) makesub(obje.subti,ForceSubPos ?SubPosForced : obje.subti.subtitleOption.twoDeePosition);
        }
       
    }
    private void Update()
    {
        if (queuedAudios.Count > 0 && !audioDevice.isPlaying) PlayNext();
    }
    public void QueueAudio(AudioObjectyeah obje)
    {
         if (obje != null)  
        {
            if (obje.ForcedSetAudiomixer != null) audioDevice.outputAudioMixerGroup = obje.ForcedSetAudiomixer;
            else audioDevice.outputAudioMixerGroup = GameControllerScript.Instance.MixerOverrideGlobalson[(int)obje.SoundTypeWahh != 0 ? ((int)obje.SoundTypeWahh)-1 : 0];
            queuedAudios.Add(obje);
            if (obje.subti != null && obje.HasSubtitle) queuedSubtitles.Add(obje.subti);
        }
    }

    public void ClearQueue(bool EndCurrent = false)
    {
        queuedAudios.Clear();
        queuedSubtitles.Clear();
        if (EndCurrent)
        {
            
            audioDevice.volume = 1;
            if (audioDevice.loop) audioDevice.loop = false;
            if (audioDevice.mute) audioDevice.mute = false;
            if (audioDevice.pitch != 1) audioDevice.pitch = 1;
            Singleton<SubtitlesManagerAkaSubtitleSpawnOkSDIYBT>.Instance.endSubtitle(sourceId);
            audioDevice.Stop();
        }
    }

    private void PlayNext()
    {
        if (queuedAudios[0] != null) 
        {
            audioDevice.volume = queuedAudios[0].volume;
            audioDevice.clip = queuedAudios[0].audClip;
            audioDevice.Play();
            queuedAudios.RemoveAt(0);
        }
        if (queuedSubtitles[0] != null) 
        {
            makesub(queuedSubtitles[0],ForceSubPos ?SubPosForced : queuedSubtitles[0].subtitleOption.twoDeePosition);
            queuedSubtitles.RemoveAt(0);
        }
        
    }
    #endregion

    #region uhh
    public void makesub(subsScriptableObject sub, Vector2 hi)
    {
        Singleton<SubtitlesManagerAkaSubtitleSpawnOkSDIYBT>.Instance.summonLeSubtitle(sub.subtitleOption, sourceId, sub, audioDevice, hi);
    }
    #endregion

    #region AudioEffects
    public IEnumerator FadeOut(float duration)
    {
        float startVolume = audioDevice.volume;
        while (audioDevice.volume > 0)
        {
            audioDevice.volume -= startVolume * Time.unscaledDeltaTime / duration;
            yield return null;
        }
        yield break;
    }
    public IEnumerator FadeIn(float duration, float vol)
    {
        while (audioDevice.volume < vol)
        {
            audioDevice.volume += Time.unscaledDeltaTime / duration;
            yield return null;
        }
        audioDevice.volume = vol;
        yield break;
    }
    public void SetIgnoreListenerPause(bool toggle)
    {
        audioDevice.ignoreListenerPause = toggle;
    }
    public void SetMute(bool toggle)
    {
        audioDevice.mute = toggle;
    }
    public void SetPitch(float pitch)
    {
        audioDevice.pitch = pitch;
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

    public bool PlayAtAwake,StartingLoop;
    public AudioObjectyeah thoseWhoObject;
    public bool ForceSubPos;
    public Vector3 SubPosForced;
    public int sourceId;
	public static int totalIds;
    public AudioSource audioDevice;
    public List<AudioObjectyeah> queuedAudios = new List<AudioObjectyeah>();
    public List<subsScriptableObject> queuedSubtitles = new List<subsScriptableObject>();
    #endregion
}