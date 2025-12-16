using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;

public class FamishedModeController : MonoBehaviour
{
    #region SingletonSetup
    private void Awake() => Instance = this;
    public static FamishedModeController Instance;
    #endregion
    private void Start()
    {
        TimethatUhhh = Onebouncemain.length;
        if (gc.mode == "famished")
        {
            scoreSystemManager.Instance.PointsMultiplier += 1f;
            gc.UpdateNotebookCount();
            StartCoroutine(easing(new Color(0.9803922f, 0.5019608f, 0.4470589f, 1f), 0, 2, 0));
            gc.WindowLayermask = "Ignore Raycast";
            Singleton<VertexGlitchManager>.Instance.mustGlitch = true;
            //StartCoroutine(zoomtiem());
        }
    }
    /*public void FixedUpdate()
    {
        float shadvalh = 2f;
        if (shadvalh <= 2f)
        {
            shadvalh -= Time.deltaTime;
            Shader.SetGlobalFloat("_VertexGlitchIntensity", shadvalh);
        }
    }*/
    private void FixedUpdate()
    {
        if (frameCountShit && Time.timeScale != 0)
        {
            lmsFrame += Time.deltaTime;
        }
    }
    public void Update()
    {
        if (specialLmsToggle2)
        {
            if (!AllowCountdown)
            {
                TimethatUhhh = Mathf.Lerp(TimethatUhhh,puppyplay.length, 3*Time.deltaTime);
            }
            if (TimethatUhhh >= puppyplay.length-1)
            {
                if (!onetime)
                {
                    onetime = true;
                    TimethatUhhh = puppyplay.length;
                    StartCoroutine(evenmorepeak());
                    
                }
            }
        }
        if (frameCountShit)
        {
            for (int i = 0; i < eventTypes.Length; ++i)
            {
                if (lmsFrame >= eventTypes[i].SpecialLmsFrameShit && !eventTypes[i].alreadyActivated)
                {
                    eventTypes[i].alreadyActivated = true;
                    
                    if (eventTypes[i].wallshake)
                    {
                        DoEvent(SpecialLMSEventType.wallshake,new Color(1f, 1f, 1f, 1f),eventTypes[i].SpecificsEventDetailsWallShake);
                    }
                    if (eventTypes[i].camchangestuff)
                    {
                        DoEvent(SpecialLMSEventType.camfuck,new Color(1f, 1f, 1f, 1f),eventTypes[i].SpecificsEventDetailsCamera,eventTypes[i].addedFov);
                    }
                    if (eventTypes[i].lightingchange)
                    {
                        DoEvent(SpecialLMSEventType.lightingChange,eventTypes[i].lightingColor,eventTypes[i].SpecificsEventDetailsLightingChange);
                    }
                    if (eventTypes[i].customEvent)
                    {
                        DoEvent(SpecialLMSEventType.normal,new Color(1f, 1f, 1f, 1f),eventTypes[i].SpecificsEventDetailsCustom);
                    }
                }
            }
        }
        if (TimethatUhhh < 0f)
		{
            AllowCountdown = false;
            if (!dontupdatebr)
            {
            Endonebounc();
            } 
		}
        if (AllowCountdown)
		{
            gc.modeState = !specialLmsToggle2 ?"ONEBOUNCE | " + (int)TimethatUhhh+" Seconds Left" : "?????? | " + (int)TimethatUhhh+" Seconds Left" ;
			TimethatUhhh -= Time.deltaTime;
            
		}
        if (pitchdown)
		{
			pitchval -= Time.deltaTime;
            funnyaudiotuff.pitch = pitchval;
            
		}
        int num = Mathf.FloorToInt(TimethatUhhh / 60f);
        int num2 = Mathf.FloorToInt(TimethatUhhh % 60f);
        TimerText.text = string.Format("{0:00}:{1:00}", num, num2);
        if (turnOnLightAutoChange && !specialLmsToggle)
        {
            gc.voxLight.ambientLight = colortext == "red" ? new Color(1f, 0f, 0f, 1f) : colortext == "white" ? new Color(0.65f, 0.65f, 0.65f, 1f) : new Color(0.65f, 0.65f, 0.65f, 1f);
        }
        if (forceupdatelight)
        {
            gc.voxLight.ambientLight = new Color(1f, 1f, 1f, 1f);
        }
    }
    public void DoEvent(SpecialLMSEventType EventTypes,Color wha,string EventDetails,float fovIncreaseAmmount= 0)
    {
        switch (EventTypes)
		{
            case SpecialLMSEventType.wallshake:
            Singleton<VertexGlitchManager>.Instance.Glitch();
            break;
            case SpecialLMSEventType.lightingChange:
            if (wha != new Color(1f, 1f, 1f, 1f))
            {
            gc.voxLight.ambientLight = wha;
            }
            break;
            case SpecialLMSEventType.camfuck:
            if (EventDetails == "playerCamShakeLow") 
            {
            CameraScript.Instance.TempShakeAmount += 0.6f;
            }
            if (EventDetails == "playerCamShake") 
            {
            CameraScript.Instance.TempShakeAmount += 1.5f;
            }
            if (EventDetails == "playerCamShakeLong") 
            {
            CameraScript.Instance.TempShakeAmount += 2f;
            }
            if (EventDetails == "fovstuff")
            {
                AdditionalGameCustomizer.Instance.FovAmmount += fovIncreaseAmmount;
            }
            break;
            case SpecialLMSEventType.normal:
            if (EventDetails == "spawnbutch")
            {
                foreach (GameObject chaosclon in gc.npcCloneList)
                {
                    chaosclon.SetActive(true);
                }
                angerMultipler = 1.15f;
                gc.player.DefaultWalkSpeed += 40;
                gc.player.DefaultRunSpeed += 50;
            }
            if (EventDetails == "chillPhase")
            {
                gc.player.DefaultWalkSpeed -= 40;
                gc.player.DefaultRunSpeed -= 50;
                angerMultipler = 0.2f;
            }
            if (EventDetails == "unchill")
            {
                gc.player.DefaultWalkSpeed += 40;
                gc.player.DefaultRunSpeed += 50;
                angerMultipler = 1.3f;
            }
            if (EventDetails == "flashbang")
            {
                ZerullClassic.Instance.yourflashbang.Rebind();
                ZerullClassic.Instance.yourflashbang.Play("flashAnim", -1, 0f);
            }
            if (EventDetails == "startConstantZoom")
            {
                stopbouncezoom = false;
                StartCoroutine(zoomtiem());
            }
            if (EventDetails == "EndConstantZoom")
            {
                stopbouncezoom = true;
            }
            break;
        }
    }
    public void Endonebounc()
    {
        if (ZerullClassic.Instance.spawnBlockagesDuringTheBossfight)
        {
            ZerullClassic.Instance.blockages.SetActive(false);
        }
        float valthing = scoreSystemManager.Instance.scorevalue / 1.5f;
        scoreSystemManager.Instance.AddScore((int)valthing, false);
        AdditionalGameCustomizer.Instance.FovAmmount = 60;
        gc.modeState = "its over (fr this time)";
        alwaysKnowIp = false;
        angerMultipler = 0.1f;
        colortext = "white";
        if (!specialLmsToggle)
        {
            pitchdown = true;
            funnyaudiotuff.clip = OneBounceEnd;
            funnyaudiotuff.loop = false;
            funnyaudiotuff.Play();
            dontupdatebr = true;
            gc.player.DefaultWalkSpeed -= 40;
            gc.player.DefaultRunSpeed -= 50;
        }
        gc.ElevdorRea.ForEach(ed => ed.finaleActivated = false);
        gc.Gatesrea.ForEach(g => g.Down(false));
        gc.ElevdorRea.ForEach(ed => ed.Opendor = true);
        
        return;
    }

