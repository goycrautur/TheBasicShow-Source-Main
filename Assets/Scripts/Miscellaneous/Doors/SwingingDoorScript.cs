using UnityEngine;

public class SwingingDoorScript : MonoBehaviour
{
    #region Initialization
    private void Start()
    {
        myAudio = GetComponent<AudioSource>();
        bDoorLocked = true;
        SwinDorMapSprite1.sprite = AdditionalGameCustomizer.Instance.dorMapLockedSprite;
        SwinDorMapSprite2.sprite = AdditionalGameCustomizer.Instance.dorMapLockedSprite;
    }
    #endregion

    #region UpdateLoopHandlers
    private void Update()
    {
        HandleUnlocking();
        HandleTimers();
        HandleDoorClosing();
    }

    private void HandleUnlocking()
    {
        if (bUnlockDoor && bDoorLocked)
        {
            SetLock(false);
            bUnlockDoor = false;
        }

        if (!requirementMet && gc.notebooks >= gc.UnlockAmount)
        {
            requirementMet = true;
            SwinDorMapSprite1.sprite = AdditionalGameCustomizer.Instance.dorMapSprite;
            SwinDorMapSprite2.sprite = AdditionalGameCustomizer.Instance.dorMapSprite;
            SetLock(false);
        }
    }

    private void HandleTimers()
    {
        if (lockTime.CountdownWithDeltaTime() == 0 & bDoorLocked & requirementMet)
        {
            SetLock(false);
        }
    }

    private void HandleDoorClosing()
    {
        if (openTime.CountdownWithDeltaTime() == 0 & bDoorOpen & !bDoorLocked)
        {
            SetDoorState(false);
        }
    }
    #endregion

    #region TriggerDetection
    private void OnTriggerStay(Collider opened)
    {
        if ((opened.CompareTag("Player") || (opened.CompareTag("NPC") & opened.isTrigger) && !bDoorLocked || (opened.CompareTag("cork") & opened.isTrigger)) && !bDoorLocked)
        {
            SetDoorState(true);
            openTime = 2f;
        }
    }

    private void OnTriggerEnter(Collider open)
    {
        if (open.CompareTag("Player"))
        {
            HandlePlayerInteraction();
        }
        else if (open.CompareTag("NPC") || open.CompareTag("cork") || open.CompareTag("Projectile"))
        {
            HandleNPCInteraction();
        }
    }
    #endregion

    #region DoorInteraction
    public void LockDoor(float time)
    {
        SetLock(true);
        lockTime = time;
    }

    private void HandlePlayerInteraction()
    {
        if ((gc.notebooks <= gc.maxNotebooks || gc.mode == "endless") && !heardDoor && !bDoorLocked)
        {
            PlayDoorSound();
            gc.baldiScrpt?.Hear(transform.position, 1);
            gc.famishScrpt?.Hear(transform.position, 1);
            gc.muchoing?.Hear(transform.position, 1);
            gc.zerulscrpt?.Hear(transform.position, 1);
        }
    }

    private void HandleNPCInteraction()
    {
        if ((!myAudio.isPlaying || !bDoorOpen) && !bDoorLocked)
        {
            PlayDoorSound();
        }
    }

    private void PlayDoorSound()
    {
        myAudio.PlayOneShot(doorOpen);
        gc.SubsManager.summonLeSubtitle(gc.subtitlesScriptableObject[4].subtitleOption,gc.subtitlesScriptableObject[4],0f,GetComponent<AudioSource>());
    }
    #endregion

    #region DoorState
    private void SetDoorState(bool set)
    {
        heardDoor = set;
        bDoorOpen = set;
        inside.material = set ? opened : normal;
        outside.material = set ? opened2 : normal2;
    }

    private void SetLock(bool lockState)
    {
        barrier.enabled = lockState;
        obstacle.SetActive(lockState);
        bDoorLocked = lockState;
        SwinDorMapSprite1.sprite = lockState ? AdditionalGameCustomizer.Instance.dorMapLockedSprite : AdditionalGameCustomizer.Instance.dorMapSprite;
        SwinDorMapSprite2.sprite = lockState ? AdditionalGameCustomizer.Instance.dorMapLockedSprite : AdditionalGameCustomizer.Instance.dorMapSprite;

        inside.material = lockState ? locked : normal;
        outside.material = lockState ? locked2 : normal2;
    }
    #endregion

    #region SerializedFields
    [Header("References")]
    [SerializeField] private GameControllerScript gc;

    [Header("Door Mechanics and Materials")]
    [SerializeField] private MeshCollider barrier;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private MeshRenderer inside, outside;
    [SerializeField] private Material normal, opened, locked, normal2, opened2, locked2;
    [SerializeField] private SpriteRenderer SwinDorMapSprite1,SwinDorMapSprite2;

    [Header("Door state and timing")]
    [SerializeField] private bool bDoorOpen;
    [SerializeField] private bool bDoorLocked;
    [SerializeField] private float openTime;

    [Header("Audio-related variables")]
    private AudioSource myAudio;
    [SerializeField] private AudioClip doorOpen;

    [Header("Flags and conditions")]
    [SerializeField] private bool heardDoor;
    [SerializeField] private bool bUnlockDoor;
    #endregion

    #region RuntimeState
    private float lockTime;
    private bool requirementMet;
    #endregion
}