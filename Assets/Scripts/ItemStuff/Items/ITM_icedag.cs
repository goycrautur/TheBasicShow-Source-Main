using System.Collections;
using UnityEngine;
using TMPro;

public class ITM_icedag : BaseItem
{
    public override bool OnUse()
    {
        if (used) return false;
        GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Add, GameControllerScript.Instance.player.maxHealth * ((float)HpHealPercentage/100), 0f, true,false);
        GameControllerScript.Instance.player.SetStamina(PlayerScript.StaminaChangeMode.Add, GameControllerScript.Instance.player.maxStamina * ((float)StaminaHealPercentage/100));
        GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(Used);
        used = true;
        StartCoroutine(amwaitin(coohdown));
        return true;
    }
    private IEnumerator amwaitin(float time)
    {
        while (time > 0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        used = false;
        yield break;
    }
    [SerializeField] private int HpHealPercentage,StaminaHealPercentage;
    [SerializeField] private float coohdown;
    [SerializeField] private AudioObjectyeah Used;
    [SerializeField] private bool used;
}
