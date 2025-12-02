using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EffectScript : MonoBehaviour
{

    #region Initialization
    private void Start() => agent = GetComponent<NavMeshAgent>();
    #endregion
    #region Per-Frame Logic
    private void Update()
    {
        if (inProjectile)
        {
            agent.velocity = otherVelocity;
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
            otherVelocity = other.GetComponent<Rigidbody>().velocity;
            failSave = 1;
        }
        else if (other.CompareTag("cork"))
        {
            if (!notNpcEntirely)
            {
                npcreal.Stun(npcStunTime);
                GameControllerScript.Instance.audioDevice.PlayOneShot(GameControllerScript.Instance.punchsoun);
                Destroy(other.gameObject, 0f);
            }
        }
        else if (other.CompareTag("friesBday"))
        {
            if (!notNpcEntirely)
            {
                npcreal.Stun(npcStunTime);
                Instantiate(GameControllerScript.Instance.ConfettiEffect, other.transform.position, other.transform.rotation);
                Destroy(other.gameObject, 0f);
            }
        }
        else if (other.transform.name == "Gotta Sweep" || other.transform.name == "1945 tom")
        {
            inProjectile = true;
            otherVelocity = 0.1f * agent.speed * transform.forward + other.GetComponent<NavMeshAgent>().velocity;
            failSave = 1;
        }
    }
    #endregion
    [SerializeField] private NPC npcreal;
    [SerializeField] private float npcStunTime;
    [SerializeField] private bool notNpcEntirely;

    #region Internal State
    private NavMeshAgent agent;
    private Vector3 otherVelocity;
    private bool inProjectile;
    private float failSave;
    #endregion
}