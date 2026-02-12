using UnityEngine;

public class craldiCylinder : SpecialCharStuff
{
    public override void OnUpdates()
    {
        SlowSpeed = GameControllerScript.Instance.player.walkSpeed/2;
        slowerStuff();
    }
    public void slowerStuff()
    {
        bool lowstamina = GameControllerScript.Instance.player.stamina < 0f && !ZerullClassic.Instance.BossStarted;
        GameControllerScript.Instance.player.OverridePlayerSpeed = lowstamina;
        if (lowstamina)GameControllerScript.Instance.player.playerSpeed = SlowSpeed;
        
    }
    public float SlowSpeed;
}
