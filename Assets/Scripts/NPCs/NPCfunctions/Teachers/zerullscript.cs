using UnityEngine;
using System.Collections;

public class zerullscript : NPC
{
    public bool offmesh;
    #region Unity Lifecycle
    public override void OnStart()
    {
        bool chair = PlayerPrefsExtension.GetBool("BeatedUpZerull");
        if (chair) normalSprite.sprite = ChairSprite;
        base.OnStart();
        zeraudio = GetComponent<AudioManagerLiveReaction>();
        GetAngry(0f);
        Move();
        if (endless) Endless();
        Wander();
    }
    public override void OnEnable()
    {
        base.OnEnable();
        gc.zerscr.Add(this);
        Move();
    }
    public override void OnDisable()
    {
        base.OnDisable();
        gc.zerscr.Remove(this);
    }

    public override void OnUpdate()
    {
        if (antiHearing)AntiHearingDuratio -= Time.deltaTime;
        if (AntiHearingDuratio < 0f) antiHearing = false;
        targe();
        base.OnUpdate();
        base.agentSpeed = base.DefaultAgentSpeed * base.agentSpeedScale;
        if (TempAnger > 0f) TempAnger -= 0.02f * Time.deltaTime;
        else TempAnger = 0f;
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
        foreach (basicshowWindowScript w in FindObjectsOfType<basicshowWindowScript>()) 
        {
            w.enableOffMeshScript = offmesh;
            if (!w.broken) if (Vector3.Distance(this.transform.position, w.transform.position) <= 10) w.SetWindowState(false, 0f, 0f, 0, true, 0);
        }
    }
    public void targe()
    {
        if (player == null) return;
        if ((transform.position + Vector3.up * 2f).RaycastFromPosition(player.position - transform.position, out RaycastHit raycastHit))
        {
            if ((transform.position + Vector3.up * 2f).RaycastFromPosition(player.position - transform.position, out RaycastHit hitVape, QueryTriggerInteraction.UseGlobal))
            {
                if (hitVape.transform.gameObject.layer == 11) return;
            }
            if (raycastHit.transform.CompareTag("Player") && !gc.player.invisi && !gc.player.invisichalk) TargetPlayer();
        }
    }
    #endregion

    #region Movement
    protected override void Wander(string locationType = "default")
    {
        base.Wander(locationType);
        currentPriority = 0f;
        offmesh = false;
    }

    protected override void TargetPlayer()
    {
        base.TargetPlayer();
        currentPriority = 0f;
        offmesh = true;
        Hear(player.position, 9999, false);
    }

    public void Move()
    {
        if (isActiveAndEnabled)
        {
            if (Wait < 30f) agent.speed = base.agentSpeed;
            if (Wait > 30f) agent.speed = base.agentSpeed * (Wait/30);
            if (!stopMoving) Invoke(nameof(OnMoveDone), timeToMove);
            resetWaitTime();
        }
    }
    public void resetWaitTime()
    {
        Wait = (-3 - TempAnger) * Anger / (Anger+3 / SpeedScale) + 3;
    }

    private void OnMoveDone()
    {
        agent.speed = 0;
        if (agent.remainingDistance <= 0.1f) Wander();
        if (!stopMoving) Invoke(nameof(Move), Wait);
    }
    #endregion
    private void OnTriggerStay(Collider play)
    {
        if (play.CompareTag("Player") & !gc.debugMode & !gc.player.titlecard)
        {
            if (base.IsHitboxValid)
			{
				gc.player.SetHP(PlayerScript.HealthChangeMode.Remove, 50 / gc.player.PlayerDmgResistance, 0.75f, false, true, false);
				gc.player.killedbyhim = true;
			}
        }
    }

    #region Anger System
    public void GetAngry(float value)
    {
        Anger += value;

        if (Anger < 0.5f) Anger = 0.5f;
    }

    public void GetTempAngry(float value) => TempAnger += value;

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
                    erucato.Rebind();
                    erucato.Play("Indicator_Heared", -1, 0f);
                }
            }
        }
        else
        {
            if (AdditionalGameCustomizer.Instance.Indicator && indicator)
            {
                if (!inNoSqueeArea)
                {
                    erucato.Rebind();
                    erucato.Play("Indicator_Confused", -1, 0f);
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
    public float Anger;
    public float TempAnger, Wait, SpeedScale;

    [Header("Movement and Behavior")]
    [SerializeField] private float timeToMove;
    public bool stopMoving, antiHearing;

    [Header("Anger Management")]
    [SerializeField] private float angerRate;
    [SerializeField] private float angerRateRate, angerFrequency, timeToAnger,AntiHearingDuratio = 1f;
    public bool endless;

    [Header("Audio and Animation")]
    [SerializeField] private Animator erucato;

    private float currentPriority;
    private AudioManagerLiveReaction zeraudio;
    [SerializeField] private SpriteRenderer normalSprite;
    [SerializeField] private Sprite ChairSprite;
    #endregion
}