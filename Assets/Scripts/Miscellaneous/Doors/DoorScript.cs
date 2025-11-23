using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour
{
    #region Initialization
    private void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }
    #endregion

    #region UpdateLoopHandlers
    private void Update()
    {
        HandleLockTimer();
        HandleOpenTimer();
        HandleDoorInteraction();
    }

    private void HandleLockTimer()
    {
        if (lockTime.CountdownWithDeltaTime(1f) == 0 & bDoorLocked)
        {
            UnlockDoor();
        }
    }

    private void HandleOpenTimer()
    {
        if (openTime.CountdownWithDeltaTime() == 0 & bDoorOpen)
        {
            CloseDoor();
        }
    }

    private void HandleDoorInteraction()
    {
        if ((Input.GetMouseButtonDown(0) | Singleton<InputManager>.Instance.GetActionKey(InputAction.Interact)) && trigger.ScreenRaycastMatchesCollider(out _, GameC.player.LocalRange) && Time.timeScale != 0f)
        {
            if (bDoorLocked)
            {
                myAudio.PlayOneShot(GameC.aud_Rattling);
            }
            else
            {
                if (!bDoorOpen)
                {
                    Singleton<OtherMainStuffManager>.Instance.HearingShit(1f, this.transform, new Vector3(0f,0f,0f), "all",false);
                }
                OpenDoor(3);
            }
        }
    }
    #endregion

    #region DoorStateManagement
    public void OpenDoor(float time)
    {
        if (!bDoorOpen)
        {
            myAudio.PlayOneShot(doorOpen);
            GameC.SubsManager.summonLeSubtitle(GameC.subtitlesScriptableObject[0].subtitleOption,GameC.subtitlesScriptableObject[0],0f,GetComponent<AudioSource>());
        }
        SetDoorState(true, time);
    }

    private void CloseDoor()
    {
        SetDoorState(false);
        myAudio.PlayOneShot(doorClose);
        GameC.SubsManager.summonLeSubtitle(GameC.subtitlesScriptableObject[1].subtitleOption,GameC.subtitlesScriptableObject[1],0f,GetComponent<AudioSource>());
    }

    private void SetDoorState(bool open, float time = 3)
    {
        barrier.enabled = !open;
        invisibleBarrier.enabled = !open;
        secondBarrier.enabled = !open;
        bDoorOpen = open;

        inside.material = open ? doorOpenmat1 : doorClosemat1;
        outside.material = open ? doorOpenmat2 : doorClosemat2;

        if (time != 0)
        {
            openTime = time;
        }
    }
    #endregion

    #region LockingMechanics
    public void LockDoor(float time)
    {
        myAudio.PlayOneShot(Click);
        bDoorLocked = true;
        DorMapSprite1.sprite = AdditionalGameCustomizer.Instance.dorMapLockedSprite;
        DorMapSprite2.sprite = AdditionalGameCustomizer.Instance.dorMapLockedSprite;
        lockTime = time;
    }

    public void UnlockDoor()
    {
        bDoorLocked = false;
        DorMapSprite1.sprite = AdditionalGameCustomizer.Instance.dorMapSprite;
        DorMapSprite2.sprite = AdditionalGameCustomizer.Instance.dorMapSprite;
        myAudio.PlayOneShot(GameC.aud_Unlocked);
    }

    public bool DoorLocked => bDoorLocked;
    #endregion

    #region CollisionHandlers
    private void OnTriggerStay(Collider OPEN)
    {
        if (!bDoorLocked & OPEN.CompareTag("NPC") & !Check || !bDoorLocked & OPEN.CompareTag("cork") & !Check || !bDoorLocked & OPEN.CompareTag("Projectile") & !Check)
        {
            OpenDoor(3);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!(GameC.principal.angry || other.transform.name != "Principal of the Thing") || !(GameC.maxplayGames.angry || other.transform.name != "maxplay games if he was principal"))
        {
            HandlePrincipalInteraction();
        }

        if (bDoorLocked && other.transform.name == "Baldi" || bDoorLocked && other.transform.name == "Jerry" || bDoorLocked && other.transform.name == "Mucho" || bDoorLocked && other.CompareTag("cork"))
        {
            UnlockDoor();
            OpenDoor(3);
        }
    }

    private void HandlePrincipalInteraction()
    {
        if (Faculty)
        {
            if (FacultyTimesTwo)
            {
                if (!bDoorOpen)
                {
                    StartCoroutine(FacultyDoor());
                }
            }
            else
            {
                if (GameC.principal.isActiveAndEnabled)
                {
                    if (GameC.principal.onFaculty)
                    {
                        GameC.principal.onFaculty = false;
                    }
                }
                if (GameC.maxplayGames.isActiveAndEnabled)
                {
                    if (GameC.maxplayGames.onFaculty)
                    {
                        GameC.maxplayGames.onFaculty = false;
                    }
                }
                else if (!bDoorOpen)
                {
                    StartCoroutine(FacultyDoor());
                }
            }
        }

        if (bDoorLocked)
        {
            OpenDoor(0.33f);
        }
    }

    public IEnumerator FacultyDoor()
    {
        myAudio.PlayOneShot(KnockKnock, 1f);
        Check = true;
        if (GameC.principal.isActiveAndEnabled)
        {
            StartCoroutine(GameC.principal.CheckTheDoor());
        }
        if (GameC.maxplayGames.isActiveAndEnabled)
        {
            StartCoroutine(GameC.maxplayGames.CheckTheDoor());
        }
        yield return new WaitForSeconds(2);
        Check = false;
    }
    #endregion

    #region SerializedConfig
    [Header("Audio Settings")]
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip doorClose, KnockKnock, Click;
    private AudioSource myAudio;

    [Header("Barrier Settings")]
    [SerializeField] private MeshCollider barrier;
    [SerializeField] private MeshCollider secondBarrier, trigger, invisibleBarrier;

    [Header("Renderer Settings")]
    [SerializeField] private MeshRenderer inside;
    [SerializeField] private MeshRenderer outside;

    [Header("Door Behavior Settings")]
    [SerializeField] private GameControllerScript GameC;
    [SerializeField] private bool Faculty, FacultyTimesTwo, Check;
    [SerializeField] private SpriteRenderer DorMapSprite1,DorMapSprite2;

    [Header("Lock/Unlock Settings")]
    public float lockTime;
    public float openTime;
    #endregion

    #region RuntimeState
    private bool bDoorOpen, bDoorLocked;
    #endregion
    public Material doorOpenmat1, doorClosemat1,doorOpenmat2, doorClosemat2;
}