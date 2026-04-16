using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using FluidMidi;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Video;
using UnityEngine.Audio;

public class GameControllerScript : MonoBehaviour
{
    
    #region SingletonSetup
    private void Awake() => Instance = this;
    public static GameControllerScript Instance;
    #endregion

    #region UnityCallbacks
    private void Start()
    {
        lbams = lowBudgetAudioManagementShit.Instance;
        InitializeGameSettings();
        UpdateNotebookCount();
    }
    private void FixedUpdate()
    {
        if (MusicGO && Time.timeScale != 0) nuzzlframes++;
    }

    private void Update()
    {
        if (!KF.gamePaused) FinaleModeAnnoyance();
        if (KF.gamePaused)
        {
            bal.SetActive(muchoing.isActiveAndEnabled);
            famishit.SetActive(famishScrpt.isActiveAndEnabled);
        }
        GameOverFunction();
        muchofinaleStuff();
        randomASSstuff();
    }
    #endregion
    public void randomASSstuff()
    {
        foreach (NPC enpeecee in GlobalNpcList)
        {
            enpeecee.agentSpeedScale = metalpipeStun ? 0f : 1f;
            enpeecee.SetToXrayLayer(ipleak);
        }
        foreach (GameObject npcmapicon in NpcMinimapIcon) npcmapicon.SetActive(ipleak ? true : false);
        foreach (GameObject dox in xrayObjectList) if (dox != null) dox.layer = !ipleak ? LayerMask.NameToLayer("npcLayer") : LayerMask.NameToLayer("npcXrayLayer");
        lbams.EscapeMusic.SetMute(!SecretEndingGot ? timeout : warrealest ? true : true);
        lbams.WarMusic.SetMute(!SecretEndingGot ? timeout : warrealest ? true : true);
        
        DiscordRPC_stuff.current.UpdateStatus(modeDetails, modeState, largeImagething, largeImageText);
    }
    public void muchofinaleStuff()
    {
        for (int i = 0; i < nuzzlesframeshit.Length; ++i) if (nuzzlframes == nuzzlesframeshit[i]) Singleton<VertexGlitchManager>.Instance.Glitch();
    }

    #region Initialization
    private void InitializeGameSettings()
    {
        vidplay.enabled = false;
        thatRawImageThatIHate.enabled = false;
        ItemManager.Instance.enabled = true;
        Singleton<OtherMainStuffManager>.Instance.UpdateInventoryLength();
        Singleton<OtherMainStuffManager>.Instance.ResizeAltInventory();
        Singleton<OtherMainStuffManager>.Instance.UpdateAltInventory();
        CharacterManagement.Instance.noiseiscallingpickupphone();
        foreach (PickupScript pick in FindObjectsOfType<PickupScript>())
        {
            ItemsToRespawn.Add(pick.transform.gameObject);
            if (pick.instahide)
            {
                pick.transform.gameObject.SetActive(false);
                pick.mapIconSprite.enabled = false;
            }
        }
        foreach (VendingMachineScript vend in FindObjectsOfType<VendingMachineScript>()) if (vend.isOutOfGoods) MachinesToRestock.Add(vend);
        war = PlayerPrefsExtension.GetBool("warreal");
        warrealest = war;
        Singleton<Options>.Instance.GetVolume();
        Singleton<Options>.Instance.GetVSync();

        Time.timeScale = 1f;
        AudioListener.pause = false;
        cullingMask = PlayerCamera.cullingMask;

        audioQueue = GetComponent<AudioManagerLiveReaction>();
        Math = GetComponent<LearningGameManager>();
        progress = GetComponent<EndingManager>();

        mode = PlayerPrefs.GetString("CurrentMode");
        if (mode == "LappingOfAsylum") LapManag.doShit();
        
        gameOverDelay = 0.5f;
        if (mode == "endless")
        {
            baldiScrpt.endless = true;
            sockCript.endless = true;
        }
        discordupdate();
        CirclAnimator.Rebind();
        CirclAnimator.SetTrigger("yooo");
        
    }
    public void discordupdate(string StateUpdateType = "chees")
    {
        if (StateUpdateType != "youwo")
        {
            bool ChaosMode = PlayerPrefsExtension.GetBool("Chaos");
            string chaosString = !ChaosMode ? "" : " - your local suffering enabled (Chaos Mode)";
            if (StateUpdateType == "chees")
            {
                modeState = notebooks + "/" + maxNotebooks + " Cheese Blocks | " + "Score: " + scoreSystemManager.Instance.scorevalue +" | " + "Ranks: " + scoreSystemManager.Instance.CurRank;
            }
            if (StateUpdateType == "exit")
            {
                modeState = exitsReached + "/" + maxExits + " Exits | " + "Score: " + scoreSystemManager.Instance.scorevalue +" | " + "Ranks: " + scoreSystemManager.Instance.CurRank;
            }
            if (mode == "endless")
            {
                modeDetails = "Endless Mode" + chaosString;
                largeImagething = "teacherjerproto";
                largeImageText = "hi i am teachr jery and im gonna smac you";
            }
            if (mode == "story")
            {
                modeDetails = "Story Mode" + chaosString;
                largeImagething = "teacherjerproto";
                largeImageText = "hi i am teachr jery and im gonna smac you";
            }
            if (mode == "famished")
            {
                modeDetails = "???????" + chaosString;
                largeImagething = "creepydarkmf";
                largeImageText = "*hungry ass noise intenstify*";
                if (exitsReached == 5)
                {
                    modeState = exitsReached + "/" + maxExits + " Exits" + " | its over..? | " + "Score: " + scoreSystemManager.Instance.scorevalue +" | " + "Ranks: " + scoreSystemManager.Instance.CurRank;
                }
            }
            if (mode == "wegaChallenge")
            {
                modeDetails = "holy shits its wega challenge" + chaosString;
                largeImagething = "van";
                largeImageText = "WEGA CHALLENGEEE";
            }
            if (mode == "zerullclassic")
            {
                bool chair = PlayerPrefsExtension.GetBool("BeatedUpZerull");
                modeDetails = chair ? "c  h  a  i  r" + chaosString: "???????" + chaosString;
                largeImagething = "van";
                largeImageText = chair ? "c  h  a  i  r" : "*CLASSIFIED INFO*";
            }
            if (mode == "LappingOfAsylum")
            {
                modeDetails = "lapping mode - lap " + LapManag.CurrentLap + " |"+ chaosString;
                largeImagething = "teacherjerproto";
                largeImageText = "the lapping grindset begin";
            }
        }
        else
        {
            modeDetails = "Win Screen";
            largeImagething = "levictory";
            largeImageText = "YOU WON LES GOOOO";
        }
    }
    public void musi()
    {
        if (mode == "story")
        {
            if (!warrealest)
            {
                //midishit1.Play();
                lbams.SchoolMusic.ClearQueue(true);
                lbams.SchoolMusic.SetLoop(true);
                lbams.SchoolMusic.QueueAudio(lbams.schoolClip);
            }
        }
    }
    
