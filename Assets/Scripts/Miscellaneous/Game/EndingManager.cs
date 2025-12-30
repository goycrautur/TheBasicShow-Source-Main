using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndingManager : MonoBehaviour
{
    #region UnityCallbacks
    private void Start() => Game = FindObjectOfType<GameControllerScript>();
    #endregion
    #region SingletonSetup
    private void Awake() => Instance = this;
    public static EndingManager Instance;
    #endregion
    public IEnumerator KeepTheHudOff()
	{
		while (hidehud)
		{
			Game.player.hud.SetActive(false);
            if (!movething)
            {
                Game.player.isForcedToLook = true;
            }
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}
    public void Update()
    {
        Vector3 ogposforTs = EndingForceLook[CurExitID].position;
        if (movething)
        {
            EndingForceLook[CurExitID].position = Vector3.MoveTowards(EndingForceLook[CurExitID].position, new Vector3(ogposforTs.x, 66, ogposforTs.z), 7 * Time.deltaTime);
        }
    }
    public void endingShit(int ID,bool secret = false)
    {
        CurExitID = ID;
        if (Game.mode != "story")
        {
            LoadNormalResults();
            return;
        }
        if (Game.warrealest)
        {
            LoadNormalResults(secret);
            return;
        }
        StartCoroutine(Game.easingExit(Color.white, 0, 2, 5));
        Game.audioDevice.loop = false;
        Game.audioDevice.Stop();
        Game.audioDevice2.Stop();
        for (int i = 0; i < Game.EvapV2FinaleSounSource.Length; ++i)
        {
            if (Game.EvapV2FinaleSounSource[i] != null)
            {
                Game.EvapV2FinaleSounSource[i].Stop();
            }
        }
        Game.escapeMusic.clip = !secret ? NormEnd : SecretEnd;
        Game.escapeMusic.loop = false;
        Game.escapeMusic.Play();
        hidehud=true;
        StartCoroutine(KeepTheHudOff());
        StartCoroutine(EndingSequence(!secret ? NormEnd.length : SecretEnd.length,!secret ? 11f : 10.5f,ID,secret));
        StartCoroutine(byebus(!secret ? 5.44f : SecretEnd.length,ID));
    }
    #region PublicMethods
    public IEnumerator byebus(float timetoActuallyMove,int whatWasTheObjectId)
    {
        yield return new WaitForSeconds(timetoActuallyMove);
        movething = true;
        EndingBillObj[whatWasTheObjectId].enabled = true;
        EndingBillObj[whatWasTheObjectId].shaking = true;
        EndingAudiSource[whatWasTheObjectId].clip = helicop;
        EndingAudiSource[whatWasTheObjectId].loop = false;
        EndingAudiSource[whatWasTheObjectId].Play();
        Game.SubsManager.summonLeSubtitle(helisubs.subtitleOption,helisubs,EndingAudiSource[whatWasTheObjectId]);
        yield return new WaitForSeconds(0.2f);
        movething = false;
        yield return null;
    }
    public IEnumerator EndingSequence(float WaitTime,float timeflash,int ForceLookTargetInt,bool secretEnd)
    {
        ZerullClassic.Instance.yourflashbang.Rebind();
        ZerullClassic.Instance.yourflashbang.Play("flashAnim", -1, 0f);
        Game.player.forceLookSpeed = 500f;
        Game.player.targetToForcelyLookAt = EndingForceLook[ForceLookTargetInt];
        Game.player.transform.position = EndingTransformTP[ForceLookTargetInt].transform.position + Vector3.up * Game.player.height;
        Game.player.titlecard = true;
        Game.player.movementLocked = true;
        Game.playerCollider.enabled = false;
        Game.npcCloneList.ForEach(o => o.SetActive(false));
        yield return new WaitForSeconds(WaitTime-(WaitTime-timeflash));
        ZerullClassic.Instance.yourflashbang.Rebind();
        ZerullClassic.Instance.yourflashbang.Play("flashAnim", -1, 0f);
        
        black.SetActive(true);
        yield return new WaitForSeconds(WaitTime-timeflash);
        black.SetActive(false);
        LoadNormalResults(secretEnd);
        hidehud = false;
        Game.player.isForcedToLook = false;
        Game.player.hud.SetActive(true);
        yield return null;
    }
    public void LoadNormalResults(bool secret = false)
    {
        PlayerPrefsExtension.SetBool("storyBeaten", true);
        if (!secret)
        {
            GetResults = true;
        }

        DestroyIfExists("JumpRope(Clone)");
        DestroyIfExists("CraftersAttacker(Clone)");

        Game.baldiScrpt.stopMoving = true;
        results.SetActive(true);
        AudioListener.pause = true;

        Game.audioDevice.loop = false;
        Game.audioDevice.Stop();
        Game.audioDevice2.Stop();
        Game.escapeMusic.Stop();

        Game.playerCharacter.enabled = true;
        Game.playerCollider.enabled = true;
        if (Game.LapManag.Meeptimar.isActiveAndEnabled)
        {
            Game.LapManag.Meeptimar.AddTime(60f, Color.green);
            Game.LapManag.Meeptimar.inEnding = true;
            Game.LapManag.Meeptimar.canTime = false;
            Game.warmusic.Stop();
        }

        Game.npcCloneList.ForEach(o => o.SetActive(false));
        if (secret)
        {
            GetSecret = true;
        }
    }

    public void LoadSecretEnding(string rankcheck = "none")
    {
        if (rankcheck == "J")
        {
            pitHole.Instance.hiIgotChanged();
        }
        Game.modeState = "???????????????????";
        StartCoroutine(BlackFlash());

        Game.SecretEndingGot = true;
        if (Game.exitEasingCoroutine != null)
        {
            StopCoroutine(Game.exitEasingCoroutine);
            Game.exitEasingCoroutine = null;
        }
        results.SetActive(false);

        portal.SetActive(true);
        AudioListener.pause = false;
        //NULL.SetActive(true);

        Game.voxLight.ambientLight = new Color(0.45f, 0.45f, 0.45f, 1f);
        ApplySkybox();

        Game.player.transform.position = SecretWarpPoint.transform.position + Vector3.up * Game.player.height;

        Environment.ForEach(s => s.SetActive(false));

        Game.player.forceLookSpeed = 750f;
        Game.player.targetToForcelyLookAt = SecretWallText;
        Game.player.isForcedToLook = true;
        if (Game.LapManag.Meeptimar.isActiveAndEnabled)
        {
            Game.LapManag.Meeptimar.AddTime(60f, Color.green);
            Game.LapManag.Meeptimar.inEnding = true;
            Game.LapManag.Meeptimar.canTime = false;
            Game.warmusic.Stop();
        }
    }
    #endregion

    #region Coroutines
    private IEnumerator BlackFlash()
    {
        black.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        black.SetActive(false);
    }
    #endregion

    #region PrivateHelpers
    private void DestroyIfExists(string objectName)
    {
        GameObject obj = GameObject.Find(objectName);
        if (obj != null) Destroy(obj);
    }

    private void ApplySkybox()
    {
        if (AdditionalGameCustomizer.Instance == null) return;

        switch (AdditionalGameCustomizer.Instance.currentSkybox)
        {
            case AdditionalGameCustomizer.SkyboxStyle.Day:
                RenderSettings.skybox = AdditionalGameCustomizer.Instance.NormalSky;
                break;
            case AdditionalGameCustomizer.SkyboxStyle.Sunset:
                RenderSettings.skybox = AdditionalGameCustomizer.Instance.TwilightSky;
                break;
            case AdditionalGameCustomizer.SkyboxStyle.Night:
                RenderSettings.skybox = AdditionalGameCustomizer.Instance.NightSky;
                break;
        }
    }
    #endregion

    #region SerializedFields
    [Header("Ending References")]
    [SerializeField] private Transform SecretWallText;
    [SerializeField] private Transform[] EndingTransformTP,EndingForceLook;
    [SerializeField] private Billboard[] EndingBillObj;
    [SerializeField] private AudioSource[] EndingAudiSource;
    [SerializeField] private AudioClip NormEnd,SecretEnd,helicop;
    [SerializeField] private subsScriptableObject helisubs;
    [SerializeField] private List<GameObject> Environment = new List<GameObject>();
    [SerializeField] public GameObject office, results, black, SecretWarpPoint, NULL, portal;
    #endregion

    #region PublicFields
    [Header("Ending Detection")]
    public bool GetResults;
    public bool GetSecret,hidehud,movething;
    #endregion

    #region PrivateFields
    private GameControllerScript Game;
    private int CurExitID;
    #endregion
}