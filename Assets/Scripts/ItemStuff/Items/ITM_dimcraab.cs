using System.Collections;
using UnityEngine;

public class ITM_dimcraab : BaseItem
{
    public override bool OnUse()
    {
        
        StartCoroutine(amwaitin(duration));
        return true;
    }
    private IEnumerator amwaitin(float time)
    {
        Gauge newGauge = GaugeManager.Instance.CreateGaugeInstance(Spritee, duration);
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
        yield break;
    }
    [SerializeField] private float duration = 60f;
    [SerializeField] private Sprite Spritee;
    [SerializeField] private AudioClip audioa;
    [SerializeField] private MovementModifier SpeedModifier = new MovementModifier(default(Vector3), 0f);
}