    #endregion

    #region NotebookManagement
    public void UpdateNotebookCount()
    {
        notebookCount.text = mode != "endless" ? $"{notebooks}/{maxNotebooks}" : $"{notebooks}";
        discordupdate("chees");

        if (mode == "endless" && notebooks / maxNotebooks > lastRespawnCount)
        {
            lastRespawnCount = notebooks / maxNotebooks;
            EndlessModeRestart();
        }
        if (notebooks == maxNotebooks && mode != "endless" && mode != "LappingOfAsylum" && !finaleMode) ActivateFinaleMode();
    }

    public void CollectNotebook(float numberOfNotebooks)
    {
        notebooks += Mathf.FloorToInt(numberOfNotebooks);
        if (mode != "LappingOfAsylum")if (LapManag.Meeptimar.isActiveAndEnabled) meepTimerScript.Instance.AddTime(25f,Color.green);
        UpdateNotebookCount();
        bool ChaosMode = PlayerPrefsExtension.GetBool("Chaos");
        if (ChaosMode)
        {
            if (mode == "story")
            {
                foreach (GameObject obj in ObjectsToEnable)
                {
                    if (obj != null)
                    {
                        GameObject clone = Instantiate(obj, obj.transform.position, obj.transform.rotation);
                        clone.name = obj.name;
                        clone.SetActive(true);
                    }
                }
            }
            if (mode == "famished")
            {
                if (fmc.butch != null)
                {
                    GameObject clone = Instantiate(fmc.butch, fmc.butch.transform.position, fmc.butch.transform.rotation);
                    clone.name = fmc.butch.name;
                    clone.GetComponent<FamishedScript>().famishedSpd = 0.5f;
                    clone.SetActive(true);
                }
            }
            if (mode == "zerullclassic")
            {
                if (zerull.zer != null)
                {
                    GameObject clone = Instantiate(zerull.zer, zerull.zer.transform.position, zerull.zer.transform.rotation);
                    clone.name = zerull.zer.name;
                    clone.GetComponent<zerullscript>().Anger = 0.5f;
                    clone.SetActive(true);
                }
            }
        }
    }
    #endregion

    #region HelperFunctions
    private void EndlessModeRestart()
    {
        ItemsToRespawn.ForEach(item => item.SetActive(true));
        MachinesToRestock.ForEach(machine => machine?.RestockVendingMachine());
    }
    #endregion

    #region SpoopModeHandling


