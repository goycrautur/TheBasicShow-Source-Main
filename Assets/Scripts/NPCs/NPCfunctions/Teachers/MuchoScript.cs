using UnityEngine;
using System.Collections;

public class MuchoScript : NPC
{
    #region Unity Lifecycle
    public override void OnStart()
    {
        base.OnStart();
        GetAngry(0f);

        if (endless)
        {
            Endless();
        }

        Wander();
        Move();
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
        if (antiHearing)
		{
			AntiHearingDuratio -= Time.deltaTime;
		}
        if (AntiHearingDuratio < 0f)
		{
            antiHearing = false;
        }
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
        if (baldiTempAnger > 0f)
        {
            baldiTempAnger -= 0.05f * Time.deltaTime;
        }
        else
        {
            baldiTempAnger = 0f;
        }

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
            slams++;
            ThrowProjectile(Random.Range(0,3));
            gc.SubsManager.summonLeSubtitle(slamSound.subtitleOption,slamSound,baldiAudio);
            if (baldiAnger < 40f)
            {
                agent.speed = base.agentSpeed;
            }
            if (baldiAnger > 40f)
            {
                agent.speed = base.agentSpeed * (baldiAnger/40);
            }
            baldiAudio.PlayOneShot(slap);
            baldiAnimator.SetTrigger("slam");

            if (!stopMoving)
            {
                if (slams != 20)
                {
                    Invoke(nameof(OnMoveDone), timeToMove);
                }
                if (slams == 20)
                {
                    Invoke(nameof(Teleport), teleportCD);
                    slams = 0;
                    agent.speed = 0;
                }
            }
            resetWaitTime();
        }
    }
    public void resetWaitTime()
    {
        baldiWait = (-3 - baldiTempAnger) * baldiAnger / (baldiAnger+2f / baldiSpeedScale) + 3f;
    }
    public void ThrowProjectile(int val = 0)
    {
        if (val == 2)
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
                if (raycastHit.transform.CompareTag("Player"))
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

        if (agent.remainingDistance <= 0.1f)
        {
            Wander();
        }

        if (!stopMoving)
        {
            Invoke(nameof(Move), baldiWait);
        }
    }
    private void Teleport()
    {
        if (agent.remainingDistance <= 0.1f)
        {
            Wander();
        }
        baldiAudio.PlayOneShot(snadtp);
        gc.SubsManager.summonLeSubtitle(snadtpsubs.subtitleOption,snadtpsubs,baldiAudio);
        Invoke(nameof(Move), teleportCD);
        Vector3 tpTransform = base.wanderer.SetNewTargetForAgent(null, "default") + Vector3.up * this.transform.position.y;
        agent.Warp(tpTransform);

    }
    #endregion
    private void OnTriggerStay(Collider play)
    {
        if (play.CompareTag("Player") & !gc.debugMode & !gc.player.titlecard)
        {
            if (base.IsHitboxValid)
			{
				gc.player.SetHP(PlayerScript.HealthChangeMode.Remove, 2f / gc.player.PlayerDmgResistance, 0.0125f, false, true, false);
				gc.player.killedbybaldi = true;
			}
        }
    }

    #region Anger System
    public void GetAngry(float value)
    {
        baldiAnger += value;

        if (baldiAnger < 0.5f)
        {
            baldiAnger = 0.5f;
        }

    }

    public void GetTempAngry(float value) => baldiTempAnger += value;

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
        if (!isActiveAndEnabled) return;

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
            agent.SetDestination(soundLocation);
            currentPriority = priority;

            if (AdditionalGameCustomizer.Instance.Indicator && indicator)
            {
                if (!inNoSqueeArea)
                {
                    baldicator.Rebind();
                    baldicator.Play("BjIndicator_Heared", -1, 0f);
                }
            }
        }
        else
        {
            if (AdditionalGameCustomizer.Instance.Indicator && indicator)
            {
                if (!inNoSqueeArea)
                {
                    baldicator.Rebind();
                    baldicator.Play("BjIndicator_Confused", -1, 0f);
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
    [Header("Baldi's Stats")]
    [SerializeField] private float baldiAnger;
    public float baldiTempAnger, baldiWait, baldiSpeedScale;

    [Header("Movement and Behavior")]
    [SerializeField] private float timeToMove;
    public bool stopMoving, antiHearing;

    [Header("Anger Management")]
    [SerializeField] private float angerRate;
    [SerializeField] private float slams,teleportCD,angerRateRate, angerFrequency, timeToAnger,AntiHearingDuratio = 1f;
    public bool endless;

    [Header("Audio and Animation")]
    [SerializeField] private AudioClip slap,snadtp;
    [SerializeField] private Animator baldicator, baldiAnimator;

    private float currentPriority;
    [SerializeField] private AudioSource baldiAudio;
    [SerializeField] private subsScriptableObject slamSound,snadtpsubs;
    #endregion
}