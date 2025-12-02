using UnityEngine;
using System.Collections;

public class BaldiScript : NPC
{
    #region Unity Lifecycle
    public override void OnStart()
    {
        base.OnStart();
        baldiAudio = GetComponent<AudioSource>();
        GetAngry(0f);
        Move();

        if (endless)
        {
            Endless();
        }

        Wander();
    }
    public void OnEnable()
    {
        Move();
        gc.balscr.Add(this);
    }
    public void OnDisable()
    {
        gc.balscr.Remove(this);
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
        base.OnUpdate();
        base.agentSpeed = base.DefaultAgentSpeed * base.agentSpeedScale;
        if (baldiTempAnger > 0f)
        {
            baldiTempAnger -= 0.05f * Time.deltaTime;
        }
        else
        {
            baldiTempAnger = 0f;
        }
    }

    public override void OnFixedUpdate()
    {
        if (player == null) return;

        if ((transform.position + Vector3.up * 2f).RaycastFromPosition(player.position - transform.position, out RaycastHit raycastHit))
        {
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
            agent.speed = base.agentSpeed;
            gc.SubsManager.summonLeSubtitle(slapSound.subtitleOption, slapSound, 0f, baldiAudio);
            baldiAudio.PlayOneShot(slap);
            baldiAnimator.SetTrigger("slap");

            if (!stopMoving)
            {
                Invoke(nameof(OnMoveDone), timeToMove);
            }
            resetWaitTime();
        }
    }
    public void resetWaitTime()
    {
        baldiWait = (-3 - baldiTempAnger) * baldiAnger / (baldiAnger + 2f / baldiSpeedScale) + 3f;
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
    #endregion
    private void OnTriggerStay(Collider play)
    {
        if (play.CompareTag("Player") & !gc.debugMode & !gc.player.titlecard)
        {
            if (!base.IsHitboxValid)
			{
				gc.player.SetHP(PlayerScript.HealthChangeMode.Remove, 30 / gc.player.PlayerDmgResistance, 2f, false, true, false);
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

            if (!inNoSqueeArea && AdditionalGameCustomizer.Instance.Indicator && indicator)
            {
                baldicator.Rebind();
                baldicator.Play("Indicator_Heared", -1, 0f);
            }
        }
        else
        {
            if (!inNoSqueeArea && AdditionalGameCustomizer.Instance.Indicator && indicator)
            {
                baldicator.Rebind();
                baldicator.Play("Indicator_Confused", -1, 0f);
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
    [SerializeField] private float baldiAnger;
    public float baldiTempAnger, baldiWait, baldiSpeedScale;

    [Header("Movement and Behavior")]
    [SerializeField] private float timeToMove;
    public bool stopMoving, antiHearing;

    [Header("Anger Management")]
    [SerializeField] private float angerRate;
    [SerializeField] private float angerRateRate, angerFrequency, timeToAnger,AntiHearingDuratio = 1f;
    public bool endless;

    [Header("Audio and Animation")]
    [SerializeField] private AudioClip slap;
    [SerializeField] private Animator baldicator, baldiAnimator;
    [SerializeField] private GameObject sprite;

    private float currentPriority;
    private AudioSource baldiAudio;
    [SerializeField] private subsScriptableObject slapSound;
    #endregion
}