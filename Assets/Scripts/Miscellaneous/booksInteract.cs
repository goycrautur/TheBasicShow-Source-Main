using UnityEngine;

public class booksInteract : Interactable
{
    private bool hidden = false;
    private float respawnTime = 120f;
    private GameControllerScript gc;
    private Transform player;
    private AudioSource audioDevice;
    private SpriteRenderer notebooSprite;
    public GameObject CheeseMapIcon;
    #region Fields
    [Header("Think Pad")]
    private LearningGameManager lgm;
    #endregion

    private void Start()
    {
        notebooSprite = GetComponentInChildren<SpriteRenderer>();
        respawnTime = 120f;
        audioDevice = GetComponent<AudioSource>();
        gc = GameControllerScript.Instance;
        lgm = LearningGameManager.Instance;
        player = GameControllerScript.Instance.player.transform;

        if (AdditionalGameCustomizer.Instance != null && AdditionalGameCustomizer.Instance.RandomizeBookColor)
        {
            notebooSprite.sprite = AdditionalGameCustomizer.Instance.BookColors[
                Random.Range(0, AdditionalGameCustomizer.Instance.BookColors.Length)
            ];
        }
    }

    private void Update()
    {
        if (gc.mode != "endless") return;

        if (hidden && respawnTime > 0f)
        {
            respawnTime -= Time.deltaTime;
        }
        else if (!transform.IsWithinDistanceFrom(player, gc.player.LocalRange) && respawnTime <= 0f && hidden)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        Hide(false);
        hidden = false;
        audioDevice.Play();
        gc.SubsManager.summonLeSubtitle(gc.subtitlesScriptableObject[10].subtitleOption, gc.subtitlesScriptableObject[10], audioDevice);
        respawnTime = 120f;
    }

