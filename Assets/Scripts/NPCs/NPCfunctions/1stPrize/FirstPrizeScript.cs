using UnityEngine;

public class FirstPrizeScript : NPC
{
    #region Initialization
    public override void OnStart()
    {
        base.OnStart();
        coolDown = 1f;
        Wander();
    }
    #endregion

    #region Main Update Loops
    public override void OnUpdate()
    {
        base.OnUpdate();
        NormalModel.SetActive(!base.stun);
        StunModel.SetActive(base.stun);
        base.agentSpeed = base.DefaultAgentSpeed * base.agentSpeedScale;
        runSpeed = DefaultRunspeed * base.agentSpeedScale;
        if (base.stun)
        {
            runSpeed = 0f;
            HandleLostPlayer();
        }
        if (base.StunTime < 0f)
        {
            runSpeed = DefaultRunspeed * base.agentSpeedScale;
        }
        UpdateAutoBrakeCooldown();
        UpdateRotationAndSpeed();
        HandleCrazyMode();
        UpdateAudioPitch();
        DetectSuddenStop();

        if (coolDown > 0f)
        {
            coolDown -= Time.deltaTime;
        }
        CheckForPlayer();

        if (!playerSeen)
        {
            HandleWandering();
        }
    }
    #endregion
    public override void Stun(float duration)
    {
        base.Stun(duration);
        crazyTime += duration/2;
    }
    #region Player Detection
    protected override void CheckForPlayer()
    {
        if (crazyTime > 0) return;
        
        if (transform.position.RaycastFromPosition(player.position - transform.position, out RaycastHit raycastHit))
        {
            if (transform.position.RaycastFromPosition(player.position - transform.position, out RaycastHit hitVape, QueryTriggerInteraction.UseGlobal))
            {
                if (hitVape.transform.gameObject.layer == 11) return;
            }
            if (raycastHit.transform.CompareTag("Player") && !gc.player.invisi && !gc.player.invisichalk || player.GetComponent<PlayerScript>().hugging)
            {
                HandlePlayerSeen();
            }
            else
            {
                HandleLostPlayer();
            }
        }
    }

    private void HandlePlayerSeen()
    {
        if (!playerSeen) PlayFoundAudio();

        playerSeen = true;
        TargetPlayer();
        currentSpeed = runSpeed;
        if (base.stun) currentSpeed = 0f;
        if (base.StunTime < 0f)  currentSpeed = runSpeed;
    }

    private void HandleLostPlayer()
    {
        if (coolDown <= 0f)
        {
            if (base.stun) currentSpeed = 0f;
            if (base.StunTime < 0f) currentSpeed = base.agentSpeed;
            if (playerSeen)
            {
                PlayLostAudio();
                playerSeen = false;
                Wander();
            }
        }
        else if (coolDown <= 0f && agent.remainingDistance <= 1f && !agent.pathPending) Wander();
    }
    #endregion

    #region Audio Responses
    private void PlayFoundAudio()
    {
        if (!audioDevice.audioDevice.isPlaying)
        {
            //int num = (int)Random.Range(0f, aud_Found.Length);
            //audioDevice.PlayOneShot(aud_Found[num]);
        }
    }

    private void PlayLostAudio()
    {
        if (!audioDevice.audioDevice.isPlaying)
        {
            //int num2 = (int)Random.Range(0f, aud_Lost.Length);
            //audioDevice.PlayOneShot(aud_Lost[num2]);
        }
    }

    private void PlayRandomAudio()
    {
        int num = Mathf.RoundToInt(Random.Range(0f, 9f));
        if (!audioDevice.audioDevice.isPlaying && num == 0 && coolDown <= 0f)
        {
            //int num3 = (int)Random.Range(0f, aud_Random.Length);
            //audioDevice.PlayOneShot(aud_Random[num3]);
        }
    }

    private void PlayHuggingAudio()
    {
        if (!audioDevice.audioDevice.isPlaying & !hugAnnounced)
        {
            //int num4 = (int)Random.Range(0f, aud_Hug.Length);
            //audioDevice.PlayOneShot(aud_Hug[num4]);
            hugAnnounced = true;
        }
    }
    #endregion

    #region Wandering & Movement
    protected override void Wander(string locationType = "default")
    {
        base.Wander(locationType);
        hugAnnounced = false;
        PlayRandomAudio();
        coolDown = 1f;
    }

    private void HandleWandering()
    {
        if (!playerSeen && agent.remainingDistance <= 0.5f && !agent.pathPending && coolDown <= 0f) Wander();
    }

