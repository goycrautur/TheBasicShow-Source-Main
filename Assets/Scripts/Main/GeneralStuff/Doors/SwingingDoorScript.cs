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
        InitializeShaderVariableStuff();
        SwinDorMapSprite1.sprite = SwinDorMapSprite2.sprite = AdditionalGameCustomizer.Instance.dorMapLockedSprite;
    }
    #endregion

    private void InitializeShaderVariableStuff()
    {
        inside.material.SetTexture("_MainTex", GameControllerScript.Instance.GlobalTextures[(int)WalTypSideIn].tex);
        outside.material.SetTexture("_MainTex", GameControllerScript.Instance.GlobalTextures[(int)WalTypSideOut].tex);
    }

    #region UpdateLoopHandlers
    private void Update()
    {
        HandleUnlocking();
        HandleTimers();
        HandleDoorClosing();
        //end myself
        inside.material.SetFloat("_VertexGlitchSeed", Singleton<VertexGlitchManager>.Instance.global_VertexGlitchSeed);
        inside.material.SetFloat("_VertexGlitchIntensity", Singleton<VertexGlitchManager>.Instance.global_VertexGlitchIntensitySpecialCare/8);
        inside.material.SetInt("_ValueX", Singleton<VertexGlitchManager>.Instance.global_glitchColorRvalue);
        inside.material.SetInt("_ValueY", Singleton<VertexGlitchManager>.Instance.global_glitchColorGvalue);
        inside.material.SetInt("_ValueZ", Singleton<VertexGlitchManager>.Instance.global_glitchColorBvalue);
        outside.material.SetFloat("_VertexGlitchSeed", Singleton<VertexGlitchManager>.Instance.global_VertexGlitchSeed);
        outside.material.SetFloat("_VertexGlitchIntensity", Singleton<VertexGlitchManager>.Instance.global_VertexGlitchIntensitySpecialCare/8);
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
            SwinDorMapSprite1.sprite = SwinDorMapSprite2.sprite = AdditionalGameCustomizer.Instance.dorMapSprite;
            SetLock(false);
        }
    }

    private void HandleTimers()
    {
        if (lockTime.CountdownWithDeltaTime() == 0 & bDoorLocked & requirementMet & !destroyed) SetLock(false);
    }

    private void HandleDoorClosing()
    {
        if (openTime.CountdownWithDeltaTime() == 0 & bDoorOpen & !bDoorLocked & !destroyed) SetDoorState(false);
    }
    #endregion

    #region TriggerDetection
    private void OnTriggerStay(Collider opened)
    {
        if ((opened.CompareTag("Player") || (opened.CompareTag("NPC") & opened.isTrigger) && !bDoorLocked || (opened.CompareTag("cork") & opened.isTrigger)|| (opened.CompareTag("Projectile") & opened.isTrigger)) && !bDoorLocked)
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
    public void PleaseDie(bool makeNoise = true)
    {
        SetDoorState(true);
        lockTime = 10;
        destroyed = true;
        obstacle.SetActive(false);
        insidecol.enabled = false;
        outsidecol.enabled = false;
        if (makeNoise) 
        {
            Singleton<OtherMainStuffManager>.Instance.HearingShit(8f, this.transform, new Vector3(0f,0f,0f), "all",false);
            if (!myAudio.GetForcedMute) foreach (MaxcipalScript maxi in GameControllerScript.Instance.maxiScr) if (maxi.isActiveAndEnabled) maxi.callToSMTH(this.transform.position);
        }
        CameraScript.Instance.TempShakeAmount = 0.5f;
        myAudio.ClearQueue(true);
        myAudio.PlaySingleClip(GameControllerScript.Instance.lbams.GlobalDoorBreak);
        gameObject.layer = LayerMask.NameToLayer("npcInteractionLayer");
        SwinDorMapSprite1.enabled = SwinDorMapSprite2.enabled = false;
        inside.material.SetColor("_Color1", Color.clear);
        outside.material.SetColor("_Color1", Color.clear);
    }
    public void CreateFakeSwingDoorPrefab(Vector3 force)
    {
        obstacle.SetActive(false);
        insidecol.enabled = false;
        outsidecol.enabled = false;
        GameObject gameObject = Object.Instantiate<GameObject>(inside.GetComponentInChildren<Renderer>().gameObject);
        gameObject.name = "Broken Swinging Door(Clone)";
		BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
		Bounds bounds = gameObject.GetComponent<MeshFilter>().sharedMesh.bounds;
		boxCollider.center = bounds.center;
		boxCollider.size = bounds.size + new Vector3(0f, 0f, 0.06f);
        boxCollider.center = new Vector3(0f, -0.075f, 0f);
        boxCollider.size = new Vector3(0.745f, 0.8f, 0.06f);
		Destroy(gameObject.GetComponent<MeshCollider>());
		Material material = Object.Instantiate<Material>(inside.material);
		Renderer component = gameObject.GetComponent<Renderer>();
		component.sharedMaterial = material;
        material.SetInt("_Swap", 0);
		component.sharedMaterial.mainTexture = inside.material.GetTexture("_SecondTex");
		gameObject.transform.SetPositionAndRotation(inside.GetComponentInChildren<Renderer>().transform.position, inside.transform.rotation);
		gameObject.transform.localScale = Vector3.Scale(inside.GetComponentInChildren<Renderer>().transform.lossyScale, new Vector3(1f, 1f, 1f));
		GameObject gameObject2 = Object.Instantiate<GameObject>(inside.GetComponentInChildren<Renderer>().gameObject, gameObject.transform);
		Destroy(gameObject2.GetComponent<MeshCollider>());
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localEulerAngles = Vector3.zero;
		gameObject2.transform.localScale = Vector3.one;
		Renderer component2 = gameObject2.GetComponent<Renderer>();
		component2.sharedMaterial = material;
		gameObject2.transform.localEulerAngles += new Vector3(0f, 180f, 0f);
		Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
		rigidbody.mass = 9f;
        rigidbody.AddExplosionForce(35000f, force, 4f, 1f, 0);
		rigidbody.angularVelocity += Random.insideUnitSphere / 24f;
		rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
		rigidbody.detectCollisions = true;
		Physics.IgnoreCollision(GameControllerScript.Instance.player.cc, boxCollider, true);
    }
    #endregion

    #region DoorState
    private void SetDoorState(bool set)
    {
        if (destroyed) return;
        gameObject.layer = set ? LayerMask.NameToLayer("npcInteractionLayer") : LayerMask.NameToLayer("Default");
        heardDoor = set;
        bDoorOpen = set;
        int shift = set ? 1 : 0;
        inside.material.SetInt("_Swap", shift);
        outside.material.SetInt("_Swap", shift);
    }

    private void SetLock(bool lockState)
    {
        if (destroyed) return;
        gameObject.layer = LayerMask.NameToLayer("Default");
        obstacle.SetActive(lockState);
        insidecol.enabled = lockState;
        bDoorLocked = lockState;
        SwinDorMapSprite1.sprite = SwinDorMapSprite2.sprite = lockState ? AdditionalGameCustomizer.Instance.dorMapLockedSprite : AdditionalGameCustomizer.Instance.dorMapSprite;

        inside.material = lockState ? locked : normal;
        outside.material = lockState ? locked : normal;
        InitializeShaderVariableStuff();
    }
    #endregion

    #region SerializedFields
    private GameControllerScript gc;
    [SerializeField] private WallType WalTypSideIn = WallType.Normal,WalTypSideOut = WallType.Normal;

    [Header("Door Mechanics and Materials")]
    [SerializeField] private GameObject obstacle;
    [SerializeField] private MeshCollider insidecol, outsidecol;
    [SerializeField] private MeshRenderer inside, outside;
    [SerializeField] private Material normal, locked;
    [SerializeField] private SpriteRenderer SwinDorMapSprite1,SwinDorMapSprite2;

    [Header("Door state and timing")]
    [SerializeField] private bool bDoorOpen;
    public bool bDoorLocked,destroyed,DestroyImmunity;
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