using UnityEngine;

public class SweepScript : NPC
{
    #region Initialization and Setup
    public override void OnStart()
    {
        audioDevice = GetComponent<AudioManagerLiveReaction>();
        waitTime = Random.Range(10f, 20f);
    }
    #endregion

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
            activeTime = Random.Range(30f, 60f);
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
        waitTime = Random.Range(60f, 180f);
    }
    #endregion

    #region Collision Interaction
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC") && other.transform.GetComponent<NPC>() || other.CompareTag("Player"))
        {
            if (base.IsHitboxValid)
            {
                audioDevice.PlaySingleClip(aud_Sweep);
                if (other.transform.name == "Its a Bully") base.Wander();
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
    [SerializeField] private AudioObjectyeah aud_Intro;

    [Header("State Management")]
    [SerializeField] private bool active;
    #endregion

    #region Internal References
    private AudioManagerLiveReaction audioDevice;
    #endregion
}