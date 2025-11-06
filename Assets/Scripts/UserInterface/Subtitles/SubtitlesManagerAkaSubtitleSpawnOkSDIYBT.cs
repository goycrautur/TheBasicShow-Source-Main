using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
public class SubtitlesManagerAkaSubtitleSpawnOkSDIYBT : MonoBehaviour //atp this is just yuri script with semi decent ammount of modification kms
	{
		public subtitlesScriptReal summonLeSubtitle(subtitlingIt subtitle, subsScriptableObject subscriptobj,float trueDuration, AudioSource audiSourc)
		{
			if (!subtitle.enabled)
			{
				return null;
			}
			subtitlesScriptReal result;
			if (subtitleCanvas != null)
			{
				subtitlesScriptReal component = Instantiate<GameObject>(subtitlePrefab, subtitleCanvas.transform.position, Quaternion.identity, subtitleCanvas.transform).GetComponent<subtitlesScriptReal>();
				component.producerAud = audiSourc;
                component.subtitlys = subtitle;
                component.audiObject = subscriptobj;
				component.infinite = audiSourc.loop;
				component.is3d = true;
				component.realduration = trueDuration;
				result = component;
			}
			else
			{
				subtitlesScriptReal component2 = Instantiate<GameObject>(subtitlePrefab, base.transform.position, Quaternion.identity, base.transform).GetComponent<subtitlesScriptReal>();
                component2.producerAud = audiSourc;
                component2.subtitlys = subtitle;
                component2.audiObject = subscriptobj;
				component2.infinite = audiSourc.loop;
				component2.is3d = true;
				component2.realduration = trueDuration;
				result = component2;
			}
			return result;
		}
		public subtitlesScriptReal summonLeSubtitle2D(subtitlingIt subtitle, subsScriptableObject subscriptobj,float trueDuration, Vector3 position, AudioSource audiSourc)
		{
			if (!subtitle.enabled)
			{
				return null;
			}
			subtitlesScriptReal result;
			if (subtitleCanvas != null)
			{
				subtitlesScriptReal component = Instantiate<GameObject>(subtitlePrefab, subtitleCanvas.transform.position, Quaternion.identity, subtitleCanvas.transform).GetComponent<subtitlesScriptReal>();
               	component.producerAud = audiSourc;
                component.subtitlys = subtitle;
                component.audiObject = subscriptobj;
				component.infinite = audiSourc.loop;
				component.is3d = false;
				component.fixesPosition = position;
				component.realduration = trueDuration;
				result = component;
			}
			else
			{
				subtitlesScriptReal component2 = Instantiate<GameObject>(subtitlePrefab, base.transform.position, Quaternion.identity, base.transform).GetComponent<subtitlesScriptReal>();
                component2.producerAud = audiSourc;
                component2.subtitlys = subtitle;
                component2.audiObject = subscriptobj;
				component2.infinite = audiSourc.loop;
				component2.is3d = false;
				component2.fixesPosition = position;
				component2.realduration = trueDuration;
				result = component2;
			}
			return result;
		}
		public subtitlesScriptReal summonLeSingleSubtitle(subtitlingIt subtitle, subsScriptableObject subscriptobj,float trueDuration, AudioSource audiSourc)
		{
			if (!subtitle.enabled)
			{
				return null;
			}
			subtitlesScriptReal result;
			if (subtitleCanvas != null)
			{
				subtitlesScriptReal component = Instantiate<GameObject>(subtitlePrefab, subtitleCanvas.transform.position, Quaternion.identity, subtitleCanvas.transform).GetComponent<subtitlesScriptReal>();
                component.producerAud = audiSourc;
                component.subtitlys = subtitle;
                component.audiObject = subscriptobj;
				component.infinite = audiSourc.loop;
				component.is3d = true;
				component.realduration = trueDuration;
				result = component;
			}
			else
			{
				subtitlesScriptReal component2 = Instantiate<GameObject>(subtitlePrefab, base.transform.position, Quaternion.identity, base.transform).GetComponent<subtitlesScriptReal>();
                component2.producerAud = audiSourc;
                component2.subtitlys = subtitle;
                component2.audiObject = subscriptobj;
				component2.infinite = audiSourc.loop;
				component2.is3d = true;
				component2.realduration = trueDuration;
				result = component2;
			}
			return result;
		}
		public subtitlesScriptReal summonLeSingleSubtitle2D(subtitlingIt subtitle, subsScriptableObject subscriptobj,float trueDuration, Vector3 position, AudioSource audiSourc)
		{
			if (!subtitle.enabled)
			{
				return null;
			}
			subtitlesScriptReal result;
			if (subtitleCanvas != null)
			{
				subtitlesScriptReal component = Instantiate<GameObject>(subtitlePrefab, subtitleCanvas.transform.position, Quaternion.identity, subtitleCanvas.transform).GetComponent<subtitlesScriptReal>();
				component.producerAud = audiSourc;
                component.subtitlys = subtitle;
                component.audiObject = subscriptobj;
				component.infinite = audiSourc.loop;
				component.is3d = false;
				component.fixesPosition = position;
				component.realduration = trueDuration;
				result = component;
			}
			else
			{
				subtitlesScriptReal component2 = Instantiate<GameObject>(subtitlePrefab, base.transform.position, Quaternion.identity, base.transform).GetComponent<subtitlesScriptReal>();
				component2.producerAud = audiSourc;
                component2.subtitlys = subtitle;
                component2.audiObject = subscriptobj;
				component2.infinite = audiSourc.loop;
				component2.is3d = false;
				component2.fixesPosition = position;
				component2.realduration = trueDuration;
				result = component2;
				
			}
			return result;
		}

    [SerializeField]
    private GameObject subtitlePrefab;
    public Canvas subtitleCanvas;
    private subtitlesScriptReal currentSubtitle;
    public List<subtitlesScriptReal> singleSubtitle = new List<subtitlesScriptReal>();
	}
