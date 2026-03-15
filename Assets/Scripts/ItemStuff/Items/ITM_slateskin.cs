using System.Collections;
using UnityEngine;

public class ITM_slateskin : BaseItem
{
    public override bool OnUse()
    {
        GameControllerScript.Instance.lbams.PlayClip(GameControllerScript.Instance.lbams.MainSource3,drink);
        if (!GameControllerScript.Instance.player.outdoorsfr)
		{
			if (GameControllerScript.Instance.player.door.lockTime <= 0f) GameControllerScript.Instance.player.ResetGuilt("drink", 1f);
		}
        GameControllerScript.Instance.player.SetStamina(PlayerScript.StaminaChangeMode.Add, energy);
        GameControllerScript.Instance.player.walkSpeedMultipler -= walkspeedmultiplerminus;
        GameControllerScript.Instance.player.runSpeedMultipler -= runspeedmultiplerminus;
        GameControllerScript.Instance.player.PlayerDmgResistance += shieldadd;
        StartCoroutine(amwaitin(duration));
        return true;
    }
    private IEnumerator amwaitin(float time)
    {
        Gauge newGauge = GaugeManager.Instance.CreateGaugeInstance(slatespr, duration);
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
        GameControllerScript.Instance.player.walkSpeedMultipler += walkspeedmultiplerminus;
        GameControllerScript.Instance.player.runSpeedMultipler += runspeedmultiplerminus;
        GameControllerScript.Instance.player.PlayerDmgResistance -= shieldadd;
        yield break;
    }
    [SerializeField] private float duration = 60f, energy, shieldadd, walkspeedmultiplerminus,runspeedmultiplerminus;
    [SerializeField] private Sprite slatespr;
    [SerializeField] private AudioObjectyeah drink;
}
