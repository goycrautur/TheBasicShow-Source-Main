using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using FluidMidi;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using System;

public class GameControllerScript : MonoBehaviour
{
    #region SingletonSetup
    private void Awake() => Instance = this;
    public static GameControllerScript Instance;
    #endregion

    #region UnityCallbacks
    private void Start()
    {
        
        InitializeGameSettings();
        UpdateNotebookCount();
        Singleton<OtherMainStuffManager>.Instance.slot();
        Shader.SetGlobalFloat("_VertexGlitchIntensity", 0f);
        Shader.SetGlobalFloat("_VertexGlitchSeed", 0f);
    }
    private void FixedUpdate()
    {
        if (MusicGO && Time.timeScale != 0)
        {
            nuzzlframes++;
        }
    }

    private void Update()
    {
        if (!KF.gamePaused)
        {
            FinaleModeAnnoyance();
        }
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
        foreach (NPC enpeecee in NPCThatGetAffectedByMetalPipe)
        {
            enpeecee.agentSpeedScale = metalpipeStun ? 0f : 1f;
        }
        foreach (GameObject npcmapicon in NpcMinimapIcon)
        {
            npcmapicon.SetActive(ipleak ? true : false);
        }
        escapeMusic.mute = timeout;
        warmusic.mute = timeout;
        for (int i = 0; i < EvapV2FinaleSounSource.Length; ++i)
        {
            if (EvapV2FinaleSounSource[i] != null)
            {
                EvapV2FinaleSounSource[i].mute = timeout;
            }
        }
        if (maxNotebooks == failedNotebooks && !warrealest)
        {
            FinaleSecret = true;
        }
        DiscordRPC_stuff.current.UpdateStatus(modeDetails, modeState, largeImagething, largeImageText);
    }
    public void muchofinaleStuff()
    {
        for (int i = 0; i < nuzzlesframeshit.Length; ++i)
        {
            if (nuzzlframes == nuzzlesframeshit[i])
            {
                Singleton<VertexGlitchManager>.Instance.Glitch();
            }
        }
    }

    #region Initialization
    private void InitializeGameSettings()
    {
        war = PlayerPrefsExtension.GetBool("warreal");
        warrealest = war;
        Singleton<Options>.Instance.GetVolume();
        Singleton<Options>.Instance.GetVSync();

        Time.timeScale = 1f;
        AudioListener.pause = false;
        cullingMask = PlayerCamera.cullingMask;

        audioQueue = GetComponent<AudioQueueScript>();
        Math = GetComponent<LearningGameManager>();
        progress = GetComponent<EndingManager>();

        mode = PlayerPrefs.GetString("CurrentMode");
        if (mode == "LappingOfAsylum")
        {
            LapManag.doShit();
        }
        
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
                schoolMusic.Play();
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

        if (notebooks == maxNotebooks && mode != "endless" && mode != "LappingOfAsylum" && !finaleMode)
        {
            ActivateFinaleMode();
        }
    }

