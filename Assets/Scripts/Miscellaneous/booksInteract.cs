using UnityEngine;

public class booksInteract : Interactable
{
    public bool hidden = false;
    private float respawnTime = 120f;
    private GameControllerScript gc;
    private Transform player;
    private AudioManagerLiveReaction audioDevice;
    private SpriteRenderer notebooSprite;
    public GameObject CheeseMapIcon;
    public Color HighlightColor;
    private MaterialPropertyBlock mpb;
    [Header("Audio stuff")]
    public AudioObjectyeah cheeseCollect;
    public AudioObjectyeah RespawnSound;
    #region Fields
    [Header("Think Pad")]
    private LearningGameManager lgm;
    #endregion

    private void Start()
    {
        mpb = new MaterialPropertyBlock();
        notebooSprite = GetComponentInChildren<SpriteRenderer>();
        respawnTime = 120f;
        audioDevice = GetComponent<AudioManagerLiveReaction>();
        gc = GameControllerScript.Instance;
        lgm = LearningGameManager.Instance;
        player = GameControllerScript.Instance.player.transform;

        if (AdditionalGameCustomizer.Instance != null && AdditionalGameCustomizer.Instance.RandomizeBookColor)
        {
            notebooSprite.sprite = AdditionalGameCustomizer.Instance.BookColors[Random.Range(0, AdditionalGameCustomizer.Instance.BookColors.Length)];
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
            if (Sych.ScreenCenterRaycast(out RaycastHit hit,KeyFunctions.hi.PlayerClickablesLayer.value))
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

        if (hidden && respawnTime > 0f) respawnTime -= Time.deltaTime;
        else if (!transform.IsWithinDistanceFrom(player, gc.player.LocalRange) && respawnTime <= 0f && hidden) Respawn();
    }

    public void Respawn()
    {
        Hide(false);
        hidden = false;
        audioDevice.PlaySingleClip(RespawnSound);
        respawnTime = 120f;
    }

    public override void Interact()
    {
        if (hidden)return;
        Hide(true);
        respawnTime = 120f;
        gc.CollectNotebook(1);
        if (gc.mode == "LappingOfAsylum") gc.LapManag.UpdateManually();

        if (gc.mode == "famished") gc.fmc.manualUpdate();
        if (gc.mode == "zerullclassic")
        {
            gc.zerull.jusUpdatebr();
            Singleton<OtherMainStuffManager>.Instance.HearingShit(7f, player, new Vector3(0f,0f,0f), "zerull",false);
        }
        if (gc.mode == "wegaChallenge")  gc.wegchal.manualUpdate();
        if (AdditionalGameCustomizer.Instance?.NoYCTP == true) NoYCTPMode();
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
        string dific = PlayerPrefs.GetString("CurDifficulity", "normal");
        int problemcap = dific == "easy" ? 3 : dific == "normal" ? 6 : dific == "hard" ? 9 : dific == "expert" || dific == "maniac" ? 12 : 3;
        scoreSystemManager.Instance.AddScore(1000+(75*problemcap), true,true);
        lowBudgetAudioManagementShit.Instance.MainSource2.PlaySingleClip(cheeseCollect);
        if (gc.player.stamina < 100f) gc.player.SetStamina(PlayerScript.StaminaChangeMode.Set, 100f);
        if (gc.notebooks == 1 && !gc.spoopMode)
        {
            if (gc.mode == "story")
            {
                lgm.quarter.SetActive(true);
                lgm.Tutor.tutorSource.ClearQueue(true);
                lgm.Tutor.tutorSource.QueueAudio(lgm.Tutor.aud_Prize);
            }
        }
        if (gc.notebooks == 2)
        {
            if (gc.mode == "story") lgm.Tutor.tutorSource.ClearQueue(true);
            gc.ActivateSpoopMode();
        }
        if (gc.notebooks == gc.maxNotebooks && gc.mode != "endless" && gc.mode != "LappingOfAsylum") TriggerFinalSequence();
        if (gc.spoopMode)
        {
            Singleton<OtherMainStuffManager>.Instance.AngerShit(1.1f*LearningGameManager.Instance.angerMult, 0f,false, "all");
            Singleton<OtherMainStuffManager>.Instance.AngerShit(0.1f*LearningGameManager.Instance.angerMult, 0f,false, "famished");
            Singleton<OtherMainStuffManager>.Instance.AngerShit(0.4f*LearningGameManager.Instance.angerMult, 0f,false, "zerull");
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
        mathGame.nbScri = this;
    }
    #endregion
    private void TriggerFinalSequence()
    {
        lowBudgetAudioManagementShit lbams = lowBudgetAudioManagementShit.Instance;
        if (gc.mode != "story") gc.Gatesrea.ForEach(g => g.Down(false));
        if (AdditionalGameCustomizer.Instance?.FinalModeTV == true)
        {
            if (gc.mode == "story")
            {
                //lgm.Television.baldingit = true;
                //StartCoroutine(lgm.timeounaleshit(lgm.aud_AllNotebooks,lgm.balSubs));
                lgm.Television.TeacherJerryingIt = true;
                if (!gc.FinaleSecret) StartCoroutine(lgm.timeounaleshit(lgm.aud_TeacherJerryAllCheese));
            }
            if (gc.mode == "famished")
            {
                lgm.Television.famishingit = true;
                StartCoroutine(lgm.timeounaleshit(lbams.deadbel));
            }
        }
        else
        {
            if (gc.mode == "story") lbams.MainSource3.PlaySingleClip(lgm.aud_AllNotebooks);
            if (gc.mode == "famished") lbams.MainSource3.PlaySingleClip(lbams.deadbel);
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
                                lbams.EscapeMusic.ClearQueue(true);
                                lbams.EscapeMusic.SetLoop(true);
                                lbams.EscapeMusic.QueueAudio(lbams.SchoolhouseEscape);
                                
                                gc.ElevdorRea.ForEach(ed => ed.Opendor = true);
                                gc.Gatesrea.ForEach(g => g.Down(false));
                                gc.finaleMode = true;
                                break;
                            case AdditionalGameCustomizer.EscapeFunsies.Taldi:
                                lbams.EscapeMusic.ClearQueue(true);
                                lbams.EscapeMusic.SetLoop(true);
                                lbams.EscapeMusic.QueueAudio(lbams.TaldiEscape);
                                
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
                                StartCoroutine(gc.basicShowMusicShit());
                                break;
                        }
                    }
                }
                
                if (gc.FinaleSecret)
                {
                    lbams.EscapeMusic.ClearQueue(true);
                    lbams.EscapeMusic.QueueAudio(lbams.EvapV2Finale[0]);
                    lbams.EscapeMusic.QueueAudio(lbams.EvapV2FinaleIntros[0]);
                    lbams.EscapeMusic.SetLoop(true);
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
