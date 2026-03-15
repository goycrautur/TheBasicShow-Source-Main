using System.Collections;
using UnityEngine;

public class ITM_AllTheModiumEdibles : BaseItem
{
    public override bool OnUse()
    {
        GameControllerScript.Instance.lbams.PlayClip(GameControllerScript.Instance.lbams.MainSource3,eat);
        GameControllerScript.Instance.player.SetStamina(PlayerScript.StaminaChangeMode.Add, energy);
        GameControllerScript.Instance.player.walkSpeedMultipler += walkspeedmultiplerAdd;
        GameControllerScript.Instance.player.runSpeedMultipler += runspeedmultiplerAdd;
        
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
        GameControllerScript.Instance.player.walkSpeedMultipler -= walkspeedmultiplerAdd;
        GameControllerScript.Instance.player.runSpeedMultipler -= runspeedmultiplerAdd;
        yield break;
    }
    [SerializeField] private float duration = 60f, energy, walkspeedmultiplerAdd,runspeedmultiplerAdd;
    [SerializeField] private Sprite Spritee;
    [SerializeField] private AudioObjectyeah eat;
}
