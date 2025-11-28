using UnityEngine;

public class ITM_Scissors : BaseItem
{
    public override bool OnUse()
    {
        if (AdditionalGameCustomizer.Instance.DetentionAfterScissorUse)
        {
            if (!GameControllerScript.Instance.player.outdoorsfr)
		    {
			    if (GameControllerScript.Instance.player.door.lockTime <= 0f)
			    {
			    GameControllerScript.Instance.player.ResetGuilt("bully", 1f);
			    }
		    }
        }

        if (GameControllerScript.Instance.player.jumpRope)
        {
            GameControllerScript.Instance.player.DeactivateJumpRope();
            GameControllerScript.Instance.audioDevice.PlayOneShot(aud_Snip);
            GameControllerScript.Instance.playtimeScript.Disappoint();
            return true;
        }

        if (SendRay("", out RaycastHit Ray, GameControllerScript.Instance.player.LocalRange))
        {
            if (Ray.collider.name == "1st Prize" || Ray.collider.name == "washingmachine")
            {
                GameControllerScript.Instance.firstPrizeScript.GoCrazy();
                GameControllerScript.Instance.audioDevice.PlayOneShot(aud_Snip);
                return true;
            }
        }
        return false;
    }
    
    [SerializeField] protected AudioClip aud_Snip;
}
