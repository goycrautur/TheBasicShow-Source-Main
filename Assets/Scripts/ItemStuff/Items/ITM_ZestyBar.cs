using UnityEngine;

public class ITM_ZestyBar : BaseItem
{
    public override bool OnUse()
    {
        GameControllerScript.Instance.audioDevice.PlayOneShot(aud_Crunch);

        if (AdditionalGameCustomizer.Instance.AnOldRule)
        {
            GameControllerScript.Instance.player.ResetGuilt("eat", 1f);
        }

        GameControllerScript.Instance.player.SetStamina(PlayerScript.StaminaChangeMode.Add, ZestyStamina);
        GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Add, ZestyHP, 0f, true,false);

        return true;
    }
    
    [SerializeField] private int ZestyStamina = 200;
    [SerializeField] private int ZestyHP = 20;
    [SerializeField] private AudioClip aud_Crunch;
}
