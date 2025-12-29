using UnityEngine;
using System.Collections;

public class MaxcipalScript : NPC
{
    #region NPC Lifecycle
    public override void OnStart()
    {
        base.OnStart();
        AudioDevice = GetComponent<AudioSource>();
        Wander();
    }
    public void OnEnable()
    {

        gc.maxiScr.Add(this);
    }
    public void OnDisable()
    {
        gc.maxiScr.Remove(this);
    }

    public override void OnUpdate()
    {
        base.agentSpeed = base.DefaultAgentSpeed * base.agentSpeedScale;
        agent.speed = movin ? base.agentSpeed * PrinSpeedMult : 0;
        PrinSpeedMult = OGPrinSpeedMult;
        if (base.stun)
        {
            PrinSpeedMult = 0f;
        }
        if (base.StunTime < 0f)
        {
            PrinSpeedMult = OGPrinSpeedMult;
        }
        base.OnUpdate();
        if (seesRuleBreak)
        {
            HandleRuleBreaking();
        }
        else if (timeSeenRuleBreak > 0f)
        {
            timeSeenRuleBreak -= 0.5f * Time.deltaTime;
        }
        if (gauge != null)
        {
            if (officeDoor.lockTime > 0f)
            {
                gauge.Set(maxGaugeLockTime, officeDoor.lockTime);
            }
            if (officeDoor.lockTime <= 0f)
            {
                gauge.Hide();
            }
        }
        detectionStuff();
        /*{
            foreach (WindowScript w in FindObjectsOfType<WindowScript>())
            {
                if (w.broken)
                {
                    if (Vector3.Distance(prin.position, w.transform.position) <= 10)
                    {
                        w.Window(false,false, 0f);
                    }
                }
            }
        }*/
    }