    protected override void TargetPlayer()
    {
        if (player.GetComponent<PlayerScript>().hugging) HandleHuggingPlayer();
        else MoveTowardsPlayer();
    }

    private void HandleHuggingPlayer()
    {
        if (gc.player.jumpropes.Count > 0) gc.player.jumpropes[0].End(false);
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
        Vector3 furthestPoint = Vector3.zero;
        float maxDistance = 0f;

        foreach (Vector3 dir in directions)
        {
            float distance = (player.position + dir * 5f - transform.position).magnitude;
            if (distance > maxDistance)
            {
                maxDistance = distance;
                furthestPoint = dir;
            }
        }
        SetDestination(player.position + furthestPoint * 5f);
    }

    private void MoveTowardsPlayer()
    {
        Vector3 vectorToPlayer = player.position - transform.position;
        float magnitude = vectorToPlayer.magnitude;
        float angle = Mathf.Round(Mathf.Atan2(vectorToPlayer.z, vectorToPlayer.x) * 57.29578f / 90f) * 90f;
        Vector3 direction = new Vector3(Mathf.Cos(angle * 0.017453292f), 0f, Mathf.Sin(angle * 0.017453292f));
        SetDestination(transform.position + direction * Mathf.Max(5f, magnitude));
    }

    private void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
        coolDown = 1f;
        autoBrakeCool = 1f;
    }
    #endregion

    #region Motion and Rotation
    private void UpdateAutoBrakeCooldown()
    {
        if (autoBrakeCool > 0f) autoBrakeCool -= Time.deltaTime;
        else agent.autoBraking = true;
    }

    private void UpdateRotationAndSpeed()
    {
        angleDiff = Mathf.DeltaAngle(transform.eulerAngles.y, Mathf.Atan2(agent.steeringTarget.x - transform.position.x, agent.steeringTarget.z - transform.position.z) * 57.29578f);
        if (Mathf.Abs(angleDiff) < 5f)
        {
            agent.speed = currentSpeed;
            if (base.stun)  agent.speed = 0f;
            if (base.StunTime < 0f) agent.speed = currentSpeed;
        }
        else
        {
            transform.Rotate(new Vector3(0f, turnSpeed * Mathf.Sign(angleDiff) * Time.deltaTime, 0f));
            agent.speed = 0f;
        }
    }

    private void HandleCrazyMode()
    {
        if (crazyTime > 0f)
        {
            agent.speed = 0f;
            transform.Rotate(new Vector3(0f, 180f * Time.deltaTime, 0f));
            crazyTime -= Time.deltaTime;
        }
    }

    private void UpdateAudioPitch() => motorAudio.SetPitch((agent.velocity.magnitude + 1f) * Time.timeScale);

    private void DetectSuddenStop()
    {
        if (prevSpeed - agent.velocity.magnitude > 25f)
        {
            Stun(2f);
            if (!GameControllerScript.Instance.player.isactuallyusingboots) GameControllerScript.Instance.player.ActivateBoots(2000,false);
            foreach (basicshowWindowScript w in FindObjectsOfType<basicshowWindowScript>())  if (!w.broken) if (Vector3.Distance(this.transform.position, w.transform.position) <= 10) w.SetWindowState(true, 8f, 0f, 3);
            audioDevice.ClearQueue(true);
            audioDevice.PlaySingleClip(audBang);
            Singleton<OtherMainStuffManager>.Instance.HearingShit(8f, this.transform, new Vector3(0f,0f,0f), "all",false);
        }
        prevSpeed = agent.velocity.magnitude;
    }
    #endregion

    #region Triggers and Events
    public void GoCrazy(float duration = 15f) => Stun(duration);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (crazyTime > 0) return;
            PlayHuggingAudio();
            agent.autoBraking = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) autoBrakeCool = 1f;
    }
    #endregion

    #region Serialized Variables

    [SerializeField] private GameObject NormalModel,StunModel;
    [Header("Movement & Speed Settings")]
    [SerializeField] private float turnSpeed;
    [SerializeField] private float runSpeed,DefaultRunspeed, currentSpeed, autoBrakeCool, angleDiff;
    public float crazyTime;

    [Header("Player Interaction")]
    [SerializeField] private bool playerSeen;
    [SerializeField] private bool hugAnnounced;

    [Header("Audio Settings")]
    [SerializeField] private AudioObjectyeah audBang;
    //[SerializeField] private AudioObjectyeah[] aud_Found, aud_Lost, aud_Hug, aud_Random = new AudioObjectyeah[2];
    [SerializeField] private AudioManagerLiveReaction audioDevice, motorAudio;
    #endregion

    #region Internal State
    private float prevSpeed;
    
    #endregion
}