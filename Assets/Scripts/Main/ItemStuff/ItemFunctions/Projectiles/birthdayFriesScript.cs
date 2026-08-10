using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class birthdayFriesScript : MonoBehaviour
{
    private bool stunnedBoss;
    #region Initialization
    private void Start()
    {
        if (shouldRotate)
        {
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.z = Mathf.Round(Random.Range(0f, 359f));
            transform.eulerAngles = eulerAngles;
        }

        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }
    #endregion
    private void OnTriggerStay(Collider cork)
    {
        if (cork.CompareTag("floor") && cork.name.StartsWith("Wall") || cork.name.StartsWith("Fence") || cork.name.StartsWith("Ceiling") || cork.name.StartsWith("Floor") || cork.name.StartsWith("ElvDoor"))
        {
            Instantiate(GameControllerScript.Instance.ConfettiEffect, transform.position, transform.rotation);
            Destroy(base.gameObject);
            return;
        }
        if (ZerullClassic.Instance.realBossStarted && ZerullClassic.Instance.health != 1)
        {
            if (cork.GetComponent<ZerullBossScript>() != null && !stunnedBoss)
            {
                StartCoroutine(StunBoss());
                IEnumerator StunBoss()
                {
                    while (ZerullClassic.Instance.maxHealth == ZerullClassic.Instance.health - 1 && !ZerullClassic.Instance.realBossStarted && ZerullClassic.Instance.GetBoss().hitted || ZerullClassic.Instance.isbroyapping)
                    {
                        yield return null;
                    }
                    stunnedBoss = true;
                    ZerullClassic.Instance.OnHit(ZerullClassic.Instance.zs.hit.audClip.length,0,false);
                    Instantiate(GameControllerScript.Instance.ConfettiEffect, transform.position, transform.rotation);
                    Destroy(base.gameObject);
                }
                return;
            }
        }
    }

    #region Per-Frame Logic
    private void Update()
    {
        rb.velocity = new Vector3(rb.velocity.x, fallvelocity, rb.velocity.z);
        lifeSpan -= Time.deltaTime;
        fallvelocity -= VerticalGrav * Time.deltaTime;
        if (iframe > 0f) iframe -= Time.deltaTime;
        if (speed <= 5f) speed -= HorizontalGrav * Time.deltaTime;
        if (lifeSpan < 0f)
        {
            Destroy(gameObject, 0f);
        }
    }
    #endregion

    #region Serialized Configuration
    [Header("Movement Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float fallvelocity,iframe,VerticalGrav,HorizontalGrav;

    [Header("Lifespan Settings")]
    [SerializeField] private float lifeSpan;

    [Header("Rotation Settings")]
    [SerializeField] private bool shouldRotate;
    #endregion
    #region Internal References
    private Rigidbody rb;
    private Vector3 direction;
    #endregion
}