    public IEnumerator meeptimerwai()
    {
        if (mode == "story")
        {
            if (warrealest)
            {
                yield return new WaitForSeconds(1);
                LapManag.MeepTimer.SetActive(true);
                lbams.WarMusic.ClearQueue(true);
                lbams.WarMusic.SetLoop(true);
                lbams.WarMusic.QueueAudio(lbams.WAR);
            }
        }
    }
    public void ActivateSpoopMode(bool frompad = false)
    {
        spoopMode = true;
        if (mode == "story")
        {
            //Singleton<TimeOutManagerFUCKYEA>.Instance.InitializeTimeoutStuff(600f);
            ObjectsToDisable.ForEach(o => o.SetActive(false));
            ObjectsToEnable.ForEach(o => o.SetActive(true));
            if (warrealest)
            {
                StartCoroutine(easingExit(new Color(0.9803922f, 0.5019608f, 0.4470589f, 1f), 0, 2, 5));
                StartCoroutine(meeptimerwai());
            }
        }
        if (mode != "zerullclassic" && mode != "LappingOfAsylum" || mode != "LappingOfAsylum" && mode != "zerullclassic") Gatesrea.ForEach(g => g.Down());

        //midishit1.Stop();
        lbams.SchoolMusic.ClearQueue(true);
        Math.learnMusic.ClearQueue(true);

        if (AdditionalGameCustomizer.Instance != null && !AdditionalGameCustomizer.Instance.NoYCTP && mode == "story")Math.learnMusic.PlaySingleClip(lowBudgetAudioManagementShit.Instance.hangAudio);
        else lbams.SchoolMusic.ClearQueue(true);
        if (frompad)
        {
            foreach (subtitlesScriptReal Subtit in FindObjectsOfType<subtitlesScriptReal>())
            Subtit.hidesub = true;
        }
    }
    #endregion
    bool onetimeupdate = false;

    #region GameOverLogic
    private void GameOverFunction()
    {
        if (!player.gameOver) return;
        if (player.killedbyhim)
        {
            if (!onetimeupdate)
            {
                gameOverDelay = 7f;
                Singleton<VertexGlitchManager>.Instance.ShakeGlitch();
                Singleton<VertexGlitchManager>.Instance.mustGlitch = true;
                onetimeupdate = true;
            }
            if (ZerullClassic.Instance.realBossStarted) Singleton<MusicManagerMaes>.Instance.HangMidi(true);
            if (!lbams.GameOverSource.audioDevice.isPlaying) lbams.GameOverSource.PlaySingleClip(lbams.zerullGameover);
            AdditionalGameCustomizer.Instance.FovAmmount = 120;
        }
        if (player.jumpropes.Count > 0) player.jumpropes[0].End(false);
        AudioListener.pause = true;
        lbams.GameOverSource.SetIgnoreListenerPause(true);
        Time.timeScale = 0f;

        PlayerCamera.farClipPlane = gameOverDelay * 400f;
        gameOverDelay -= Time.unscaledDeltaTime;
        Singleton<TimeOutManagerFUCKYEA>.Instance.ResetTimeoutStuff();
        if (Singleton<VertexGlitchManager>.Instance.mustGlitch) Singleton<VertexGlitchManager>.Instance.mustGlitch = false;

        if (!lbams.GameOverSource.audioDevice.isPlaying && !player.killedbyhim)
        {
            audOverVal = (int)UnityEngine.Random.Range(0f, lbams.LoseSounds.Length);
            lbams.GameOverSource.PlaySingleClip(lbams.LoseSounds[audOverVal]);
        }
        if (mode == "endless" && notebooks > PlayerPrefs.GetInt("HighBooks") && !highScoreText.activeSelf) highScoreText.SetActive(true);
        if (gameOverDelay <= 0f)
        {
            if (mode == "endless")
            {
                if (notebooks > PlayerPrefs.GetInt("HighBooks")) PlayerPrefs.SetInt("HighBooks", notebooks);
                PlayerPrefs.SetInt("CurrentBooks", notebooks);
            }
            if (player.killedbyhim)
            {
                Singleton<VertexGlitchManager>.Instance.mustGlitch = false;
                Application.Quit();
            }
            Time.timeScale = 1f;
            SceneManager.LoadScene(gameoverScene);
        }
    }
    #endregion
    private float t;
    #region FinaleModeManagement
    private void ActivateFinaleMode()
    {
        for (int i = 0; i < AdditionalGameCustomizer.Instance.ExitImages.Length; ++i) StartCoroutine(tweeniconSolo(Color.black, 0, 1, 1f, i));
        
        if (mode == "story")
        {
            if (AdditionalGameCustomizer.Instance != null)
            {
                switch (AdditionalGameCustomizer.Instance.EscapeMusicFunsies)
                {
                    case AdditionalGameCustomizer.EscapeFunsies.BBCR:
                        ElevdorRea.ForEach(ed => ed.Opendor = true);
                        finaleMode = true;
                        break;
                    case AdditionalGameCustomizer.EscapeFunsies.Daldi:
                        ElevdorRea.ForEach(ed => ed.Opendor = true);
                        finaleMode = true;
                        break;
                    case AdditionalGameCustomizer.EscapeFunsies.Taldi:
                        ElevdorRea.ForEach(ed => ed.Opendor = true);
                        finaleMode = true;
                        break;
                }
            }
            if (timeout) finaleMode = true;
        }
        if (mode != "story")
        {
            ElevdorRea.ForEach(ed => ed.Opendor = true);
            finaleMode = true;
        }
        FinaleObjectToDisable.ForEach(o => o.SetActive(false));
    }

