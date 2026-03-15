using UnityEngine;
using System.Collections;

public class TeacherJerScript : NPC
{
    #region Unity Lifecycle
    public override void OnStart()
    {
        base.OnStart();
        jerAudio = GetComponent<AudioManagerLiveReaction>();
        GetAngry(0f);
        Move();
        if (endless) Endless();

        Wander();
    }
    /*public void OnEnable()
    {
        Move();
        gc.balscr.Add(this);
    }
    public void OnDisable()
    {
        gc.balscr.Remove(this);
    }*/

    public override void OnUpdate()
    {
        if (antiHearing)AntiHearingDuratio -= Time.deltaTime;
        if (AntiHearingDuratio < 0f) antiHearing = false;
        if (SlashCoolDown > 0f) SlashCoolDown -= Time.deltaTime; 
        if (base.stun)
        {
            stopMoving = true;
            agent.speed = 0;
        }
        if (base.StunTime < 0f)
        {
            stopMoving = false;
            resetWaitTime();
            SwitchState();
            //Move();
        }
        base.OnUpdate();
        base.agentSpeed = base.DefaultAgentSpeed * base.agentSpeedScale;
        if (jerTempAnger > 0f) jerTempAnger -= 0.05f * Time.deltaTime;
        else jerTempAnger = 0f;
    }

    public override void OnFixedUpdate()
    {
        if (player == null) return;
        if ((transform.position + Vector3.up * 2f).RaycastFromPosition(player.position - transform.position, out RaycastHit raycastHit))
        {
            if ((transform.position + Vector3.up * 2f).RaycastFromPosition(player.position - transform.position, out RaycastHit hitVape, QueryTriggerInteraction.UseGlobal))
            {
                if (hitVape.transform.gameObject.layer == 11) return;
            }
            if (raycastHit.transform.CompareTag("Player") && !gc.player.invisi && !gc.player.invisichalk)  TargetPlayer();
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
    public void SwitchState()
    {
        
    }

    public void Move()
    {
        if (this.isActiveAndEnabled)
        {
            agent.speed = base.agentSpeed;
            jerAudio.PlaySingleClip(slash);
            jerAnimator.SetTrigger("slap");
            if (!stopMoving)  Invoke(nameof(OnMoveDone), timeToMove);
            resetWaitTime();
        }
    }
    public void resetWaitTime()
    {
        jerWait = (-3 - jerTempAnger) * jerAnger / (jerAnger + 2f / jerSpeedScale) + 3f;
    }


    private void OnMoveDone()
    {
        agent.speed = 0;
        if (agent.remainingDistance <= 0.1f)  Wander();
        if (!stopMoving) Invoke(nameof(Move), jerWait);
    }
    #endregion
    private void OnTriggerStay(Collider play)
    {
        if (play.CompareTag("Player") & !gc.debugMode & !gc.player.titlecard) if (!base.IsHitboxValid) Slashing();
    }
    private void Slashing()
    {
        if (SlashCoolDown > 0f) return;
        SlashCoolDown = 0.5f/(jerAnger-1);
        gc.player.SetHP(PlayerScript.HealthChangeMode.Remove, 20 / gc.player.PlayerDmgResistance, 2f, false, true, false);
		gc.player.killedbybaldi = true;
        jerAnimator.SetTrigger("slap");
    }


    #region Anger System
    public void GetAngry(float value)
    {
        jerAnger += value;

        if (jerAnger < 0.5f) jerAnger = 0.5f;
    }

    public void GetTempAngry(float value) => jerTempAnger += value;

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
                if (!antiHearing || !inNoSqueeArea)
                {
                    tjerCator.Rebind();
                    tjerCator.Play("Indicator_Heared", -1, 0f);
                }
            }
        }
        else
        {
            if (AdditionalGameCustomizer.Instance.Indicator && indicator)
            {
                if (!antiHearing || !inNoSqueeArea)
                {
                    tjerCator.Rebind();
                    tjerCator.Play("Indicator_Confused", -1, 0f);
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

    #region Serialized Field States
    [Header("Baldi's Stats")]
    [SerializeField] private float jerAnger;
    public float jerTempAnger, jerWait, jerSpeedScale;

    [Header("Movement and Behavior")]
    [SerializeField] private float timeToMove;
    public bool stopMoving, antiHearing;

    [Header("Anger Management")]
    [SerializeField] private float angerRate;
    [SerializeField] private float angerRateRate, angerFrequency, timeToAnger,AntiHearingDuratio = 1f,SlashCoolDown;
    public bool endless;

    [Header("Audio and Animation")]
    [SerializeField] private AudioObjectyeah slash;
    [SerializeField] private Animator tjerCator, jerAnimator;
    [SerializeField] private GameObject sprite;

    private float currentPriority;
    private AudioManagerLiveReaction jerAudio;
    [SerializeField] private subsScriptableObject slashSound;
    #endregion
}