using UnityEngine;

public class ITM_SwingDoorLock : BaseItem
{
    public override bool OnUse()
    {
        if (SendRay("", out RaycastHit Ray, GameControllerScript.Instance.player.LocalRange))
        {
            if (Ray.collider.CompareTag("SwingingDoor"))
            {
                SwingingDoorScript swing = Ray.collider.gameObject.GetComponent<SwingingDoorScript>();
                if (!swing.bDoorLocked)
                {
                    swing.LockDoor(DoorLockTime);
                    GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(aud_Locked);
                    return true;
                }
                else return false;
            }
        }
        return false;
    }
    
    [SerializeField] private int DoorLockTime = 15;
    [SerializeField] private AudioObjectyeah aud_Locked;
}
