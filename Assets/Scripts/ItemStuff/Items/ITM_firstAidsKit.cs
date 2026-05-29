using System.Collections;
using UnityEngine;
using TMPro;

public class ITM_firstAidsKit : BaseItem
{
    public override bool OnUse()
    {
        GameControllerScript.Instance.player.KitPenalty += 1;
        if (GameControllerScript.Instance.player.KitPenalty-1 > ButterfingersDurationAdds.Length) GameControllerScript.Instance.player.KitPenalty = 0;
        GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Add, GameControllerScript.Instance.player.maxHealth * ((float)(HpHealPercentage-(GameControllerScript.Instance.player.KitPenalty*PercentageDecreasePerUse))/100), 0f, true,false);
        GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(Used);
        return true;
    }
    public override void CustomSpecialFunction()
    {
        SelfRevive();
    }
    public void SelfRevive()
    {
        GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Add, GameControllerScript.Instance.player.maxHealth * ((float)(HpHealPercentage-(GameControllerScript.Instance.player.KitPenalty*PercentageDecreasePerUse))/100), 0f, true,false);
        GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(Used);
        ItemManager.Instance.Butterfingers(ButterfingersDurationTime + ButterfingersDurationAdds[GameControllerScript.Instance.player.KitPenalty]);
    }
    [SerializeField] private int HpHealPercentage,PercentageDecreasePerUse;
    [SerializeField] private float ButterfingersDurationTime;
    [SerializeField] private float[] ButterfingersDurationAdds;
    [SerializeField] private AudioObjectyeah Used;
}
