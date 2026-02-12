using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class corkSparyScript : MonoBehaviour
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
        if (cork.name.StartsWith("Wall") || cork.name.StartsWith("Fence") || cork.name.StartsWith("Ceiling") || cork.name.StartsWith("Floor") || cork.name.StartsWith("ElvDoor"))
        {
            GameControllerScript.Instance.audioDevice.PlayOneShot(GameControllerScript.Instance.punchsoun);
            Destroy(gameObject, 0f);
            return;
        }
        if (cork.CompareTag("Window") && cork.GetComponent<WindowScript>() != null && !cork.GetComponent<WindowScript>().broken)
        {
            cork.GetComponent<WindowScript>().Window(true, true, 6f);
            GameControllerScript.Instance.audioDevice.PlayOneShot(GameControllerScript.Instance.punchsoun);
            Destroy(gameObject, 0f);
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
                    ZerullClassic.Instance.OnHit(ZerullClassic.Instance.zs.hit.length, 0);
                    GameControllerScript.Instance.audioDevice.PlayOneShot(GameControllerScript.Instance.punchsoun);
                    Destroy(base.gameObject);
                }
                return;
            }
        }
    }

    #region Per-Frame Logic
    private void Update()
    {
        rb.velocity = transform.forward * speed;
        lifeSpan -= Time.deltaTime;
        if (lifeSpan < 0f)
        {
            Destroy(gameObject, 0f);
        }
    }
    #endregion

    #region Serialized Configuration
    [Header("Movement Settings")]
    [SerializeField] private float speed;

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