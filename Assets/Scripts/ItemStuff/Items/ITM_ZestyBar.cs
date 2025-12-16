using UnityEngine;

public class ITM_ZestyBar : BaseItem
{
    public override bool OnUse()
    {
        GameControllerScript.Instance.audioDevice.PlayOneShot(aud_Crunch);

        if (AdditionalGameCustomizer.Instance.AnOldRule)
        {
            if (!GameControllerScript.Instance.player.outdoorsfr)
		    {
			    if (GameControllerScript.Instance.player.door.lockTime <= 0f)
			    {
			    GameControllerScript.Instance.player.ResetGuilt("eat", 1f);
			    }
		    }
        }

        GameControllerScript.Instance.player.SetStamina(PlayerScript.StaminaChangeMode.Add, ZestyStamina);
        GameControllerScript.Instance.player.SetHP(!ConvertToRemoveHp ? PlayerScript.HealthChangeMode.Add : PlayerScript.HealthChangeMode.Remove, ZestyHP, 0f, true,false);
        if (SummonSubtitles)
        {
            if (Subtitles != null)
            {
            GameControllerScript.Instance.SubsManager.summonLeSubtitle2D(Subtitles.subtitleOption,Subtitles,new Vector3(0f,-170.5f,0f),GameControllerScript.Instance.audioDevice);
            }
        }
        return true;
    }
    
    [SerializeField] private int ZestyStamina = 200;
    [SerializeField] private int ZestyHP = 20;
    [SerializeField] private bool ConvertToRemoveHp = false;
    [SerializeField] private AudioClip aud_Crunch;
    [SerializeField] private bool SummonSubtitles;
    [SerializeField] private subsScriptableObject Subtitles;
}
