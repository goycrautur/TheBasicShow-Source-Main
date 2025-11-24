using UnityEngine;
using System.Collections;

public class FamishedScript : NPC
{
    #region Unity Lifecycle
    public override void OnStart()
    {
        base.OnStart();
        baldiAudio = GetComponent<AudioSource>();
        GetAngry(0f);
        if (endless)
        {
            Endless();
        }

        Wander();
    }
    public void OnEnable()
    {
        gc.famishscr.Add(this);
    }
    public void OnDisable()
    {
        gc.famishscr.Remove(this);
    }

    public override void OnUpdate()
    {
        MOOOVEYOUBITCH();
        base.OnUpdate();
        base.agentSpeed = !gc.fmc.isAbleToMove ? 0 : base.DefaultAgentSpeed * base.agentSpeedScale;
        if (famishedtempSpd > 0f)
        {
            famishedtempSpd -= 0.02f * Time.deltaTime;
        }
        else
        {
            famishedtempSpd = 0f;
        }
        if (activatewindowbreak)
        {
            foreach (WindowScript w in FindObjectsOfType<WindowScript>())
            {
                w.enableOffMeshScript = true;
                if (!w.broken)
                {
                    if (Vector3.Distance(transform.position, w.transform.position) <= 10)
                    {
                        w.Window(true, false, 0f);
                    }
                }
            }
        }
        if (this.isActiveAndEnabled)
        {
            agent.speed = base.agentSpeed * (famishedSpd / 2) * gc.fmc.angerMultipler;
        }
    }
    public void MOOOVEYOUBITCH()
    {
        if (player == null) return;
        if (player != null && agent.enabled && gameObject.activeSelf && gc.fmc.alwaysKnowIp)
        {
            agent.SetDestination(player.position);
        }

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
    #endregion
    private void OnTriggerStay(Collider play)
    {
        if (play.CompareTag("Player") & !gc.debugMode & !gc.player.titlecard)
        {
            if (!base.squished)
			{
				gc.player.SetHP(PlayerScript.HealthChangeMode.Remove, 50 / gc.player.PlayerDmgResistance, 1.5f, false, true, false);
				gc.player.killedbyfamished = true;
			}
        }
    }

    #region Anger System
    public void GetAngry(float value)
    {
        famishedSpd += value;

        if (famishedSpd < 0.5f)
        {
            famishedSpd = 0.5f;
        }
    }

    public void GetTempAngry(float value) => famishedtempSpd += value;

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
                famishedcator.Rebind();
                famishedcator.Play("famishedicator_Heared", -1, 0f);
            }
        }
        else
        {
            if (!inNoSqueeArea && AdditionalGameCustomizer.Instance.Indicator && indicator)
            {
                famishedcator.Rebind();
                famishedcator.Play("famishedicator_Confused", -1, 0f);
            }
        }
    }

    public void ActivateAntiHearing(float SetTime) => StartCoroutine(SetHearingTimer(SetTime));

    private IEnumerator SetHearingTimer(float Timer)
    {
        Wander();
        antiHearing = true;
        yield return new WaitForSeconds(Timer);
        antiHearing = false;
    }
    #endregion

    #region Serialized Field States
    [Header("Baldi's Stats")]
    public float famishedSpd;
    public float famishedtempSpd;

    [Header("Movement and Behavior")]
    [SerializeField] private float timeToMove;
    public bool stopMoving, antiHearing;

    [Header("Anger Management")]
    [SerializeField] private float angerRate;
    [SerializeField] private float angerRateRate, angerFrequency, timeToAnger;
    public bool endless;

    [Header("Audio and Animation")]
    [SerializeField] private Animator famishedcator;

    private float currentPriority;
    private AudioSource baldiAudio;
    public bool activatewindowbreak;
    #endregion
}