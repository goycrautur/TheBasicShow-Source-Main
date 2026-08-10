using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif


[RequireComponent(typeof(CharacterController))]
public class NPC : MonoBehaviour
{
    #region Unity Lifecycle
    protected virtual void Start()
    {
        mainAgentSpeedScale = agentSpeedScale;
        if (!dosentUseNavmesh) agent = GetComponent<NavMeshAgent>();
        if (!dosentUseNavmesh) gc.GlobalNpcList.Add(this);
        OgLayerName = "npcLayer";
        XrayLayerName = "npcXrayLayer";
        hp = maxhp;
        OnStart();
    }
    protected virtual void OnDestroy()
    {
        if (!dosentUseNavmesh) gc.GlobalNpcList.Remove(this);
    }

    protected virtual void Update()
    {
        confusionEffect.SetActive(stun);
        DeadSprite.SetActive(fuckingdead);
        if (NpcIsNotSpriteRenderer) NpcGameObj.SetActive(!fuckingdead);
        if (IsMetalPiped) MetalPipedEffect();
        else NonMetalPipedEffect();
        if (canTargetPlayer)CheckForPlayer();
        if (!isInteracting && !dosentUseNavmesh) HandleMovement();
        
        if (!fuckingdead && (!stun || stopOverridingStun))IsHitboxValid = !squished;
        if (StunTime < 0f)
		{
            stun = false;
            IsHitboxValid = true;
            StunTime = 1f;
		}
        if (stun)
		{
			StunTime -= Time.deltaTime; 
            IsHitboxValid = false;
		}
        if (hp <= 0 && !fuckingdead)
        {
            StunTime = -1f;
            stun = false;
            DeathRespawnTime = DeathRespawnTimeSet;
            fuckingdead = true;
            
        }
        if (DeathRespawnTime < 0f)
        {
            hp = maxhp;
            fuckingdead = false;
            IsHitboxValid = true;
            DeathRespawnTime = 1f;
            agentSpeedScale = 1f;
            if (!NpcIsNotSpriteRenderer) NpcSprites.color = new Color(NpcSprites.color.r,NpcSprites.color.g,NpcSprites.color.b,1f);
        }
        if (fuckingdead)
        {
            if (RespawnAfterDeath) DeathRespawnTime -= Time.deltaTime;
            IsHitboxValid = false;
            agentSpeedScale = 0f;
            if (!NpcIsNotSpriteRenderer) NpcSprites.color = new Color(NpcSprites.color.r,NpcSprites.color.g,NpcSprites.color.b,0f);
        }

        OnUpdate();
    }
    protected virtual void FixedUpdate()
    {
        OnFixedUpdate();
    }
    #endregion


    #region AI Behavior
    protected virtual void CheckForPlayer()
    {
        Vector3 direction = player.position - transform.position;
        if (transform.position.RaycastFromPosition(direction, out RaycastHit hit))
        {
            isInteracting = hit.transform.CompareTag("Player");
            if (transform.position.RaycastFromPosition(direction, out RaycastHit hitVape, QueryTriggerInteraction.UseGlobal))
            {
                if (hitVape.transform.gameObject.layer == 11) return;
            }
            if (isInteracting && canTargetPlayer && !gc.player.invisi && !gc.player.invisichalk)
            {
                TargetPlayer();
            }
        }
    }



    protected virtual void HandleMovement()
    {
        if ((agent.isActiveAndEnabled && !agent.IsReadyToMove() && !navmeshNpcPushing)|| coolDown.CountdownWithDeltaTime() != 0) return;

        if (!canTargetPlayer || !isInteracting)
        {
            Wander();
        }
    }
    public virtual void MetalPipedEffect()
    {
        agentSpeedScale = 0f;
    }
    public virtual void NonMetalPipedEffect()
    {
        agentSpeedScale = 1f;
    }
    public virtual void DrainHp(int Value)
    {
        hp -= Value;
    }
    public virtual void Stun(float Duration)
    {
        if (fuckingdead) return;
        StunTime = Duration;
        stun = true;
    }
    public virtual void SetToXrayLayer(bool xray = true)
    {
        for (int i = 0; i < ObjectToGetXrayed.Length; ++i) 
        {
            if (ObjectToGetXrayed[i] != null) 
            ObjectToGetXrayed[i].layer = !xray ? LayerMask.NameToLayer(OgLayerName) : LayerMask.NameToLayer(XrayLayerName);
        }
    }


