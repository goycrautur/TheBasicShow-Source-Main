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

    #region PublicMethods
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
    [SerializeField] private List<GameObject> Environment = new List<GameObject>();
    [SerializeField] public GameObject office, results, black, SecretWarpPoint, NULL, portal;
    #endregion

    #region PublicFields
    [Header("Ending Detection")]
    public bool GetResults;
    public bool GetSecret;
    #endregion

    #region PrivateFields
    private GameControllerScript Game;
    #endregion
}