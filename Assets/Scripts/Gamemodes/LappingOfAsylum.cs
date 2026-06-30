using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Reflection;
using DG.Tweening;

public class LappingOfAsylumController : MonoBehaviour
{
    #region SingletonSetup
    private void Awake() => LapInstance = this;
    public static LappingOfAsylumController LapInstance;
    #endregion
    //thanks oppo your code will be in good hands
    private IEnumerator Crossfade(AudioManagerLiveReaction fromSource, AudioManagerLiveReaction toSource, AudioObjectyeah toClip, float duration,bool setLoop,bool QueueLoop = false, AudioObjectyeah LoopClip = null) 
	{
        isCrossfading = true;
		toSource.QueueAudio(toClip);
        if (QueueLoop) toSource.QueueAudio(LoopClip);
		toSource.SetVolume(0f);
        toSource.SetLoop(setLoop);
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
        isCrossfading = false;
        yield break;

	}
    public void Update()
    {
        string mode = PlayerPrefs.GetString("CurrentMode");
        if (mode != "LappingOfAsylum") return;
        if (randomPrelapQueue && mode == "LappingOfAsylum") PlayPreLaps();
        if (vanishScore) scoreDecreaseTimer -= Time.deltaTime;
        if (scoreDecreaseTimer < 0f)
		{
            scoreSystemManager.Instance.AddScore(-10*CurrentLap);
            scoreDecreaseTimer = 1f/CurrentLap;
        }
        if (Singleton<TimeOutManagerFUCKYEA>.Instance.TimeDuratiOk <= Lap1OutroTimer && Singleton<TimeOutManagerFUCKYEA>.Instance.countItDown && !lap1NearEndThingActive && !NinetyNineToggle && CurrentLap <= 1)
        {
            StartCoroutine(Crossfade(LapSound, LapSound2, LapMusikOutro, 2f,false));
            LapRouletteTagThing++;
            lap1NearEndThingActive = true;
        }
        gc.wegchal.globalWegaSpeed = gc.player.walkSpeed/1.1f;
        if (CurrentLap == 99)
        {
            cd += Time.deltaTime;
            if (cd >= 0.25f) 
            {
                Singleton<VertexGlitchManager>.Instance.Glitch(0.25f,0.25f,0.25f,128);
                cd = 0f;
            }
        }
    }
    private void PlayPreLaps()
    {
        if (CurAudioMan.audioDevice.time <= 1f && !isCrossfading) 
        {
            LapRouletteTagThingQueue++;
            var audLapVal = (int)UnityEngine.Random.Range(0f, RandomizedPrelapQueueOkBro.Length);
            if (LapRouletteTagThingQueue == 2)
            {
                StartCoroutine(Crossfade(PrelapQueue2, PrelapQueue1, RandomizedPrelapQueueOkBro[audLapVal], 1.5f,true));
                CurAudioMan = PrelapQueue1;
                LapRouletteTagThingQueue = 0;
            }
            else 
            {
                StartCoroutine(Crossfade(PrelapQueue1, PrelapQueue2, RandomizedPrelapQueueOkBro[audLapVal], 1.5f,true));
                CurAudioMan = PrelapQueue2;
            }
        }
    }
    private void SetCustomLapMusic()
    {
        return;
        for (int i = 0; i < lappingHi.Length; i++)
        {
            stupidTextIntro[i] = PlayerPrefs.GetString($"Lap {i+1} Music Intros", "Default");
            stupidTextLoop[i] = PlayerPrefs.GetString($"Lap {i+1} Music", "Default");
            //stupidTextLoop[i] = PlayerPrefs.GetString($"Lap {i+1} Music Loops", "Default");
            if (stupidTextIntro[i] != "Default" && lappingHi[i].LapMusikIntro != null) 
            {
                AudioObjectyeah newsound = ScriptableObject.CreateInstance<AudioObjectyeah>();
                newsound.SoundTypeWahh = SoundAudioType.Music;
                newsound.audClip = Sych.LoadSound(stupidTextIntro[i]);
                newsound.volume = 1f;
                lappingHi[i].LapMusikIntro = newsound;
            }
            if (stupidTextLoop[i] != "Default" && lappingHi[i].LapMusikLoop != null) 
            {
                AudioObjectyeah newsound1 = ScriptableObject.CreateInstance<AudioObjectyeah>();
                newsound1.SoundTypeWahh = SoundAudioType.Music;
                newsound1.audClip = Sych.LoadSound(stupidTextLoop[i]);
                newsound1.volume = 1f;
                lappingHi[i].LapMusikLoop = newsound1;
            }
        }
        stupidTextOutro = PlayerPrefs.GetString($"Lap 1 Music Outros", "Default");
        if (stupidTextOutro != "Default" && LapMusikOutro != null) 
        {
            AudioObjectyeah newsound2 = ScriptableObject.CreateInstance<AudioObjectyeah>();
            newsound2.SoundTypeWahh = SoundAudioType.Music;
            newsound2.audClip = Sych.LoadSound(stupidTextOutro);
            newsound2.volume = 1f;
            LapMusikOutro = newsound2;
        }
    }
    private void Start()
    {
        if (NinetyNineToggle && GoodMode)
        {
            NinetyNineToggle = false;
            GoodMode = false;
            Debug.Log("cant have both good mode and lap 99 lmfaooo");
        }
        PrelapQueue1.SetVolume(0f);
		PrelapQueue2.SetVolume(0f);
        CurAudioMan = PrelapQueue2;
        Lap1OutroTimer = LapMusikOutro.audClip.length;
        realCurrentMaxNoteboo = gc.maxNotebooks;
        CurrentMaxNotebooks = gc.maxNotebooks;
        LapPortals.ForEach(lap => lap.SetActive(false));
        SetCustomLapMusic();
    }
            