    public void detectionStuff()
    {
        if (!angry)
        {
            HandlePlayerDetection();

            if (!seesRuleBreak)
            {
                HandleBullyDetection();
            }
        }
        else
        {
            HandlePlayerTargeting();
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!angry && !seesRuleBreak && !inOffice && !brogotcalled)
            {
                Wander();
            }
        }
    }
    #endregion

    #region Rule and Guilt Handling
    private void HandleRuleBreaking()
    {
        timeSeenRuleBreak += Time.deltaTime;

        float ruleBreakThreshold = playerScript.guiltType == "escape" ? 0f : playerScript.guiltType == "faculty" ? 0f : playerScript.guiltType == "running" ? 0.25f : 0.35f;
        audioQueue.ClearQueue();
        if (timeSeenRuleBreak >= ruleBreakThreshold && !angry)
        {
            angry = true;
            seesRuleBreak = false;
            timeSeenRuleBreak = 0f;
            ruleBreakObservationTime = 0f;
            TargetPlayer();
            CorrectPlayer();
        }
    }

    private void HandlePlayerDetection()
    {
        aim = player.position - transform.position;
        if (transform.position.RaycastFromPosition(aim, out hit))
        {
            if (transform.position.RaycastFromPosition(aim, out RaycastHit hitVape, QueryTriggerInteraction.UseGlobal))
            {
                if (hitVape.transform.gameObject.layer == 11) return;
            }
            if (hit.transform.CompareTag("Player") && playerScript.guilt > 0f && !playerScript.alsoInOffice && !gc.player.invisi && !gc.player.invisichalk)
            {
                ruleBreakObservationTime += Time.deltaTime;

                if (ruleBreakObservationTime >= 0.5f)
                {
                    seesRuleBreak = true;
                }
            }
            else
            {
                ruleBreakObservationTime = 0f;
            }
        }
        else
        {
            ruleBreakObservationTime = 0f;
        }
    }

    private void HandlePlayerTargeting()
    {
        aim = player.position - transform.position;
        if (transform.position.RaycastFromPosition(aim, out hit))
        {
            if (transform.position.RaycastFromPosition(aim, out RaycastHit hitVape, QueryTriggerInteraction.UseGlobal))
            {
                if (hitVape.transform.gameObject.layer == 11) return;
            }
            if (hit.transform.CompareTag("Player") && !gc.player.invisi && !gc.player.invisichalk)
            {
                TargetPlayer();
            }
            else
            {
                LosePlayer();
            }
        }
        else
        {
            LosePlayer();
        }
    }

    private void LosePlayer()
    {
        if (angry && !agent.isStopped)
        {
            WanderWithAnger();
        }
    }

    private void HandleBullyDetection()
    {
        foreach (BullyScript bul in GameControllerScript.Instance.buliScr)
        {
            aim = bul.transform.position - transform.position;
            if (transform.position.RaycastFromPosition(aim, out hit, QueryTriggerInteraction.UseGlobal))
            {
                if (hit.transform.CompareTag("bully") && hit.transform.GetComponent<BullyScript>().guilt > 0f && !inOffice)
                {
                    TargetBully(hit.transform.GetComponent<BullyScript>().transform);
                }
            }
        }
    }
    #endregion

    #region Movement & Navigation
    protected override void Wander(string locationType = "default")
    {
        brogotcalled = false;
        OGPrinSpeedMult = 1f;
        playerScript.principalBugFixer = 1;
        base.Wander(locationType);
        if (agent.isStopped)
        {
            agent.isStopped = false;
        }
        if (bullySeen)
        {
            bullySeen = false;
        }
        ResetCooldown();
        if (Random.Range(0f, 10f) <= 1f)
        {
            if (!AudioDevice.isPlaying)
            {
                AudioDevice.PlayOneShot(aud_Whistle);
                GameControllerScript.Instance.SubsManager.summonLeSubtitle(whistleCaptions.subtitleOption, whistleCaptions, GetComponent<AudioSource>());
            }
        }
    }

    private void WanderWithAnger()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Wander();
        }
    }

    protected override void TargetPlayer()
    {
        brogotcalled = false;
        OGPrinSpeedMult = 1f;
        base.TargetPlayer();
        movin = true;
    }

    private void TargetBully(Transform tranfo)
    {
        if (!bullySeen)
        {
            brogotcalled = false;
            OGPrinSpeedMult = 1f;
            agent.SetDestination(tranfo.position);
            audioQueue.QueueAudio(audNoBullying,noBullyingCaptions);
            movin = true;
            bullySeen = true;
        }
    }
    #endregion

    #region Detention System
    public IEnumerator CheckTheDoor()
    {
        coolDown = 3;
        movin = false;
        yield return new WaitForSeconds(2);
        movin = true;
        onFaculty = true;
    }

    private void CorrectPlayer()
    {
        audioQueue.ClearQueue();
        switch (playerScript.guiltType)
        {
            case "faculty":
                audioQueue.QueueAudio(audNoFaculty, noFacultyCaptions);
                break;
            case "running":
                audioQueue.QueueAudio(audNoRunning, noRunningCaptions);
                break;
            case "drink":
                audioQueue.QueueAudio(audNoDrinking, noDrinkingCaptions);
                break;
            case "escape":
                audioQueue.QueueAudio(audNoEscaping, noEscapingCaptions);
                break;
            case "eat":
                audioQueue.QueueAudio(audNoEating, noEatingCaptions);
                break;
            case "bully":
                audioQueue.QueueAudio(audNoBullying, noBullyingCaptions);
                break;
            case "destroyingproperty":
                audioQueue.QueueAudio(audNoDestroying, noDestroyingCaptions);
                break;
        }
    }

    public void GiveDetention(Transform target,bool isitPlayer)
    {
        if (isitPlayer)
        {
            playerScript.alsoInOffice = true;
            if (playerScript.hugging)
            {
                playerScript.hugging = false;
                playerScript.sweepingFailsave = 0f;
                target.transform.SetParent(null);
            }
            if (playerScript.jumpropes.Count > 0)
            {
                playerScript.jumpropes[0].End(false);
                target.transform.SetParent(null);
            }
            playerScript.guilt = 0f;
            playerScript.principalBugFixer = 0;
        }
        officeDoor.openTime = 0f;
            inOffice = true;
        agent.isStopped = true;
        cc.enabled = false;
        Vector3 vector = new Vector3(point.position.x, target.position.y, point.position.z);
        cc.enabled = true;
        target.transform.position = vector;
        agent.Warp(vector + target.forward * 10f);
        audioQueue.QueueAudio(audTimes[detentions], subtitlthingballs[detentions]);
        audioQueue.QueueAudio(audDetention, detentionCaptions);
        int num = (int)Random.Range(0f, audScolds.Length);
        audioQueue.QueueAudio(audScolds[num],scoldsubs[num]);
        officeDoor.LockDoor(lockTime[detentions]);
        maxGaugeLockTime = lockTime[detentions];
        if (gauge == null)
        {
            gauge = GaugeManager.Instance.CreateGaugeInstance(gaugeDetentionSprite, maxGaugeLockTime);
        }
        Singleton<OtherMainStuffManager>.Instance.HearingShit(9f, this.transform, new Vector3(0f,0f,0f), "all",false);

        if (officeDoor.lockTime <= 99f)
        {
            coolDown = 3f;
        }
        else
        {
            coolDown = 10f;
        }
        angry = false;
        seesRuleBreak = false;
        detentions++;
        if (detentions > lockTime.Length - 1)
        {
            detentions = lockTime.Length - 1;
        }
        
        StartCoroutine(QuickDelay());
    }

    public IEnumerator QuickDelay()
    {
        yield return new WaitForSeconds(3.5f);
        agent.isStopped = false;
        inOffice = false;
        Wander();
    }
    #endregion

    #region Trigger Events
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" & summon)
        {
            if (base.IsHitboxValid)
            {
            summon = false;
            movin = true;
            }
            movin = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "OfficeTrigger")
        {
            inOffice = true;
        }
        if (other.CompareTag("Player") && angry)
        {
            if (base.IsHitboxValid)
            {
                scoreSystemManager.Instance.AddScore(-500);
                GiveDetention(other.transform, true);
            }
            else if (!base.IsHitboxValid)
            {
                GiveDetention(nonexistance.transform,false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "OfficeTrigger")
        {
            inOffice = false;
        }
        if (other.CompareTag("bully"))
        {
            bullySeen = false;
        }
    }
    #endregion
    public void callToSMTH(Vector3 tranfo)
    {
        brogotcalled = true;
        OGPrinSpeedMult = 6f;
	    agent.SetDestination(tranfo);
    }

    #region Serialized Field States
    [Header("Player and Bully Detection")]
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private Transform point, prin;
    public bool angry, onFaculty;

    [Header("Audio and Feedback")]
    [SerializeField] private AudioQueueScript audioQueue;
    [SerializeField] private AudioClip[] audTimes, audScolds;
    [SerializeField] private subsScriptableObject[] subtitlthingballs, scoldsubs;
    [SerializeField] private AudioClip audDetention, audNoDrinking, audNoEating, audNoBullying, audNoFaculty, audNoRunning, audNoEscaping, audNoDestroying, aud_Whistle;
    [SerializeField] private subsScriptableObject detentionCaptions,noDrinkingCaptions,noEatingCaptions, noBullyingCaptions, noFacultyCaptions, noRunningCaptions, noEscapingCaptions, noDestroyingCaptions, whistleCaptions;

    [Header("Office and Detention")]
    [SerializeField] private DoorScript officeDoor;
    [SerializeField] private CharacterController cc;

    [Header("References")]
    [SerializeField] private Sprite gaugeDetentionSprite;

    public int detentions;
    public float maxGaugeLockTime, ruleBreakObservationTime, timeSeenRuleBreak, OGPrinSpeedMult = 1f, PrinSpeedMult = 1f;
    [SerializeField] private int[] lockTime = { 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 99 };
    public AudioSource AudioDevice;
    public bool summon, seesRuleBreak, bullySeen, inOffice,brogotcalled;
    public RaycastHit hit;
    public Vector3 aim;
    public Gauge gauge;
    [SerializeField] private Transform nonexistance;
    [SerializeField] private bool movin;
    #endregion
}