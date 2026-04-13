using UnityEngine;
using System.Collections.Generic;

public class SwingingDoorScript : MonoBehaviour
{
    #region Initialization
    private void Start()
    {
        gc = GameControllerScript.Instance;
        myAudio = GetComponent<AudioManagerLiveReaction>();
        SetLock(true);
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
        if (lockTime.CountdownWithDeltaTime() == 0 & bDoorLocked & requirementMet) SetLock(false);
    }

    private void HandleDoorClosing()
    {
        if (openTime.CountdownWithDeltaTime() == 0 & bDoorOpen & !bDoorLocked) SetDoorState(false);
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
        if (open.CompareTag("Player")) HandlePlayerInteraction();
        else if (open.CompareTag("NPC") || open.CompareTag("cork") || open.CompareTag("Projectile")) HandleNPCInteraction();
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
            Singleton<OtherMainStuffManager>.Instance.HearingShit(1f, this.transform, new Vector3(0f,0f,0f), "all",false);
        }
    }

    private void HandleNPCInteraction()
    {
        if ((!myAudio.audioDevice.isPlaying || !bDoorOpen) && !bDoorLocked) PlayDoorSound();
    }

    private void PlayDoorSound()
    {
        myAudio.PlaySingleClip(doorOpen);
    }
    #endregion

    #region DoorState
    private void SetDoorState(bool set)
    {
        heardDoor = set;
        bDoorOpen = set;
        int shift = set ? 1 : 0;
        inside.material.SetInt("_Swap", shift);
        outside.material.SetInt("_Swap", shift);
    }

    private void SetLock(bool lockState)
    {
        obstacle.SetActive(lockState);
        insidecol.enabled = lockState;
        outsidecol.enabled = !lockState;
        bDoorLocked = lockState;
        SwinDorMapSprite1.sprite = lockState ? AdditionalGameCustomizer.Instance.dorMapLockedSprite : AdditionalGameCustomizer.Instance.dorMapSprite;
        SwinDorMapSprite2.sprite = lockState ? AdditionalGameCustomizer.Instance.dorMapLockedSprite : AdditionalGameCustomizer.Instance.dorMapSprite;

        inside.material = lockState ? lockedIn : normal1;
        outside.material = lockState ? lockedOut : normal2;
    }
    #endregion

    #region SerializedFields
    private GameControllerScript gc;

    [Header("Door Mechanics and Materials")]
    [SerializeField] private GameObject obstacle;
    [SerializeField] private MeshCollider insidecol, outsidecol;
    [SerializeField] private MeshRenderer inside, outside;
    [SerializeField] private Material normal1, lockedIn, normal2, lockedOut;
    [SerializeField] private SpriteRenderer SwinDorMapSprite1,SwinDorMapSprite2;

    [Header("Door state and timing")]
    [SerializeField] private bool bDoorOpen;
    public bool bDoorLocked;
    [SerializeField] private float openTime;

    [Header("Audio-related variables")]
    private AudioManagerLiveReaction myAudio;
    [SerializeField] private AudioObjectyeah doorOpen;

    [Header("Flags and conditions")]
    [SerializeField] private bool heardDoor;
    [SerializeField] private bool bUnlockDoor;
    #endregion

    #region RuntimeState
    private float lockTime;
    private bool requirementMet;
    #endregion
}