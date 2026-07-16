using UnityEngine;
using System;

public class ITM_jukijuice : BaseItem
{
    public override void Awake()
    {
        base.Awake();
        BaseItem ite = this;
        ite.SmallSprite = JukiJuiceStatsistic[ite.Uses-1].JukiJuiceTexture;
        ite.ItmInfoText = JukiJuiceStatsistic[ite.Uses-1].description;
        GameControllerScript.Instance.player.SetStamina(PlayerScript.StaminaChangeMode.Add, JukiJuiceStatsistic[ite.Uses-1].Stamina);
        GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Add, JukiJuiceStatsistic[ite.Uses-1].helth, 0f, true,false);
        Debug.Log($"set to juki juic stats thing num {ite.Uses-1}");
    }
    public override bool OnUse()
    {
        GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(drink);
        if (!GameControllerScript.Instance.player.outdoorsfr)
		{
			if (GameControllerScript.Instance.player.door.lockTime <= 0f)GameControllerScript.Instance.player.ResetGuilt("drink", 1f);
		}
        BaseItem ite = this;
        if (ite.Uses == 1)
        {
            GameControllerScript.Instance.player.SetStamina(PlayerScript.StaminaChangeMode.Add, JukiJuiceStatsistic[0].Stamina);
            GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Add, JukiJuiceStatsistic[0].helth, 0f, true,false);
        }
        return true;
    }
    public override void AfterUse()
    {
        BaseItem ite = ItemManager.Instance.GetSelectedItemObject();
        ite.SmallSprite = JukiJuiceStatsistic[ite.Uses-1].JukiJuiceTexture;
        ite.ItmInfoText = JukiJuiceStatsistic[ite.Uses-1].description;
        GameControllerScript.Instance.player.SetStamina(PlayerScript.StaminaChangeMode.Add, JukiJuiceStatsistic[ite.Uses-1].Stamina);
        GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Add, JukiJuiceStatsistic[ite.Uses-1].helth, 0f, true,false);
        ItemManager.Instance.AnimateSwap(ItemManager.Instance.ItemSelection,ite.SmallSprite);
    }

    [Serializable]
    public struct JukiJuiceThing 
    {
        public int Stamina;
        public float helth;
        public Texture JukiJuiceTexture;
        public string description;
    }
    [SerializeField] private JukiJuiceThing[] JukiJuiceStatsistic;
    [SerializeField] private AudioObjectyeah drink;
    
}
