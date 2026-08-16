using UnityEngine;
using System.Collections;

public class MuchoScript : NPC
{
    #region Unity Lifecycle
    public override void OnStart()
    {
        base.OnStart();
        GetAngry(0f);
        if (endless) Endless();
        Wander();
    }
    public void OnEnable()
    {
        gc.muchscr.Add(this);
        Move();
    }
    public void OnDisable()
    {
        gc.muchscr.Remove(this);
    }

    public override void OnUpdate()
    {
        if (LOEManager.Instance.activated)
        {
            Hear(player.position, 9999, false);
            MuchoAnger += 0.001f * Time.deltaTime;
        }
        if (antiHearing) AntiHearingDuratio -= Time.deltaTime;
        if (AntiHearingDuratio < 0f) antiHearing = false;
        targe();
        base.OnUpdate();
        base.agentSpeed = base.DefaultAgentSpeed * base.agentSpeedScale;
        if (base.stun)
        {
            stopMoving = true;
            agent.speed = 0;
        }
        if (base.StunTime < 0f)
        {
            stopMoving = false;
            resetWaitTime();
            Move();
        }
        if (base.fuckingdead)
        {
            stopMoving = true;
        }
        if (base.DeathRespawnTime < 0f)
        {
            stopMoving = false;
            resetWaitTime();
            Move();
        }
        if (MuchoTempAnger > 0f) MuchoTempAnger -= 0.05f * Time.deltaTime;
        else MuchoTempAnger = 0f;
    }
    public void targe()
    {
        if (player == null) return;
        if ((transform.position + Vector3.up * 2f).RaycastFromPosition(player.position - transform.position, out RaycastHit raycastHit))
        {
            if ((transform.position + Vector3.up * 2f).RaycastFromPosition(player.position - transform.position, out RaycastHit hitVape, QueryTriggerInteraction.UseGlobal))
            {
                if (hitVape.transform.gameObject.layer == 11) 
                {
                    //Debug.Log("saw bro but get blocked");
                    return;
                }
            }
            if (raycastHit.transform.CompareTag("Player") && !gc.player.invisi && !gc.player.invisichalk)
            {
                TargetPlayer();
            }
            //else Debug.Log("didnt saw bro");
        }
    }
    #endregion


    #region Movement
    protected override void Wander(string locationType = "default")
    {
        base.Wander(locationType);
        currentPriority = 0f;
    }

    protected override void TargetPlayer()
    {
        base.TargetPlayer();
        currentPriority = 0f;
        Hear(player.position, 9999, false);
    }

    public void Move()
    {
        if (this.isActiveAndEnabled)
        {
            if (deafened)
            {
                Debug.Log("he fogor");
                Muchocator.Rebind();
                Muchocator.Play("BjIndicator_Confused", -1, 0f);
            }
            deafened = false;
            ThrowProjectile(Random.Range(0,3));
            if (MuchoAnger < 40f) agent.speed = base.agentSpeed;
            if (MuchoAnger > 40f) agent.speed = base.agentSpeed * (MuchoAnger/40);
            MuchoAudio.PlaySingleClip(slam);
            MuchoAnimator.SetTrigger("slam");
            if (!stopMoving)
            {
                slams++;
                if (slams < 20) Invoke(nameof(OnMoveDone), timeToMove);
                else if (slams >= 20)
                {
                    Muchocator.Rebind();
                    Debug.Log("gon telepor");
                    Invoke(nameof(Teleport), teleportCD*1.5f);
                    slams = 0;
                    agent.speed = 0;
                    deafened = true;
                }
            }
            resetWaitTime();
        }
    }
    public void resetWaitTime()
    {
        MuchoWait = (-3 - MuchoTempAnger) * MuchoAnger / (MuchoAnger+2f / MuchoSpeedScale) + 3f;
    }
    public void ThrowProjectile(int val = 0)
    {
        if (val != 2)
        {
            if ((transform.position + Vector3.up * 2f).RaycastFromPosition(player.position - transform.position, out RaycastHit raycastHit))
            {
                if ((transform.position + Vector3.up * 2f).RaycastFromPosition(player.position - transform.position, out RaycastHit hitVape, QueryTriggerInteraction.UseGlobal))
                {
                    if (hitVape.transform.gameObject.layer == 11) 
                    {
                        //Debug.Log("saw bro but get blocked");
                        return;
                    }
                }
                if (raycastHit.transform.CompareTag("Player") && !gc.player.invisi && !gc.player.invisichalk)
                {
                    transform.LookAt(this.player.position);
                    Vector3 direction = this.player.position - base.transform.position;
                    Vector3 vector = new Vector3(base.transform.position.x, 5f, base.transform.position.z);
                    Vector3 upithink = new Vector3(base.transform.position.x, base.transform.position.y + 2f, base.transform.position.z);
                    Instantiate(projectilePrefabs[Random.Range(0,projectilePrefabs.Length)], upithink, Quaternion.LookRotation(this.player.position - vector));
                }
            }
        }
    }