    public override void Interact()
    {
        if (hidden)
        {
            return;
        }
        Hide(true);
        respawnTime = 120f;

        gc.CollectNotebook(1);
        if (gc.mode == "LappingOfAsylum")
        {
            gc.LapManag.UpdateManually();
        }

        if (AdditionalGameCustomizer.Instance?.NoYCTP == true)
        {
            NoYCTPMode();
            if (gc.mode == "famished")
            {
                gc.fmc.manualUpdate();
                Singleton<OtherMainStuffManager>.Instance.HearingShit(7f, player, new Vector3(0f,0f,0f), "famished",false);
            }
            if (gc.mode == "zerullclassic")
            {
                gc.zerull.jusUpdatebr();
                Singleton<OtherMainStuffManager>.Instance.HearingShit(7f, player, new Vector3(0f,0f,0f), "zerull",false);
            }
        }
        else
        {
            StartLearningGame();
            if (gc.mode == "famished")
            {
                gc.fmc.manualUpdate();
                Singleton<OtherMainStuffManager>.Instance.HearingShit(7f, player, new Vector3(0f,0f,0f), "famished",false);
            }
            if (gc.mode == "zerullclassic")
            {
                gc.zerull.jusUpdatebr();
                Singleton<OtherMainStuffManager>.Instance.HearingShit(7f, player, new Vector3(0f,0f,0f), "zerull",false);
                
            }
            return;
        }
    }
    private void NoYCTPMode()
    {
        
        gc.Icon.Rebind();
        gc.Icon.Play("IconSpinMain", -1, 0f);
        gc.audioDevice2.PlayOneShot(gc.aud_Collected);

        if (gc.player.stamina < 100f)
        {
            gc.player.SetStamina(PlayerScript.StaminaChangeMode.Set, 100f);
        }

        if (gc.notebooks == 1 && !gc.spoopMode)
        {
            if (gc.mode == "story")
            {
                scoreSystemManager.Instance.AddScore(1200, true,true);
                lgm.Tutor.tutorSource.Stop();
                lgm.quarter.SetActive(true);
                lgm.Tutor.tutorSource.PlayClip(lgm.aud_Prize, false, 1f);
            }
        }
        if (gc.notebooks >= 2)
        {
        scoreSystemManager.Instance.AddScore(1400, true,true);
        }

        if (gc.notebooks == 2)
        {
            
            gc.ActivateSpoopMode();
            if (gc.mode == "story")
            {
                lgm.Tutor.StartCoroutine(lgm.Tutor.captions());
            }
        }

        if (gc.notebooks == gc.maxNotebooks && gc.mode != "endless" && gc.mode != "LappingOfAsylum")
        {
            TriggerFinalSequence();
        }
        if (gc.spoopMode)
        {
            Singleton<OtherMainStuffManager>.Instance.AngerShit(1.1f, 0f,false, "all");
            Singleton<OtherMainStuffManager>.Instance.AngerShit(0.1f, 0f,false, "famished");
            Singleton<OtherMainStuffManager>.Instance.AngerShit(0.4f, 0f,false, "zerull");
        }
    }
    #region Learning Game Launch
    private void StartLearningGame()
    {
        GameObject game = Instantiate(gc.learnpadmuehehe);
        var mathGame = game.GetComponent<MathGameScript>();
        mathGame.gc = gc;
        mathGame.lg = lgm;
        mathGame.playerPosition = player.position;
    }
    #endregion
    private void TriggerFinalSequence()
    {
        if (gc.mode != "story")
        {
            gc.Gatesrea.ForEach(g => g.Down(false));
        }
        if (AdditionalGameCustomizer.Instance?.FinalModeTV == true)
        {
            if (gc.mode == "story")
            {
                lgm.Television.baldingit = true;
                StartCoroutine(lgm.timeounaleshit(lgm.aud_AllNotebooks,lgm.balSubs));
            }
            if (gc.mode == "famished")
            {
                lgm.Television.famishingit = true;
                StartCoroutine(lgm.timeounaleshit(gc.deathbell,lgm.famSubs));
            }
        }
        else
        {
            if (gc.mode == "story")
            {
                gc.audioDevice.PlayClip(lgm.aud_AllNotebooks, true, 0.8f);
            }
            if (gc.mode == "famished")
            {
                gc.audioDevice.PlayClip(gc.deathbell, true, 0.8f);
            }
        }

        if (gc.mode == "story")
        {
            if (gc.warrealest || gc.timeout)
            {
                gc.ElevdorRea.ForEach(ed => ed.Opendor = true);
                gc.Gatesrea.ForEach(g => g.Down(false));
            }
            if (!gc.warrealest)
            {
                if (!gc.FinaleSecret && !gc.timeout)
                {
                    if (AdditionalGameCustomizer.Instance != null)
                    {
                        switch (AdditionalGameCustomizer.Instance.EscapeMusicFunsies)
                        {
                            case AdditionalGameCustomizer.EscapeFunsies.BBCR:
                                gc.escapeMusic.clip = gc.SchoolhouseEscape;
                                gc.escapeMusic.loop = true;
                                gc.escapeMusic.Play();
                                gc.Gatesrea.ForEach(g => g.Down(false));
                                break;
                            case AdditionalGameCustomizer.EscapeFunsies.Daldi:
                                StartCoroutine(gc.ambatudaldi());
                                gc.Gatesrea.ForEach(g => g.Down(false));
                                break;
                            case AdditionalGameCustomizer.EscapeFunsies.TBS:
                                gc.Gatesrea.ForEach(g => g.Down());
                                StartCoroutine(Singleton<MusicShitass>.Instance.basicShowMusicShit(0));
                                break;
                        }
                    }
                }
                
                if (gc.FinaleSecret)
                {
                    StartCoroutine(Singleton<MusicShitass>.Instance.truerfinale(0));
                    gc.ElevdorRea.ForEach(ed => ed.Opendor = true);
                    gc.Gatesrea.ForEach(g => g.Down(false));
                }
            }
        }
    }
    private void Hide(bool hide)
    {
        hidden = hide;
        transform.GetChild(0).gameObject.SetActive(!hide);
        CheeseMapIcon.SetActive(!hide);
        gameObject.tag = hide ? "Untagged" : "Notebook";
    }
}
