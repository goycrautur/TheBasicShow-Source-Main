using UnityEngine;
using System.Collections.Generic;
using System.Security.Principal;

public class LifetimeScript : MonoBehaviour
{
    #region Initialization & Cleanup
    private void Start()
    {
        Physics.IgnoreCollision(GetComponent<BoxCollider>(), GameControllerScript.Instance.player.cc, true);
        ToggleShitsInRange(true);
    }
    private void OnDestroy()
    {
        ToggleShitsInRange(false);
        disabledAudioManagers.Clear();
        OkayDude.Clear();
    }
    #endregion

    #region Per-Frame Logic
    private void Update()
    {
        if (lifetime != 0); ToggleShitsInRange(true);
        if (lifetime.CountdownWithDeltaTime() == 0) Destroy(gameObject);
    }
    #endregion

    #region Trigger Handlers
    private void OnTriggerStay(Collider other)
    {//Fuck you sadcolored
        AudioManagerLiveReaction AudioToAdd = other.GetComponentInChildren<AudioManagerLiveReaction>();
        if (AudioToAdd != null) ToggleAudioSourceManager(AudioToAdd, true);
        NPC dude = other.GetComponentInChildren<NPC>();
        if (dude != null) ToggleNpcAudioMute(dude, true);
    }
    private void OnTriggerExit(Collider other)
    {
        AudioManagerLiveReaction AudioToAdd = other.GetComponentInChildren<AudioManagerLiveReaction>();
        if (AudioToAdd != null) ToggleAudioSourceManager(AudioToAdd, false);
        NPC dude = other.GetComponentInChildren<NPC>();
        if (dude != null) ToggleNpcAudioMute(dude, false);
    }
    #endregion

    #region Audio Management
    private void ToggleShitsInRange(bool mute)
    {
        Vector3 range = new Vector3(10*transform.localScale.x, 1, 10*transform.localScale.z);
        var colliders = Physics.OverlapBox(transform.position, range, Quaternion.identity);
        foreach (var collider in colliders) 
        {
            ToggleAudioSourceManager(collider.GetComponentInChildren<AudioManagerLiveReaction>(), mute);
            ToggleNpcAudioMute(collider.GetComponentInChildren<NPC>(), mute);
            //ToggleAudioSourceManager(collider.GetComponentInChildren<AudioManagerLiveReaction>(), mute);
        }
    }

    private void ToggleAudioSourceManager(AudioManagerLiveReaction source, bool mute)
    {
        if (source == null) return;
        bool isCurrentlyMuted = disabledAudioManagers.Contains(source);
        if (isCurrentlyMuted != mute)
        {
            source.ForcedMute = mute;
            if (mute) disabledAudioManagers.Add(source);
            else disabledAudioManagers.Remove(source);
        }
    }
    private void ToggleNpcAudioMute(NPC source, bool mute)
    {
        if (source == null) return;
        bool isCurrentlyMuted = OkayDude.Contains(source);
        if (isCurrentlyMuted != mute)
        {
            source.MuteAllAudioMan = mute;
            if (mute) OkayDude.Add(source);
            else OkayDude.Remove(source);
        }
    }
    #endregion

    #region Serialized Configuration
    [Header("Lifetime Settings")]
    [SerializeField] private float lifetime = 300f;

    [Header("Audio Management")]
    [SerializeField] private List<AudioManagerLiveReaction> disabledAudioManagers = new List<AudioManagerLiveReaction>();
    [SerializeField] private List<NPC> OkayDude = new List<NPC>();
    #endregion
}