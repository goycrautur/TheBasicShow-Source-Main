using System.Collections;
using UnityEngine;
using TMPro;

public class ITM_shucks : BaseItem
{
    [SerializeField] private float duration = 60f;
    [SerializeField] private AudioObjectyeah shucy;
    [SerializeField] private Sprite iwatchasyoubleed;
    [SerializeField] private AudioManagerLiveReaction[] audioManagersToSDIYBT;
    [SerializeField] private bool used;

    public override bool OnUse()
    {
        if (used) return false;
        GameControllerScript.Instance.lbams.PlayClip(GameControllerScript.Instance.lbams.MainSource2,shucy);
        GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Set, (int)GameControllerScript.Instance.player.maxHealth/2f, 6f, true, true, false);
        GameControllerScript.Instance.player.maxHealth += 50f;
        used = true;
        StartCoroutine(Wait());
        return true;
    }

    private IEnumerator Wait()
    {
        for (int i = 0; i < audioManagersToSDIYBT.Length; ++i)
        {
            audioManagersToSDIYBT[i].SetMute(true);
        }
        float time = shucy.audClip.length;
        GameControllerScript.Instance.player.breakwindow = true;
        Gauge newGauge = GaugeManager.Instance.CreateGaugeInstance(iwatchasyoubleed, duration);
        while (time > 0f)
        {
            time -= Time.deltaTime;
            if (newGauge != null && (AdditionalGameCustomizer.Instance != null && AdditionalGameCustomizer.Instance.Gauges || AdditionalGameCustomizer.Instance == null))
            {
                newGauge.Set(shucy.audClip.length, time);
            }
            yield return null;
        }
        newGauge.Hide();
        GameControllerScript.Instance.player.breakwindow = false;
        for (int i = 0; i < audioManagersToSDIYBT.Length; ++i)
        {
            audioManagersToSDIYBT[i].SetMute(false);
        }
        used = false;
        yield break;
    }
}
