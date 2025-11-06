using UnityEngine;
using System.Collections;

public class TapePlayerScript : MonoBehaviour
{
    #region Initialization
    private void Start()
    {
        audioDevice = GetComponent<AudioSource>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    #endregion

    #region Per-Frame Logic
    private void Update()
    {
        if (!GameControllerScript.Instance.baldiScrpt.antiHearing)
        {
            sprite.sprite = openSprite;
        }
    }
    #endregion

    #region Public Methods
    public void Play()
    {
        sprite.sprite = closedSprite;
        audioDevice.Play();
        if (GameControllerScript.Instance.baldiScrpt.isActiveAndEnabled)
        {
            GameControllerScript.Instance.baldiScrpt.ActivateAntiHearing(AntiHearingDuration);
        }
        if (GameControllerScript.Instance.famishScrpt.isActiveAndEnabled)
        {
            GameControllerScript.Instance.famishScrpt.ActivateAntiHearing(AntiHearingDuration);
        }
        if (GameControllerScript.Instance.zerulscrpt.isActiveAndEnabled)
        {
            GameControllerScript.Instance.zerulscrpt.ActivateAntiHearing(AntiHearingDuration);
        }
        if (GameControllerScript.Instance.muchoing.isActiveAndEnabled)
        {
            GameControllerScript.Instance.muchoing.ActivateAntiHearing(AntiHearingDuration);
        }
        if (ZerullClassic.Instance.zs != null && ZerullClassic.Instance.RealBossStarted)
        {
            StartCoroutine(StunBoss());
            IEnumerator StunBoss()
            {
                while (ZerullClassic.Instance.maxHealth == ZerullClassic.Instance.health-1 && !ZerullClassic.Instance.realBossStarted && ZerullClassic.Instance.GetBoss().hitted || ZerullClassic.Instance.isbroyapping)
                {
                    yield return null;
                }
                ZerullClassic.Instance.OnHit(AntiHearingDuration/2,5f);
            }
        }
    }
    #endregion

    public float AntiHearingDuration = 30f;
    #region Serialized Configuration
    [Header("Sprite Settings")]
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;
    #endregion

    #region Internal State
    private SpriteRenderer sprite;
    private AudioSource audioDevice;
    #endregion
}