    public void doShit(string stuff = "beginning")
    {
        if (stuff == "beginning")
        {
            gc.UnlockAmount = 0;
            gc.ObjectsToEnable.ForEach(o => o.SetActive(true));
            gc.Math.quarter.SetActive(true);
            string mode = PlayerPrefs.GetString("CurrentMode");
            if (mode != "LappingOfAsylum") LapPortals.ForEach(lap => Destroy(lap));
            if (gc.warrealest) gc.LapManag.MeepTimer.SetActive(true);
            
        }
        if (stuff == "exitstuff")
        {
            if (CurrentLap < MaxLap) LapPortals.ForEach(lap => lap.SetActive(true));
            gc.Gatesrea.ForEach(g => g.Down(false));
            gc.ElevdorRea.ForEach(ed => ed.Opendor = true);
            for (int e = 0; e < AdditionalGameCustomizer.Instance.ExitImages.Length; ++e) StartCoroutine(gc.tweeniconSolo(Color.black, 0, 1, 1f, e));
            elevatorTriggers.ForEach(et => et.SetActive(true));
            allowClosElev = true;
        }
    }
    public void UpdateManually()
    {
        ChaosCheeseCount++;
        if (ChaosCheeseCount == 7)
        {
            bool ChaosMode = PlayerPrefsExtension.GetBool("Chaos");
            if (ChaosMode)
            {
                if (gc.mode == "LappingOfAsylum")
                {
                    foreach (GameObject obj in gc.ObjectsToEnable)
                    {
                        if (obj != null)
                        {
                            GameObject clone = Instantiate(obj, obj.transform.position, obj.transform.rotation);
                            clone.name = obj.name;
                            clone.SetActive(true);
                        }
                    }
                }
            }
            ChaosCheeseCount = 0;
        }
        if (Meeptimar.isActiveAndEnabled && CurrentLap <= 5 && gc.notebooks <= CurrentMaxNotebooks) meepTimerScript.Instance.AddTime(gc.warrealest && CurrentLap < 4 ? 18/(CurrentLap+1) : 10f, Color.green);
        if (LapFamishShit) FamishCheeseCount++;
        if (Lap5TimingStuff) Lap5CheeseCount++;
        if (Lap5CheeseCount == 14)
        {
            gc.maxNotebooks += realCurrentMaxNoteboo;
            CurrentMaxNotebooks += realCurrentMaxNoteboo;
            gc.player.maxHealth += 25;
            gc.player.totemshit(false);
            for (int i = 0; i < noteboos.Length; ++i) if (noteboos[i].hidden) noteboos[i].Respawn();
            gc.UpdateNotebookCount();
            Lap5TimingStuff = false;
            Lap5CheeseCount = 0;
        }
        if (FamishCheeseCount == 3) gc.fmc.angerMultipler = 1.25f;
        if (FamishCheeseCount == 4) gc.fmc.angerMultipler = 1.35f;
        if (FamishCheeseCount == 5) gc.fmc.angerMultipler = 1.25f;
        if (FamishCheeseCount == 6) gc.fmc.angerMultipler = 0.9f;
        if (FamishCheeseCount == 7)
        {
            gc.fmc.angerMultipler = 0.65f;
            gc.famishScrpt.activatewindowbreak = true;
            LapFamishShit = false;
            FamishCheeseCount = 0;
        }
        if (CurrentLap >= 3)
        {
            Chaos3CheeseCount++;
            if (Chaos3CheeseCount == 7)
            {
                bool ChaosMode = PlayerPrefsExtension.GetBool("Chaos");
                if (ChaosMode)
                {
                    if (gc.mode == "LappingOfAsylum")
                    {
                        if (gc.fmc.butch != null)
                        {
                            GameObject clone = Instantiate(gc.fmc.butch, gc.fmc.butch.transform.position, gc.fmc.butch.transform.rotation);
                            clone.name = gc.fmc.butch.name;
                            clone.GetComponent<FamishedScript>().famishedSpd = 0.5f;
                            clone.SetActive(true);
                        }
                        /*if (gc.zerull.zer != null)
                        {
                            GameObject clone = Instantiate(gc.zerull.zer, gc.zerull.zer.transform.position, gc.zerull.zer.transform.rotation);
                            clone.name = gc.zerull.zer.name;
                            clone.GetComponent<zerullscript>().Anger = 0.5f;
                            clone.SetActive(true);
                        }*/
                        if (gc.wegchal.WEGA != null)
                        {
                            GameObject clone = Instantiate(gc.wegchal.WEGA, gc.wegchal.WEGA.transform.position, gc.wegchal.WEGA.transform.rotation);
                            clone.name = gc.wegchal.WEGA.name;
                            clone.SetActive(true);
                        }
                    }
                }
                Chaos3CheeseCount = 0;
            }
        }
        if (CurrentLap == 6) 
        {
            gc.PlayerTeleport("evilleaf");
            meepTimerScript.Instance.AddTime(4f, Color.green);
        }
        if (gc.notebooks == CurrentMaxNotebooks)
        {
            if (CurrentLap == 0)
            {
                if (!allowClosElev)
                {
                    randomPrelapQueue = false;
                    PrelapQueue1.ClearQueue(true);
                    PrelapQueue2.ClearQueue(true);
                    gc.lbams.EscapeMusic.ClearQueue(true);
                    
                    gc.lbams.EscapeMusic.QueueAudio(gc.lbams.SchoolhouseEscape);
                    gc.lbams.EscapeMusic.SetLoop(true);
                    
                    doShit("exitstuff");
                    LapPortals.ForEach(lap => lap.SetActive(false));
                }
                else;
            }
            else if (!allowClosElev) doShit("exitstuff");
        }
        if (CurrentLap == 0)
        {
            if (gc.exitsReached == 1)
            {
                gc.lbams.EscapeMusic.SetPitch(0.9f);
                gc.easingExit(new Color(1f, 0.7f / gc.exitsReached, 0.7f / gc.exitsReached, 1f), 0, 2, 5);
            }
            if (gc.exitsReached == 2)
            {
                gc.lbams.EscapeMusic.SetPitch(0.75f);
                gc.lbams.ChaosAudioSource.ClearQueue(true);
                gc.lbams.ChaosAudioSource.SetLoop(true);
                gc.lbams.ChaosAudioSource.QueueAudio(gc.lbams.ChaosStart);
                gc.easingExit(new Color(1f, 0.7f / gc.exitsReached, 0.7f / gc.exitsReached, 1f), 0, 2, 5);
            }
            if (gc.exitsReached == 3)
            {
                gc.lbams.ChaosAudioSource.ClearQueue(true);
                gc.lbams.ChaosAudioSource.SetLoop(true);
                gc.lbams.ChaosAudioSource.QueueAudio(gc.lbams.ChaosStartLoop);
                gc.lbams.EscapeMusic.SetPitch(0.6f);
                gc.easingExit(new Color(1f / (gc.exitsReached / 1.75f), 0.7f / gc.exitsReached, 0.7f / gc.exitsReached, 1f), 0, 2, 5);
            }
            if (gc.exitsReached == 4)
            {
                gc.lbams.EscapeMusic.SetPitch(0.45f);
                gc.easingExit(new Color(1f / (gc.exitsReached / 1.5f), 0.7f / gc.exitsReached, 0.7f / gc.exitsReached, 1f), 0, 2, 5);
                gc.lbams.ChaosAudioSource.ClearQueue(true);
                gc.lbams.ChaosAudioSource.SetLoop(true);
                gc.lbams.ChaosAudioSource.QueueAudio(gc.lbams.ChaosBuildUp);
            }
            if (gc.exitsReached == 5)
            {
                gc.lbams.EscapeMusic.SetPitch(0.3f);
                gc.easingExit(new Color(1f / (gc.exitsReached / 1.25f), 0.7f / gc.exitsReached, 0.7f / gc.exitsReached, 1f), 0, 2, 5);
                gc.maxExits += 1;
            }
            if (gc.exitsReached == 6)
            {
                StartCoroutine(zawadro());
                gc.lbams.ChaosAudioSource.ClearQueue(true);
                gc.lbams.EscapeMusic.ClearQueue(true);
                gc.easingExit(new Color(1f, 1, 1, 1f), 0, 2, 5);
            }
        }
    }
    public IEnumerator zawadro()
    {
        zawuardoStartTransistionStuff();
        // some lap 99 stuff dont mind it being there smth
        if (!NinetyNineToggle)yield return new WaitForSeconds(zawarudo.audClip.length);
        else if (GoodMode) yield return new WaitForSeconds(0.005f);
        //gc.player.transform.position = EndingManager.Instance.SecretWarpPoint.transform.position + Vector3.up * gc.player.height;
        yield return new WaitForSeconds(0.1f);
        zawuardoMiddleTransistionStuff();
        //well yeah again
        if (!NinetyNineToggle) yield return new WaitForSeconds(zawarudo.audClip.length / 2);
        else if (GoodMode) yield return new WaitForSeconds(0.005f);
        else yield return new WaitForSeconds(9f);
        zawuardoEndTransistionStuff();
        yield return null;
        yield break;
    }
    public void zawuardoStartTransistionStuff()
    {
        if (!NinetyNineToggle && !GoodMode)
        {
            gc.lbams.MainSource2.PlaySingleClip(zawarudo);
            AdditionalGameCustomizer.Instance.donthaveanamelmfao = AdditionalGameCustomizer.Instance.zaColor;
        }
        if (NinetyNineToggle && !GoodMode) 
        {
            gc.lbams.MainSource2.PlaySingleClip(lightsoutohfuck);
            AdditionalGameCustomizer.Instance.donthaveanamelmfao = new Color(0f,0f,0f,1f);
        }
        if (gc.player.stamina < 100f) gc.player.SetStamina(PlayerScript.StaminaChangeMode.Set, 100f);
        vanishScore = false;
        LapSound.ClearQueue(true);
        gc.player.DisableCamMove = true;
        gc.player.movementLocked = true;
        gc.player.titlecard = true;
        gc.playerCollider.enabled = false;
        gc.npcCloneList.ForEach(o => o.SetActive(false));
        if (GoodMode) gc.player.transform.position = EndingManager.Instance.SecretWarpPoint.transform.position + Vector3.up * Game.player.height;
        else
        {
            gc.player.transform.DOMove(EndingManager.Instance.SecretWarpPoint.transform.position + Vector3.up * gc.player.height, zawarudo.audClip.length/1.5f);
            gc.player.forceLookSpeed = 50f;
            gc.player.targetToForcelyLookAt = EndingManager.Instance.SecretWarpPoint.transform;
            gc.player.isForcedToLook = true;
        }
    }
    public void zawuardoMiddleTransistionStuff()
    {
        if (NinetyNineToggle)
        {
            gc.maxNotebooks += realCurrentMaxNoteboo * 99;
            CurrentMaxNotebooks += realCurrentMaxNoteboo * 99;
            gc.player.maxHealth = 999;
        }
        else
        {
            gc.maxNotebooks += realCurrentMaxNoteboo;
            CurrentMaxNotebooks += realCurrentMaxNoteboo;
            gc.player.maxHealth += 25;
        }
        gc.player.totemshit(false);
        for (int i = 0; i < noteboos.Length; ++i) if (noteboos[i].hidden) noteboos[i].Respawn();
        for (int e = 0; e < AdditionalGameCustomizer.Instance.ExitImages.Length; ++e) StartCoroutine(gc.tweeniconSolo(new Color(0, 0, 0, 0), 0, 1, 1f, e));
        gc.UpdateNotebookCount();
    }
    public void zawuardoEndTransistionStuff()
    {
        allowClosElev = false;
        gc.exitsReached = 0;
        if (CurrentLap > MaxLap - 1) LapPortals.ForEach(lap => lap.SetActive(false));
        gc.ElevdorRea.ForEach(ed => ed.Close());
        gc.ElevdorRea.ForEach(ed => ed.finaleActivated = false);
        gc.Gatesrea.ForEach(g => g.Down(false));
        LapPortals.ForEach(lap => lap.SetActive(false));
        LapSound.ClearQueue(true);
        LapSound.SetLoop(true);
        if (NinetyNineToggle)
        {
            gc.ItemsToRespawn.ForEach(item => item.SetActive(true));
            gc.ItemsToRespawn.ForEach(item => item.GetComponent<PickupScript>().ItemRespawning());
            gc.MachinesToRestock.ForEach(machine => machine?.RestockVendingMachine(true));
            if (NinetynineIntro != null) LapSound.QueueAudio(NinetynineIntro);
            if (NinetynineLoop != null) LapSound.QueueAudio(NinetynineLoop);
            CurrentLap = 99;
            MaxLap = 99;
            Singleton<TimeOutManagerFUCKYEA>.Instance.InitializeTimeoutStuff(0f);
            for (int i = 0; i < noteboos.Length; ++i)
            {
                noteboos[i].MultiCollect = true;
                noteboos[i].MultiCollectTime = 99;
            }
            if (LapFlaninety != null) StartCoroutine(flagmove(LapFlaninety,5f));
            gc.fmc.butch.SetActive(true);
            gc.zerull.zer.SetActive(true);
            gc.wegchal.WEGA.SetActive(true);
            gc.player.walkSpeedMultipler += 3f;
            gc.player.runSpeedMultipler += 3f;
            ItemManager.Instance.ClearAllItems();
            foreach (PickupScript pickmygames in FindObjectsOfType<PickupScript>()) if (pickmygames.DroppedItem) Destroy(pickmygames.transform.parent.gameObject);
            foreach (BaseItem itemem in FindObjectsOfType<BaseItem>()) itemem.Uses = itemem.Uses * 5;
        }
        else
        {
            if (GoodMode)
            {
                ZerullClassic.Instance.yourflashbang.Rebind();
                ZerullClassic.Instance.yourflashbang.Play("flashAnim", -1, 0f);
                gc.lbams.MainSource2.PlaySingleClip(gc.lbams.TeleporterTp);
                gc.player.walkSpeedMultipler += 0.25f;
                gc.player.runSpeedMultipler += 0.25f;
            }
            else
            {
                if (lappingHi[CurrentLap].LapMusikIntro != null) LapSound.QueueAudio(lappingHi[CurrentLap].LapMusikIntro);
                if (lappingHi[CurrentLap].LapMusikLoop != null) LapSound.QueueAudio(lappingHi[CurrentLap].LapMusikLoop);
                Singleton<TimeOutManagerFUCKYEA>.Instance.InitializeTimeoutStuff(300f);
                gc.player.walkSpeedMultipler += 0.1f;
                gc.player.runSpeedMultipler += 0.1f;
            }
            
            CurrentLap++;
            vanishScore = true;
            
        }
        gc.lbams.MainSource2.PlaySingleClip(BellSoundLapping);
        gc.maxExits -= 1;
        gc.player.movementLocked = false;
        gc.player.titlecard = false;
        gc.playerCollider.enabled = true;
        gc.player.DisableCamMove = false;
        gc.npcCloneList.ForEach(o => o.SetActive(true));
        AdditionalGameCustomizer.Instance.donthaveanamelmfao = AdditionalGameCustomizer.Instance.canvascolormain;
        
        bool shrinky = PlayerPrefsExtension.GetBool("shrink");
        if (shrinky) Singleton<OtherMainStuffManager>.Instance.ChangeItemSlot(Singleton<OtherMainStuffManager>.Instance.realMaxSlotsAmmou);
        foreach (MuchoScript muc in GameControllerScript.Instance.muchscr) if (muc.isActiveAndEnabled) muc.MuchoSpeedScale += 0.1f;
    }
    public IEnumerator flagmove(Sprite sprit,float duration = 3f)
    {
        lapFlagImages.sprite = sprit;
        float ratioy = (float)Screen.width / 360f;
        LapFlag.transform.DOMoveY(ratioy * 160, 1f);
        yield return new WaitForSeconds(duration);
        LapFlag.transform.DOMoveY(ratioy * 300, 1f);
        yield break;
    }
    public IEnumerator LapPortal()
    {
        if (CurrentLap >= 1)
        {
            if (!inportalALREADY)
            {
                LapPortalEntering();
                yield return new WaitForSeconds(PortalEnteringSound.audClip.length + 1);
                LapPortalExiting();
                yield return new WaitForSeconds(PortalExitingSound.audClip.length);
                LapPortalAfterExiting();
                yield return null;
            }
        }
        yield break;
    }
    public void LapPortalEntering()
    {
        if (gc.player.stamina < 100f) gc.player.SetStamina(PlayerScript.StaminaChangeMode.Set, 100f);
        vanishScore = false;
        scoreSystemManager.Instance.AddScore(5500*CurrentLap);
        if (Meeptimar.isActiveAndEnabled) meepTimerScript.Instance.AddTime(55f,Color.green);
        gc.player.maxHealth += 25;
        gc.player.totemshit(false);
        
        for (int e = 0; e < AdditionalGameCustomizer.Instance.ExitImages.Length; ++e) StartCoroutine(gc.tweeniconSolo(new Color(0, 0, 0, 0), 0, 1, 1f, e));
        for (int i = 0; i < noteboos.Length; ++i) if (noteboos[i].hidden) noteboos[i].Respawn();
        gc.exitsReached = 0;
        gc.maxNotebooks += realCurrentMaxNoteboo;
        CurrentMaxNotebooks += realCurrentMaxNoteboo;
        allowClosElev = false;
        inportalALREADY = true;
        gc.player.movementLocked = true;
        gc.player.titlecard = true;
        gc.playerCollider.enabled = false;
        gc.lbams.MainSource2.PlaySingleClip(PortalEnteringSound);
        gc.npcCloneList.ForEach(o => o.SetActive(false));
        gc.CirclAnimator.SetTrigger("nooo");
    }
    public void LapPortalExiting()
    {
        gc.player.titlecard = false;
        gc.player.movementLocked = false;
        gc.playerCollider.enabled = true;
        gc.lbams.MainSource2.PlaySingleClip(PortalExitingSound);
        gc.player.transform.position = EndingManager.Instance.SecretWarpPoint.transform.position + Vector3.up * gc.player.height;
        
        gc.ElevdorRea.ForEach(ed => ed.Close());
        gc.ElevdorRea.ForEach(ed => ed.finaleActivated = false);
        gc.Gatesrea.ForEach(g => g.Down(false));
        LapPortals.ForEach(lap => lap.SetActive(false));
        gc.CirclAnimator.SetTrigger("yooo");
    }
    public void LapPortalAfterExiting()
    {
        LapSpecificsStuff();
        gc.lbams.MainSource2.PlaySingleClip(BellSoundLapping);
        gc.player.walkSpeedMultipler += 0.1f;
        gc.player.runSpeedMultipler += 0.1f;
        CurrentLap++;
        gc.UpdateNotebookCount();
        inportalALREADY = false;
        bool shrinky = PlayerPrefsExtension.GetBool("shrink");
        if (shrinky) Singleton<OtherMainStuffManager>.Instance.ChangeItemSlot(Singleton<OtherMainStuffManager>.Instance.realMaxSlotsAmmou);
    }
    public void LapSpecificsStuff()
    {
        
        vanishScore = true;

        if (!GoodMode)
        {
            LapRouletteTagThing++;
            if (CurrentLap >= 1 && CurrentLap <= 4) gc.npcCloneList.ForEach(o => o.SetActive(true));
            if (CurrentLap == 1) tim.SetActive(true);
            if (CurrentLap == 2)
            {
                gc.ItemsToRespawn.ForEach(item => item.SetActive(true));
                gc.ItemsToRespawn.ForEach(item => item.GetComponent<PickupScript>().ItemRespawning());
                gc.MachinesToRestock.ForEach(machine => machine?.RestockVendingMachine(true));
                Singleton<TimeOutManagerFUCKYEA>.Instance.TimeDuratiOk = 0;
                gc.fmc.butch.SetActive(true);
                //
                LapFamishShit = true;
            }
            if (CurrentLap == 3)
            {
                gc.player.walkSpeedMultipler = 1f;
                gc.player.runSpeedMultipler = 1f;
                gc.fmc.angerMultipler = 0.45f;
                MeepTimer.SetActive(true);
            }
            if (CurrentLap == 4)
            {
                gc.notebooks = 0;
                gc.maxNotebooks = realCurrentMaxNoteboo;
                CurrentMaxNotebooks = realCurrentMaxNoteboo;
                gc.player.walkSpeedMultipler = 1.25f;
                gc.player.runSpeedMultipler = 1.25f;
                gc.zerull.zer.SetActive(true);
                ZerullClassic.Instance.health = 20;
                if (ZerullClassic.Instance.spawnBlockagesDuringTheBossfight) ZerullClassic.Instance.blockages.SetActive(true);
                gc.ObjectsToEnable.ForEach(o => o.SetActive(false));
                mucho.SetActive(true);
                Lap5TimingStuff = true;
                
            }
            if (CurrentLap == 5)
            {
                gc.player.walkSpeedMultipler += 1.3f;
                gc.player.runSpeedMultipler += 1.3f;
                gc.ItemsToRespawn.ForEach(item => item.SetActive(true));
                gc.ItemsToRespawn.ForEach(item => item.GetComponent<PickupScript>().ItemRespawning());
                gc.MachinesToRestock.ForEach(machine => machine?.RestockVendingMachine(true));
                gc.ObjectsToEnable.ForEach(o => o.SetActive(false));
                gc.notebooks += realCurrentMaxNoteboo * 4;
                gc.maxNotebooks += realCurrentMaxNoteboo * 6;
                CurrentMaxNotebooks += realCurrentMaxNoteboo * 6;
                for (int i = 0; i < noteboos.Length; ++i)
                {
                    noteboos[i].MultiCollect = true;
                    noteboos[i].MultiCollectTime = 3;
                }
            }
            //if (lappingHi[CurrentLap].LapMusikIntro != null) LapSound.QueueAudio(lappingHi[CurrentLap].LapMusikIntro);
            //if (lappingHi[CurrentLap].LapMusikLoop != null) LapSound.QueueAudio(lappingHi[CurrentLap].LapMusikLoop);
            bool HasLapMusicIntro = lappingHi[CurrentLap].LapMusikIntro != null;
            bool HasLapMusicLoop = lappingHi[CurrentLap].LapMusikLoop != null;
            AudioObjectyeah clip1 = HasLapMusicIntro ? lappingHi[CurrentLap].LapMusikIntro : lappingHi[CurrentLap].LapMusikLoop;

            if (LapRouletteTagThing == 2)
            {
                StartCoroutine(Crossfade(LapSound2, LapSound, clip1, 2f,true, HasLapMusicLoop ? true : false, lappingHi[CurrentLap].LapMusikLoop));
                LapRouletteTagThing = 0;
            }
            else StartCoroutine(Crossfade(LapSound, LapSound2, clip1, 2f,true, HasLapMusicLoop ? true : false, lappingHi[CurrentLap].LapMusikLoop));

            
        }
        if (lappingHi[CurrentLap].usesFlag) StartCoroutine(flagmove(lappingHi[CurrentLap].LapFlag));
        foreach (MuchoScript muc in GameControllerScript.Instance.muchscr) if (muc.isActiveAndEnabled) muc.MuchoSpeedScale += 0.1f;
    }

