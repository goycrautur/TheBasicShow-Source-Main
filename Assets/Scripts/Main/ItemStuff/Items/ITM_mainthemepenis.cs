using System.Collections;
using UnityEngine;
using TMPro;

public class ITM_mainthemepenis : BaseItem
{
    private float duration = 60f;
    [SerializeField] private AudioObjectyeah peniMusicsnd;
    [SerializeField] private Sprite peniMusic;
    [SerializeField] private AudioManagerLiveReaction[] audioManagersToSDIYBT;
    [SerializeField] private MovementModifier funnySpeedMod = new MovementModifier(default(Vector3), 0f);
    [SerializeField] private float massZoomDelay,ZoomOutDelay,ZoomInDelay,MinSpeedcap,MinSpeedcapLap,MaxSpeedcap,FOVmin,FOVmax;
    private float fovMain,speedMain;
    private bool used,StopBro;
    private PlayerScript playrara => GameControllerScript.Instance.player;
    private AdditionalGameCustomizer agc => AdditionalGameCustomizer.Instance;

    public override bool OnUse()
    {
        if (used) return false;
        StopBro = false;
        GameControllerScript.Instance.lbams.ShuckSource.PlaySingleClip(peniMusicsnd);
        playrara.SetHP(PlayerScript.HealthChangeMode.Set, (int)playrara.maxHealth/2f, 6f, true, true, false);
        playrara.maxHealth += 50f;
        used = true;
        StartCoroutine(Wait());
        StartCoroutine(MotherfuckingPenisMusicCamZoomANDSpeedGfys());
        return true;
    }
    private void Update()
    {
        if (!used) return;
        funnySpeedMod.movementMultiplier = Mathf.Lerp(funnySpeedMod.movementMultiplier, speedMain, 4f * Time.deltaTime);
        agc.FovAmmount = Mathf.Lerp(agc.FovAmmount, fovMain, 4f * Time.deltaTime);
        if (speedMain > 1.25f) playrara.breakwindow = true;
        else playrara.breakwindow = false;
    }

    private IEnumerator MotherfuckingPenisMusicCamZoomANDSpeedGfys()
    {
        agc.FuckLerp = true;
        float FuckassDelayIguess = 0;
        bool DidZoomoutDelay = false;
        bool DidZoominDelay = false;
        bool DidZoomoutTween = false;
        bool DidZoominTween = false;
        bool Lapper = PlayerPrefs.GetString("CurrentMode") == "LappingOfAsylum";
        
        FuckassDelayIguess = 0;
        playrara.pModManag.movementModifiers.Add(funnySpeedMod);
        while (!StopBro)
        {
            while (!DidZoominTween && FuckassDelayIguess < ZoomInDelay)
            {
                speedMain = MaxSpeedcap;
                fovMain = FOVmax;
                FuckassDelayIguess += Time.deltaTime;
                yield return null;
            }
            DidZoomoutTween = false;
            DidZoominTween = true;
            FuckassDelayIguess = 0;
            while (!DidZoomoutTween && FuckassDelayIguess < ZoomOutDelay)
            {
                speedMain = Lapper ? MinSpeedcapLap : MinSpeedcap;
                fovMain = FOVmin;
                FuckassDelayIguess += Time.deltaTime;

                yield return null;
            }
            DidZoomoutTween = true;
            DidZoominTween = false;
            FuckassDelayIguess = 0;
            

            yield return null;
        }
        playrara.pModManag.movementModifiers.Remove(funnySpeedMod);
        agc.FovAmmount = agc.DefaultFovAmmount;
        agc.FuckLerp = false;
        yield break;
    }

    private IEnumerator Wait()
    {
        for (int i = 0; i < audioManagersToSDIYBT.Length; ++i)audioManagersToSDIYBT[i].ForcedMute = true;
        float time = peniMusicsnd.audClip.length;
        Gauge newGauge = GaugeManager.Instance.CreateGaugeInstance(peniMusic, duration);
        while (time > 0f)
        {
            time -= Time.deltaTime;
            if (newGauge != null && (AdditionalGameCustomizer.Instance != null && AdditionalGameCustomizer.Instance.Gauges || AdditionalGameCustomizer.Instance == null))
            {
                newGauge.Set(peniMusicsnd.audClip.length, time);
            }
            yield return null;
        }
        newGauge.Hide();
        for (int i = 0; i < audioManagersToSDIYBT.Length; ++i) audioManagersToSDIYBT[i].ForcedMute = false;
        used = false;
        StopBro = true;
        yield break;
    }
}
