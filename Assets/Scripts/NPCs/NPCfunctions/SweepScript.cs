using UnityEngine;

public class SweepScript : NPC
{
    #region Initialization and Setup
    public override void OnStart()
    {
        Physics.IgnoreCollision(base.cecil, GetComponent<CapsuleCollider>(), ignore: true);
        audioDevice = GetComponent<AudioManagerLiveReaction>();
        waitTime = Random.Range(10f, 20f);
    }
    #endregion
    public void OnEnable()
    {
        activeTime = 0f;
        GoHome();
    }

    #region Activity and Timer Logic
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (waitTime > 0f)
        {
            waitTime -= Time.deltaTime;
            return;
        }
        base.agentSpeed = base.DefaultAgentSpeed * base.agentSpeedScale;
        agent.speed = base.agentSpeed;
        if (base.stun) agent.speed = 0f;
        if (base.StunTime < 0f) agent.speed = base.agentSpeed;

        if (!active)
        {
            active = true;
            activeTime = Random.Range(60f, 120f);
            Wander();
            audioDevice.PlaySingleClip(aud_Intro);
            return;
        }

        if (active)
        {
            activeTime -= Time.deltaTime;
            if (activeTime <= 0f) GoHome();
        }
        waitStuff();
    }
    #endregion

    #region Movement Handling
    protected override void HandleMovement()
    {
        if (waitTime <= 0f && active) base.HandleMovement();
    }

    public void waitStuff()
    {
        if (waitTime > 0f) return;
        if (active && activeTime > 0f)
        {
            if (agent.remainingDistance <= 1f && !agent.pathPending && coolDown <= 0f) base.Wander();
        }
    }

    private void GoHome()
    {
        active = false;
        agent.SetDestination(homeLocation.position);
        waitTime = Random.Range(30f, 60f);
    }
    #endregion

    #region Collision Interaction
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC") && other.transform.GetComponent<NPC>() || other.CompareTag("Player"))
        {
            if (base.IsHitboxValid)
            {
                
                if (other.transform.name == "Its a Bully") base.Wander();
            }
        }
        if (other.CompareTag("NPC"))
        {
            NPC npeecee = other.transform.GetComponent<NPC>();
            if (base.IsHitboxValid && !npeecee.fuckingdead)
            {
                ChainsawAudio.ClearQueue(true);
                ChainsawAudio.PlaySingleClip(aud_Attack);
                audioDevice.PlaySingleClip(aud_Sweep);
                npeecee.Stun(2f);
                npeecee.DrainHp(25);
                npeecee.PushNpc(npeecee.GetNPCPushDirection(-transform.forward),64, 1f);
            }
        }
        if (other.CompareTag("Player") & !gc.debugMode & !gc.player.titlecard)
        {
            if (base.IsHitboxValid)
			{
                ChainsawAudio.ClearQueue(true);
                ChainsawAudio.PlaySingleClip(aud_Attack);
                audioDevice.PlaySingleClip(aud_Sweep);
				gc.player.SetHP(PlayerScript.HealthChangeMode.Remove, 25f / gc.player.PlayerDmgResistance, 1f, false, true, false);
                gc.player.PushPlayer(gc.player.GetPlayerPushDirection(transform.position), 64f, 0.5f);
			}
        }
    }
    #endregion

    #region Serialized Fields
    [Header("Movement and Navigation")]
    [SerializeField] private Transform homeLocation;
    [SerializeField] private float waitTime, activeTime;

    [Header("Audio")]
    [SerializeField] private AudioObjectyeah aud_Sweep;
    [SerializeField] private AudioObjectyeah aud_Intro,aud_Attack;
    [SerializeField] private AudioManagerLiveReaction ChainsawAudio;

    [Header("State Management")]
    [SerializeField] private bool active;
    #endregion

    #region Internal References
    private AudioManagerLiveReaction audioDevice;
    #endregion
}