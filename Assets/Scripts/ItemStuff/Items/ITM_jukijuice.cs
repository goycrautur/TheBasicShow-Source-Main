using UnityEngine;

public class ITM_jukijuice : BaseItem
{
    public override bool OnUse()
    {
        GameControllerScript.Instance.audioDevice.PlayOneShot(aud);

        if (AdditionalGameCustomizer.Instance.AnOldRule)
        {
            GameControllerScript.Instance.player.ResetGuilt("eat", 1f);
        }

        GameControllerScript.Instance.player.SetStamina(PlayerScript.StaminaChangeMode.Add, Stamina);
        GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Add, helth, 0f, true,false);
        if (replaceitem)
        {
            ItemManager.Instance.ReplaceCurrentItem(idyum);
        }
        return true;
    }

    [SerializeField] private int Stamina,idyum;
    [SerializeField] private float helth;
    [SerializeField] private bool replaceitem;

    [SerializeField] private AudioClip aud;
}
