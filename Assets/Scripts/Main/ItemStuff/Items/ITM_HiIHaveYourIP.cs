using System.Collections;
using UnityEngine;
using TMPro;

public class ITM_HiIHaveYourIP : BaseItem
{
    public override bool OnUse()
    {
        if (used) return false;
        GameControllerScript.Instance.player.PlayerDmgResistance += DefendAdd;
        GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(Used);
        used = true;
        StartCoroutine(amwaitin(duration));
        return true;
    }
    private IEnumerator amwaitin(float time)
    {
        GameControllerScript.Instance.ipleak = true;
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
        GameControllerScript.Instance.ipleak = false;
        GameControllerScript.Instance.player.PlayerDmgResistance -= DefendAdd;
        yield break;
    }
    [SerializeField] private float duration = 60f,DefendAdd;
    [SerializeField] private AudioObjectyeah Used;
    [SerializeField] private Sprite Sprite;
    [SerializeField] private bool used;
}
