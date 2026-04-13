using UnityEngine;

public class ITM_Scissors : BaseItem
{
    public override bool OnUse()
    {
        if (AdditionalGameCustomizer.Instance.DetentionAfterScissorUse)
        {
            if (!GameControllerScript.Instance.player.outdoorsfr)
		    {
			    if (GameControllerScript.Instance.player.door.lockTime <= 0f) GameControllerScript.Instance.player.ResetGuilt("bully", 1f);
		    }
        }
        if (GameControllerScript.Instance.player.jumpropes.Count > 0)
        {
            GameControllerScript.Instance.player.jumpropes[0].End(false);
            GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(aud_Snip);
            return true;
        }

        if (SendRay("", out RaycastHit Ray, GameControllerScript.Instance.player.LocalRange))
        {
            if (Ray.collider.name == "1st Prize" || Ray.collider.name == "washingmachine")
            {
                GameControllerScript.Instance.firstPrizeScript.GoCrazy();
                GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(aud_Snip);
                return true;
            }
        }
        return false;
    }
    
    [SerializeField] protected AudioObjectyeah aud_Snip;
}
