using UnityEngine;
using System.Collections.Generic;

public class LifetimeScript : MonoBehaviour
{
    #region Initialization & Cleanup
    private void Start()
    {
        Physics.IgnoreCollision(GetComponent<BoxCollider>(), GameControllerScript.Instance.player.cc, true);
        ToggleAudioSourcesInRange(true);
    }
    private void OnDestroy()
    {
        ToggleAudioSourcesInRange(false);
        disabledAudioSources.Clear();
    }
    #endregion

    #region Per-Frame Logic
    private void Update()
    {
        if (lifetime != 0); ToggleAudioSourcesInRange(true);
        if (lifetime.CountdownWithDeltaTime() == 0) Destroy(gameObject);
    }
    #endregion

    #region Trigger Handlers
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<AudioManagerLiveReaction>() != null) 
        {
            ToggleAudioSourceManager(other.GetComponent<AudioManagerLiveReaction>(), true);
            //ToggleAudioSourceManager(other.GetComponentInChildren<AudioManagerLiveReaction>(), true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<AudioManagerLiveReaction>() != null)
        {
            ToggleAudioSourceManager(other.GetComponent<AudioManagerLiveReaction>(), false);
            //ToggleAudioSourceManager(other.GetComponentInChildren<AudioManagerLiveReaction>(), false);
        }
    }
    #endregion

    #region Audio Management
    private void ToggleAudioSourcesInRange(bool mute)
    {
        Vector3 range = new Vector3(10*transform.localScale.x, 1, 10*transform.localScale.z);
        var colliders = Physics.OverlapBox(transform.position, range, Quaternion.identity);
        foreach (var collider in colliders) 
        {
            ToggleAudioSourceManager(collider.GetComponent<AudioManagerLiveReaction>(), mute);
            //ToggleAudioSourceManager(collider.GetComponentInChildren<AudioManagerLiveReaction>(), mute);
        }
    }

    private void ToggleAudioSourceManager(AudioManagerLiveReaction source, bool mute)
    {
        if (source == null) return;
        bool isCurrentlyMuted = disabledAudioManagers.Contains(source);
        if (isCurrentlyMuted != mute)
        {
            source.SetMute(mute);
            if (mute) disabledAudioManagers.Add(source);
            else disabledAudioManagers.Remove(source);
        }
    }
    #endregion

    #region Serialized Configuration
    [Header("Lifetime Settings")]
    [SerializeField] private float lifetime = 300f;

    [Header("Audio Management")]
    [SerializeField] private List<AudioSource> disabledAudioSources = new List<AudioSource>();
    [SerializeField] private List<AudioManagerLiveReaction> disabledAudioManagers = new List<AudioManagerLiveReaction>();
    #endregion
}