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
                Vector3 pushDirection = (npcreal.GetComponent<Collider>().transform.position - other.transform.position).normalized;
                StopCoroutine(npcreal.SmoothPush(npcreal.GetComponent<Transform>(), pushDirection, 4f, 0.1f));
                StartCoroutine(npcreal.SmoothPush(npcreal.GetComponent<Transform>(), pushDirection, 4f, 0.1f));
                GameControllerScript.Instance.audioDevice.PlayOneShot(GameControllerScript.Instance.punchsoun);
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
    [SerializeField] private bool notNpcEntirely;

    #region Internal State
    private NavMeshAgent agent;
    private Vector3 otherVelocity;
    private bool inProjectile;
    private float failSave;
    #endregion
}