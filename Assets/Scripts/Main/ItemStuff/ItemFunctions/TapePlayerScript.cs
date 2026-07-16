using UnityEngine;
using System.Collections;

public class TapePlayerScript : MonoBehaviour
{
    #region Initialization
    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    #endregion

    #region Per-Frame Logic
    private void Update()
    {
        if (!GameControllerScript.Instance.baldiScrpt.antiHearing) sprite.sprite = openSprite;
        if (TapeCDEnable) TapeCD -= Time.deltaTime;
        if (TapeCD < 0f) TapeCDEnable = false;
    }
    #endregion

    #region Public Methods
    public void Play(string tapeType = "normal")
    {
        if (!TapeCDEnable)
        {
            if (holyshitIsItRealTape == IsTape.TapePlayer)
            {
                if (tapeType == "normal")
                {
                    sprite.sprite = closedSprite;
                    tapeDevice.ClearQueue(true);
                    tapeDevice.PlaySingleClip(NormTapeAudio);
                    TapeCD = AntiHearingDuration;
                    TapeCDEnable = true;
                    Singleton<OtherMainStuffManager>.Instance.deafshit(AntiHearingDuration,"All");
                }
                if (tapeType == "JEPDVDD")
                {
                    sprite.sprite = closedSprite;
                    tapeDevice.ClearQueue(true);
                    tapeDevice.PlaySingleClip(welcomeOld);
                    TapeCD = welcomeOld.audClip.length;
                    TapeCDEnable = true;
                    Singleton<OtherMainStuffManager>.Instance.deafshit(welcomeOld.audClip.length,"All");
                    if (ZerullClassic.Instance.zs != null && ZerullClassic.Instance.RealBossStarted)
                    {
                        StartCoroutine(StunBoss());
                        IEnumerator StunBoss()
                        {
                            while (ZerullClassic.Instance.maxHealth == ZerullClassic.Instance.health-1 && !ZerullClassic.Instance.realBossStarted && ZerullClassic.Instance.GetBoss().hitted || ZerullClassic.Instance.isbroyapping)
                            {
                                yield return null;
                            }
                            ZerullClassic.Instance.OnHit(welcomeOld.audClip.length/2,5f);
                        }
                    }
                }
                if (tapeType == "jerrypeakassDisc")
                {
                    sprite.sprite = closedSprite;
                    tapeDevice.ClearQueue(true);
                    tapeDevice.PlaySingleClip(LitearllyPEAK);
                    TapeCD = LitearllyPEAK.audClip.length;
                    TapeCDEnable = true;
                    Singleton<OtherMainStuffManager>.Instance.PeakStun(LitearllyPEAK.audClip.length,"All");
                }
                if (tapeType == "jerrypeakassDiscExpert")
                {
                    sprite.sprite = closedSprite;
                    tapeDevice.ClearQueue(true);
                    tapeDevice.PlaySingleClip(peakbutShorter);
                    TapeCD = peakbutShorter.audClip.length;
                    TapeCDEnable = true;
                    Singleton<OtherMainStuffManager>.Instance.PeakStun(peakbutShorter.audClip.length,"All");
                }
            }
            if (holyshitIsItRealTape == IsTape.PayPhone)
            {
                sprite.sprite = closedSprite;
                tapeDevice.ClearQueue(true);
                tapeDevice.PlaySingleClip(NormTapeAudio);
                TapeCD = AntiHearingDuration;
                TapeCDEnable = true;
                Singleton<OtherMainStuffManager>.Instance.deafshit(AntiHearingDuration,"All");
            }
            
        }
    }
    #endregion

    public float AntiHearingDuration = 30f, TapeCD = 30f;
    public bool TapeCDEnable;
    #region Serialized Configuration
    [Header("Sprite Settings")]
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;
    #endregion

    #region Internal State
    private SpriteRenderer sprite;
    public AudioObjectyeah NormTapeAudio,welcomeOld,LitearllyPEAK,peakbutShorter;
    public AudioManagerLiveReaction tapeDevice;
    #endregion
    public IsTape holyshitIsItRealTape;
    public enum IsTape
    {
        PayPhone,
        TapePlayer
    }
}