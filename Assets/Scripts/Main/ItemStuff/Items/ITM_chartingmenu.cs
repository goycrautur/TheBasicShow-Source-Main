using System.Collections;
using UnityEngine;

public class ITM_chartingmenu : BaseItem
{
    public override bool OnUse()
    {
        if (cantuse) return false;
        GameControllerScript.Instance.player.walkSpeedMultipler += walkspeedmultiplerAdd;
        GameControllerScript.Instance.player.runSpeedMultipler += runspeedmultiplerAdd;
        GameControllerScript.Instance.player.PlayerDmgResistance += shieldadd;
        StartCoroutine(amwaitin(duration));
        return true;
    }
    private IEnumerator amwaitin(float time)
    {
        cantuse = true;
        Gauge newGauge = GaugeManager.Instance.CreateGaugeInstance(chartsprt, duration);
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
        GameControllerScript.Instance.player.PlayerDmgResistance -= shieldadd;
        cantuse = false;
        yield break;
    }
    [SerializeField] private float duration = 120f, shieldadd, walkspeedmultiplerAdd,runspeedmultiplerAdd;
    [SerializeField] private Sprite chartsprt;
    [SerializeField] private bool cantuse;
}
