using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class NPC : MonoBehaviour
{
    #region Unity Lifecycle
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        OnStart();
    }

    protected virtual void Update() => OnUpdate();

    protected virtual void FixedUpdate()
    {
        if (canTargetPlayer)
        {
            CheckForPlayer();
        }

        if (!isInteracting)
        {
            HandleMovement();
        }

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

            if (isInteracting && canTargetPlayer && !gc.player.invisi && !gc.player.invisichalk)
            {
                TargetPlayer();
            }
        }
    }



    protected virtual void HandleMovement()
    {
        if (!agent.IsReadyToMove() || coolDown.CountdownWithDeltaTime() != 0) return;

        if (!canTargetPlayer || !isInteracting)
        {
            Wander();
        }
    }

    protected virtual void Wander(string locationType = "default")
    {
        wanderer?.SetNewTargetForAgent(agent, locationType);
        ResetCooldown();
    }

    protected virtual void TargetPlayer()
    {
        agent.SetDestination(player.position);
        ResetCooldown();
    }
    #endregion

    #region Hooks
    public virtual void OnStart() { }
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
    public IEnumerator SmoothPush(Transform transform, Vector3 pushDirection, float pushDistance, float duration)
    {
        pushDirection.y = 0f;
        pushDirection.Normalize();

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + pushDirection * pushDistance;
        float elapsedTime = 0f;

        if (Physics.Raycast(startPosition, pushDirection, out RaycastHit hit, pushDistance))
        {
            targetPosition = hit.point - pushDirection * 0.1f;
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;
    }

    #region Serialized Fields
    [Header("NPC Functions")]
    [SerializeField] protected Transform player;
    public GameControllerScript gc;
    [SerializeField] protected AILocationSelectorScript wanderer;
    public bool squished;
    public bool isInteracting, canTargetPlayer;

    [Header("Gizmo Settings")]
    [SerializeField] private bool showPath = true;
    [SerializeField] private Color pathColor = Color.red;
    [SerializeField] private float pathWidth = 15f;
    #endregion

    #region Internal State
    protected float coolDown;
    public float agentSpeedScale = 1f, agentSpeed,DefaultAgentSpeed;
    protected NavMeshAgent agent;
    #endregion
    public int hp, maxhp;
    private bool fuckingdead;
}