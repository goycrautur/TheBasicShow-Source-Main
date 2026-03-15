using UnityEngine;

public class ITM_ZestyBar : BaseItem
{
    public override bool OnUse()
    {
        lowBudgetAudioManagementShit.Instance.MainSource1.PlaySingleClip(aud_Crunch);

        if (AdditionalGameCustomizer.Instance.AnOldRule)
        {
            if (!GameControllerScript.Instance.player.outdoorsfr)
		    {
			    if (GameControllerScript.Instance.player.door.lockTime <= 0f) GameControllerScript.Instance.player.ResetGuilt("eat", 1f);
		    }
        }

        GameControllerScript.Instance.player.SetStamina(PlayerScript.StaminaChangeMode.Add, ZestyStamina);
        GameControllerScript.Instance.player.SetHP(!ConvertToRemoveHp ? PlayerScript.HealthChangeMode.Add : PlayerScript.HealthChangeMode.Remove, ZestyHP, 0f, true,false);
        return true;
    }
    
    [SerializeField] private int ZestyStamina = 200;
    [SerializeField] private int ZestyHP = 20;
    [SerializeField] private bool ConvertToRemoveHp = false;
    [SerializeField] private AudioObjectyeah aud_Crunch;
}
