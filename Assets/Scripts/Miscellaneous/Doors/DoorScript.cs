using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour
{
    #region Initialization
    private void Start()
    {
        myAudio = GetComponent<AudioManagerLiveReaction>();
    }
    #endregion

    #region UpdateLoopHandlers
    private void Update()
    {
        HandleLockTimer();
        HandleOpenTimer();
        HandleDoorInteraction();
        //end myself
        inside.material.SetFloat("_VertexGlitchSeed", Singleton<VertexGlitchManager>.Instance.global_VertexGlitchSeed);
        inside.material.SetFloat("_VertexGlitchIntensity", Singleton<VertexGlitchManager>.Instance.global_VertexGlitchIntensity);
        inside.material.SetInt("_ValueX", Singleton<VertexGlitchManager>.Instance.global_glitchColorRvalue);
        inside.material.SetInt("_ValueY", Singleton<VertexGlitchManager>.Instance.global_glitchColorGvalue);
        inside.material.SetInt("_ValueZ", Singleton<VertexGlitchManager>.Instance.global_glitchColorBvalue);
        outside.material.SetFloat("_VertexGlitchSeed", Singleton<VertexGlitchManager>.Instance.global_VertexGlitchSeed);
        outside.material.SetFloat("_VertexGlitchIntensity", Singleton<VertexGlitchManager>.Instance.global_VertexGlitchIntensity);
        outside.material.SetInt("_ValueX", Singleton<VertexGlitchManager>.Instance.global_glitchColorRvalue);
        outside.material.SetInt("_ValueY", Singleton<VertexGlitchManager>.Instance.global_glitchColorGvalue);
        outside.material.SetInt("_ValueZ", Singleton<VertexGlitchManager>.Instance.global_glitchColorBvalue);
    }

    private void HandleLockTimer()
    {
        if (lockTime.CountdownWithDeltaTime(1f) == 0 & bDoorLocked) UnlockDoor();
    }

    private void HandleOpenTimer()
    {
        if (openTime.CountdownWithDeltaTime() == 0 & bDoorOpen) CloseDoor();
    }

    private void HandleDoorInteraction()
    {
        if ((Input.GetMouseButtonDown(0) | Singleton<InputManager>.Instance.GetActionKey(InputAction.Interact)) && trigger.ScreenRaycastMatchesCollider(out _, GameControllerScript.Instance.player.LocalRange,KeyFunctions.hi.PlayerClickablesLayer.value) && Time.timeScale != 0f)
        {
            if (bDoorLocked) myAudio.PlaySingleClip(Rattle);
            else
            {
                if (!bDoorOpen) Singleton<OtherMainStuffManager>.Instance.HearingShit(1f, this.transform, new Vector3(0f,0f,0f), "all",false);
                OpenDoor(3);
            }
        }
    }
    #endregion

    #region DoorStateManagement
    public void OpenDoor(float time)
    {
        if (!bDoorOpen) myAudio.PlaySingleClip(doorOpen);
        SetDoorState(true, time);
    }

    private void CloseDoor()
    {
        SetDoorState(false);
        myAudio.PlaySingleClip(doorClose);
    }

    private void SetDoorState(bool open, float time = 3)
    {
        barrier.enabled = !open;
        invisibleBarrier.enabled = !open;
        secondBarrier.enabled = !open;
        bDoorOpen = open;

        int shift = open ? 1 : 0;
        inside.material.SetInt("_Swap", shift);
        outside.material.SetInt("_Swap", shift);

        if (time != 0)
        {
            openTime = time;
        }
    }
    #endregion

    #region LockingMechanics
    public void LockDoor(float time)
    {
        myAudio.PlaySingleClip(Click);
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
        myAudio.PlaySingleClip(Unlocked);
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
        if (!(GameControllerScript.Instance.principal.angry || other.transform.name != "Principal of the Thing") || !(GameControllerScript.Instance.maxplayGames.angry || other.transform.name != "maxplay games if he was principal"))
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
            if (FacultyTimesTwo && !bDoorOpen) StartCoroutine(FacultyDoor());
            else
            {
                if (GameControllerScript.Instance.principal.isActiveAndEnabled && GameControllerScript.Instance.principal.onFaculty)GameControllerScript.Instance.principal.onFaculty = false;
                if (GameControllerScript.Instance.maxplayGames.isActiveAndEnabled && GameControllerScript.Instance.maxplayGames.onFaculty)GameControllerScript.Instance.maxplayGames.onFaculty = false;
                else if (!bDoorOpen) StartCoroutine(FacultyDoor());
            }
        }

        if (bDoorLocked)
        {
            OpenDoor(0.33f);
        }
    }

    public IEnumerator FacultyDoor()
    {
        myAudio.PlaySingleClip(KnockKnock);
        Check = true;
        if (GameControllerScript.Instance.principal.isActiveAndEnabled) StartCoroutine(GameControllerScript.Instance.principal.CheckTheDoor());
        if (GameControllerScript.Instance.maxplayGames.isActiveAndEnabled) StartCoroutine(GameControllerScript.Instance.maxplayGames.CheckTheDoor());
        yield return new WaitForSeconds(2);
        Check = false;
    }
    #endregion

    #region SerializedConfig
    [Header("Audio Settings")]
    private AudioManagerLiveReaction myAudio;
    [SerializeField] private AudioObjectyeah doorOpen,doorClose, KnockKnock, Click,Rattle,Unlocked;
    
    [Header("Barrier Settings")]
    [SerializeField] private MeshCollider barrier;
    [SerializeField] private MeshCollider secondBarrier, trigger, invisibleBarrier;

    [Header("Renderer Settings")]
    [SerializeField] private MeshRenderer inside;
    [SerializeField] private MeshRenderer outside;

    [Header("Door Behavior Settings")]
    [SerializeField] private bool Faculty;
    [SerializeField] private bool FacultyTimesTwo, Check;
    [SerializeField] private SpriteRenderer DorMapSprite1,DorMapSprite2;
   

    [Header("Lock/Unlock Settings")]
    public float lockTime;
    public float openTime;
    #endregion

    #region RuntimeState
    private bool bDoorOpen, bDoorLocked;
    #endregion
}