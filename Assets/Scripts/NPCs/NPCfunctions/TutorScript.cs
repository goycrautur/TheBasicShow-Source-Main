using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class TutorScript : MonoBehaviour
{
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        tutorSource = GetComponent<AudioManagerLiveReaction>();
        tutorSource.ClearQueue(true);
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerObject.transform;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (IsLolbit) StartCoroutine(sillyRiseLols());
    }

    private void Update()
    {
        if (tiemer > 0f && !IsLolbit) tiemer -= Time.deltaTime;
        if (agent.enabled && gameObject.activeSelf && !IsLolbit) agent.SetDestination(target.position);
        if (tiemer < 0f && !gottothepositio && !IsLolbit)
        {
            gottothepositio = true;
            //agent.speed = 1f;
            intro();
        }
        if (Input.GetKeyDown(KeyCode.P) && !triggeredCounting && gottothepositio && !IsLolbit)
        {
            tutorAnimation.Rebind();
            tutorAnimation.Play("BaldiWave", -1, 0f);
            tutorSource.ClearQueue(true);
            tutorSource.QueueAudio(aud_Hi);
        }

        if (Countdown && !triggeredCounting && gottothepositio)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance > spoopDistance)
            {
                triggeredCounting = true;
                StartCoroutine(PlayCountdown());
            }
        }
    }
    private IEnumerator sillyRiseLols()
    {
        bitRiseAnim.Rebind();
        bitRiseAnim.Play("rise", -1, 0f);
        tutorSource.ClearQueue(true);
        tutorSource.QueueAudio(concret);
        yield return new WaitForSeconds(5.2f);
        intro();
    }
    public void intro()
    {
        gc.musi();
        tutorSource.ClearQueue(true);
        tutorSource.QueueAudio(aud_Hi);
    }

    private IEnumerator PlayCountdown()
    {
        while (tutorAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f && !tutorAnimation.IsInTransition(0))
        {
            yield return null;
        }
        tutorAnimation.enabled = false;

        foreach (AudioObjectyeah clip in countdownClips)
        {
            yield return StartCoroutine(DelayWithSpriteChange());

            spriteRenderer.sprite = talkingSprite;
            tutorSource.QueueAudio(clip);
            yield return new WaitForSeconds(clip.audClip.length);
        }

        if (ReadyOrNot != null)
        {
            yield return StartCoroutine(DelayWithSpriteChange());

            spriteRenderer.sprite = talkingSprite;
            tutorSource.QueueAudio(ReadyOrNot);
            yield return new WaitForSeconds(ReadyOrNot.audClip.length);
        }

        GameControllerScript.Instance.ActivateSpoopMode();
        gameObject.SetActive(false);
    }

    private IEnumerator DelayWithSpriteChange()
    {
        float rand = Random.value;
        if (rand < peekChance) spriteRenderer.sprite = peekingSprite;
        else spriteRenderer.sprite = closedSprite;
        yield return new WaitForSeconds(delayClips);
    }
    [Header("Lolbit Settings")]
    [SerializeField] private bool IsLolbit;
    [SerializeField] private Animator bitRiseAnim;
    public AudioObjectyeah concret;
    [SerializeField] private float riseIntroDelay = 3f;

    [Header("Basic")]
    [SerializeField] private Animator tutorAnimation;
    public AudioObjectyeah aud_Hi,aud_Prize, ReadyOrNot;
    [SerializeField] private List<AudioObjectyeah> countdownClips;
    [SerializeField] private bool Countdown;

    [Header("Sprite States")]
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite talkingSprite, peekingSprite;

    private float spoopDistance = 75f;
    private float delayClips = 1f;
    private bool triggeredCounting = false, gottothepositio;
    private float peekChance = 0.15f;
    private NavMeshAgent agent;
    [SerializeField] private float tiemer;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform playerTransform, target;
    [HideInInspector] public AudioManagerLiveReaction tutorSource;
    public GameControllerScript gc;
}