    public IEnumerator ambatudaldi()
    {
        if (mode == "story")
        {
            //Singleton<VertexGlitchManager>.Instance.mustGlitch = true;
            lbams.EscapeMusic.ClearQueue(true);
            lbams.EscapeMusic.QueueAudio(lbams.shithourIntro);
            //MusicGO = true;
            yield return new WaitForSeconds(lbams.shithourIntro.audClip.length);
            lbams.EscapeMusic.ClearQueue(true);
            lbams.EscapeMusic.SetLoop(true);
            lbams.EscapeMusic.QueueAudio(lbams.shithourLoop);
            
            //Singleton<VertexGlitchManager>.Instance.sourceToSyncIn = lbams.EscapeMusic.audioDevice;
        }
    }
    public IEnumerator basicShowMusicShit()
    {
        if (mode == "story")
        {
            lbams.EscapeMusic.ClearQueue(true);
            lbams.EscapeMusic.QueueAudio(lbams.NormalTbsFinale[0]);
            yield return new WaitForSeconds(lbams.NormalTbsFinale[0].audClip.length);
            ElevdorRea.ForEach(ed => ed.Opendor = true);
            Gatesrea.ForEach(g => g.Down(false));
            lbams.EscapeMusic.ClearQueue(true);
            lbams.EscapeMusic.SetLoop(true);
            lbams.EscapeMusic.QueueAudio(lbams.NormalTbsFinale[1]);
            yield return new WaitForSeconds(0.1f);
            finaleMode = true;
        }
    }
    public IEnumerator tiemoutStu()
    {
        yield return new WaitForSeconds(LearningGameManager.Instance.Television.Markings ? 3f : 0.75f);
        StartCoroutine(easingExit(new Color(0.45f, 0.45f, 0.45f, 1f), 0, 2, 5));
        Singleton<TimeOutManagerFUCKYEA>.Instance.spamupdatethese =true;
        lbams.TimeoutMusic.ClearQueue(true);
        lbams.TimeoutMusic.SetLoop(true);
        lbams.TimeoutMusic.QueueAudio(lbams.timeoutMusicAud);
    }
    private void isvidfinished(VideoPlayer vp)
	{
        Time.timeScale = 1;
        MainHudFade.Rebind();
        MainHudFade.Play("hudFadeIn", -1, 0f);
        RainbowHudFade.Rebind();
        RainbowHudFade.Play("hudFadeInRainb", -1, 0f);
        SubtitlesHudFade.Rebind();
        SubtitlesHudFade.Play("hudFadeInsubs", -1, 0f);
        ZerullClassic.Instance.yourflashbang.Rebind();
        ZerullClassic.Instance.yourflashbang.Play("flashAnim", -1, 0f);
        lbams.EscapeMusic.ClearQueue(true);
        lbams.EscapeMusic.SetLoop(true);
        lbams.EscapeMusic.QueueAudio(lbams.NormalTbsFinale[4]);
        
        vidplay.enabled = false;
        thatRawImageThatIHate.enabled = false;
        youCantPause = false;
        AudioListener.pause = false;
	}


    private void FinaleModeAnnoyance()
    {
        if (mode == "story")
        {
            if (!finaleMode || lbams.ChaosAudioSource.audioDevice.isPlaying) return;

            if (exitsReached == 3)
            {
                if (AdditionalGameCustomizer.Instance != null)
                {
                    switch (AdditionalGameCustomizer.Instance.EscapeMusicFunsies)
                    {
                        case AdditionalGameCustomizer.EscapeFunsies.BBCR:
                            lbams.ChaosAudioSource.ClearQueue(true);
                            lbams.ChaosAudioSource.SetLoop(true);
                            lbams.ChaosAudioSource.QueueAudio(lbams.ChaosStartLoop);
                            
                            break;
                        case AdditionalGameCustomizer.EscapeFunsies.TBS:
                            lbams.ChaosAudioSource.ClearQueue(true);
                            lbams.ChaosAudioSource.SetLoop(true);
                            lbams.ChaosAudioSource.QueueAudio(lbams.ChaosStartLoop);
                            break;
                    }
                }
            }
            else if (exitsReached == 5 && !progress.GetSecret & !progress.GetResults)
            {
                if (AdditionalGameCustomizer.Instance != null)
                {
                    switch (AdditionalGameCustomizer.Instance.EscapeMusicFunsies)
                    {
                        case AdditionalGameCustomizer.EscapeFunsies.BBCR:
                            lbams.ChaosAudioSource.ClearQueue(true);
                            lbams.ChaosAudioSource.SetLoop(true);
                            lbams.ChaosAudioSource.QueueAudio(lbams.ChaosFinal);
                            break;
                        case AdditionalGameCustomizer.EscapeFunsies.TBS:
                            lbams.ChaosAudioSource.ClearQueue(true);
                            lbams.ChaosAudioSource.SetLoop(true);
                            lbams.ChaosAudioSource.QueueAudio(lbams.ChaosStartLoop);
                            break;
                    }
                }
            }
        }
    }
    #endregion