    public void onebounc(Transform ok)
    {
        ZerullClassic.Instance.yourflashbang.Rebind();
        ZerullClassic.Instance.yourflashbang.Play("flashAnim", -1, 0f);
        ZerullClassic.Instance.health = 10;
        if (ZerullClassic.Instance.spawnBlockagesDuringTheBossfight)
        {
            ZerullClassic.Instance.blockages.SetActive(true);
        }
        foreach (WindowScript w in FindObjectsOfType<WindowScript>())
        {
            if (!w.broken)
            {
                w.Window(true, false, 0f);
            }
        }
        for (int a = 0; a < tweenOutitems.Length; ++a)
        {
            tweenOutitems[a].transform.DOMoveX(-700, 3f);
        }
        funnyaudiotuff.Stop();
        foreach (FamishedScript fam in gc.famishscr)
        {
            if (fam.isActiveAndEnabled)
            {
                fam.agent.Warp(new Vector3(ok.position.x,fam.transform.position.y,ok.position.z));
            }
        }
        scoreSystemManager.Instance.stopUpdatingTSDiscord = true;
        if (!specialLmsToggle)
        {
            scoreSystemManager.Instance.stopUpdatingTSDiscord = true;
            gc.modeState = "OH GOD NO WHAT";
            colortext = "red";
            gc.player.DefaultWalkSpeed += 40;
            gc.player.DefaultRunSpeed += 50;
            angerMultipler = 1.35f;
            AdditionalGameCustomizer.Instance.FovAmmount += 30;
            isAbleToMove = false;
            
            GameControllerScript.Instance.audioDevice.PlayOneShot(windCras);
            
            turnOnLightAutoChange = true;
            StartCoroutine(peak());
        }
        else
        {
            gc.modeState = "fear";
            foreach (GameObject chaosclon in gc.npcCloneList)
            {
                chaosclon.SetActive(false);
            }
            obTimer.SetActive(true);
            if (!specialLmsToggle2)
            {
                specialLmsToggle2=true;
            }
            ZerullClassic.Instance.GlitchShaders(true);
            Singleton<OtherMainStuffManager>.Instance.HighSchoolDropOut();
            gc.SlotsAmmount = 5;
            Singleton<OtherMainStuffManager>.Instance.slot();
        }
    }
    public IEnumerator peak()
    {
        yield return new WaitForSeconds(windCras.length);
        funnyaudiotuff.clip = Onebouncemain;
        funnyaudiotuff.loop = false;
        funnyaudiotuff.Play();
        isAbleToMove = true;
        alwaysKnowIp = true;
        AllowCountdown = true;
        OneBounceFamis = true;
        obTimer.SetActive(true);
    }
    public IEnumerator evenmorepeak()
    {
        yield return new WaitForSeconds(0.1f);
        forceupdatelight = true;
        yield return new WaitForSeconds(0.3f);
        forceupdatelight = false;
        funnyaudiotuff.clip = puppyplay;
        funnyaudiotuff.loop = false;
        funnyaudiotuff.Play();
        alwaysKnowIp = true;
        frameCountShit = true;
        AllowCountdown = true;
        OneBounceFamis = true;
        obTimer.SetActive(true);
    }
    public void manualUpdate()
    {
        if (gc.notebooks == 2)
        {
            gc.voxLight.ambientLight = Color.black;
            StartCoroutine(easing(new Color(0.45f, 0.45f, 0.45f, 1f), 0, 2, 2));
            angerMultipler = 2.5f;
            StartCoroutine(ohgodwhat());
            butch.SetActive(true);
            gc.notebookCount.color = Color.Lerp(Color.white, new Color(0.55f, 0.55f, 0.55f, 1f), 1 - Mathf.Repeat(1f, 0.2f));
            ItemManager.Instance.ItemNameText.color = Color.Lerp(Color.white, new Color(0.55f, 0.55f, 0.55f, 1f), 1 - Mathf.Repeat(1f, 0.2f));
            if (gc.warrealest)
            {
                gc.LapManag.MeepTimer.SetActive(true);
            }
        }
        if (gc.notebooks == 3)
        {
            angerMultipler = 1.9f;
        }
        if (gc.notebooks == 4)
        {
            angerMultipler = 1.75f;
        }
        if (gc.notebooks == 5)
        {
            angerMultipler = 1.55f;
        }
        if (gc.notebooks == 6)
        {
            angerMultipler = 1.35f;
        }
        if (gc.notebooks == 7)
        {
            gc.voxLight.ambientLight = new Color(0.05f, 0.05f, 0.05f, 0f);
            StartCoroutine(easing(new Color(0.35f, 0.35f, 0.35f, 1f), 0, 1, 1));
            StopCoroutine(ohgodwhat());
            StartCoroutine(imdespairingit());
            angerMultipler = 1.65f;
            gc.famishScrpt.activatewindowbreak = true;
        }
        if (gc.notebooks == 8)
        {
            angerMultipler = 1.15f;
        }
        if (gc.notebooks == gc.maxNotebooks && gc.exitsReached == 0)
        {
            gc.voxLight.ambientLight = new Color(1f, 0f, 0f, 1f);
            StartCoroutine(easing(new Color(0.25f, 0.25f, 0.25f, 1f), 0, 1, 1));
            angerMultipler = 1.35f;
            corspesspawn = true;
            funnyaudiotuff.clip = despairloopfinale1;
            funnyaudiotuff.loop = true;
            funnyaudiotuff.Play();
        }
        if (gc.exitsReached == 1)
        {
            funnyaudiotuff.clip = despairloopfinale2;
            funnyaudiotuff.loop = true;
            funnyaudiotuff.Play();
            StartCoroutine(easing(new Color(0.2f, 0.2f, 0.2f, 1f), 0, 1, 1));
        }
        if (gc.exitsReached == 3)
        {
            StartCoroutine(finale());
            StartCoroutine(easing(new Color(0.15f, 0.15f, 0.15f, 1f), 0, 1, 1));
        }
        if (gc.exitsReached == 5)
        {
            angerMultipler = 0.3f;
            StopCoroutine(finale());
            StartCoroutine(finaleaga());
            StartCoroutine(easing(new Color(0.65f, 0.65f, 0.65f, 1f), 0, 1, 1));
            gc.modeState = gc.exitsReached + "/" + gc.maxExits + " Exits" + " | its over..?";
        }
    }
    public IEnumerator ohgodwhat()
    {
        funnyaudiotuff.clip = despairIntro;
        funnyaudiotuff.Play();
        yield return new WaitForSeconds(despairIntro.length);
        funnyaudiotuff.clip = despairloop1;
        funnyaudiotuff.loop = true;
        funnyaudiotuff.Play();
    }
    public IEnumerator imdespairingit()
    {
        funnyaudiotuff.clip = despairIntro2;
        funnyaudiotuff.Play();
        yield return new WaitForSeconds(despairIntro2.length);
        funnyaudiotuff.clip = despairloop2;
        funnyaudiotuff.loop = true;
        funnyaudiotuff.Play();
    }
    public IEnumerator finale()
    {
        funnyaudiotuff.clip = despairSuddenstop;
        funnyaudiotuff.Play();
        yield return new WaitForSeconds(despairSuddenstop.length);
        funnyaudiotuff.clip = despairloopfinale3;
        funnyaudiotuff.loop = true;
        funnyaudiotuff.Play();
    }
    public IEnumerator finaleaga()
    {
        funnyaudiotuff.clip = despairSuddenstop2;
        funnyaudiotuff.Play();
        yield return new WaitForSeconds(despairSuddenstop2.length);
        funnyaudiotuff.clip = despairloopfinale4;
        funnyaudiotuff.loop = true;
        funnyaudiotuff.Play();
    }
    public IEnumerator zoomtiem()
    {
        if (stopbouncezoom) yield break;
        CameraScript.Instance.TempShakeAmount += 0.2f;
        AdditionalGameCustomizer.Instance.FovAmmount -= 45f;
        yield return new WaitForSeconds(0.05f);
        AdditionalGameCustomizer.Instance.FovAmmount += 45f;
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(zoomtiem());
    }

