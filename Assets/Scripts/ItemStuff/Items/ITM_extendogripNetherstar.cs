using System.Collections;
using UnityEngine;
using TMPro;

public class ITM_extendogripNetherstar : BaseItem
{
    public override bool OnUse()
    {
        GameControllerScript.Instance.audioDevice.PlayOneShot(gloveUsed);
        StartCoroutine(amwaitin(duration));
        return true;
    }
    private IEnumerator amwaitin(float time)
    {
        Gauge newGauge = GaugeManager.Instance.CreateGaugeInstance(gloveSprite, duration);
        time = duration;
        yield return null;
        while (time > 0f)
        {
            prevDistance = GameControllerScript.Instance.player.defaultlocalRange;
            GameControllerScript.Instance.player.LocalRange = distance;
            time -= Time.deltaTime;
            if (newGauge != null && (AdditionalGameCustomizer.Instance != null && AdditionalGameCustomizer.Instance.Gauges || AdditionalGameCustomizer.Instance == null))
            {
                newGauge.Set(duration, time);
            }
            yield return null;
        }
        newGauge.Hide();
        GameControllerScript.Instance.player.LocalRange = prevDistance;
        yield break;
    }
    [SerializeField] private float duration = 60f, prevDistance;
    [SerializeField] private int distance = 60;
    [SerializeField] private AudioClip gloveUsed;
    [SerializeField] private Sprite gloveSprite;
    [SerializeField] private bool used;
}
