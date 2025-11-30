using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
public class SubtitlesManagerAkaSubtitleSpawnOkSDIYBT : MonoBehaviour //atp this is just yuri script with semi decent ammount of modification kms
	{
		public void summonLeSubtitle(subtitlingIt subtitle, subsScriptableObject subscriptobj,float trueDuration, AudioSource audiSourc)
		{
			subtitlesScriptReal component = Instantiate<GameObject>(subtitlePrefab, subtitleCanvas.transform.position, Quaternion.identity, subtitleCanvas.transform).GetComponent<subtitlesScriptReal>();
			component.producerAud = audiSourc;
            component.subtitlys = subtitle;
            component.audiObject = subscriptobj;
			component.infinite = audiSourc.loop;
			component.is3d = true;
			component.realduration = trueDuration;
			component.cameraTransf = cameraTransorm;
			component.aspectRatio = 1f;
			if (!component.is3d)
        	{
        	component.bg.localScale = new Vector3(1f, 1f, 1f);
        	component.bg.anchoredPosition = component.fixesPosition;
        	}
        	if (component.is3d)
        	{
            	component.bg.anchoredPosition = new Vector3(0f, -266.66f / component.aspectRatio, 0f);
        	}
			component.updateSubPostion();
		}
		public void summonLeSubtitle2D(subtitlingIt subtitle, subsScriptableObject subscriptobj,float trueDuration, Vector3 position, AudioSource audiSourc)
		{
			subtitlesScriptReal component = Instantiate<GameObject>(subtitlePrefab, subtitleCanvas.transform.position, Quaternion.identity, subtitleCanvas.transform).GetComponent<subtitlesScriptReal>();
            component.producerAud = audiSourc;
            component.subtitlys = subtitle;
            component.audiObject = subscriptobj;
			component.infinite = audiSourc.loop;
			component.is3d = false;
			component.fixesPosition = position;
			component.realduration = trueDuration;
			component.cameraTransf = cameraTransorm;
			component.aspectRatio = 1f;
			if (!component.is3d)
        	{
        	component.bg.localScale = new Vector3(1f, 1f, 1f);
        	component.bg.anchoredPosition = component.fixesPosition;
        	}
        	if (component.is3d)
        	{
            	component.bg.anchoredPosition = new Vector3(0f, -266.66f / component.aspectRatio, 0f);
        	}
			component.updateSubPostion();
		}
		public void summonLeSingleSubtitle(subtitlingIt subtitle, subsScriptableObject subscriptobj,float trueDuration, AudioSource audiSourc)
		{
			subtitlesScriptReal component = Instantiate<GameObject>(subtitlePrefab, subtitleCanvas.transform.position, Quaternion.identity, subtitleCanvas.transform).GetComponent<subtitlesScriptReal>();
            component.producerAud = audiSourc;
            component.subtitlys = subtitle;
            component.audiObject = subscriptobj;
			component.infinite = audiSourc.loop;
			component.is3d = true;
			component.realduration = trueDuration;
			component.cameraTransf = cameraTransorm;
			component.aspectRatio = 1f;
			if (!component.is3d)
        	{
        	component.bg.localScale = new Vector3(1f, 1f, 1f);
        	component.bg.anchoredPosition = component.fixesPosition;
        	}
        	if (component.is3d)
        	{
            	component.bg.anchoredPosition = new Vector3(0f, -266.66f / component.aspectRatio, 0f);
        	}
			component.updateSubPostion();
		}
		public void summonLeSingleSubtitle2D(subtitlingIt subtitle, subsScriptableObject subscriptobj,float trueDuration, Vector3 position, AudioSource audiSourc)
		{
			subtitlesScriptReal component = Instantiate<GameObject>(subtitlePrefab, subtitleCanvas.transform.position, Quaternion.identity, subtitleCanvas.transform).GetComponent<subtitlesScriptReal>();
			component.producerAud = audiSourc;
            component.subtitlys = subtitle;
            component.audiObject = subscriptobj;
			component.infinite = audiSourc.loop;
			component.is3d = false;
			component.fixesPosition = position;
			component.realduration = trueDuration;
			component.cameraTransf = cameraTransorm;
			component.aspectRatio = 1f;
			if (!component.is3d)
        	{
        	component.bg.localScale = new Vector3(1f, 1f, 1f);
        	component.bg.anchoredPosition = component.fixesPosition;
        	}
        	if (component.is3d)
        	{
            	component.bg.anchoredPosition = new Vector3(0f, -266.66f / component.aspectRatio, 0f);
        	}
			component.updateSubPostion();
		}

    [SerializeField]
	public Transform cameraTransorm;
    public GameObject subtitlePrefab;
    public Canvas subtitleCanvas;
	}
