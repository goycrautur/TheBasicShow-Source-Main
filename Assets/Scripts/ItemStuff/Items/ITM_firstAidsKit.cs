using System.Collections;
using UnityEngine;
using TMPro;

public class ITM_firstAidsKit : BaseItem
{
    public override bool OnUse()
    {
        BaseItem ite = ItemManager.Instance.GetSelectedItemObject();
        GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Add, GameControllerScript.Instance.player.maxHealth * ((float)(HpHealPercentage-((ite.OgUsesAmmount-ite.Uses)*PercentageDecreasePerUse))/100), 0f, true,false);
        GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(Used);
        return true;
    }
    public override void CustomSpecialFunction()
    {
        SelfRevive();
    }
    public void SelfRevive()
    {
        GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Add, GameControllerScript.Instance.player.maxHealth * ((float)(HpHealPercentage-((OgUsesAmmount-Uses)*PercentageDecreasePerUse))/100), 0f, true,false);
        Debug.Log($"aids kit have {OgUsesAmmount-Uses} use left, and got used for self revive");
        GameControllerScript.Instance.player.Iframes = 2.5f;
        GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(Used);

        ItemManager.Instance.Butterfingers(ButterfingersDurationTime + ButterfingersDurationAdds[OgUsesAmmount-Uses]);
    }
    [SerializeField] private int HpHealPercentage,PercentageDecreasePerUse;
    [SerializeField] private float ButterfingersDurationTime;
    [SerializeField] private float[] ButterfingersDurationAdds;
    private float[] buttFigDura;
    [SerializeField] private AudioObjectyeah Used;
}
