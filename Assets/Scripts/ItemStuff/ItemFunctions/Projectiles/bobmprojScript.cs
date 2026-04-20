using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class bobmprojScript : MonoBehaviour
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
            boom(true);
            return;
        }
        if (cork.CompareTag("floor"))
        {
            boom(false, 30f);
            return;
        }
        if (cork.CompareTag("Window") && cork.GetComponent<basicshowWindowScript>() != null && !cork.GetComponent<basicshowWindowScript>().broken)
        {
            
            cork.GetComponent<basicshowWindowScript>().SetWindowState(true, 6f, 0f, bouncetime+2);
            boom(true);
            return;
        }
        if (cork.GetComponent<NPC>() != null)
        {
            cork.GetComponent<NPC>().Stun(10f);
            boom(true);
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
                    ZerullClassic.Instance.OnHit(ZerullClassic.Instance.zs.hit.audClip.length,bouncetime+1);
                    boom(true);
                }
                return;
            }
        }
    }
    private void boom(bool KillReal = false,float fallvelo = 15f)
    {
        if (iframe > 0f && !KillReal) return;
        if (bouncetime == 0 || KillReal)
        {
            Destroy(gameObject, 0f);
        }
        iframe += 0.25f;
        fallvelocity += fallvelo;
        Instantiate(prefa, transform.position, transform.rotation);
        Debug.Log($"bouncetime left : {bouncetime}");
        bouncetime--;
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
    [SerializeField] private int bouncetime;

    [Header("Lifespan Settings")]
    [SerializeField] private float lifeSpan;

    [Header("Rotation Settings")]
    [SerializeField] private bool shouldRotate;
    public GameObject prefa;
    #endregion
    #region Internal References
    private Rigidbody rb;
    private Vector3 direction;
    #endregion
}