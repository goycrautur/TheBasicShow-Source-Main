using System.Collections;
using UnityEngine;

public class ITM_vancream : BaseItem
{
    public override bool OnUse()
    {
        if (used) return false;
        GameControllerScript.Instance.player.invisi = true;
        GameControllerScript.Instance.audioDevice.PlayOneShot(audioa);
        used = true;
        StartCoroutine(amwaitin(duration));
        return true;
    }
    private IEnumerator amwaitin(float time)
    {
        Gauge newGauge = GaugeManager.Instance.CreateGaugeInstance(vancream, duration);
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
        GameControllerScript.Instance.player.invisi = false;
        used = false;
        yield break;
    }
    [SerializeField] private float duration = 60f;
    [SerializeField] private Sprite vancream;
    [SerializeField] private AudioClip audioa;
    [SerializeField] private bool used;
}
