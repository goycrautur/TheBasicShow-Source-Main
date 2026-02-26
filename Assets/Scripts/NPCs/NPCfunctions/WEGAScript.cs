using UnityEngine;
using System.Collections;
public class WEGAScript : NPC
{
	public override void OnStart()
	{
		base.OnStart();
		
	}

	public override void OnUpdate()
	{
		base.SetToXrayLayer();
		CurWegaSpeed = gc.wegchal.globalWegaSpeed;
		if (base.stun)
        {
            WegaSpeed = 0f;
        }
        if (base.StunTime <= 0f)
        {
            WegaSpeed = CurWegaSpeed;
        }
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(base.player.position.x,base.player.position.y -4,base.player.position.z), WegaSpeed * Time.deltaTime);
	}
	private void OnTriggerStay(Collider play)
    {
        if (play.CompareTag("Player") & !gc.debugMode & !gc.player.titlecard)
        {
            if (base.IsHitboxValid)
			{
				gc.player.SetHP(PlayerScript.HealthChangeMode.Remove, 45f / gc.player.PlayerDmgResistance, 0.5f, false, true, false);
			}
        }
    }
	public float WegaSpeed,CurWegaSpeed;
}