using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
//namespace quangg.YuriArchive.subtitlesYaoiYuri
//{
public class SubtitlesManagerAkaSubtitleSpawnOkSDIYBT : MonoBehaviour //atp this is just yuri script with semi decent ammount of modification kms
	{
		public void summonLeSubtitle(subtitlingIt subtitle, subsScriptableObject subscriptobj, AudioSource audiSourc)
		{
			subtitlesScriptReal component = Instantiate<GameObject>(subtitlePrefab, subtitleCanvas.transform.position, Quaternion.identity, subtitleCanvas.transform).GetComponent<subtitlesScriptReal>();
			subtitle3dList.Add(component);
			component.producerAud = audiSourc;
            component.subtitlys = subtitle;
            component.audiObject = subscriptobj;
			component.infinite = audiSourc.loop;
			component.is3d = true;
			component.cameraTransf = cameraTransorm;
			component.aspectRatio = 1f;
            component.bg.anchoredPosition = new Vector3(0f, -266.66f / component.aspectRatio, 0f);
			component.updateSubPostion();
		}
		public void summonLeSubtitle2D(subtitlingIt subtitle, subsScriptableObject subscriptobj, Vector3 position, AudioSource audiSourc)
		{
			subtitlesScriptReal component = Instantiate<GameObject>(subtitlePrefab, subtitleCanvas.transform.position, Quaternion.identity, subtitleCanvas.transform).GetComponent<subtitlesScriptReal>();
			subtitle2dList.Add(component);
            component.producerAud = audiSourc;
            component.subtitlys = subtitle;
            component.audiObject = subscriptobj;
			component.infinite = audiSourc.loop;
			component.is3d = false;
			component.fixesPosition = position;
			component.cameraTransf = cameraTransorm;
			component.aspectRatio = 1f;
        	component.bg.localScale = new Vector3(1f, 1f, 1f);
        	component.bg.anchoredPosition = component.fixesPosition;
			component.updateSubPostion();
		}
	public void hideSub(subsScriptableObject subscriptobj)
	{
        foreach(subtitlesScriptReal subtitle in subtitle3dList)
        {
            if (subtitle.audiObject = subscriptobj)
            {
                subtitle.hidesub = true;
                return;
            }
        }
        foreach(subtitlesScriptReal subtitle in subtitle2dList)
        {
            if (subtitle.audiObject = subscriptobj)
            {
                subtitle.hidesub = true;
                return;
            }
        }
	}
    [SerializeField]
	public Transform cameraTransorm;
    public GameObject subtitlePrefab;
    public Canvas subtitleCanvas;
	public List<subtitlesScriptReal> subtitle2dList, subtitle3dList= new List<subtitlesScriptReal>();
	}
//}
[Serializable]
public class subtitlingIt
{
    // hi yuri if u reading this hi
	public bool enabled = true;
	public string headText = "Placeholder";
	[Min(0f)] public float duration;
	public ColorMode colorMode = ColorMode.Default;
    public Color textColor = new Color(1f, 1f, 1f, 1f);
	[Header("rainbow and gradient color stuff")]
    public Gradient textGradient = new Gradient();
    public bool gradientAnimate = false;
    public GradientAnimationMode gradientAnimationMode = GradientAnimationMode.Rotate;
    public float gradientSpeed = 1f;
    public float rainbowSpeed = 1f;
	
	[Header("funny other options")]
	public bool undertaleStyleTextAppearing = true;
	public float TextAppearSpeed = 1f;
	public bool shakey;
	public float shakeyspeed = 1f;
    public float shakeyradius = 0.3f;
    public int fonts;
	public bool useLocalization = false,unreadable = false,upsideDown = false,textReverse = false;
	[HideInInspector] public ColorMode _FixedMode = ColorMode.Default;
    [HideInInspector] public ColorMode _GradientMode = ColorMode.Gradient;
    [HideInInspector] public ColorMode _RainbowMode = ColorMode.Rainbow;
	public enum ColorMode // joink
    {
        Default,
        Gradient,
        Rainbow
    }
    public enum GradientAnimationMode
    {
        Rotate,
        ColorChanging,
        OrderLerp,
        LeftToRight,
        RightToLeft
    }
}