    protected virtual void Wander(string locationType = "default")
    {
        if (!dosentUseNavmesh && agent.isActiveAndEnabled && !navmeshNpcPushing) wanderer?.SetNewTargetForAgent(agent, locationType);
        ResetCooldown();
    }

    protected virtual void TargetPlayer()
    {
        if (!dosentUseNavmesh && agent.isActiveAndEnabled && !navmeshNpcPushing) agent.SetDestination(player.position);
        else if (!dosentUseNavmesh  && navmeshNpcPushing) PushedTargetPos = player.position;
        ResetCooldown();
    }
    #endregion

    #region Hooks
    public virtual void OnStart() { }
    public virtual void OnEnable() { }
    public virtual void OnDisable() { }
    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate() { }
    #endregion

    #region Utility
    protected void ResetCooldown() => coolDown = 1;
    #endregion

    #region Editor Gizmos
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (showPath && agent != null)
        {
            NavMeshPath path = agent.path;
            if (path != null)
            {
                Handles.color = pathColor;
                for (int i = 0; i < path.corners.Length - 1; i++)
                {
                    Handles.DrawAAPolyLine(pathWidth, path.corners[i], path.corners[i + 1]);
                }
            }
        }
    }
#endif
    #endregion
    #region Npc Push Stuff
    public virtual void PushNpc(Vector3 pushDirection, float pushForce, float duration)
	{
        if (fuckingdead) return;
		if (NpcPushCorou != null)
        {
            StopCoroutine(NpcPushCorou);
			AfterPushStuff();
            NpcPushCorou = null;
        }
		NpcPushCorou = StartCoroutine(NpcSmoothPush(pushDirection, pushForce, duration));
	}
	public Vector3 GetNPCPushDirection(Vector3 OtherPosition)
	{
		Vector3 pushDirection = (OtherPosition).normalized;
		return pushDirection;
	}
    private IEnumerator NpcSmoothPush(Vector3 pushDirection, float pushForce, float duration)
    {
        pushDirection.y = 0f;
        pushDirection.Normalize();

        float elapsed = 0f;
        float currentSpeed = pushForce;

        while (elapsed < duration)
        {
            if (!dosentUseNavmesh) 
            {
                navmeshNpcPushing = true;
                agent.enabled = false;
            }
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float speed = Mathf.Lerp(currentSpeed, 0f, t);
            Vector3 move = pushDirection * speed * Time.deltaTime;
            cecil.Move(move);
            yield return null;
        }
        AfterPushStuff();
    }
    public virtual void AfterPushStuff()
    {
        Vector3 PushTransform = transform.position;
        if (!dosentUseNavmesh) 
        {
            navmeshNpcPushing = false;
            agent.enabled = true;
            agent.Warp(PushTransform);
            if (PushedTargetPos != null)
            {
                agent.SetDestination(PushedTargetPos);
                PushedTargetPos = Vector3.zero;
            }
            else Wander();
        }
    }
    #endregion
    #region Serialized Fields
    [Header("NPC Functions")]
    [SerializeField] protected Transform player;
    [SerializeField] protected Vector3 PushedTargetPos;
    public GameControllerScript gc;
    [SerializeField] protected AILocationSelectorScript wanderer;
    public GameObject[] ObjectToGetXrayed;
    public string OgLayerName,XrayLayerName;
    public bool isInteracting, canTargetPlayer, IsHitboxValid = true, IsMetalPiped;

    [Header("Gizmo Settings")]
    [SerializeField] private bool showPath = true;
    [SerializeField] private Color pathColor = Color.red;
    [SerializeField] private float pathWidth = 15f;
    #endregion
    [Header("Misc Stuffs")]
    #region Internal State
    public float coolDown;
    public float agentSpeedScale = 1f, agentSpeed,DefaultAgentSpeed,StunTime,DeathRespawnTime,DeathRespawnTimeSet;
    public bool squished,stun,stopOverridingStun;
    public NavMeshAgent agent;
    public CharacterController cecil;
    public GameObject confusionEffect,StunSprite,DeadSprite;
    public SpriteRenderer NpcSprites;
    public bool NpcIsNotSpriteRenderer;
    public GameObject NpcGameObj;
    public int hp, maxhp = 100;
    public bool fuckingdead,RespawnAfterDeath,UsesStunSprite,dosentUseNavmesh,navmeshNpcPushing;
    
    public Coroutine NpcPushCorou;
    [HideInInspector] public float mainAgentSpeedScale;
    #endregion
    
}