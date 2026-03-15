using UnityEngine;
using System.Collections;
public class SausageJerry : NPC
{
	public override void OnStart()
	{
		base.OnStart();
	}

	public override void OnUpdate()
	{
		base.agentSpeed = base.DefaultAgentSpeed * base.agentSpeedScale;
		agent.speed = base.agentSpeed;
		if (base.stun)
        {
            agent.speed = 0f;
        }
        if (base.StunTime < 0f)
        {
            agent.speed = base.agentSpeed;
        }
		if (fartCooldown > 0f)
		{
			fartCooldown -= Time.deltaTime;
			
		}
		if (fartCooldown <= 0f)
		{
			fart();
		}
	}
	public void fart()
	{
		SausageJerAudio.PlaySingleClip(farth);
		Instantiate(Itemspawn, new Vector3(transform.position.x, 4f, transform.position.z), transform.rotation);
		Instantiate(FartParticle, transform.position, transform.rotation);
		fartCooldown = DefaultfartCooldown;
	}

	//protected override void CheckForPlayer() => base.CheckForPlayer();

	protected override void HandleMovement() => base.HandleMovement();

	protected override void Wander(string locationType = "default") => base.Wander(locationType);

	//protected override void TargetPlayer() => base.TargetPlayer();
	public float DefaultfartCooldown;
	public float fartCooldown;
	public GameObject Itemspawn, FartParticle;
	public AudioManagerLiveReaction SausageJerAudio;
	public AudioObjectyeah farth;
}