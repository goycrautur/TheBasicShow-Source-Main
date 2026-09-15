using UnityEngine;
using System.Collections;
/*#if UNITY_EDITOR
[ExecuteInEditMode]
#endif*/

public class DoorScript : MonoBehaviour
{
    #region Initialization
    /*#if UNITY_EDITOR
    private void OnGUI() 
    {
        UpdateShaderVariableStuff();
        InitializeShaderVariableStuff();
    }
    #endif*/
    private void InitializeShaderVariableStuff()
    {
        outside.material.SetTextureScale("_SecondTex", new Vector2(-1, 1));
        outside.material.SetTextureScale("_SecondaryDiffrent", new Vector2(-1, 1));
        inside.material.SetTexture("_MainTex", GameControllerScript.Instance.GlobalTextures[(int)WalTypSideIn].tex);
        outside.material.SetTexture("_MainTex", GameControllerScript.Instance.GlobalTextures[(int)WalTypSideOut].tex);
        if (DoorTextureDatas != null)
        {
            outside.material.SetTexture("_SecondTex", DoorTextureDatas.doorOpenTex);
            outside.material.SetTexture("_SecondaryDiffrent", DoorTextureDatas.doorCloseTex);
            outside.material.SetTexture("_Mask", DoorTextureDatas.doorMask);
            inside.material.SetTexture("_SecondTex", DoorTextureDatas.doorOpenTex);
            inside.material.SetTexture("_SecondaryDiffrent", DoorTextureDatas.doorCloseTex);
            inside.material.SetTexture("_Mask", DoorTextureDatas.doorMask);
        }
    }
    private void Start()
    {
        CurTrigger = MainTrigger;
        AltTrigger.enabled = false;
        myAudio.audioDevice.spatialBlend = 1;
        myAudio.audioDevice.minDistance = 35;
        myAudio.audioDevice.maxDistance = 125;
        InitializeShaderVariableStuff();
    }
    #endregion

    #region UpdateLoopHandlers
    private void Update()
    {
        HandleLockTimer();
        HandleOpenTimer();
        HandleDoorInteraction();
        UpdateShaderVariableStuff();
        
        
    }
    private void UpdateShaderVariableStuff()
    {
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

    private void HandleLockTimer()
    {
        if (lockTime.CountdownWithDeltaTime(1f) == 0 & bDoorLocked & !destroyed) UnlockDoor();
    }

    private void HandleOpenTimer()
    {
        if (openTime.CountdownWithDeltaTime() == 0 & bDoorOpen & !destroyed) CloseDoor();
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
        if (destroyed) return;
        if (!bDoorOpen && SoundData != null) myAudio.PlaySingleClip(SoundData.doorOpen);
        SetDoorState(true, time);
    }

    private void CloseDoor()
    {
        if (destroyed) return;
        SetDoorState(false);
        if (SoundData != null) myAudio.PlaySingleClip(SoundData.doorClose);
    }

    public void DestroyDoor(bool makeNoise = true)
    {
        SetDoorState(true, 10);

        destroyed = true;
        myAudio.ClearQueue(true);
        
        myAudio.PlaySingleClip(GameControllerScript.Instance.lbams.GlobalDoorBreak);
        CameraScript.Instance.TempShakeAmount = 0.5f;
        if (makeNoise) 
        {
            if (!myAudio.GetForcedMute) Singleton<OtherMainStuffManager>.Instance.HearingShit(8f, this.transform, new Vector3(0f,0f,0f), "all",false);
            foreach (MaxcipalScript maxi in GameControllerScript.Instance.maxiScr) if (maxi.isActiveAndEnabled) maxi.callToSMTH(this.transform.position);
        }
        AltTrigger.gameObject.layer = LayerMask.NameToLayer(MainTriggerLayerString);
        inside.material.SetColor("_Color1", Color.clear);
        outside.material.SetColor("_Color1", Color.clear);
        
    }

    public void CreateFakeDoorPrefab(Vector3 force)
    {
        MainTrigger.enabled = false;
        AltTrigger.enabled = false;
        MainDoorBarrier.enabled = false;
        GameObject gameObject = Object.Instantiate<GameObject>(inside.GetComponentInChildren<Renderer>().gameObject);
        gameObject.name = "Broken Door(Clone)";
		BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
		Bounds bounds = gameObject.GetComponent<MeshFilter>().sharedMesh.bounds;
		boxCollider.center = bounds.center;
		boxCollider.size = bounds.size + new Vector3(0f, 0f, 0.06f);
        boxCollider.center = new Vector3(0f, -0.05f, 0f);
        boxCollider.size = new Vector3(0.4f, 0.85f, 0.06f);
		Object.Destroy(gameObject.GetComponent<MeshCollider>());
		Material material = Object.Instantiate<Material>(inside.material);
		Renderer component = gameObject.GetComponent<Renderer>();
		component.sharedMaterial = material;
        material.SetInt("_Swap", 0);
		material.mainTexture = DoorTextureDatas.doorCloseTex;
		gameObject.transform.SetPositionAndRotation(inside.GetComponentInChildren<Renderer>().transform.position, inside.transform.rotation);
		gameObject.transform.localScale = Vector3.Scale(inside.GetComponentInChildren<Renderer>().transform.lossyScale, new Vector3(1f, 1f, 1f));
		GameObject gameObject2 = Object.Instantiate<GameObject>(outside.GetComponentInChildren<Renderer>().gameObject, gameObject.transform);
		Object.Destroy(gameObject2.GetComponent<MeshCollider>());
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
        if (destroyed) return;
        if (SoundData != null) myAudio.PlaySingleClip(SoundData.Click);
        bDoorLocked = true;
        DorMapSprite1.sprite = DorMapSprite2.sprite = AdditionalGameCustomizer.Instance.dorMapLockedSprite;
        lockTime = time;
    }

    public void UnlockDoor()
    {
        if (destroyed) return;
        bDoorLocked = false;
        DorMapSprite1.sprite = DorMapSprite2.sprite = AdditionalGameCustomizer.Instance.dorMapSprite;
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
    [Header("Audio Settings and other stuffs")]
    [SerializeField] private AudioManagerLiveReaction myAudio;
    [SerializeField] private NormalDoorSoundData SoundData;
    [SerializeField] private DoorTextureData DoorTextureDatas;
    [SerializeField] private WallType WalTypSideIn = WallType.Normal,WalTypSideOut = WallType.Normal;
    
    [Header("Barrier Stuff Settings")]
    public BoxCollider MainDoorBarrier;
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
   

    [Header("sum Settings")]
    public float lockTime;
    public float openTime;
    public bool DestroyImmunity = false;
    #endregion

    #region RuntimeState
    public bool bDoorOpen, bDoorLocked, destroyed;
    #endregion
}