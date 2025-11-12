using System.Collections;
using UnityEngine;

public class ITM_MetalPipe : BaseItem
{
    [SerializeField] private float duration = 60f, cantuseduration, energy, walkspeedmultAdd,runspeedmultAdd,staminaDropmultAdd,staminaRisemultAdd;
    [SerializeField] private Sprite pipeSprite,UnuseablepipeSprite;
    [SerializeField] private AudioClip audiopip, fail;
    [SerializeField] private bool piped, CantUse;
    [SerializeField] private subsScriptableObject Subtitlesthing;
    public override bool OnUse()
    {
        if (CantUse)
        {
            GameControllerScript.Instance.audioDevice.PlayOneShot(fail);
            return false;
        }
        if (!CantUse)
        {
            GameControllerScript.Instance.SubsManager.summonLeSubtitle2D(Subtitlesthing.subtitleOption,Subtitlesthing,0f,new Vector3(0f,-170.5f,0f),GameControllerScript.Instance.audioDevice);
            GameControllerScript.Instance.audioDevice.PlayOneShot(audiopip);
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
    private IEnumerator Waitin()
    {
        GameControllerScript.Instance.metalpipeStun = true;
        GameControllerScript.Instance.player.walkSpeedMultipler += walkspeedmultAdd;
        GameControllerScript.Instance.player.runSpeedMultipler += runspeedmultAdd;
        GameControllerScript.Instance.player.staminaDropMultiple += staminaDropmultAdd;
        GameControllerScript.Instance.player.staminaRiseMultiple += staminaRisemultAdd;
        StartCoroutine(cantuse(cantuseduration));
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
