using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lowBudgetAudioManagementShit : MonoBehaviour
{
    #region SingletonSetup
    private void Awake() => Instance = this;
    public static lowBudgetAudioManagementShit Instance;
    #endregion
    public void PlayAudioClip(AudioClip clip,AudioSource audiosourc, bool loop)
    {
        audiosourc.clip = clip;
        audiosourc.loop = loop;
        audiosourc.Play();
    }

    [Header("Audiosource shit")]
    public AudioSource HurtSource;
    public AudioSource DIMCSource;

    public AudioSource FinaleSource;
    public AudioSource[] FinaleSecretSource,FinaleSecretSourcePainMode;
    [Header("Audio clip shit")]
    public AudioClip drivinginmy;
    public AudioClip[] ThroughTheFireAndFlamesClipIntro,ThroughTheFireAndFlamesClipLoop;
}
