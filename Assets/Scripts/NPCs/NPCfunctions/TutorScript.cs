using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class TutorScript : MonoBehaviour
{
    private void Start()
    {
        tutorAnimation.enabled = false;
        tutorSource.Stop();
        agent = GetComponent<NavMeshAgent>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerObject.transform;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        tutorSource = GetComponent<AudioSource>();
        gc = FindObjectOfType<GameControllerScript>();
    }

    private void Update()
    {
        if (tiemer > 0f)
        {
           tiemer -= Time.deltaTime;
        }
        
        if (agent.enabled && gameObject.activeSelf)
        {
            agent.SetDestination(target.position);
        }
        if (tiemer < 0f && !gottothepositio)
        {
            gottothepositio = true;
            agent.speed = 0f;
            intro();
        }
        if (Input.GetKeyDown(KeyCode.P) && !triggeredCounting && gottothepositio)
        {
            tutorAnimation.Rebind();
            tutorSource.Stop();
            tutorAnimation.Play("BaldiWave", -1, 0f);
            tutorSource.PlayOneShot(aud_Hi);
            //UIPopupTextManagerWithMovement.Show("baldhi", Color.green, base.transform, 2.221f, 1.319f);
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
    public void intro()
    {
        gc.musi();
        tutorAnimation.enabled = true;
        tutorAnimation.Rebind();
        tutorSource.Stop();
        tutorAnimation.Play("BaldiWave", -1, 0f);
        //tutorSource.PlayOneShot(aud_Hi);
        //UIPopupTextManagerWithMovement.Show("baldhi", Color.green, base.transform, 5.221f, 1.319f);
    }

    public IEnumerator captions()
    {
        yield return new WaitForSeconds(1.7f);
    }

    private IEnumerator PlayCountdown()
    {
        while (tutorAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f && !tutorAnimation.IsInTransition(0))
        {
            yield return null;
        }
        tutorAnimation.enabled = false;

        foreach (AudioClip clip in countdownClips)
        {
            yield return StartCoroutine(DelayWithSpriteChange());

            spriteRenderer.sprite = talkingSprite;
            tutorSource.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }

        if (ReadyOrNot != null)
        {
            yield return StartCoroutine(DelayWithSpriteChange());

            spriteRenderer.sprite = talkingSprite;
            tutorSource.PlayOneShot(ReadyOrNot);
            yield return new WaitForSeconds(ReadyOrNot.length);
        }

        GameControllerScript.Instance.ActivateSpoopMode();
        gameObject.SetActive(false);
    }

    private IEnumerator DelayWithSpriteChange()
    {
        float rand = Random.value;
        if (rand < peekChance)
        {
            spriteRenderer.sprite = peekingSprite;
        }
        else
        {
            spriteRenderer.sprite = closedSprite;
        }

        yield return new WaitForSeconds(delayClips);
    }

    [Header("Basic")]
    [SerializeField] private Animator tutorAnimation;
    [SerializeField] private AudioClip aud_Hi, ReadyOrNot;
    [SerializeField] private List<AudioClip> countdownClips;
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
    [HideInInspector] public AudioSource tutorSource;
    public GameControllerScript gc;
}