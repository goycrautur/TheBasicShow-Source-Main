using UnityEngine;

public class ITM_Tape : BaseItem
{
    public override bool OnUse()
    {
        if (SendRay("", out RaycastHit Ray, GameControllerScript.Instance.player.LocalRange))
        {
            if (Ray.collider.CompareTag("TapePlayer"))
            {
                if (!Ray.collider.gameObject.GetComponent<TapePlayerScript>().TapeCDEnable)
                {
                    Ray.collider.gameObject.GetComponent<TapePlayerScript>().Play(holyshitTapeTypes == TapeTypes.BaldiLeastFavouriteTape ? "normal" : holyshitTapeTypes == TapeTypes.JerryEarPiercingDvdDisk ? "JEPDVDD" : holyshitTapeTypes == TapeTypes.JerryAbsoloutelyFuckingBangerDvdDisk ? "jerrypeakassDisc" :holyshitTapeTypes == TapeTypes.JerryAbsoloutelyFuckingBangerDvdDiskExpertMode ? "jerrypeakassDiscExpert" :"");
                    GameControllerScript.Instance.lbams.PlayClip(GameControllerScript.Instance.lbams.MainSource3,aud_Insert);
                    return true;
                }
            }
        }
        return false;
    }

    public TapeTypes holyshitTapeTypes;
    public enum TapeTypes
    {
        BaldiLeastFavouriteTape,
        JerryEarPiercingDvdDisk,
        JerryAbsoloutelyFuckingBangerDvdDisk,
        JerryAbsoloutelyFuckingBangerDvdDiskExpertMode
        
    }
    [SerializeField] private AudioObjectyeah aud_Insert;
}