    public IEnumerator easing(Color kolor, float a, float b, float duration)
    {
        Color start = gc.voxLight.ambientLight;

        for (float t = a; t < b; t += Time.deltaTime / duration)
        {
            gc.voxLight.ambientLight = Color.Lerp(start, kolor, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        gc.voxLight.ambientLight = kolor;
    }
    [SerializeField] private GameControllerScript gc;
    public GameObject[] tweenOutitems;
    public bool corspesspawn, isAbleToMove, alwaysKnowIp,AllowCountdown, pitchdown,dontupdatebr,OneBounceFamis,turnOnLightAutoChange;
    public float angerMultipler,TimethatUhhh,pitchval = 1f;
	public TMP_Text TimerText;
    private string colortext;
    public GameObject butch,token,obTimer;
    
    public AudioSource funnyaudiotuff;
    public AudioClip despairIntro, despairloop1, despairIntro2, despairloop2, despairloopfinale1, despairloopfinale2, despairSuddenstop, despairloopfinale3, despairSuddenstop2,despairloopfinale4,windCras,Onebouncemain,OneBounceEnd;
    
    [Header("oh boy")]
    public AudioClip puppyplay;
    public bool specialLmsToggle,specialLmsToggle2,frameCountShit,forceupdatelight,onetime,stopbouncezoom;
    public enum SpecialLMSEventType {wallshake,normal,camfuck,lightingChange};
    public float lmsFrame;
    
    public WhatTypeOfEventBro[] eventTypes;
	[Serializable]
	public class WhatTypeOfEventBro
	{
        [SerializeField]public float SpecialLmsFrameShit;
		[SerializeField]public bool wallshake,camchangestuff,lightingchange,alreadyActivated,customEvent;
        [SerializeField]public float addedFov;
        [SerializeField]public string SpecificsEventDetailsWallShake,SpecificsEventDetailsCamera,SpecificsEventDetailsLightingChange,SpecificsEventDetailsCustom;
        [SerializeField]public Color lightingColor;
	}
}