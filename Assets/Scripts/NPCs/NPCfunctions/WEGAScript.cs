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
		WegaAudio.SetActive(!base.stun && !base.fuckingdead);
		if (!base.stun && !base.fuckingdead && base.StunTime >= 0f)
        {
            WegaSpeed = CurWegaSpeed;
        }
        else WegaSpeed = 0f;
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
	public GameObject WegaAudio;
}