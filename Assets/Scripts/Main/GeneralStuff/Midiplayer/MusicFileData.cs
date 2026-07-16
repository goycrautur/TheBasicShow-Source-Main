using UnityEngine;
using UnityEngine.Audio;

public class MusicFileData
{
	public AudioClip clip;

	public AudioMixerGroup mixer;

	public MusicFileData(AudioClip clip, AudioMixerGroup mixer)
	{
		this.clip = clip;
		this.mixer = mixer;
	}
}
