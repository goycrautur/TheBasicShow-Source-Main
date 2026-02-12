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
        if (lifetime != 0);
        {
            ToggleAudioSourcesInRange(true);
        }
        if (lifetime.CountdownWithDeltaTime() == 0)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Trigger Handlers
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<AudioSource>() != null)
        {
            ToggleAudioSource(other.GetComponent<AudioSource>(), true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<AudioSource>() != null)
        {
            ToggleAudioSource(other.GetComponent<AudioSource>(), false);
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
            ToggleAudioSource(collider.GetComponent<AudioSource>(), mute);
        }
    }

    private void ToggleAudioSource(AudioSource source, bool mute)
    {
        if (source == null) return;

        bool isCurrentlyMuted = disabledAudioSources.Contains(source);
        if (isCurrentlyMuted != mute)
        {
            source.mute = mute;

            if (mute)
            {
                disabledAudioSources.Add(source);
            }
            else
            {
                disabledAudioSources.Remove(source);
            }
        }
    }
    #endregion

    #region Serialized Configuration
    [Header("Lifetime Settings")]
    [SerializeField] private float lifetime = 300f;

    [Header("Audio Management")]
    [SerializeField] private List<AudioSource> disabledAudioSources = new List<AudioSource>();
    #endregion
}