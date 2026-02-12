using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lowBudgetAudioManagementShit : MonoBehaviour
{
    #region SingletonSetup
    private void Awake() => Instance = this;
    public static lowBudgetAudioManagementShit Instance;
    #endregion

    [Header("Audiosource shit")]
    public AudioSource HurtSource;
    public AudioSource FinaleSource;
    public AudioSource[] FinaleSecretSource,FinaleSecretSourcePainMode;
    [Header("Audio clip shit")]
    public AudioClip[] ThroughTheFireAndFlamesClipIntro;
    public AudioClip[] ThroughTheFireAndFlamesClipLoop;
}
