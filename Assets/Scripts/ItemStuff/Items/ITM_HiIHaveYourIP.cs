using System.Collections;
using UnityEngine;
using TMPro;

public class ITM_HiIHaveYourIP : BaseItem
{
    public override bool OnUse()
    {
        if (used) return false;
        GameControllerScript.Instance.audioDevice.PlayOneShot(Used);
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
        yield break;
    }
    [SerializeField] private float duration = 60f;
    [SerializeField] private AudioClip Used;
    [SerializeField] private Sprite Sprite;
    [SerializeField] private bool used;
}
