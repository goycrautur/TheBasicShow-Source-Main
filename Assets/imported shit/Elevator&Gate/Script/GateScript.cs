using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GateScript : MonoBehaviour
{
    [Header("Gate Settings")]
    [SerializeField] private float moveSpeed = 9f;
    [SerializeField] private float gateDownY = -10f;
    [SerializeField] private float gateneutralY = 0f;
    public GameObject barrier;
    public GameObject gate;
    [SerializeField] private GameObject MapSideIcon;
    [SerializeField] private AudioClip gateSlamClip;
    [SerializeField] public GameControllerScript gc;

    [Header("State")]
    public bool closed = false;
    public bool activated = false;

    private AudioSource audioSource;
    private Transform gateTransform;
    private Vector3 upPosition;
    private Vector3 downPosition,neutralPosition;

    private void Start()
    {
        gateTransform = transform;
        audioSource = GetComponent<AudioSource>();

        upPosition = gateTransform.position;
        downPosition = new Vector3(upPosition.x, gateDownY, upPosition.z);
        neutralPosition = new Vector3(upPosition.x, gateneutralY, upPosition.z);
    }

    private void Update()
    {
        HandleMovement();
        HandleAudio();
    }

    private void HandleMovement()
    {
        if (activated)
        {
            gateTransform.position = Vector3.MoveTowards(gateTransform.position, downPosition, moveSpeed * Time.deltaTime);
            barrier.SetActive(true);
            gate.SetActive(true);
            MapSideIcon.SetActive(true);

            if (!closed && gateTransform.position.y <= gateDownY)
                closed = true;
        }
        else
        {
            gateTransform.position = Vector3.MoveTowards(gateTransform.position, neutralPosition, moveSpeed * Time.deltaTime);
            barrier.SetActive(false);
            gate.SetActive(true);
            closed = false; // reset if rising
            MapSideIcon.SetActive(false);
            
        }
    }

    private void HandleAudio()
    {
        if (Time.timeScale == 0)
        {
            audioSource.Pause();
            return;
        }

        if (!audioSource.isPlaying && activated)
        {
            audioSource.UnPause();
        }
    }
    private IEnumerator gateSubsCoroutine()
    {
        GameControllerScript.Instance.SubsManager.summonLeSubtitle(GameControllerScript.Instance.subtitlesScriptableObject[5].subtitleOption, GameControllerScript.Instance.subtitlesScriptableObject[5], gateSlamClip.length - 2.4f, GetComponent<AudioSource>());
        yield return new WaitForSeconds(gateSlamClip.length - 2.4f);
        GameControllerScript.Instance.SubsManager.summonLeSubtitle(GameControllerScript.Instance.subtitlesScriptableObject[6].subtitleOption, GameControllerScript.Instance.subtitlesScriptableObject[6], 1.4f, GetComponent<AudioSource>());
        yield break;
    }

    public void Down(bool hi = true)
    {
        activated = hi;
        if (gateTransform.position.y > gateDownY)
        {
            if (hi)
            {
                audioSource.PlayOneShot(gateSlamClip);
                StartCoroutine(gateSubsCoroutine());
                return;
            }
            return;
        }
    }
}