    #region ExitCounterHandling
    public void ExitReached(int eIds)
    {
        exitsReached++;
        if (mode == "famished") fmc.manualUpdate();
        if (mode == "zerullclassic") zerull.jusUpdatebr();
        StopCoroutine(tweeniconSolo(Color.white, 0, 1, 0.5f, eIds));
        StartCoroutine(tweeniconSolo(Color.white, 0, 1, 0.5f, eIds));

        if (mode == "story")
        {
            if (!FinaleSecret)
            {
                if (exitEasingCoroutine != null) StopCoroutine(exitEasingCoroutine);
                if (AdditionalGameCustomizer.Instance != null)
                {
                    switch (AdditionalGameCustomizer.Instance.EscapeMusicFunsies)
                    {
                        case AdditionalGameCustomizer.EscapeFunsies.BBCR:
                            exitEasingCoroutine = StartCoroutine(easingExit(new Color(1f, 0.7f / exitsReached, 0.7f / exitsReached, 1f), 0, 2, 5));
                            break;
                        case AdditionalGameCustomizer.EscapeFunsies.TBS:
                            exitEasingCoroutine = StartCoroutine(easingExit(new Color(1f, 0.7f / exitsReached, 0.7f / exitsReached, 1f), 0, 2, 5));
                            break;
                    }
                }
            }
        }


        if (exitsReached == 1)
        {
            if (mode == "story")
            {
                if (!FinaleSecret)
                {
                    if (AdditionalGameCustomizer.Instance != null)
                    {
                        switch (AdditionalGameCustomizer.Instance.currentSkybox)
                        {
                            case AdditionalGameCustomizer.SkyboxStyle.Day:
                                RenderSettings.skybox = AdditionalGameCustomizer.Instance.NormalRedSky;
                                break;
                            case AdditionalGameCustomizer.SkyboxStyle.Sunset:
                                RenderSettings.skybox = AdditionalGameCustomizer.Instance.RedTwilightSky;
                                break;
                            case AdditionalGameCustomizer.SkyboxStyle.Night:
                                RenderSettings.skybox = AdditionalGameCustomizer.Instance.RedNightSky;
                                break;
                        }
                    }
                    if (AdditionalGameCustomizer.Instance != null)
                    {
                        switch (AdditionalGameCustomizer.Instance.EscapeMusicFunsies)
                        {
                            case AdditionalGameCustomizer.EscapeFunsies.BBCR:
                                lbams.EscapeMusic.SetPitch(0.9f);
                                break;
                        }
                    }
                }
                if (FinaleSecret)
                {
                    lbams.EscapeMusic.ClearQueue(true);
                    lbams.EscapeMusic.SetLoop(true);
                    lbams.EscapeMusic.QueueAudio(lbams.EvapV2Finale[1]);
                    lbams.EscapeMusic.QueueAudio(lbams.EvapV2FinaleIntros[1]);
                }
            }
        }
        if (exitsReached == 2)
        {
            if (mode == "story")
            {
                if (!FinaleSecret)
                {
                    if (AdditionalGameCustomizer.Instance != null)
                    {
                        switch (AdditionalGameCustomizer.Instance.EscapeMusicFunsies)
                        {
                            case AdditionalGameCustomizer.EscapeFunsies.BBCR:
                                lbams.EscapeMusic.SetPitch(0.75f);
                                lbams.ChaosAudioSource.ClearQueue(true);
                                lbams.ChaosAudioSource.SetLoop(true);
                                lbams.ChaosAudioSource.QueueAudio(lbams.ChaosStart);
                                
                                break;
                            case AdditionalGameCustomizer.EscapeFunsies.TBS:
                                lbams.EscapeMusic.ClearQueue(true);
                                lbams.EscapeMusic.SetLoop(true);
                                lbams.EscapeMusic.QueueAudio(lbams.NormalTbsFinale[2]);
                                lbams.ChaosAudioSource.ClearQueue(true);
                                lbams.ChaosAudioSource.SetLoop(true);
                                lbams.ChaosAudioSource.QueueAudio(lbams.ChaosStart);
                                break;
                        }
                    }
                }
                if (FinaleSecret)
                {
                    lbams.EscapeMusic.ClearQueue(true);
                    lbams.EscapeMusic.SetLoop(true);
                    lbams.EscapeMusic.QueueAudio(lbams.EvapV2Finale[2]);
                    lbams.EscapeMusic.QueueAudio(lbams.EvapV2FinaleIntros[2]);
                }
            }
        }
        if (exitsReached == 3)
        {
            if (mode == "story")
            {
                
                if (!FinaleSecret)
                {
                    if (AdditionalGameCustomizer.Instance != null)
                    {
                        switch (AdditionalGameCustomizer.Instance.EscapeMusicFunsies)
                        {
                            case AdditionalGameCustomizer.EscapeFunsies.BBCR:
                                lbams.ChaosAudioSource.ClearQueue(true);
                                lbams.ChaosAudioSource.SetLoop(true);
                                lbams.ChaosAudioSource.QueueAudio(lbams.ChaosStartLoop);
                                lbams.EscapeMusic.SetPitch(0.6f);
                                StartCoroutine(easingExit(new Color(1f / (exitsReached / 1.75f), 0.7f / exitsReached, 0.7f / exitsReached, 1f), 0, 2, 5));
                                break;
                            case AdditionalGameCustomizer.EscapeFunsies.TBS:
                                VideoFade.Rebind();
                                VideoFade.Play("VidFadein", -1, 0f);
                                MainHudFade.Rebind();
                                MainHudFade.Play("hudFadeOutNearly", -1, 0f);
                                RainbowHudFade.Rebind();
                                RainbowHudFade.Play("hudFadeOutRainb", -1, 0f);
                                SubtitlesHudFade.Rebind();
                                SubtitlesHudFade.Play("hudFadeOutsubs", -1, 0f);
                                youCantPause = true;
                                vidplay.enabled = true;
                                AudioListener.pause = true;
                                thatRawImageThatIHate.enabled = true;
                                vidplay.Play();
                                vidplay.loopPointReached += isvidfinished;
                                Time.timeScale = 0;
                                lbams.ChaosAudioSource.ClearQueue(true);
                                lbams.ChaosAudioSource.SetLoop(true);
                                lbams.ChaosAudioSource.QueueAudio(lbams.ChaosStartLoop);
                                StartCoroutine(easingExit(new Color(1f / (exitsReached / 1.75f), 0.7f / exitsReached, 0.7f / exitsReached, 1f), 0, 2, 5));
                                break;
                        }
                    }
                }
                if (FinaleSecret)
                {
                    lbams.EscapeMusic.ClearQueue(true);
                    lbams.EscapeMusic.SetLoop(true);
                    lbams.EscapeMusic.QueueAudio(lbams.EvapV2Finale[3]);
                    AdditionalGameCustomizer.Instance.donthaveanamelmfao = AdditionalGameCustomizer.Instance.darkencanva;
                }
            }
        }
        if (exitsReached == 4)
        {
            if (mode == "story")
            {
                if (!FinaleSecret)
                {
                    if (AdditionalGameCustomizer.Instance != null)
                    {
                        switch (AdditionalGameCustomizer.Instance.EscapeMusicFunsies)
                        {
                            case AdditionalGameCustomizer.EscapeFunsies.BBCR:
                                lbams.EscapeMusic.SetPitch(0.45f);
                                StartCoroutine(easingExit(new Color(1f / (exitsReached / 1.5f), 0.7f / exitsReached, 0.7f / exitsReached, 1f), 0, 2, 5));
                                lbams.ChaosAudioSource.ClearQueue(true);
                                lbams.ChaosAudioSource.SetLoop(true);
                                lbams.ChaosAudioSource.QueueAudio(lbams.ChaosBuildUp);
                                break;
                            case AdditionalGameCustomizer.EscapeFunsies.TBS:
                                StartCoroutine(easingExit(new Color(1f / (exitsReached / 1.5f), 0.7f / exitsReached, 0.7f / exitsReached, 1f), 0, 2, 5));
                                lbams.ChaosAudioSource.ClearQueue(true);
                                lbams.ChaosAudioSource.SetLoop(true);
                                lbams.ChaosAudioSource.QueueAudio(lbams.ChaosBuildUp);
                                break;
                        }
                    }
                }
                if (FinaleSecret)
                {
                    lbams.EscapeMusic.ClearQueue(true);
                    lbams.EscapeMusic.SetLoop(true);
                    lbams.EscapeMusic.QueueAudio(lbams.EvapV2Finale[4]);
                    lbams.EscapeMusic.QueueAudio(lbams.EvapV2FinaleIntros[3]);
                    AdditionalGameCustomizer.Instance.donthaveanamelmfao = AdditionalGameCustomizer.Instance.canvascolormain;
                }
            }
        }
        if (exitsReached == 5)
        {
            if (mode == "story")
            {
                if (!FinaleSecret)
                {
                    if (AdditionalGameCustomizer.Instance != null)
                    {
                        switch (AdditionalGameCustomizer.Instance.EscapeMusicFunsies)
                        {
                            case AdditionalGameCustomizer.EscapeFunsies.BBCR:
                                lbams.EscapeMusic.SetPitch(0.3f);
                                StartCoroutine(easingExit(new Color(1f / (exitsReached / 1.25f), 0.7f / exitsReached, 0.7f / exitsReached, 1f), 0, 2, 5));
                                break;
                            case AdditionalGameCustomizer.EscapeFunsies.TBS:
                                lbams.EscapeMusic.ClearQueue(true);
                                
                                lbams.EscapeMusic.QueueAudio(lbams.NormalTbsFinaleIntros[0]);
                                lbams.EscapeMusic.QueueAudio(lbams.NormalTbsFinale[5]);
                                lbams.EscapeMusic.SetLoop(true);
                                StartCoroutine(easingExit(new Color(1f / (exitsReached / 1.25f), 0.7f / exitsReached, 0.7f / exitsReached, 1f), 0, 2, 5));
                                break;
                        }
                    }
                }
                if (FinaleSecret)
                {
                    lbams.EscapeMusic.ClearQueue(true);
                    lbams.EscapeMusic.QueueAudio(lbams.EvapV2Finale[5]);
                    lbams.EscapeMusic.QueueAudio(lbams.EvapV2FinaleIntros[4]);
                    lbams.EscapeMusic.SetLoop(true);
                }
            }
        }
        discordupdate("exit");
    }

