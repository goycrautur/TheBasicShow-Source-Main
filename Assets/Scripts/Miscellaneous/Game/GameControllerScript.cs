using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using FluidMidi;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Diagnostics;

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
        slot();
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
        if (maxNotebooks == failedNotebooks && !warrealest)
        {
            FinaleSecret = true;
        }
        DiscordRPC_stuff.current.UpdateStatus(modeDetails, modeState, largeImagething, largeImageText);
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
    }
    #endregion
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
        if (StateUpdateType == "chees")
        {
        modeState = notebooks + "/" + maxNotebooks + " Cheese Blocks";
        }
        if (StateUpdateType == "exit")
        {
        modeState = exitsReached + "/" + maxExits + " Exits";
        }
        if (mode == "endless")
        {
            modeDetails = "Endless Mode";
            largeImagething = "teacherjerproto";
            largeImageText = "hi i am teachr jery and im gonna smac you";
        }
        if (mode == "story")
        {
            modeDetails = "Story Mode";
            largeImagething = "teacherjerproto";
            largeImageText = "hi i am teachr jery and im gonna smac you";
        }
        if (mode == "famished")
        {
            modeDetails = "???????";
            largeImagething = "creepydarkmf";
            largeImageText = "*hungry ass noise intenstify*";
        }
        if (mode == "zerullclassic")
        {
            modeDetails = "???????";
            largeImagething = "van";
            largeImageText = "*CLASSIFIED INFO*";
        }
        if (mode == "LappingOfAsylum")
        {
            modeDetails = "lapping mode - lap " + LapManag.CurrentLap;
            largeImagething = "teacherjerproto";
            largeImageText = "the lapping grindset begin";
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
    public void HighSchoolDropOut()
    {
        for (int i = 0; i < SlotsAmmount; ++i)
        {
            if (ItemManager.Instance.Inventory[i].ItemInstance != null)
            {
                ItemManager.Instance.DropItem(i);
            }
        }
    }
    public void slot()
    {
        if (ItemManager.Instance.ItemSelection >= SlotsAmmount)
        {
            ItemManager.Instance.ItemSelection = SlotsAmmount - 1;
        }
        if (SlotsAmmount >= 9)
        {
            ItemManager.Instance.Inventory = AdditionalGameCustomizer.Instance.Inventory9slot;
            for (int i = 0; i < 9; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(true);
            }
            AdditionalGameCustomizer.Instance.ItemImageSlots[8].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[2+(CharacterIntVal*3)];
            for (int i = 1; i < 8; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemImageSlots[i].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[1+(CharacterIntVal*3)];
            }
            AdditionalGameCustomizer.Instance.ItemImageSlots[0].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[0+(CharacterIntVal*3)];
        }
        if (SlotsAmmount == 8)
        {
            ItemManager.Instance.Inventory = AdditionalGameCustomizer.Instance.Inventory8slot;
            ItemManager.Instance.ChangeReferences(AdditionalGameCustomizer.Instance.ItemImages8slot, AdditionalGameCustomizer.Instance.ItemImageBGs8slot);
            AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[8].SetActive(false);
            AdditionalGameCustomizer.Instance.ItemSlotsGameObj[8].SetActive(false);
            AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[8].SetActive(false);
            for (int i = 0; i < 8; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(true);
            }
            AdditionalGameCustomizer.Instance.ItemImageSlots[7].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[2+(CharacterIntVal*3)];
            for (int i = 1; i < 7; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemImageSlots[i].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[1+(CharacterIntVal*3)];
            }
            AdditionalGameCustomizer.Instance.ItemImageSlots[0].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[0+(CharacterIntVal*3)];
        }
        if (SlotsAmmount == 7)
        {
            ItemManager.Instance.Inventory = AdditionalGameCustomizer.Instance.Inventory7slot;
            ItemManager.Instance.ChangeReferences(AdditionalGameCustomizer.Instance.ItemImages7slot, AdditionalGameCustomizer.Instance.ItemImageBGs7slot);
            for (int i = 6; i < 9; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(false);
            }
            for (int i = 0; i < 7; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(true);
            }
            AdditionalGameCustomizer.Instance.ItemImageSlots[6].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[2+(CharacterIntVal*3)];
            for (int i = 1; i < 6; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemImageSlots[i].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[1+(CharacterIntVal*3)];
            }
            AdditionalGameCustomizer.Instance.ItemImageSlots[0].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[0+(CharacterIntVal*3)];
        }
        if (SlotsAmmount == 6)
        {
            ItemManager.Instance.Inventory = AdditionalGameCustomizer.Instance.Inventory6slot;
            ItemManager.Instance.ChangeReferences(AdditionalGameCustomizer.Instance.ItemImages6slot, AdditionalGameCustomizer.Instance.ItemImageBGs6slot);
            for (int i = 5; i < 9; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(false);
            }
            for (int i = 0; i < 6; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(true);
            }
            AdditionalGameCustomizer.Instance.ItemImageSlots[5].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[2+(CharacterIntVal*3)];
            for (int i = 1; i < 5; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemImageSlots[i].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[1+(CharacterIntVal*3)];
            }
            AdditionalGameCustomizer.Instance.ItemImageSlots[0].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[0+(CharacterIntVal*3)];
        }
        if (SlotsAmmount == 5)
        {
            ItemManager.Instance.Inventory = AdditionalGameCustomizer.Instance.Inventory5slot;
            ItemManager.Instance.ChangeReferences(AdditionalGameCustomizer.Instance.ItemImages5slot, AdditionalGameCustomizer.Instance.ItemImageBGs5slot);
            for (int i = 4; i < 9; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(false);
            }
            for (int i = 0; i < 5; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(true);
            }
            AdditionalGameCustomizer.Instance.ItemImageSlots[4].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[2+(CharacterIntVal*3)];
            for (int i = 1; i < 4; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemImageSlots[i].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[1+(CharacterIntVal*3)];
            }
            AdditionalGameCustomizer.Instance.ItemImageSlots[0].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[0+(CharacterIntVal*3)];
        }
        if (SlotsAmmount == 4)
        {
            ItemManager.Instance.Inventory = AdditionalGameCustomizer.Instance.Inventory4slot;
            ItemManager.Instance.ChangeReferences(AdditionalGameCustomizer.Instance.ItemImages4slot, AdditionalGameCustomizer.Instance.ItemImageBGs4slot);
            for (int i = 3; i < 9; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(false);
            }
            for (int i = 0; i < 4; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(true);
            }
            AdditionalGameCustomizer.Instance.ItemImageSlots[3].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[2+(CharacterIntVal*3)];
            for (int i = 1; i < 3; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemImageSlots[i].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[1+(CharacterIntVal*3)];
            }
            AdditionalGameCustomizer.Instance.ItemImageSlots[0].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[0+(CharacterIntVal*3)];
        }
        if (SlotsAmmount == 3)
        {
            ItemManager.Instance.Inventory = AdditionalGameCustomizer.Instance.Inventory3slot;
            ItemManager.Instance.ChangeReferences(AdditionalGameCustomizer.Instance.ItemImages3slot, AdditionalGameCustomizer.Instance.ItemImageBGs3slot);
            for (int i = 2; i < 9; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(false);
            }
            for (int i = 0; i < 3; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(true);
            }
            AdditionalGameCustomizer.Instance.ItemImageSlots[2].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[2+(CharacterIntVal*3)];
            for (int i = 1; i < 2; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemImageSlots[i].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[1+(CharacterIntVal*3)];
            }
            AdditionalGameCustomizer.Instance.ItemImageSlots[0].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[0+(CharacterIntVal*3)];
        }
        if (SlotsAmmount == 2)
        {
            ItemManager.Instance.Inventory = AdditionalGameCustomizer.Instance.Inventory2slot;
            ItemManager.Instance.ChangeReferences(AdditionalGameCustomizer.Instance.ItemImages2slot, AdditionalGameCustomizer.Instance.ItemImageBGs2slot);
            for (int i = 1; i < 9; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(false);
            }
            for (int i = 0; i < 2; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(true);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(true);
            }
            AdditionalGameCustomizer.Instance.ItemImageSlots[1].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[2+(CharacterIntVal*3)];
            AdditionalGameCustomizer.Instance.ItemImageSlots[0].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[0+(CharacterIntVal*3)];
        }
        if (SlotsAmmount == 1)
        {
            ItemManager.Instance.Inventory = AdditionalGameCustomizer.Instance.Inventory1slot;
            ItemManager.Instance.ChangeReferences(AdditionalGameCustomizer.Instance.ItemImages1slot, AdditionalGameCustomizer.Instance.ItemImageBGs1slot);
            for (int i = 0; i < 9; ++i)
            {
                AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsGameObj[i].SetActive(false);
                AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[i].SetActive(false);
            }
            AdditionalGameCustomizer.Instance.ItemBackgroundsGameObj[0].SetActive(true);
            AdditionalGameCustomizer.Instance.ItemSlotsGameObj[0].SetActive(true);
            AdditionalGameCustomizer.Instance.ItemSlotsImagesGameObj[0].SetActive(true);
            AdditionalGameCustomizer.Instance.ItemImageSlots[0].sprite = AdditionalGameCustomizer.Instance.ItemSlotsSprites[1+(CharacterIntVal*3)];
        }
        ItemManager.Instance.UpdateItemUI();
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
            ObjectsToDisable.ForEach(o => o.SetActive(false));
            ObjectsToEnable.ForEach(o => o.SetActive(true));
            if (warrealest)
            {
                StartCoroutine(easingExit(new Color(0.9803922f, 0.5019608f, 0.4470589f, 1f), 0, 2, 5));
                StartCoroutine(meeptimerwai());
            }
        }
        if (mode != "zerullclassic" && mode != "LappingOfAsylum" || mode != "LappingOfAsylum" && mode != "zerullclassic")
        {
            Gatesrea.ForEach(g => g.Down());
        }

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

    #region GameOverLogic
    private void GameOverFunction()
    {
        if (!player.gameOver) return;

        AudioListener.pause = true;
        gamaOvarDevice.ignoreListenerPause = true;
        Time.timeScale = 0f;

        PlayerCamera.farClipPlane = gameOverDelay * 400f;
        gameOverDelay -= Time.unscaledDeltaTime;

        if (!gamaOvarDevice.isPlaying)
        {
            audOverVal = (int)Random.Range(0f, LoseSounds.Length);
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
            Time.timeScale = 1f;
            SceneManager.LoadScene(gameoverScene);
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
        ElevdorRea.ForEach(ed => ed.Opendor = true);

        finaleMode = true;
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
    public IEnumerator truerfinale(int type)
    {
        if (mode == "story")
        {
            if (type <= 2)
            {
                escapeMusic.clip = EvapV2FinaleTypeShit[type];
                escapeMusic.Play();
                yield return new WaitForSeconds(EvapV2FinaleTypeShit[type].length);
                escapeMusic.clip = EvapV2FinaleTypeShit[5 + type];
                escapeMusic.loop = true;
                escapeMusic.Play();
            }
            if (type == 3)
            {
                escapeMusic.clip = EvapV2FinaleTypeShit[8];
                escapeMusic.loop = true;
                escapeMusic.Play();
            }
            if (type == 4)
            {
                escapeMusic.clip = EvapV2FinaleTypeShit[3];
                escapeMusic.Play();
                yield return new WaitForSeconds(EvapV2FinaleTypeShit[3].length);
                escapeMusic.clip = EvapV2FinaleTypeShit[9];
                escapeMusic.loop = true;
                escapeMusic.Play();
            }
            if (type == 5)
            {
                escapeMusic.clip = EvapV2FinaleTypeShit[4];
                escapeMusic.Play();
                yield return new WaitForSeconds(EvapV2FinaleTypeShit[4].length);
                escapeMusic.clip = EvapV2FinaleTypeShit[10];
                escapeMusic.loop = true;
                escapeMusic.Play();
            }
        }
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
                    StopCoroutine(truerfinale(0));
                    StartCoroutine(truerfinale(1));
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
                        }
                    }
                }
                if (FinaleSecret)
                {
                    StopCoroutine(truerfinale(1));
                    StartCoroutine(truerfinale(2));
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
                        }
                    }
                }
                if (FinaleSecret)
                {
                    StopCoroutine(truerfinale(2));
                    StartCoroutine(truerfinale(3));
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
                        }
                    }
                }
                if (FinaleSecret)
                {
                    StopCoroutine(truerfinale(3));
                    StartCoroutine(truerfinale(4));
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
                        }
                    }
                }
                if (FinaleSecret)
                {
                    StopCoroutine(truerfinale(4));
                    StartCoroutine(truerfinale(5));
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

    public IEnumerator TeleporterFunction()
    {
        player.titlecard = true;
        player.movementLocked = true;
        playerCollider.enabled = false;
        if (player.invisichalk)
        {
            player.invisichalk = false;
        }
        if (isHiding)
        {
            isHiding = false;
        }

        int teleports = Random.Range(43, 49);
        float delay = 0.05f;
        const float increaseFactor = 1.04f;

        for (int i = 0; i < teleports; i++)
        {
            yield return new WaitForSeconds(delay);
            PlayerTeleport();
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

    private void PlayerTeleport()
    {
        player.transform.position = AILocationSelector.SetNewTargetForAgent(null, "default") + Vector3.up * player.height;
        audioDevice.PlayOneShot(aud_Teleport);
    }
    #endregion

    #region SerializedFields
    [Header("subtitles object stuff")]
    public subsScriptableObject[] subtitlesScriptableObject;

    [Header("Prefab Instances")]
    public GameObject learnpadmuehehe;
    public GameObject popparti;

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
    [SerializeField] public bool debugMode, isHiding, MusicGO,youCantPause;
    [SerializeField] private string gameoverScene;

    [Header("Serialized References")]
    public TMP_Text notebookCount;
    [SerializeField] public GameObject highScoreText, baldi, tutorobj,playerGameobject;
    public List<GameObject> ObjectsToEnable = new List<GameObject>();
    public List<GateScript> Gatesrea = new List<GateScript>();
    public List<ElvDoorScript> ElevdorRea = new List<ElvDoorScript>();
    public List<GameObject> ObjectsToDisable, ItemsToRespawn = new List<GameObject>();
    [SerializeField] private List<VendingMachineScript> MachinesToRestock = new List<VendingMachineScript>();
    public Animator Icon,CirclAnimator;
    public Material SpriteRenderer;
    public Sprite Present;
    public bool[] Modifiers;

    [Header("Audio References")]
    [SerializeField] private AudioClip[] LoseSounds;
    [SerializeField] public AudioClip[] HurtSounds;
    [SerializeField] public AudioClip[] EvapV2FinaleTypeShit;
    public SongPlayer midishit1;
    public AudioSource audioDevice, audioDevice2, schoolMusic, escapeMusic, gamaOvarDevice,warmusic;
    public AudioClip aud_Hang, aud_Rattling, aud_Unlocked, aud_ItemCollect, SchoolhouseEscape, shithourIntro, shithourLoop, aud_Collected, aud_ChaosStart, aud_ChaosStartLoop, aud_ChaosBuildUp, aud_ChaosFinal, aud_Teleport, deathbell, punchsoun, totem,loboto, gastervanish;
    #endregion

    #region PrivateFields
    public AudioQueueScript audioQueue;
    private int audOverVal;
    public float[] nuzzlesframeshit;
    [SerializeField] private float gameOverDelay, nuzzlframes;
    public int lastRespawnCount, failedNotebooks, exitsReached, cullingMask;
    public bool spoopMode, finaleMode, FinaleSecret,war,warrealest;
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
    public string modeState,largeImagething,largeImageText;
}