    [SerializeField] private GameControllerScript gc;
    public List<GameObject> LapPortals, elevatorTriggers = new List<GameObject>();
    public AudioObjectyeah[] RandomizedPrelapQueueOkBro;
    public GameObject LapFlag, mucho,tim, MeepTimer;
    public meepTimerScript Meeptimar;
    public Image lapFlagImages;
    public booksInteract[] noteboos;
    public AudioManagerLiveReaction LapSound,LapSound2,PrelapQueue1,PrelapQueue2;
    public AudioObjectyeah BellSoundLapping,PortalEnteringSound,PortalExitingSound,zawarudo;
    public int CurrentLap, MaxLap,LapRouletteTagThing,LapRouletteTagThingQueue, CurrentMaxNotebooks, realCurrentMaxNoteboo,FamishCheeseCount,Lap5CheeseCount,ChaosCheeseCount,Chaos3CheeseCount;
    public float scoreDecreaseTimer,Lap1OutroTimer;
    public bool inportalALREADY, h, allowClosElev, LapFamishShit, Lap5TimingStuff, vanishScore,lap1NearEndThingActive,randomPrelapQueue,isCrossfading,canActuallyPlayPrelap;
    public string[] stupidTextIntro,stupidTextLoop;
    public string stupidTextOutro;
    public lapVariablesStuff[] lappingHi;
    [Serializable]
	public class lapVariablesStuff
    {
        public bool usesFlag;
        public AudioObjectyeah LapMusikIntro,LapMusikLoop;
		public Sprite LapFlag;
	}
    public AudioObjectyeah LapMusikOutro;
    [Header("LAP 99")]
    public Sprite LapFlaninety;
    public AudioObjectyeah NinetynineIntro,NinetynineLoop,lightsoutohfuck;
    public bool NinetyNineToggle;
    private float cd;
    private AudioManagerLiveReaction CurAudioMan;
    [Header("goog mode")]
    public bool GoodMode;
}