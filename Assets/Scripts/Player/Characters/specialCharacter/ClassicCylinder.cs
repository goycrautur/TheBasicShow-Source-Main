using UnityEngine;

public class ClassicCylinder : SpecialCharStuff
{
    public override void OnUpdates()
    {
        SlowSpeed = GameControllerScript.Instance.player.walkSpeed/2;
        OgRange = GameControllerScript.Instance.player.defaultlocalRange;
        slowerStuff();
        infiRangeStuff();
    }
    public void slowerStuff()
    {
        bool lowstamina = GameControllerScript.Instance.player.stamina < 0f && !ZerullClassic.Instance.BossStarted;
        GameControllerScript.Instance.player.OverridePlayerSpeed = lowstamina;
        if (lowstamina)GameControllerScript.Instance.player.playerSpeed = SlowSpeed;
        
    }
    public void infiRangeStuff()
    {
        if (GameControllerScript.Instance.player.OverridePlayerRange) return;
        GameControllerScript.Instance.player.LocalRange = OgRange;
        foreach (PickupScript pi in FindObjectsOfType<PickupScript>())
		{
			if (Vector3.Distance(GameControllerScript.Instance.player.PlayerTransform.position, pi.transform.position) <= OgRange/1.2f && !pi.DroppedItem) 
            {
                GameControllerScript.Instance.player.LocalRange = 9999;
            }
		}
    }
    public float OgRange,SlowSpeed;
}
