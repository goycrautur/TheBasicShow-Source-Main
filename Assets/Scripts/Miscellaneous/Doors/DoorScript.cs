using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour
{
    #region Initialization
    private void Start()
    {
        CurTrigger = MainTrigger;
        myAudio = GetComponent<AudioManagerLiveReaction>();
        AltTrigger.enabled = false;
        outside.material.SetTextureScale("_SecondTex", new Vector2(-1, 1));
        outside.material.SetTextureScale("_SecondaryDiffrent", new Vector2(-1, 1));
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
        if ((Input.GetMouseButtonDown(0) | Singleton<InputManager>.Instance.GetActionKey(InputAction.Interact)) && CurTrigger.ScreenRaycastMatchesCollider(out _, GameControllerScript.Instance.player.LocalRange,KeyFunctions.hi.PlayerClickablesLayer.value) && Time.timeScale != 0f)
        {
            if (bDoorLocked && SoundData != null) myAudio.PlaySingleClip(SoundData.Rattle);
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
        if (!bDoorOpen && SoundData != null) myAudio.PlaySingleClip(SoundData.doorOpen);
        SetDoorState(true, time);
    }

    private void CloseDoor()
    {
        SetDoorState(false);
        if (SoundData != null) myAudio.PlaySingleClip(SoundData.doorClose);
    }

    private void SetDoorState(bool open, float time = 3)
    {
        MainDoorBarrier.enabled = !open;
        CurTrigger = !open ? MainTrigger : AltTrigger;
        AltTrigger.enabled = open;
        MainTrigGmbObj.layer = !open ? LayerMask.NameToLayer(DefaultLayerString) : LayerMask.NameToLayer(MainTriggerLayerString);
        bDoorOpen = open;


        int shift = open ? 1 : 0;
        inside.material.SetInt("_Swap", shift);
        outside.material.SetInt("_Swap", shift);

        if (time != 0) openTime = time;
    }
    #endregion

    #region LockingMechanics
    public void LockDoor(float time)
    {
        if (SoundData != null) myAudio.PlaySingleClip(SoundData.Click);
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
        if (SoundData != null) myAudio.PlaySingleClip(SoundData.Unlocked);
    }

    public bool DoorLocked => bDoorLocked;

    public void HandlePrincipalInteraction()
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

        if (bDoorLocked) OpenDoor(0.33f);
    }

    public IEnumerator FacultyDoor()
    {
        if (SoundData != null) myAudio.PlaySingleClip(SoundData.KnockKnock);
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
    [SerializeField] private NormalDoorSoundData SoundData;
    
    [Header("Barrier Stuff Settings")]
    [SerializeField] private BoxCollider MainDoorBarrier;
    [SerializeField] private MeshCollider CurTrigger,MainTrigger,AltTrigger;
    [SerializeField] private GameObject MainTrigGmbObj;
    [SerializeField] private string DefaultLayerString,MainTriggerLayerString;

    [Header("Renderer Settings")]
    [SerializeField] private MeshRenderer inside;
    [SerializeField] private MeshRenderer outside;

    [Header("Door Behavior Settings")]
    [SerializeField] private bool Faculty;
    [SerializeField] private bool FacultyTimesTwo;
    public bool Check;
    [SerializeField] private SpriteRenderer DorMapSprite1,DorMapSprite2;
   

    [Header("Lock/Unlock Settings")]
    public float lockTime;
    public float openTime;
    #endregion

    #region RuntimeState
    [HideInInspector] public bool bDoorOpen, bDoorLocked;
    #endregion
}