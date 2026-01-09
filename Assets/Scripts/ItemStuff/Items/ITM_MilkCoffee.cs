using System.Collections;
using UnityEngine;

public class ITM_MilkCoffee : BaseItem
{
    public override bool OnUse()
    {
        GameControllerScript.Instance.audioDevice.PlayOneShot(audioa);
        if (!GameControllerScript.Instance.player.outdoorsfr)
		{
			if (GameControllerScript.Instance.player.door.lockTime <= 0f)
			{
			GameControllerScript.Instance.player.ResetGuilt("drink", 1f);
			}
		}
        GameControllerScript.Instance.player.SetStamina(PlayerScript.StaminaChangeMode.Add, energy);
        GameControllerScript.Instance.player.walkSpeedMultipler += walkspeedmultiplerAdd;
        GameControllerScript.Instance.player.runSpeedMultipler += runspeedmultiplerAdd;
        
        StartCoroutine(amwaitin(duration));
        return true;
    }
    private IEnumerator amwaitin(float time)
    {
        Gauge newGauge = GaugeManager.Instance.CreateGaugeInstance(coffeeSprite, duration);
        time = duration;
        yield return null;
        while (time > 0f)
        {
            if (GameControllerScript.Instance.player.stamina <= (GameControllerScript.Instance.player.maxStamina * 2.25f))
			{
			GameControllerScript.Instance.player.stamina += ((GameControllerScript.Instance.player.staminaRise/4)+passivestamina) * Time.deltaTime;
            }
            time -= Time.deltaTime;
            if (newGauge != null && (AdditionalGameCustomizer.Instance != null && AdditionalGameCustomizer.Instance.Gauges || AdditionalGameCustomizer.Instance == null))
            {
                newGauge.Set(duration, time);
            }
            yield return null;
        }
        newGauge.Hide();
        GameControllerScript.Instance.player.walkSpeedMultipler -= walkspeedmultiplerAdd;
        GameControllerScript.Instance.player.runSpeedMultipler -= runspeedmultiplerAdd;
        yield break;
    }
    [SerializeField] private float duration = 60f, energy, passivestamina, walkspeedmultiplerAdd,runspeedmultiplerAdd;
    [SerializeField] private Sprite coffeeSprite;
    [SerializeField] private AudioClip audioa;
    [SerializeField] private bool tue;
}
