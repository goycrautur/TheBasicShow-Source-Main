using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PlaytimeScript : NPC
{
    #region Unity Lifecycle
    public override void OnStart()
    {
        base.OnStart();
        runnin = true;
        canTargetPlayer = true;
        audioDevice = GetComponent<AudioManagerLiveReaction>();
        Wander();
    }
    private IEnumerator PlayCoolFunctionality()
    {
        while (playCool >= 0f)
        {
            playCool -= Time.deltaTime;
            yield return null;
        }
        while (animator.GetBool("disappointed"))
        {
            playCool = 0f;
            animator.SetBool("disappointed", false);
            yield return null;
        }
        yield break;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!runnin) base.agentSpeed = base.DefaultAgentSpeed * base.agentSpeedScale;
        else base.agentSpeed = DefaultRunSpeed * base.agentSpeedScale;
        agent.speed = base.agentSpeed;
        if (base.stun)
        {
            agent.speed = 0f;
            playerSeen = false;
            if (thosewhojumprope != null) thosewhojumprope.End(false);
        }
        if (base.StunTime < 0f) agent.speed = base.agentSpeed;
        if (!jumpRopeStarted)
        {
            if (!playerSeen && agent.velocity.magnitude <= 1f && coolDown <= 0f) Wander();
            jumpRopeStarted = false;
        }
        else jumpropeRaycastCheck();
    }
    #endregion

    #region Player Detection & Targeting
    protected override void CheckForPlayer()
    {
        if (transform.position.RaycastFromPositionWithDistance(player.position - transform.position, out RaycastHit raycastHit, 50f))
        {
            bool playerInRange = (transform.position - player.position).magnitude <= 50f;

            if (transform.position.RaycastFromPositionWithDistance(player.position - transform.position, out RaycastHit hitVape, 50f, QueryTriggerInteraction.UseGlobal)) if (hitVape.transform.gameObject.layer == 11)  return;
            if (raycastHit.transform.CompareTag("Player") && playerInRange && playCool <= 0f && !ps.invisi && !ps.invisichalk)
            {
                playerSeen = true;
                TargetPlayer();
            }
            
            else if (playerSeen && coolDown <= 0f)
            {
                playerSeen = false;
                Opposition();
                runnin = false;
                Wander();
            }
        }
        
    }
    public void jumpropeRaycastCheck()
    {
        if (transform.position.RaycastFromPositionWithDistance(player.position - transform.position, out RaycastHit raycastHit, 50f))
        {
            bool playerInRange = (transform.position - player.position).magnitude <= 50f;
            if (transform.position.RaycastFromPositionWithDistance(player.position - transform.position, out RaycastHit hitVape, 50f, QueryTriggerInteraction.UseGlobal))
            {
                if (hitVape.transform.gameObject.layer == 11) 
                {
                    if (thosewhojumprope != null) thosewhojumprope.End(false);
                    return;
                }
            }
            if (raycastHit.transform.CompareTag("Player") && playerInRange && !gc.player.invisi && !gc.player.invisichalk)
            {
            }
            else if (thosewhojumprope != null) thosewhojumprope.End(false);
        }
    }

    protected override void TargetPlayer()
    {
        animator.SetBool("disappointed", false);
        
        agent.SetDestination(player.position);
        agent.stoppingDistance = 2f;
        agent.angularSpeed = 200f;
        agent.updateRotation = true;

        runnin = true;
        coolDown = 0.2f;

        if (!playerSpotted)
        {
            playerSpotted = true;
            if (!audioDevice.audioDevice.isPlaying)
            {
                audioDevice.ClearQueue(true);
                audioDevice.QueueAudio(aud_LetsPlay);
                //UIPopupTextManagerWithMovement.Show("MemeBoiLines_1", Color.red, base.transform, aud_LetsPlay.length, 0f);
            }
        }
    }

    private void Opposition()
    {
        Vector3 directionAway = transform.position - (player.position - transform.position).normalized * 500f;
        if (NavMesh.SamplePosition(directionAway, out NavMeshHit navMeshHit, 5f, NavMesh.AllAreas)) agent.SetDestination(navMeshHit.position);
    }
    #endregion

    #region Wandering
    protected override void Wander(string locationType = "hall")
    {
        if (disablingWandering) return;
        base.Wander(locationType);

        agent.stoppingDistance = 1f;
        agent.angularSpeed = 180f;
        agent.updateRotation = true;
        runnin = false;

        playerSpotted = false;

        int audVal = UnityEngine.Random.Range(0, aud_Random.Length);
        if (!audioDevice.audioDevice.isPlaying)
        {
            audioDevice.ClearQueue(true);
            audioDevice.QueueAudio(aud_Random[audVal]);
            //UIPopupTextManagerWithMovement.Show(aud_Random[audVal].captionsTextDatarea, aud_Random[audVal].holor, base.transform, aud_Random[audVal].audios.length, 0f);
        }

        ResetCooldown();
    }
    #endregion

    #region Jump Rope State Handling
    private IEnumerator StartPlaying()
    {
        ps.isForcedToLook = true;
        canTargetPlayer = false;
        disablingWandering = true;
        runnin = false;
        Vector3 moveBackPosition = transform.position - transform.forward * 10f;
        agent.SetDestination(moveBackPosition);
        agent.speed = 16;
        while (Vector3.Distance(transform.position, moveBackPosition) > 1f) yield return null;
        agent.speed = 0f;
        yield break;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playCool <= 0f && !jumpRopeStarted && base.IsHitboxValid)
        {
            if (!ps.invisi && !ps.invisichalk || !ps.invisichalk && !ps.invisi)
			{
                audioDevice.ClearQueue(true);
                audioDevice.QueueAudio(aud_ReadyGo);
                jumpRopeStarted = true;
                JumpRopeScript jumprope = Instantiate(jumpRopeGame);
                jumprope.jumpRopTime(this);
                thosewhojumprope = jumprope;
                ps.jumpropes.Add(jumprope);
                if (playingRoutine != null) StopCoroutine(playingRoutine);
                playingRoutine = StartCoroutine(StartPlaying());
            }
        }
        else if (other.CompareTag("Player") && playCool <= 0f && !jumpRopeStarted && !base.IsHitboxValid) Disappoint();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && ps.jumpropes.Count > 0)
        {
            Collider component = GetComponent<Collider>();
            component.enabled = false;
            component.enabled = true;
        }
    }

    public void Disappoint()
    {
        if (!animator.GetBool("disappointed"))
        {
            Wander();
            runnin = false;
            animator.SetBool("disappointed", true);
            canTargetPlayer = true;
			jumpRopeStarted = false;
			playCool = 15f;
            StartCoroutine(PlayCoolFunctionality());
            audioDevice.ClearQueue(true);
            audioDevice.QueueAudio(aud_Sad);
        }
    }
    #endregion

    #region Movement Override
    protected override void HandleMovement()
    {
        base.HandleMovement();
    }
    #endregion
    private float playCool;

    public float PlayCool
    {
        get
        {
            return playCool;
        }

        set
        {
            if (playCool != value) StartCoroutine(PlayCoolFunctionality());
            playCool = value;
        }
    }

    #region Serialized Field States
    [Header("Movement Speeds")]
    [SerializeField] private float DefaultRunSpeed = 20f;
    [SerializeField] private bool runnin;
    [Header("Player and Movement")]
    [SerializeField] private PlayerScript ps;
    [Header("Jumprope"), SerializeField] private JumpRopeScript jumpRopeGame;
    public Transform eeek;

    [Header("Audio and Animations")]
    [SerializeField] private Animator animator;
    public AudioManagerLiveReaction audioDevice;
    [SerializeField] private AudioObjectyeah[] aud_Random;
    public AudioObjectyeah[] aud_Numbers;
    [SerializeField] private AudioObjectyeah aud_LetsPlay, aud_Sad;
    public AudioObjectyeah aud_Congrats, aud_ReadyGo, aud_Oops;
    private bool playerSeen, playerSpotted;
    public bool dontUpdateTheSpeedYOUFUCKINGBITCH,disablingWandering;
    private Coroutine playingRoutine;
    [HideInInspector] public bool jumpRopeStarted;
    [HideInInspector] public JumpRopeScript thosewhojumprope;
    #endregion
}