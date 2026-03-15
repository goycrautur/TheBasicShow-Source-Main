using System.Collections;
using UnityEngine;

public class ITM_smoothie : BaseItem
{
    public override bool OnUse()
    {
        GameControllerScript.Instance.lbams.PlayClip(GameControllerScript.Instance.lbams.MainSource3,drink);
        if (!GameControllerScript.Instance.player.outdoorsfr)
		{
			if (GameControllerScript.Instance.player.door.lockTime <= 0f) GameControllerScript.Instance.player.ResetGuilt("drink", 1f);
		}
        GameControllerScript.Instance.player.SetStamina(PlayerScript.StaminaChangeMode.Add, energy);
        GameControllerScript.Instance.player.walkSpeedMultipler += walkspeedmultiplerAdd;
        GameControllerScript.Instance.player.runSpeedMultipler += runspeedmultiplerAdd;
        GameControllerScript.Instance.player.maxHealth += maxHealthAdd;
        
        StartCoroutine(amwaitin(duration));
        return true;
    }
    private IEnumerator amwaitin(float time)
    {
        Gauge newGauge = GaugeManager.Instance.CreateGaugeInstance(Sprite, duration);
        time = duration;
        yield return null;
        while (time > 0f)
        {
            GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Add, passiveHealthRegen/100, 0f, true, false);
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
        GameControllerScript.Instance.player.maxHealth -= maxHealthAdd;
        yield break;
    }
    [SerializeField] private float duration = 60f, energy, passiveHealthRegen, walkspeedmultiplerAdd,runspeedmultiplerAdd,maxHealthAdd;
    [SerializeField] private Sprite Sprite;
    [SerializeField] private AudioObjectyeah drink;
    [SerializeField] private bool tue;
}
