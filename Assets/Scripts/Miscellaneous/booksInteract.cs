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
        gc.SubsManager.summonLeSubtitle(gc.subtitlesScriptableObject[10].subtitleOption, gc.subtitlesScriptableObject[10], 0f, audioDevice);
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
                if (gc.famishScrpt.isActiveAndEnabled)
                {
                    gc.famishScrpt.Hear(player.position, 7f);
                }
            }
            if (gc.mode == "zerullclassic")
            {
                gc.zerull.jusUpdatebr();
                if (gc.zerulscrpt.isActiveAndEnabled)
                {
                    gc.zerulscrpt.Hear(player.position, 7f);
                }
            }
        }
        else
        {
            StartLearningGame();
            if (gc.mode == "famished")
            {
                gc.fmc.manualUpdate();
                if (gc.famishScrpt.isActiveAndEnabled)
                {
                    gc.famishScrpt.Hear(player.position, 7f);
                }
            }
            if (gc.mode == "zerullclassic")
            {
                gc.zerull.jusUpdatebr();
                if (gc.zerulscrpt.isActiveAndEnabled)
                {
                    gc.zerulscrpt.Hear(player.position, 7f);
                }
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
                lgm.Tutor.tutorSource.Stop();
                lgm.quarter.SetActive(true);
                lgm.Tutor.tutorSource.PlayClip(lgm.aud_Prize, false, 1f);
            }
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
            if (gc.baldiScrpt.isActiveAndEnabled)
            {
                gc.baldiScrpt.GetAngry(1.1f);
            }
            if (gc.muchoing.isActiveAndEnabled)
            {
                gc.muchoing.GetAngry(1.1f);
            }
            if (gc.famishScrpt.isActiveAndEnabled)
            {
                gc.famishScrpt.GetAngry(1.2f);
            }
            if (gc.zerulscrpt.isActiveAndEnabled)
            {
                gc.zerulscrpt.GetAngry(1.5f);
            }
        }
    }
    #region Learning Game Launch
    private void StartLearningGame()
    {
        GameObject game = Instantiate(gc.learnpadmuehehe);
        var mathGame = game.GetComponent<MathGameScript>();
        mathGame.gc = gc;
        mathGame.lg = lgm;
        mathGame.baldiScript = gc.baldiScrpt;
        mathGame.playerPosition = player.position;
    }
    #endregion
    private void TriggerFinalSequence()
    {
        gc.Gatesrea.ForEach(g => g.Down(false));
        if (AdditionalGameCustomizer.Instance?.FinalModeTV == true)
        {
            if (gc.mode == "story")
            {
                lgm.Television.baldingit = true;
                StartCoroutine(lgm.Television.StartTVSequence(lgm.aud_AllNotebooks));
            }
            if (gc.mode == "famished")
            {
                lgm.Television.famishingit = true;
                StartCoroutine(lgm.Television.StartTVSequence(gc.deathbell));
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
            if (!gc.warrealest)
            {
                if (!gc.FinaleSecret)
                {
                    if (AdditionalGameCustomizer.Instance != null)
                    {
                        switch (AdditionalGameCustomizer.Instance.EscapeMusicFunsies)
                        {
                            case AdditionalGameCustomizer.EscapeFunsies.BBCR:
                                gc.escapeMusic.clip = gc.SchoolhouseEscape;
                                gc.escapeMusic.loop = true;
                                gc.escapeMusic.Play();
                                break;
                            case AdditionalGameCustomizer.EscapeFunsies.Daldi:
                                StartCoroutine(gc.ambatudaldi());
                                break;
                        }
                    }
                }
                if (gc.FinaleSecret)
                {
                    StartCoroutine(gc.truerfinale(0));
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
