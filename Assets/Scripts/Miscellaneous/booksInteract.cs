using UnityEngine;

public class booksInteract : Interactable
{
    public bool hidden = false;
    private float respawnTime = 120f;
    private GameControllerScript gc;
    private Transform player;
    private AudioSource audioDevice;
    private SpriteRenderer notebooSprite;
    public GameObject CheeseMapIcon;
    public Color HighlightColor;
    private MaterialPropertyBlock mpb;
    #region Fields
    [Header("Think Pad")]
    private LearningGameManager lgm;
    #endregion

    private void Start()
    {
        mpb = new MaterialPropertyBlock();
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
        if (!hidden)
        {
            GetComponentInChildren<SpriteRenderer>().GetPropertyBlock(mpb);
		    mpb.SetFloat("_OutlineSize", 0);
            mpb.SetColor("_OutlineColor", Color.clear);
		    GetComponentInChildren<SpriteRenderer>().SetPropertyBlock(mpb);
            if (Sych.ScreenCenterRaycast(out RaycastHit hit))
            {
                Transform hitTransform = hit.transform;
                float maxDistance = 0f;
                if (hitTransform.GetComponent<Collider>().gameObject == this.gameObject)
                {
                    maxDistance = GameControllerScript.Instance.player.LocalRange;
                    if (hitTransform.IsWithinDistanceFrom(GameControllerScript.Instance.player.transform, maxDistance))
                    {
                        GetComponentInChildren<SpriteRenderer>().GetPropertyBlock(mpb);
                        mpb.SetFloat("_OutlineSize", 2);
                        mpb.SetColor("_OutlineColor", HighlightColor);
                        GetComponentInChildren<SpriteRenderer>().SetPropertyBlock(mpb);
                    }
                }
            }
        }

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

        if (gc.mode == "famished")
        {
            gc.fmc.manualUpdate();
        }
        if (gc.mode == "zerullclassic")
        {
            gc.zerull.jusUpdatebr();
            Singleton<OtherMainStuffManager>.Instance.HearingShit(7f, player, new Vector3(0f,0f,0f), "zerull",false);
        }
        if (gc.mode == "wegaChallenge")
        {
            gc.wegchal.manualUpdate();
        }
        if (AdditionalGameCustomizer.Instance?.NoYCTP == true)
        {
            NoYCTPMode();
        }
        else
        {
            StartLearningGame();
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
                lgm.Tutor.tutorSource.Stop();
                lgm.quarter.SetActive(true);
                gc.SubsManager.hideSub(lgm.Tutor.TutorSub);
                lgm.Tutor.tutorSource.PlayClip(lgm.aud_Prize, false, 1f);
                gc.SubsManager.summonLeSubtitle(lgm.prizeSubs.subtitleOption,lgm.prizeSubs,lgm.Tutor.tutorSource);

            }
        }
        if (gc.notebooks >= 1)
        {
        scoreSystemManager.Instance.AddScore(1400, true,true);
        }

        if (gc.notebooks == 2)
        {
            if (gc.mode == "story")
            {
                gc.SubsManager.hideSub(lgm.prizeSubs);
            }
            gc.ActivateSpoopMode();
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
                //lgm.Television.baldingit = true;
                //StartCoroutine(lgm.timeounaleshit(lgm.aud_AllNotebooks,lgm.balSubs));
                lgm.Television.TeacherJerryingIt = true;
                StartCoroutine(lgm.timeounaleshit(lgm.aud_TeacherJerryAllCheese,lgm.jerSubs));
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
                gc.finaleMode = true;
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
                                gc.ElevdorRea.ForEach(ed => ed.Opendor = true);
                                gc.Gatesrea.ForEach(g => g.Down(false));
                                gc.finaleMode = true;
                                break;
                            case AdditionalGameCustomizer.EscapeFunsies.Taldi:
                                gc.escapeMusic.clip = gc.TaldiEscape;
                                gc.escapeMusic.loop = true;
                                gc.escapeMusic.Play();
                                gc.ElevdorRea.ForEach(ed => ed.Opendor = true);
                                gc.Gatesrea.ForEach(g => g.Down(false));
                                gc.finaleMode = true;
                                break;
                            case AdditionalGameCustomizer.EscapeFunsies.Daldi:
                                StartCoroutine(gc.ambatudaldi());
                                gc.ElevdorRea.ForEach(ed => ed.Opendor = true);
                                gc.Gatesrea.ForEach(g => g.Down(false));
                                gc.finaleMode = true;
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
