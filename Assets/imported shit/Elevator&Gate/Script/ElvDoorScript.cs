using System.Collections;
using UnityEngine;

public class ElvDoorScript : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private bool entranceElevator = false;
    [SerializeField] private float openDelay = 1f;

    [Header("Door State")]
    private bool opening = false;
    public bool closed = false, triggered = false, canOpen = false, finaleActivated = false;
    public bool Opendor = false;

    [Header("References")]
    [SerializeField] private MeshCollider elvInCollider;
    [SerializeField] private GameObject MapSideIcon;

    [SerializeField] private MeshCollider elvOutCollider;
    [SerializeField] private Animator elvDoorInAnimator,elvDoorOutAnimator;
    [SerializeField] private AudioObjectyeah doorOpenClip,doorCloseClip;
    [SerializeField] private GateScript currentGate;

    private GameControllerScript gc;
    private AudioManagerLiveReaction audioSource;
    private float openTimer;

    #region Unity Methods

    private void Start()
    {
        gc = FindObjectOfType<GameControllerScript>();
        audioSource = GetComponent<AudioManagerLiveReaction>();

        if (entranceElevator)
        {
            openTimer = openDelay;
            canOpen = true;
        }
        else
        {
            openTimer = 99f;
            canOpen = false;
        }
    }

    private void Update()
    {
        // Check finale trigger
        if (Opendor && !opening && !triggered && !finaleActivated)
        {
            triggered = true;
            openTimer = 0f;
            canOpen = true;
            MapSideIcon.SetActive(false);
        }
        if (entranceElevator && gc.spoopMode && !closed || entranceElevator && tutorTrigger.Instance.closeElevator && !closed)
        {
            Close();
            closed = true;
        }

        HandleOpenTimer();
        UpdateDoorState();
    }

    #endregion

    #region Door Logic

    private void HandleOpenTimer()
    {
        if (canOpen && openTimer > 0f) openTimer -= Time.deltaTime;
        if (canOpen && openTimer <= 0f && !opening) OpenDoors();
    }

    private void OpenDoors()
    {
        MapSideIcon.SetActive(false);
        opening = true;
        openTimer = -5f; // prevents repeated opening
        audioSource.ClearQueue(true);
        audioSource.PlaySingleClip(doorOpenClip);
        elvDoorInAnimator.SetTrigger("Open");
        elvDoorOutAnimator.SetTrigger("Open");
    }

    public bool IsOpening()
    {
        return opening;
    }

    private void UpdateDoorState()
    {
        bool isOpen = opening;

        elvInCollider.enabled = !isOpen;
        elvOutCollider.enabled = !isOpen;
        elvDoorInAnimator.SetBool("IsOpen", isOpen);
        elvDoorOutAnimator.SetBool("IsOpen", isOpen);
    }

    public void Close()
    {
        if (!canOpen || !opening) return;
        if (gc.finaleMode || gc.mode == "LappingOfAsylum" && gc.LapManag.allowClosElev) currentGate?.Down();
        Opendor = false;
        opening = false;
        openTimer = 99f;
        canOpen = false;
        MapSideIcon.SetActive(true);
        audioSource.ClearQueue(true);
        audioSource.PlaySingleClip(doorCloseClip);
        elvDoorInAnimator.SetTrigger("Close");
        elvDoorOutAnimator.SetTrigger("Close");
        if (triggered)
        {
            finaleActivated = true;
            triggered = false;
        }
    }

    public void SetOpenSeconds(float seconds)
    {
        canOpen = true;
        openTimer = seconds;
    }

    #endregion
}