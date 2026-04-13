using UnityEngine;

public class ITM_jukijuice : BaseItem
{
    public override bool OnUse()
    {
        GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(drink);
        if (!GameControllerScript.Instance.player.outdoorsfr)
		{
			if (GameControllerScript.Instance.player.door.lockTime <= 0f)GameControllerScript.Instance.player.ResetGuilt("drink", 1f);
		}
        GameControllerScript.Instance.player.SetStamina(PlayerScript.StaminaChangeMode.Add, Stamina);
        GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Add, helth, 0f, true,false);
        if (replaceitem) ItemManager.Instance.ReplaceCurrentItem(idyum);
        return true;
    }

    [SerializeField] private int Stamina,idyum;
    [SerializeField] private float helth;
    [SerializeField] private bool replaceitem;

    [SerializeField] private AudioObjectyeah drink;
}
