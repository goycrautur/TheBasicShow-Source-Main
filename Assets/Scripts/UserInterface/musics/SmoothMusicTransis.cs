using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMusicTransis : MonoBehaviour
{
    public void SmoothlyPlayUHHHH(AudioObjectyeah toClip, float duration)
    {
        musTagger++;
        if (musTagger == 2)
        {
            StartCoroutine(lowtaperfade(Sound2, Sound1, toClip,duration));
            musTagger = 0;
        }
        else StartCoroutine(lowtaperfade(Sound1, Sound2, toClip,duration));
    }
    private IEnumerator lowtaperfade(AudioManagerLiveReaction fromSource, AudioManagerLiveReaction toSource, AudioObjectyeah toClip, float duration) 
	{
		toSource.QueueAudio(toClip);
        toSource.SetLoop(true);
		toSource.SetVolume(0f);
		float elapsed = 0f;
        float dura = duration;
        Debug.Log($"duration: {duration}");
		while (elapsed < dura)
		{
			elapsed += Time.deltaTime;
			float t = elapsed / dura;
			fromSource.SetVolume(Mathf.Lerp(toClip.volume, 0f, t));
			toSource.SetVolume(Mathf.Lerp(0f, toClip.volume, t));
            //Debug.Log($"volum of fromSource : {fromSource.audioDevice.volume}");
            //Debug.Log($"volum of toSource : {toSource.audioDevice.volume}");
			yield return null;
		}
        fromSource.ClearQueue(true);
		fromSource.SetVolume(toClip.volume);  //volumen
        yield break;
	}
    public int musTagger;
    public AudioManagerLiveReaction Sound1,Sound2;
}
