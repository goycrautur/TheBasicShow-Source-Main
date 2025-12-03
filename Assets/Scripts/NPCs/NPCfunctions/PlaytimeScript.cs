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
        audioDevice = GetComponent<AudioSource>();
        Wander();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (ps.jumpRope && Vector3.Distance(transform.position, ps.transform.position) >= 25f)
        {
            ps.DeactivateJumpRope();
            Disappoint();
        }

        if (playCool >= 0f)
        {
            playCool -= Time.deltaTime;
        }
        else if (animator.GetBool("disappointed") && !ps.jumpRope)
        {
            playCool = 0f;
            animator.SetBool("disappointed", false);
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!runnin)
        {
            agent.speed = base.DefaultAgentSpeed * base.agentSpeedScale;
        }
        if (runnin)
        {
            agent.speed = DefaultRunSpeed * base.agentSpeedScale;
        }
        if (base.stun)
        {
            agent.speed = 0f;
        }
        if (base.StunTime < 0f)
        {
            agent.speed = base.agentSpeed;
        }
        if (!ps.jumpRope)
        {
            if (!playerSeen && agent.velocity.magnitude <= 1f && coolDown <= 0f)
            {
                Wander();
            }
            jumpRopeStarted = false;
        }
        else if (!jumpRopeStarted)
        {
            if (playingRoutine != null)
            {
                StopCoroutine(playingRoutine);
            }
            playingRoutine = StartCoroutine(StartPlaying());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !ps.jumpRope)
        {
            Collider component = GetComponent<Collider>();
            component.enabled = false;
            component.enabled = true;
        }
    }
    #endregion

    #region Player Detection & Targeting
    protected override void CheckForPlayer()
    {
        if (transform.position.RaycastFromPositionWithDistance(player.position - transform.position, out RaycastHit raycastHit, 30f))
        {
            bool playerInRange = (transform.position - player.position).magnitude <= 30f;

            if (raycastHit.transform.CompareTag("Player") && playerInRange && playCool <= 0f && !gc.player.invisi && !gc.player.invisichalk && !gc.player.jumpRope)
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
            if (!audioDevice.isPlaying)
            {
                audioDevice.PlayOneShot(aud_LetsPlay);
                //UIPopupTextManagerWithMovement.Show("MemeBoiLines_1", Color.red, base.transform, aud_LetsPlay.length, 0f);
            }
        }
    }

    private void Opposition()
    {
        Vector3 directionAway = transform.position - (player.position - transform.position).normalized * 500f;
        if (NavMesh.SamplePosition(directionAway, out NavMeshHit navMeshHit, 5f, NavMesh.AllAreas))
        {
            agent.SetDestination(navMeshHit.position);
        }
    }
    #endregion

    #region Wandering
    protected override void Wander(string locationType = "hall")
    {
        if (ps.jumpRope) return;

        base.Wander(locationType);

        agent.stoppingDistance = 1f;
        agent.angularSpeed = 180f;
        agent.updateRotation = true;
        runnin = false;

        playerSpotted = false;

        int audVal = UnityEngine.Random.Range(0, aud_Random.Length);
        if (!audioDevice.isPlaying)
        {
            audioDevice.PlayOneShot(aud_Random[audVal].audios);
            //UIPopupTextManagerWithMovement.Show(aud_Random[audVal].captionsTextDatarea, aud_Random[audVal].holor, base.transform, aud_Random[audVal].audios.length, 0f);
        }

        ResetCooldown();
    }

    public void ResumeWandering()
    {
        if (!ps.jumpRope)
        {
            runnin = false;
            playingRoutine = null;
            Wander();
        }
    }
    #endregion

    #region Jump Rope State Handling
    private IEnumerator StartPlaying()
    {
        ps.isForcedToLook = true;
        canTargetPlayer = false;
        jumpRopeStarted = true;

        Vector3 moveBackPosition = transform.position - transform.forward * 10f;
        agent.SetDestination(moveBackPosition);
        agent.speed = base.agentSpeed;

        while (Vector3.Distance(transform.position, moveBackPosition) > 1f)
        {
            yield return null;
        }

        agent.speed = 0f;
        playingRoutine = null;
    }

    public void Disappoint()
    {
        if (!animator.GetBool("disappointed"))
        {
            if (playingRoutine != null)
            {
                StopCoroutine(playingRoutine);
                playingRoutine = null;
            }

            Wander();
            runnin = false;
            animator.SetBool("disappointed", true);

            canTargetPlayer = true;
            jumpRopeStarted = false;
            playCool = 15f;

            audioDevice.Stop();
            audioDevice.PlayOneShot(aud_Sad);
            //UIPopupTextManagerWithMovement.Show("MemeBoiLines_5", Color.red, base.transform, aud_Sad.length, 0f);
        }
    }
    #endregion

    #region Movement Override
    protected override void HandleMovement()
    {
        if (ps.jumpRope) return;
        base.HandleMovement();
    }
    #endregion

    #region Serialized Field States
    [Header("Movement Speeds")]
    [SerializeField] private float DefaultRunSpeed = 20f;
    [SerializeField] private bool runnin;
    [Header("Player and Movement")]
    [SerializeField] private PlayerScript ps;
    public Transform eeek;

    [Header("Timings")]
    public float playCool;

    [Header("Audio and Animations")]
    [SerializeField] private Animator animator;
    public AudioSource audioDevice;
    [SerializeField] private audiofunny[] aud_Random;
    public audiofunny[] aud_Numbers;
    [SerializeField] private AudioClip aud_LetsPlay, aud_Sad;
    public AudioClip aud_Congrats, aud_ReadyGo, aud_Oops;
    private Coroutine playingRoutine;
    private bool playerSeen, playerSpotted;
    [HideInInspector] public bool jumpRopeStarted;
    #endregion
    [Serializable]
    public class audiofunny
    {
        [SerializeField]
        public AudioClip audios;
        [SerializeField]
        public string captionsTextDatarea;
        [SerializeField]
        public Color holor;
    }
}