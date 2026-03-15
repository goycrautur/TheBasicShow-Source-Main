using System.Collections;
using UnityEngine;

public class ITM_MetalPipe : BaseItem
{
    [SerializeField] private float duration = 60f,invinciDuration, cantuseduration, energy, walkspeedmultAdd,runspeedmultAdd,staminaDropmultAdd,staminaRisemultAdd;
    [SerializeField] private Sprite pipeSprite,UnuseablepipeSprite,invicible;
    [SerializeField] private AudioObjectyeah audiopip, fail;
    [SerializeField] private bool piped, CantUse;
    [SerializeField] private AudioManagerLiveReaction PipeDevice;
    public override bool OnUse()
    {
        if (CantUse)
        {
            lowBudgetAudioManagementShit.Instance.MainSource1.PlaySingleClip(fail);
            return false;
        }
        if (!CantUse)
        {
            PipeDevice.PlaySingleClip(audiopip);
            GameControllerScript.Instance.player.SetStamina(PlayerScript.StaminaChangeMode.Add, energy);
            StopCoroutine(Waitin());
            StartCoroutine(Waitin());
            return true;
        }
        return false;
    }
    public IEnumerator cantuse(float duration)
    {
        float time = duration;
        Gauge newGauge2 = GaugeManager.Instance.CreateGaugeInstance(UnuseablepipeSprite, duration);
        CantUse = true;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            if (newGauge2 != null && (AdditionalGameCustomizer.Instance != null && AdditionalGameCustomizer.Instance.Gauges || AdditionalGameCustomizer.Instance == null))
            {
                newGauge2.Set(duration, time);
            }
            yield return null;
        }
        newGauge2.Hide();
        CantUse = false;
        yield break;
    }
    public IEnumerator titlecar(float duration)
    {
        float time = duration;
        Gauge newGauge3 = GaugeManager.Instance.CreateGaugeInstance(invicible, duration);
        GameControllerScript.Instance.player.titlecard = true;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            if (newGauge3 != null && (AdditionalGameCustomizer.Instance != null && AdditionalGameCustomizer.Instance.Gauges || AdditionalGameCustomizer.Instance == null))
            {
                newGauge3.Set(duration, time);
            }
            yield return null;
        }
        newGauge3.Hide();
        GameControllerScript.Instance.player.titlecard = false;
        yield break;
    }
    private IEnumerator Waitin()
    {
        GameControllerScript.Instance.metalpipeStun = true;
        
        GameControllerScript.Instance.player.walkSpeedMultipler += walkspeedmultAdd;
        GameControllerScript.Instance.player.runSpeedMultipler += runspeedmultAdd;
        GameControllerScript.Instance.player.staminaDropMultiple += staminaDropmultAdd;
        GameControllerScript.Instance.player.staminaRiseMultiple += staminaRisemultAdd;
        StartCoroutine(cantuse(cantuseduration));
        StartCoroutine(titlecar(invinciDuration));
        float time = duration;
        Gauge newGauge = GaugeManager.Instance.CreateGaugeInstance(pipeSprite, duration);
        piped = true;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            if (newGauge != null && (AdditionalGameCustomizer.Instance != null && AdditionalGameCustomizer.Instance.Gauges || AdditionalGameCustomizer.Instance == null))
            {
                newGauge.Set(duration, time);
            }
            yield return null;
        }
        newGauge.Hide();
        piped = false;
        GameControllerScript.Instance.player.walkSpeedMultipler -= walkspeedmultAdd;
        GameControllerScript.Instance.player.runSpeedMultipler -= runspeedmultAdd;
        GameControllerScript.Instance.player.staminaDropMultiple -= staminaDropmultAdd;
        GameControllerScript.Instance.player.staminaRiseMultiple -= staminaRisemultAdd;
        GameControllerScript.Instance.metalpipeStun = false;
        yield break;
    }
}