    public IEnumerator easingExit(Color kolor, float a, float b, float duration)
    {
        Color start = RenderSettings.ambientLight;

        for (float t = a; t < b; t += Time.deltaTime / duration)
        {
            RenderSettings.ambientLight = Color.Lerp(start, kolor, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        RenderSettings.ambientLight = kolor;
    }
    public IEnumerator tweeniconSolo(Color kolor, float a, float b, float duration,int icon)
    {
        Color start = AdditionalGameCustomizer.Instance.ExitImages[icon].color;
        for (float t = a; t < b; t += Time.deltaTime / duration)
        {
            AdditionalGameCustomizer.Instance.ExitImages[icon].color = Color.Lerp(start, kolor, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }
        AdditionalGameCustomizer.Instance.ExitImages[icon].color = kolor;
    }
    #endregion
    #region PlayerTeleportation
    public void CraftersTeleport()
    {
        if (player.invisichalk) player.invisichalk = false;
        if (player.alsoInOffice) player.alsoInOffice = false;
        if (player.outdoorsfr) player.outdoorsfr = false;
        if (isHiding) isHiding = false;
        if (player.hugging)
        {
            player.hugging = false;
            player.sweepingFailsave = 0f;
        }
        if (player.jumpropes.Count > 0) player.jumpropes[0].End(false);

        var playPos = AILocationSelector.SetNewTargetForAgent(null, "default") + Vector3.up * player.height;
        player.transform.position = playPos;
        float safeDistance = 15.0f;
        int attempts = 0;
        foreach (MuchoScript muc in muchscr)
        {
            Vector3 mucpos;

            do
            {
                mucpos = AILocationSelector.SetNewTargetForAgent(null, "default") + Vector3.up * player.height;
                attempts++;
            }

            while (Vector3.Distance(playPos, mucpos) < safeDistance && attempts < 10);
            muc.transform.position = mucpos;
        }
    }

    public IEnumerator TeleporterFunction(string thing = "normal")
    {
        player.titlecard = true;
        player.movementLocked = true;
        playerCollider.enabled = false;
        if (player.invisichalk) player.invisichalk = false;
        if (player.alsoInOffice) player.alsoInOffice = false;
        if (player.outdoorsfr) player.outdoorsfr = false;
        if (isHiding) isHiding = false;
        if (player.hugging)
        {
            player.hugging = false;
            player.sweepingFailsave = 0f;
        }
        if (player.jumpropes.Count > 0) player.jumpropes[0].End(false);

        int teleports = UnityEngine.Random.Range(thing == "normal" ? 40 : thing == "evilleaf" ? 25 : 20, thing == "normal" ? 50 : thing == "evilleaf" ? 35 : 20);
        float delay = 0.05f;
        const float increaseFactor = 1.04f;

        for (int i = 0; i < teleports; i++)
        {
            yield return new WaitForSeconds(delay);
            PlayerTeleport(thing);
            delay *= increaseFactor;
        }

        player.titlecard = false;
        player.movementLocked = false;
        playerCollider.enabled = true;
    }
    public IEnumerator funnyportal()
    {
        //MessageBox.Show("this is streamer build exclusive, will be removed from actual release hah", "hi just so you know", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        EndingManager.Instance.black.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        Singleton<TimeOutManagerFUCKYEA>.Instance.ResetTimeoutStuff();
        AudioListener.pause = false;
        Singleton<MusicManagerMaes>.Instance.PauseMidi(false);
        PlayerPrefs.SetString("CurrentMode", "famished");
        PlayerPrefsExtension.SetBool("famishUnlockyippe", true);
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadSceneAsync("GameArea");
    }

    private void PlayerTeleport(string type)
    {
        player.transform.position = AILocationSelector.SetNewTargetForAgent(null, "default") + Vector3.up * player.height;
        AudioObjectyeah soundaaa = type == "normal" ? lowBudgetAudioManagementShit.Instance.TeleporterTp : type == "evilleaf" ? lowBudgetAudioManagementShit.Instance.evilLeafTP : lowBudgetAudioManagementShit.Instance.TeleporterTp;
        lowBudgetAudioManagementShit.Instance.TpSource.PlaySingleClip(soundaaa);
    }
    #endregion
    [Serializable]
    public class modifiersName
    {
        [SerializeField]
        public bool enabled;
        [SerializeField]
        public string name;
    }
    #region SerializedFields
    public VideoPlayer vidplay;
    public RawImage thatRawImageThatIHate;
    public AudioMixerGroup[] MixerOverrideGlobalson;
    [Header("Target Materials")]
    public List<Material> targetMaterials = new List<Material>();
    public List<Material> MaterialsThatNeedSpecialCare = new List<Material>();

    [Header("Prefab Instances")]
    public GameObject learnpadmuehehe;
    public GameObject popparti,ConfettiEffect;

    [Header("Player & Camera References")]
    public PlayerScript player;
    public Transform cameraTransform;
    public Camera PlayerCamera;
    public Collider playerCollider;
    public CharacterController playerCharacter;

    [Header("Scripts")]
    public KeyFunctions KF;
    public BaldiScript baldiScrpt;
    public zerullscript zerulscrpt;
    public FamishedScript famishScrpt;
    public MuchoScript muchoing;
    public CraftersScript sockCript;
    public PlaytimeScript playtimeScript;
    public FirstPrizeScript firstPrizeScript;
    public PrincipalScript principal;
    public MaxcipalScript maxplayGames;
    public MouseAppearingScript mousescript;
    public VoxelLightingMain voxLight;
    [SerializeField] private AILocationSelectorScript AILocationSelector;

    [Header("Game Mode & Settings")]
    public string mode;
    public string ExclusiveRoute;
    public int notebooks, maxNotebooks, maxExits, UnlockAmount, SlotsAmmount, CharacterIntVal;
    public bool debugMode, isHiding, MusicGO,youCantPause,metalpipeStun,ipleak;
    [SerializeField] private string gameoverScene;

    [Header("Serialized References")]
    public TMP_Text notebookCount;
    [SerializeField] public GameObject highScoreText, baldi, tutorobj;
    public List<GameObject> ObjectsToEnable,npcCloneList,xrayObjectList = new List<GameObject>();
    public List<GateScript> Gatesrea = new List<GateScript>();
    public List<ElvDoorScript> ElevdorRea = new List<ElvDoorScript>();
    public List<GameObject> FinaleObjectToDisable, ObjectsToDisable, ItemsToRespawn,NpcMinimapIcon = new List<GameObject>();
    public List<VendingMachineScript> MachinesToRestock = new List<VendingMachineScript>();
    public List<NPC> GlobalNpcList = new List<NPC>();
    public Animator Icon,CirclAnimator;
    public Material SpriteRenderer;
    public Sprite Present;
    public modifiersName[] Modifiers;

    [Header("Audio References")]
    /*public AudioClip[] LoseSounds;
    public AudioClip ZerullLoseSound;
    public AudioClip[] HurtSounds;
    public AudioClip[] EvapV2FinaleTypeShit, NormalTbsFinale;
    public AudioSource[] EvapV2FinaleSounSource;
    public AudioSource audioDevice, audioDevice2, schoolMusic, escapeMusic, gamaOvarDevice,warmusic,TimeoutMusic,TpSoundSource;
    public AudioClip aud_Hang, aud_Rattling, aud_Unlocked, aud_ItemCollect, SchoolhouseEscape,TaldiEscape, shithourIntro, shithourLoop, aud_Collected, aud_ChaosStart, aud_ChaosStartLoop, aud_ChaosBuildUp, aud_ChaosFinal, aud_Teleport, aud_EvilLeafyTP, deathbell,gambling, punchsoun, totem,loboto, gastervanish,monesound,LoudIncorecBugger,timeoutMusicAud,deltaexplode,agonyscream,gastersfx;
    */
    #endregion

    #region PrivateFields
    public AudioManagerLiveReaction audioQueue;
    private int audOverVal;
    [HideInInspector] public lowBudgetAudioManagementShit lbams;
    public float[] nuzzlesframeshit;
    [SerializeField] private float gameOverDelay, nuzzlframes;
    public int lastRespawnCount, failedNotebooks, exitsReached, cullingMask;
    public bool spoopMode, finaleMode, FinaleSecret,war,warrealest,timeout,SecretEndingGot,PadSEToggle;
    [HideInInspector] public Coroutine exitEasingCoroutine;
    [HideInInspector] public LearningGameManager Math;
    [HideInInspector] public EndingManager progress;
    #endregion

    [Header("modes stuf")]
    #region ModesStuff
    public FamishedModeController fmc;
    public ZerullClassic zerull;
    public LappingOfAsylumController LapManag;
    public wegachallenge wegchal;
    #endregion
    [Header("silly stuff")]
    public GameObject TETOOOOO;
    public GameObject train,dimcraab;
    [Header("casual pause menu stuff")]
    public GameObject bal;
    public GameObject moie, famishit;
    [Header("cool animators stuff")]
    public Animator VideoFade;
    public Animator MainHudFade,RainbowHudFade,SubtitlesHudFade;

    [Header("math machine stuff")]
    public int numOfBall;
    public bool isHoldingBall;
    public GameObject PickBall;
    [Header("discord stuff")]
    public string modeDetails;
    public string modeState, largeImagething, largeImageText;
    [Header("teachers management and stuff")]
    public List<BullyScript> buliScr = new List<BullyScript>();
    public List<PrincipalScript> prinScr = new List<PrincipalScript>();
    public List<MaxcipalScript> maxiScr = new List<MaxcipalScript>();
    public List<coolSkeleton97Scrip> cs97Scr = new List<coolSkeleton97Scrip>();
    public List<BaldiScript> balscr = new List<BaldiScript>();
    public List<zerullscript> zerscr = new List<zerullscript>();
    public List<MuchoScript> muchscr = new List<MuchoScript>();
    public List<FamishedScript> famishscr = new List<FamishedScript>();
}