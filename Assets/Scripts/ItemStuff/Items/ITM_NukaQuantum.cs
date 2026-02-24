using System.Collections;
using UnityEngine;

public class ITM_NukaQuantum : BaseItem
{
    public override bool OnUse()
    {
        GameControllerScript.Instance.audioDevice.PlayOneShot(audioa);
        GameControllerScript.Instance.player.SetStamina(PlayerScript.StaminaChangeMode.Add, energy);
        GameControllerScript.Instance.player.pModManag.movementModifiers.Add(SpeedModifier);
        
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
            SpeedModifier.movementMultiplier = 1f +((time/duration)*SpeedModDefault);
            if (newGauge != null && (AdditionalGameCustomizer.Instance != null && AdditionalGameCustomizer.Instance.Gauges || AdditionalGameCustomizer.Instance == null))
            {
                newGauge.Set(duration, time);
            }
            yield return null;
        }
        newGauge.Hide();
        GameControllerScript.Instance.player.pModManag.movementModifiers.Remove(SpeedModifier);
        yield break;
    }
    [SerializeField] private float duration = 60f, energy,SpeedModDefault;
    [SerializeField] private MovementModifier SpeedModifier = new MovementModifier(default(Vector3), 0f);
    [SerializeField] private Sprite Spritee;
    [SerializeField] private AudioClip audioa;
}
