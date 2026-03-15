using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class EndingManager : MonoBehaviour
{
    private float EndingSequenceWaitTime,EndingSequenceFlashTime,EndingSequenceHelicopTime;
    #region UnityCallbacks
    private void Start() => Game = FindObjectOfType<GameControllerScript>();
    #endregion
    #region SingletonSetup
    private void Awake() => Instance = this;
    public static EndingManager Instance;
    #endregion
    public void endingShit(int ID,bool secret = false)
    {
        Game.youCantPause = true;
        DestroyIfExists("JumpRope(Clone)");
        DestroyIfExists("JumpRope(Clone)");
        DestroyIfExists("JumpRope(Clone)");
        DestroyIfExists("JumpRope(Clone)");
        DestroyIfExists("JumpRope(Clone)");
        DestroyIfExists("JumpRope(Clone)");
        DestroyIfExists("JumpRope(Clone)");
        DestroyIfExists("JumpRope(Clone)");
        DestroyIfExists("JumpRope(Clone)");
        DestroyIfExists("JumpRope(Clone)");
        DestroyIfExists("JumpRope(Clone)");
        DestroyIfExists("JumpRope(Clone)");
        DestroyIfExists("JumpRope(Clone)");
        DestroyIfExists("JumpRope(Clone)");
        DestroyIfExists("JumpRope(Clone)");
        DestroyIfExists("waneAttacker(Clone)");
        DestroyIfExists("waneAttacker(Clone)");
        DestroyIfExists("waneAttacker(Clone)");
        DestroyIfExists("waneAttacker(Clone)");
        DestroyIfExists("waneAttacker(Clone)");
        DestroyIfExists("waneAttacker(Clone)");
        DestroyIfExists("waneAttacker(Clone)");
        DestroyIfExists("waneAttacker(Clone)");
        DestroyIfExists("waneAttacker(Clone)");
        DestroyIfExists("waneAttacker(Clone)");
        DestroyIfExists("waneAttacker(Clone)");
        DestroyIfExists("waneAttacker(Clone)");
        DestroyIfExists("waneAttacker(Clone)");
        DestroyIfExists("waneAttacker(Clone)");
        DestroyIfExists("waneAttacker(Clone)"); // just in case of chaos skull, could be optimized better but eh

        if (AdditionalGameCustomizer.Instance != null)
        {
            switch (AdditionalGameCustomizer.Instance.EscapeMusicFunsies)
            {
                case AdditionalGameCustomizer.EscapeFunsies.Daldi:
                    LoadNormalResults(secret);
                    return;
                case AdditionalGameCustomizer.EscapeFunsies.Taldi:
                    LoadNormalResults(secret);
                    return;
            }
        }
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
        Game.lbams.MainSource1.ClearQueue(true);
        Game.lbams.MainSource2.ClearQueue(true);
        Game.lbams.MainSource3.ClearQueue(true);
        Game.lbams.EscapeMusic.ClearQueue(true);
        GameControllerScript.Instance.MainHudFade.Rebind();
        GameControllerScript.Instance.MainHudFade.Play("hudFadeOutNearly", -1, 0f);
        GameControllerScript.Instance.RainbowHudFade.Rebind();
        GameControllerScript.Instance.RainbowHudFade.Play("hudFadeOutRainb", -1, 0f);
        
        if (!secret) 
        {
            Game.lbams.EndingMusic.PlaySingleClip(NormEnd);
            EndingSequenceWaitTime = NormEnd.audClip.length;
            EndingSequenceFlashTime = 11f;
            EndingSequenceHelicopTime = 5.44f;
        }
        else if (secret && Game.ExclusiveRoute == "") 
        {
            Game.lbams.EndingMusic.PlaySingleClip(SecretEnd);
            EndingSequenceWaitTime = SecretEnd.audClip.length;
            EndingSequenceFlashTime = 10.323f;
            EndingSequenceHelicopTime = 5.2f;
        }
        else if (secret && Game.ExclusiveRoute == "ClassicPlayerSecretEndChal") 
        {
            Game.lbams.EndingMusic.PlaySingleClip(ExclusivFinaleSecretEndClassicPlayer);
            EndingSequenceWaitTime = ExclusivFinaleSecretEndClassicPlayer.audClip.length;
            EndingSequenceFlashTime = 8f;
            EndingSequenceHelicopTime = 1f;
        }
        StartCoroutine(EndingSequence(EndingSequenceWaitTime,EndingSequenceFlashTime,ID,secret));
        StartCoroutine(byebus(EndingSequenceHelicopTime,ID));
    }
    #region PublicMethods
    public IEnumerator byebus(float timetoActuallyMove,int whatWasTheObjectId)
    {
        
        yield return new WaitForSeconds(timetoActuallyMove);
        movething = true;
        EndingForceLook[whatWasTheObjectId].transform.DOMoveY(66, 15f);
        EndingBillObj[whatWasTheObjectId].enabled = true;
        EndingBillObj[whatWasTheObjectId].shaking = true;
        EndingAudiSource[whatWasTheObjectId].PlaySingleClip(helicop);
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
        Game.baldiScrpt.stopMoving = true;
        results.SetActive(true);
        AudioListener.pause = true;
        Game.lbams.MainSource1.ClearQueue(true);
        Game.lbams.MainSource2.ClearQueue(true);
        Game.lbams.MainSource3.ClearQueue(true);
        Game.lbams.EscapeMusic.ClearQueue(true);

        Game.playerCharacter.enabled = true;
        Game.playerCollider.enabled = true;
        if (Game.LapManag.Meeptimar.isActiveAndEnabled)
        {
            Game.LapManag.Meeptimar.AddTime(60f, Color.green);
            Game.LapManag.Meeptimar.inEnding = true;
            Game.LapManag.Meeptimar.canTime = false;
            Game.lbams.WarMusic.ClearQueue(true);
        }

        Game.npcCloneList.ForEach(o => o.SetActive(false));
        if (secret) GetSecret = true;
    }

    public void LoadSecretEnding(string rankcheck = "none")
    {
        GameControllerScript.Instance.MainHudFade.Rebind();
        GameControllerScript.Instance.MainHudFade.Play("hudFadeIn", -1, 0f);
        GameControllerScript.Instance.RainbowHudFade.Rebind();
        GameControllerScript.Instance.RainbowHudFade.Play("hudFadeInRainb", -1, 0f);
        GameControllerScript.Instance.SubtitlesHudFade.Rebind();
        GameControllerScript.Instance.SubtitlesHudFade.Play("hudFadeInsubs", -1, 0f);
        if (rankcheck == "J") pitHole.Instance.hiIgotChanged();
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
        RenderSettings.ambientLight = new Color(0.45f, 0.45f, 0.45f, 1f);
        ApplySkybox();
        Game.player.transform.position = SecretWarpPoint.transform.position + Vector3.up * Game.player.height;
        Environment.ForEach(s => s.SetActive(false));
        Game.player.forceLookSpeed = 750f;
        Game.player.targetToForcelyLookAt = SecretWallText;
        Game.player.isForcedToLook = true;
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
    [SerializeField] private AudioManagerLiveReaction[] EndingAudiSource;
    [SerializeField] private AudioObjectyeah NormEnd,SecretEnd,ExclusivFinaleSecretEndClassicPlayer,helicop;
    [SerializeField] private List<GameObject> Environment = new List<GameObject>();
    [SerializeField] public GameObject office, results, black, SecretWarpPoint, NULL, portal;
    #endregion

    #region PublicFields
    [Header("Ending Detection")]
    public bool GetResults;
    public bool GetSecret,movething;
    #endregion

    #region PrivateFields
    private GameControllerScript Game;
    #endregion
}