    public void CollectNotebook(float numberOfNotebooks)
    {
        notebooks += Mathf.FloorToInt(numberOfNotebooks);
        if (mode != "LappingOfAsylum")
        {
            if (LapManag.Meeptimar.isActiveAndEnabled)
            {
                meepTimerScript.Instance.AddTime(25f,Color.green);
            }
        }
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
    public void GetAngry(float value)
    {
        if (!spoopMode)
        {
            ActivateSpoopMode();
        }
        baldiScrpt.GetAngry(value);
    }

    public IEnumerator meeptimerwai()
    {
        if (mode == "story")
        {
            if (warrealest)
            {
                yield return new WaitForSeconds(1);
                LapManag.MeepTimer.SetActive(true);
                warmusic.Play();
            }
        }
    }
    public void ActivateSpoopMode()
    {
        spoopMode = true;
        if (mode == "story")
        {
            Singleton<TimeOutManagerFUCKYEA>.Instance.InitializeTimeoutStuff(600f);
            ObjectsToDisable.ForEach(o => o.SetActive(false));
            ObjectsToEnable.ForEach(o => o.SetActive(true));
            if (warrealest)
            {
                StartCoroutine(easingExit(new Color(0.9803922f, 0.5019608f, 0.4470589f, 1f), 0, 2, 5));
                StartCoroutine(meeptimerwai());
            }
        }
        /*if (mode != "zerullclassic" && mode != "LappingOfAsylum" || mode != "LappingOfAsylum" && mode != "zerullclassic")
        {
            Gatesrea.ForEach(g => g.Down());
        }*/

        //midishit1.Stop();
        schoolMusic.Stop();
        Math.learnMusic.Stop();

        if (AdditionalGameCustomizer.Instance != null && !AdditionalGameCustomizer.Instance.NoYCTP && mode == "story")
        {
            Math.learnMusic.PlayOneShot(aud_Hang);
        }
        else
        {
            StartCoroutine(audioQueue.FadeOut(schoolMusic, 2f));
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
                onetimeupdate = true;
            }
            if (ZerullClassic.Instance.realBossStarted)
            {
            Singleton<MusicManager>.Instance.SetSpeed(0.001f, ZerullClassic.Instance.normalMidiPlayerLoop, null);
            }
        }

        AudioListener.pause = true;
        gamaOvarDevice.ignoreListenerPause = true;
        Time.timeScale = 0f;

        PlayerCamera.farClipPlane = gameOverDelay * 400f;
        gameOverDelay -= Time.unscaledDeltaTime;
        Singleton<TimeOutManagerFUCKYEA>.Instance.ResetTimeoutStuff();

        if (!gamaOvarDevice.isPlaying)
        {
            audOverVal = (int)UnityEngine.Random.Range(0f, LoseSounds.Length);
            gamaOvarDevice.PlayOneShot(LoseSounds[audOverVal]);
        }

        if (mode == "endless" && notebooks > PlayerPrefs.GetInt("HighBooks") && !highScoreText.activeSelf)
        {
            highScoreText.SetActive(true);
        }

        if (gameOverDelay <= 0f)
        {
            if (mode == "endless")
            {
                if (notebooks > PlayerPrefs.GetInt("HighBooks"))
                {
                    PlayerPrefs.SetInt("HighBooks", notebooks);
                }
                PlayerPrefs.SetInt("CurrentBooks", notebooks);
            }
            if (player.killedbyhim)
            {
                Application.Quit();
            }
            Time.timeScale = 1f;
            if (!player.killedbyhim)
            {
            SceneManager.LoadScene(gameoverScene);
            }
        }
    }
    #endregion
    private float t;
    #region FinaleModeManagement
    private void ActivateFinaleMode()
    {
        for (int i = 0; i < AdditionalGameCustomizer.Instance.ExitImages.Length; ++i)
        {
            StartCoroutine(tweeniconSolo(Color.black, 0, 1, 1f, i));
        }
        
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
                }
            }
            if (timeout)
            {
                finaleMode = true;
            }
        }
        if (mode != "story")
        {
            ElevdorRea.ForEach(ed => ed.Opendor = true);
            finaleMode = true;
        }
    }

    public IEnumerator ambatudaldi()
    {
        if (mode == "story")
        {
            Singleton<VertexGlitchManager>.Instance.mustGlitch = true;
            escapeMusic.clip = shithourIntro;
            escapeMusic.Play();
            MusicGO = true;
            yield return new WaitForSeconds(shithourIntro.length);
            escapeMusic.clip = shithourLoop;
            escapeMusic.loop = true;
            escapeMusic.Play();
            Singleton<VertexGlitchManager>.Instance.sourceToSyncIn = escapeMusic;
        }
    }
    public IEnumerator tiemoutStu()
    {
        yield return new WaitForSeconds(LearningGameManager.Instance.Television.Markings ? 3f : 0.75f);
        StartCoroutine(easingExit(new Color(0.45f, 0.45f, 0.45f, 1f), 0, 2, 5));
        Singleton<TimeOutManagerFUCKYEA>.Instance.spamupdatethese =true;
        TimeoutMusic.clip = timeoutMusicAud;
        TimeoutMusic.loop = true;
        TimeoutMusic.Play();
    }

    private void FinaleModeAnnoyance()
    {
        if (mode == "story")
        {
            if (!finaleMode || audioDevice.isPlaying) return;

            if (exitsReached == 3)
            {
                if (AdditionalGameCustomizer.Instance != null)
                {
                    switch (AdditionalGameCustomizer.Instance.EscapeMusicFunsies)
                    {
                        case AdditionalGameCustomizer.EscapeFunsies.BBCR:
                            PlayAudioClip(aud_ChaosStartLoop, true);
                            break;
                        case AdditionalGameCustomizer.EscapeFunsies.TBS:
                            PlayAudioClip(aud_ChaosStartLoop, true);
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
                            PlayAudioClip(aud_ChaosFinal, true);
                            break;
                        case AdditionalGameCustomizer.EscapeFunsies.TBS:
                            PlayAudioClip(aud_ChaosStartLoop, true);
                            break;
                    }
                }
            }
        }
    }

    private void PlayAudioClip(AudioClip clip, bool loop)
    {
        audioDevice.clip = clip;
        audioDevice.loop = loop;
        audioDevice.Play();
    }
    #endregion

    #region ExitCounterHandling
    public void ExitReached(int eIds)
    {
        exitsReached++;
        if (mode == "famished")
        {
            fmc.manualUpdate();
        }
        if (mode == "zerullclassic")
        {
            zerull.jusUpdatebr();
        }
        StopCoroutine(tweeniconSolo(Color.white, 0, 1, 0.5f, eIds));
        StartCoroutine(tweeniconSolo(Color.white, 0, 1, 0.5f, eIds));

        if (mode == "story")
        {
            if (!FinaleSecret)
            {
                if (exitEasingCoroutine != null)
                {
                    StopCoroutine(exitEasingCoroutine);
                }
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
                                escapeMusic.pitch = 0.9f;
                                break;
                        }
                    }
                }
                if (FinaleSecret)
                {
                    Singleton<MusicShitass>.Instance.KillCorou();
                    EvapV2FinaleSounSource[0].enabled = false;
                    StartCoroutine(Singleton<MusicShitass>.Instance.truerfinale(1));
                    
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
                                escapeMusic.pitch = 0.75f;
                                audioDevice.clip = aud_ChaosStart;
                                audioDevice.loop = true;
                                audioDevice.Play();
                                break;
                            case AdditionalGameCustomizer.EscapeFunsies.TBS:
                                StartCoroutine(Singleton<MusicShitass>.Instance.basicShowMusicShit(2));
                                audioDevice.clip = aud_ChaosStart;
                                audioDevice.loop = true;
                                audioDevice.Play();
                                break;
                        }
                    }
                }
                if (FinaleSecret)
                {
                    Singleton<MusicShitass>.Instance.KillCorou();
                    EvapV2FinaleSounSource[1].enabled = false;
                    StartCoroutine(Singleton<MusicShitass>.Instance.truerfinale(2));
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
                                audioDevice.clip = aud_ChaosStartLoop;
                                audioDevice.loop = true;
                                audioDevice.Play();
                                escapeMusic.pitch = 0.6f;
                                StartCoroutine(easingExit(new Color(1f / (exitsReached / 1.75f), 0.7f / exitsReached, 0.7f / exitsReached, 1f), 0, 2, 5));
                                break;
                            case AdditionalGameCustomizer.EscapeFunsies.TBS:
                                StartCoroutine(Singleton<MusicShitass>.Instance.basicShowMusicShit(3));
                                audioDevice.clip = aud_ChaosStartLoop;
                                audioDevice.loop = true;
                                audioDevice.Play();
                                StartCoroutine(easingExit(new Color(1f / (exitsReached / 1.75f), 0.7f / exitsReached, 0.7f / exitsReached, 1f), 0, 2, 5));
                                break;
                        }
                    }
                }
                if (FinaleSecret)
                {
                    Singleton<MusicShitass>.Instance.KillCorou();
                    EvapV2FinaleSounSource[2].enabled = false;
                    StartCoroutine(Singleton<MusicShitass>.Instance.truerfinale(3));
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
                                escapeMusic.pitch = 0.45f;
                                StartCoroutine(easingExit(new Color(1f / (exitsReached / 1.5f), 0.7f / exitsReached, 0.7f / exitsReached, 1f), 0, 2, 5));
                                audioDevice.clip = aud_ChaosBuildUp;
                                audioDevice.loop = true;
                                audioDevice.Play();
                                break;
                            case AdditionalGameCustomizer.EscapeFunsies.TBS:
                                StartCoroutine(Singleton<MusicShitass>.Instance.basicShowMusicShit(4));
                                StartCoroutine(easingExit(new Color(1f / (exitsReached / 1.5f), 0.7f / exitsReached, 0.7f / exitsReached, 1f), 0, 2, 5));
                                audioDevice.clip = aud_ChaosBuildUp;
                                audioDevice.loop = true;
                                audioDevice.Play();
                                break;
                        }
                    }
                }
                if (FinaleSecret)
                {
                    Singleton<MusicShitass>.Instance.KillCorou();
                    EvapV2FinaleSounSource[3].enabled = false;
                    StartCoroutine(Singleton<MusicShitass>.Instance.truerfinale(4));
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
                                escapeMusic.pitch = 0.3f;
                                StartCoroutine(easingExit(new Color(1f / (exitsReached / 1.25f), 0.7f / exitsReached, 0.7f / exitsReached, 1f), 0, 2, 5));
                                break;
                            case AdditionalGameCustomizer.EscapeFunsies.TBS:
                                StartCoroutine(Singleton<MusicShitass>.Instance.basicShowMusicShit(5));
                                StartCoroutine(easingExit(new Color(1f / (exitsReached / 1.25f), 0.7f / exitsReached, 0.7f / exitsReached, 1f), 0, 2, 5));
                                break;
                        }
                    }
                }
                if (FinaleSecret)
                {
                    Singleton<MusicShitass>.Instance.KillCorou();
                    EvapV2FinaleSounSource[4].enabled = false;
                    StartCoroutine(Singleton<MusicShitass>.Instance.truerfinale(5));
                }
            }
        }
        discordupdate("exit");
    }

    public IEnumerator easingExit(Color kolor, float a, float b, float duration)
    {
        Color start = voxLight.ambientLight;

        for (float t = a; t < b; t += Time.deltaTime / duration)
        {
            voxLight.ambientLight = Color.Lerp(start, kolor, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        voxLight.ambientLight = kolor;
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
        if (player.invisichalk)
        {
            player.invisichalk = false;
        }
        if (player.alsoInOffice)
        {
            player.alsoInOffice = false;
        }
        if (player.outdoorsfr)
        {
            player.outdoorsfr = false;
        }
        if (isHiding)
        {
            isHiding = false;
        }
        if (player.hugging)
        {
            player.hugging = false;
            player.sweepingFailsave = 0f;
        }
        else if (player.jumpRope)
        {
            player.jumpRope = false;
            player.DeactivateJumpRope();
            player.playtime.Disappoint();
        }

        var newPos = AILocationSelector.SetNewTargetForAgent(null, "default") + Vector3.up * player.height;
        player.transform.position = newPos;
        //baldi.transform.position = newPos;
    }

    public IEnumerator TeleporterFunction(string thing = "normal")
    {
        player.titlecard = true;
        player.movementLocked = true;
        playerCollider.enabled = false;
        if (player.invisichalk)
        {
            player.invisichalk = false;
        }
        if (player.alsoInOffice)
        {
            player.alsoInOffice = false;
        }
        if (player.outdoorsfr)
        {
            player.outdoorsfr = false;
        }
        if (isHiding)
        {
            isHiding = false;
        }
        if (player.hugging)
        {
            player.hugging = false;
            player.sweepingFailsave = 0f;
        }
        if (player.jumpRope)
        {
            player.jumpRope = false;
            player.DeactivateJumpRope();
            player.playtime.Disappoint();
        }

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
        EndingManager.Instance.black.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        PlayerPrefs.SetString("CurrentMode", "famished");
        PlayerPrefsExtension.SetBool("famishUnlockyippe", true);
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadSceneAsync("GameArea");
    }

    private void PlayerTeleport(string type)
    {
        player.transform.position = AILocationSelector.SetNewTargetForAgent(null, "default") + Vector3.up * player.height;
        TpSoundSource.PlayOneShot(type == "normal" ? aud_Teleport : type == "evilleaf" ? aud_EvilLeafyTP : aud_Teleport);
        SubsManager.summonLeSubtitle2D(subtitlesScriptableObject[type == "normal" ? 11 : type == "evilleaf" ? 12 : 11].subtitleOption,subtitlesScriptableObject[type == "normal" ? 11 : type == "evilleaf" ? 12 : 11],0f,new Vector3(0f,-170.5f,0f),TpSoundSource);
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
    [Header("subtitles object stuff")]
    public subsScriptableObject[] subtitlesScriptableObject;

    [Header("Prefab Instances")]
    public GameObject learnpadmuehehe;
    public GameObject popparti,ConfettiEffect;

    [Header("Player & Camera References")]
    public PlayerScript player;
    public SubtitlesManagerAkaSubtitleSpawnOkSDIYBT SubsManager;
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
    public TutorScript baldtutor;
    public CraftersScript sockCript;
    public PlaytimeScript playtimeScript;
    public FirstPrizeScript firstPrizeScript;
    public PrincipalScript principal,maxplayGames;
    public MouseAppearingScript mousescript;
    public VoxelLightingMain voxLight;
    [SerializeField] private AILocationSelectorScript AILocationSelector;

    [Header("Game Mode & Settings")]
    public string mode;
    public int notebooks, maxNotebooks, maxExits, UnlockAmount, SlotsAmmount, CharacterIntVal;
    [SerializeField] public bool debugMode, isHiding, MusicGO,youCantPause,metalpipeStun,ipleak;
    [SerializeField] private string gameoverScene;

    [Header("Serialized References")]
    public TMP_Text notebookCount;
    [SerializeField] public GameObject highScoreText, baldi, tutorobj;
    public List<GameObject> ObjectsToEnable,npcCloneList = new List<GameObject>();
    public List<GateScript> Gatesrea = new List<GateScript>();
    public List<ElvDoorScript> ElevdorRea = new List<ElvDoorScript>();
    public List<GameObject> ObjectsToDisable, ItemsToRespawn,NpcMinimapIcon = new List<GameObject>();
    [SerializeField] private List<VendingMachineScript> MachinesToRestock = new List<VendingMachineScript>();
    public List<NPC> NPCThatGetAffectedByMetalPipe = new List<NPC>();
    public Animator Icon,CirclAnimator;
    public Material SpriteRenderer;
    public Sprite Present;
    public modifiersName[] Modifiers;

    [Header("Audio References")]
    public AudioClip[] LoseSounds;
    public AudioClip[] HurtSounds;
    public AudioClip[] EvapV2FinaleTypeShit, NormalTbsFinale;
    public AudioSource[] EvapV2FinaleSounSource;
    public SongPlayer midishit1;
    public AudioSource audioDevice, audioDevice2, schoolMusic, escapeMusic, gamaOvarDevice,warmusic,TimeoutMusic,TpSoundSource;
    public AudioClip aud_Hang, aud_Rattling, aud_Unlocked, aud_ItemCollect, SchoolhouseEscape, shithourIntro, shithourLoop, aud_Collected, aud_ChaosStart, aud_ChaosStartLoop, aud_ChaosBuildUp, aud_ChaosFinal, aud_Teleport, aud_EvilLeafyTP, deathbell, punchsoun, totem,loboto, gastervanish,LoudIncorecBugger,timeoutMusicAud;
    #endregion

    #region PrivateFields
    public AudioQueueScript audioQueue;
    private int audOverVal;
    public float[] nuzzlesframeshit;
    [SerializeField] private float gameOverDelay, nuzzlframes;
    public int lastRespawnCount, failedNotebooks, exitsReached, cullingMask;
    public bool spoopMode, finaleMode, FinaleSecret,war,warrealest,timeout;
    [HideInInspector] public Coroutine exitEasingCoroutine;
    [HideInInspector] public LearningGameManager Math;
    [HideInInspector] public EndingManager progress;
    #endregion

    [Header("modes stuf")]
    #region ModesStuff
    public FamishedModeController fmc;
    public ZerullClassic zerull;
    public LappingOfAsylumController LapManag;
    public string WindowLayermask;
    #endregion
    [Header("silly stuff")]
    public GameObject TETOOOOO;
    public GameObject train,dimcraab;
    [Header("casual pause menu stuff")]
    public GameObject bal;
    public GameObject moie, famishit;

    [Header("math machine stuff")]
    public int numOfBall;
    public bool isHoldingBall;
    public GameObject PickBall;
    [Header("discord stuff")]
    public string modeDetails;
    public string modeState, largeImagething, largeImageText;
    [Header("teachers management and stuff")]
    public List<PrincipalScript> maxiScr = new List<PrincipalScript>();
    public List<BaldiScript> balscr = new List<BaldiScript>();
    public List<zerullscript> zerscr = new List<zerullscript>();
    public List<MuchoScript> muchscr = new List<MuchoScript>();
    public List<FamishedScript> famishscr = new List<FamishedScript>();
}