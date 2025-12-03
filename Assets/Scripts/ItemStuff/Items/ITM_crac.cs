using System.Collections;
using UnityEngine;
using TMPro;

public class ITM_crac : BaseItem
{
    public override bool OnUse()
    {
        if (used) return false;
        AdditionalGameCustomizer.Instance.FovAmmount += FovAmmount;
        GameControllerScript.Instance.audioDevice.PlayOneShot(Used);
        used = true;
        StartCoroutine(amwaitin(duration));
        return true;
    }
    private IEnumerator amwaitin(float time)
    {
        GameControllerScript.Instance.player.walkSpeedMultipler += speedMult;
        GameControllerScript.Instance.player.runSpeedMultipler += speedMult;
        Gauge newGauge = GaugeManager.Instance.CreateGaugeInstance(Sprite, duration);
        time = duration;
        yield return null;
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
        used = false;

        GameControllerScript.Instance.player.walkSpeedMultipler -= speedMult;
        GameControllerScript.Instance.player.runSpeedMultipler -= speedMult;
        AdditionalGameCustomizer.Instance.FovAmmount -= FovAmmount;
        yield break;
    }
    [SerializeField] private float duration = 60f, speedMult,FovAmmount;
    [SerializeField] private AudioClip Used;
    [SerializeField] private Sprite Sprite;
    [SerializeField] private bool used;
}