    private void OnMoveDone()
    {
        agent.speed = 0;
        if (agent.isActiveAndEnabled && agent.remainingDistance <= 0.01f) Wander();
        if (!stopMoving) Invoke(nameof(Move), MuchoWait);
    }
    private void Teleport()
    {
        if (agent.isActiveAndEnabled && agent.remainingDistance <= 0.01f) Wander();
        MuchoAudio.PlaySingleClip(snadtp);
        Invoke(nameof(Move), teleportCD);
        Vector3 tpTransform = base.wanderer.SetNewTargetForAgent(null, "default") + Vector3.up * this.transform.position.y;
        if (base.navmeshNpcPushing) transform.position = tpTransform;
        else agent.Warp(tpTransform);
        

    }
    #endregion
    private void OnTriggerStay(Collider play)
    {
        if (play.CompareTag("Player") & !gc.debugMode & !gc.player.titlecard)
        {
            if (base.IsHitboxValid)
			{
				gc.player.SetHP(PlayerScript.HealthChangeMode.Remove, 25f / gc.player.PlayerDmgResistance, 0.05f, false, true, false);
				gc.player.killedbybaldi = true;
                gc.player.PushPlayer(gc.player.GetPlayerPushDirection(transform.position), 64f, 0.5f);
                PushNpc(GetNPCPushDirection(-transform.position),32f, 1f);
			}
        }
    }

    #region Anger System
    public void GetAngry(float value)
    {
        MuchoAnger += value;
        if (MuchoAnger < 0.5f) MuchoAnger = 0.5f;
    }

    public void GetTempAngry(float value) => MuchoTempAnger += value;

    public void Endless()
    {
        Invoke(nameof(Endless), timeToAnger);
        timeToAnger = angerFrequency;
        GetAngry(angerRate);
        angerRate += angerRateRate;
    }
    #endregion

    #region Hearing Detection
    public void Hear(Vector3 soundLocation, float priority, bool indicator = true)
    {
        if (!isActiveAndEnabled && deafened) return;

        bool canHear = !antiHearing && priority >= currentPriority;
        bool inNoSqueeArea = false;

        foreach (Collider collider in Physics.OverlapSphere(soundLocation, 0.1f))
        {
            if (collider.gameObject.CompareTag("NoSquee Area"))
            {
                canHear = false;
                inNoSqueeArea = true;
                break;
            }
        }

        if (canHear)
        {
            if (base.navmeshNpcPushing) base.PushedTargetPos = soundLocation;
            else agent.SetDestination(soundLocation);
            currentPriority = priority;

            if (AdditionalGameCustomizer.Instance.Indicator && indicator)
            {
                if (!inNoSqueeArea)
                {
                    Muchocator.Rebind();
                    Muchocator.Play("BjIndicator_Heared", -1, 0f);
                }
            }
        }
        else
        {
            if (AdditionalGameCustomizer.Instance.Indicator && indicator && !antiHearing)
            {
                if (!inNoSqueeArea)
                {
                    Muchocator.Rebind();
                    Muchocator.Play("BjIndicator_Confused", -1, 0f);
                }
            }
        }
    }
    public void ActivateAntiHearing(float SetTime)
    {
        Wander();
        antiHearing = true;
        AntiHearingDuratio = SetTime;
    }
    #endregion
    [SerializeField] private GameObject[] projectilePrefabs;

    #region Serialized Field States
    [Header("Stats")]
    [SerializeField] private float MuchoAnger;
    public float MuchoTempAnger, MuchoWait, MuchoSpeedScale;

    [Header("Movement and Behavior")]
    [SerializeField] private float timeToMove;
    public bool stopMoving, antiHearing;
    public Coroutine MoveCoroutine;

    [Header("Anger Management")]
    [SerializeField] private float angerRate;
    [SerializeField] private float slams,teleportCD,angerRateRate, angerFrequency, timeToAnger,AntiHearingDuratio = 1f;
    public bool endless;

    [Header("Audio and Animation")]
    [SerializeField] private AudioObjectyeah slam;
    [SerializeField] private AudioObjectyeah snadtp;
    [SerializeField] private Animator Muchocator, MuchoAnimator;

    private float currentPriority;
    private bool deafened;
    [SerializeField] private AudioManagerLiveReaction MuchoAudio;
    #endregion
}