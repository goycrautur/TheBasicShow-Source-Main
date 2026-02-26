using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EffectScript : MonoBehaviour
{

    #region Initialization
    private void Start()
    {
        if ((!notNpcEntirely && !npcreal.dosentUseNavmesh) || (notNpcEntirely && notNpcUseNavmesh)) agent = GetComponent<NavMeshAgent>();
        if ((!notNpcEntirely && npcreal.dosentUseNavmesh) || (notNpcEntirely && !notNpcUseNavmesh)) rigi = GetComponent<Rigidbody>();
    }
    #endregion
    #region Per-Frame Logic
    private void Update()
    {
        if (inProjectile)
        {
            if ((!notNpcEntirely && !npcreal.dosentUseNavmesh) || (notNpcEntirely && notNpcUseNavmesh)) agent.velocity = otherVelocity;
            if ((!notNpcEntirely && npcreal.dosentUseNavmesh) || (notNpcEntirely && !notNpcUseNavmesh)) rigi.velocity = otherVelocity;
        }

        if (failSave.CountdownWithDeltaTime() == 0)
        {
            inProjectile = false;
        }
    }
    #endregion

    #region Collision Detection
    private void OnTriggerEnter(Collider other) => HandleCollision(other);

    private void OnTriggerExit()
    {
        failSave = 0;
        inProjectile = false;
    }

    private void OnTriggerStay(Collider other) => HandleCollision(other);
    #endregion

    #region BSODA Effect Handling
    private void HandleCollision(Collider other)
    {
        if (other.CompareTag("BSODA"))
        {
            inProjectile = true;
            otherVelocity = other.GetComponent<CharacterController>().velocity;
            failSave = 1;
        }
        if (other.CompareTag("cork"))
        {
            if (!notNpcEntirely)
            {
                npcreal.Stun(npcStunTime);
                GameControllerScript.Instance.audioDevice.PlayOneShot(GameControllerScript.Instance.punchsoun);
                Destroy(other.gameObject, 0f);
            }
        }
        if (other.CompareTag("Projectile"))
        {
            if (other.GetComponent<ProjectileScript>().thrown)
            {
                if (!notNpcEntirely)
                {
                    npcreal.Stun(npcStunTime);
                    Destroy(other.gameObject, 0f);
                }
            }
        }
        if (other.CompareTag("friesBday"))
        {
            if (!notNpcEntirely)
            {
                npcreal.Stun(npcStunTime);
                Instantiate(GameControllerScript.Instance.ConfettiEffect, other.transform.position, other.transform.rotation);
                Destroy(other.gameObject, 0f);
            }
        }
        if (other.transform.name == "Gotta Sweep" || other.transform.name == "1945 tom")
        {
            inProjectile = true;
            otherVelocity = 0.1f * (!npcreal.dosentUseNavmesh? agent.speed : 1) * transform.forward + (!npcreal.dosentUseNavmesh ? other.GetComponent<NavMeshAgent>().velocity : other.GetComponent<Rigidbody>().velocity);
            failSave = 1;
        }
    }
    #endregion
    [SerializeField] private NPC npcreal;
    [SerializeField] private float npcStunTime;
    [SerializeField] public bool notNpcEntirely, notNpcUseNavmesh;

    #region Internal State
    private NavMeshAgent agent;
    private Rigidbody rigi;
    private Vector3 otherVelocity;
    private bool inProjectile;
    private float failSave;
    #endregion
}