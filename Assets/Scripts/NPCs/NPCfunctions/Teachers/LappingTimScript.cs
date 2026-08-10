using UnityEngine;
using System.Collections;

public class LappingTimScript : NPC
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
        gc.laptimscr.Add(this);
    }
    public void OnDisable()
    {
        gc.laptimscr.Remove(this);
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (antiHearing)AntiHearingDuratio -= Time.deltaTime;
        if (AntiHearingDuratio < 0f) antiHearing = false;
        MOOOVEYOUBITCH();
        
        base.agentSpeed = base.DefaultAgentSpeed * base.agentSpeedScale;
        if (TimTempSpd > 0f) TimTempSpd -= 0.02f * Time.deltaTime;
        else TimTempSpd = 0f;
        if (this.isActiveAndEnabled)
        {
            agent.speed = base.agentSpeed * (TimCurSpd / 6) * (TimTempSpd + 1f);
            if (base.stun) 
            {
                agent.speed = 0;
                timObjectOkBro[0].SetActive(false);
                timObjectOkBro[1].SetActive(false);
                timObjectOkBro[2].SetActive(false);
                TimAnimator.Play("tim_Stun");
            }
            if (base.StunTime < 0f) agent.speed = base.agentSpeed * (TimCurSpd / 6)* (TimTempSpd + 1f);
        }
        StateUpdateStuffOkBr();
    }
    public override void MetalPipedEffect()
    {
        base.MetalPipedEffect();
        if (base.stun)return;
        timObjectOkBro[0].SetActive(false);
        timObjectOkBro[1].SetActive(false);
        timObjectOkBro[2].SetActive(false);
		TimAnimator.Play("tim_Idle");
    }

    public void StateUpdateStuffOkBr()
    {
        if (base.IsMetalPiped || base.stun)return;
        if (agent.speed >= 1f & agent.speed <= 20f)
		{
            timObjectOkBro[0].SetActive(true); //most inefficient and hardcoded shit ever but idfk bawebawbeawbeawbeawbeewwwebaawbabawbawe
            timObjectOkBro[1].SetActive(false);
            timObjectOkBro[2].SetActive(false);
			TimAnimator.Play("tim_Run1");
		}
		else if (agent.speed >= 21f & agent.speed <= 60f)
		{
            timObjectOkBro[0].SetActive(false);
            timObjectOkBro[1].SetActive(true);
            timObjectOkBro[2].SetActive(false);
			TimAnimator.Play("tim_Run2");
		}
		else if (agent.speed >= 61f)
		{
            timObjectOkBro[0].SetActive(false);
            timObjectOkBro[1].SetActive(false);
            timObjectOkBro[2].SetActive(true);
			TimAnimator.Play("tim_Run3");
		}
    }
    public void MOOOVEYOUBITCH()
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
    }

    protected override void TargetPlayer()
    {
        base.TargetPlayer();
        currentPriority = 0f;
        Hear(player.position, 9999,false);
    }
    #endregion
    private void OnTriggerStay(Collider play)
    {
        if (play.CompareTag("Player") & !gc.debugMode & !gc.player.titlecard)
        {
            if (base.IsHitboxValid)
			{
				gc.player.SetHP(PlayerScript.HealthChangeMode.Remove, 25 / gc.player.PlayerDmgResistance, 1f, false, true, false);
                gc.player.PushPlayer(gc.player.GetPlayerPushDirection(transform.position), 64f, 1f);
                base.Stun(4f);
                PushNpc(GetNPCPushDirection(-transform.position),64f, 1f);
			}
        }
    }

    #region Anger System
    public void GetAngry(float value)
    {
        TimCurSpd += value;

        if (TimCurSpd < 0.5f) TimCurSpd = 0.5f;
    }

    public void GetTempAngry(float value) => TimTempSpd += value;

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
            if (base.navmeshNpcPushing) base.PushedTargetPos = soundLocation;
            else agent.SetDestination(soundLocation);
            currentPriority = priority;

            if (!inNoSqueeArea && AdditionalGameCustomizer.Instance.Indicator && indicator)
            {
                TimCator.Rebind();
                TimCator.Play("timcator_Hear", -1, 0f);
            }
        }
        else
        {
            if (!inNoSqueeArea && AdditionalGameCustomizer.Instance.Indicator && indicator && !antiHearing)
            {
                TimCator.Rebind();
                TimCator.Play("timcator_Confused", -1, 0f);
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
    [Header("tim's Stats")]
    public float TimCurSpd;
    public float TimTempSpd;

    [Header("Movement and Behavior")]
    [SerializeField] private float timeToMove;
    public bool stopMoving, antiHearing;

    [Header("Anger Management")]
    [SerializeField] private float angerRate;
    [SerializeField] private float angerRateRate, angerFrequency, timeToAnger,AntiHearingDuratio = 1f;
    public bool endless;

    [Header("Audio and Animation")]
    [SerializeField] private Animator TimAnimator;
    [SerializeField] private Animator TimCator;

    private float currentPriority;
    public GameObject[] timObjectOkBro;
    #endregion
}