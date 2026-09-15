using UnityEngine;

public class ITM_Totem : BaseItem
{
    public override bool OnUse() => false;
    public override void OnSelect()
    {
        base.OnSelect();
        play.SetRealHoldTotem = true;
    }
    public override void OnDeselect() => play.SetRealHoldTotem = false;
    private PlayerScript play => GameControllerScript.Instance.player;
}
