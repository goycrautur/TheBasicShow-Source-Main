using System.Collections;
using UnityEngine;

public class Spongbobpopsicl : BaseItem
{
    public override bool OnUse()
    {
        GameControllerScript.Instance.audioDevice.PlayOneShot(audioa);
        GameControllerScript.Instance.player.SetStamina(PlayerScript.StaminaChangeMode.Add, energy);
        
        freez(Random.Range(0,3));
        return true;
    }
    private void freez(int what = 0)
    {
        if (what == 2)
        {
            StartCoroutine(freeze(FreezeDuration));
        }
    }
    private IEnumerator freeze(float time)
    {
        GameControllerScript.Instance.player.pModManag.movementModifiers.Add(SpeedModifier);
        Gauge newGauge = GaugeManager.Instance.CreateGaugeInstance(Spritee, FreezeDuration);
        yield return null;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            if (newGauge != null && (AdditionalGameCustomizer.Instance != null && AdditionalGameCustomizer.Instance.Gauges || AdditionalGameCustomizer.Instance == null))
            {
                newGauge.Set(FreezeDuration, time);
            }
            yield return null;
        }
        newGauge.Hide();
        GameControllerScript.Instance.player.pModManag.movementModifiers.Remove(SpeedModifier);
        yield break;
    }
    [SerializeField] private float FreezeDuration = 5f, energy;
    [SerializeField] private MovementModifier SpeedModifier = new MovementModifier(default(Vector3), 0f);
    [SerializeField] private Sprite Spritee;
    [SerializeField] private AudioClip audioa;
}




