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
    private void OnTriggerEnter(Collider cork)
    {
        if (cork.name.StartsWith("Wall") || cork.name.StartsWith("Fence") || cork.name.StartsWith("Ceiling") || cork.name.StartsWith("Floor") || cork.name.StartsWith("ElvDoor"))
        {
            if (GoThoughWalls) boom(false);
            else boom(true, 0f);
            return;
        }
        if (cork.CompareTag("floor"))
        {
            boom(false, BounceVelocity);
            return;
        }
    }
    private void OnTriggerStay(Collider cork)
    {
        if (cork.CompareTag("Window") && cork.GetComponent<basicshowWindowScript>() != null && !cork.GetComponent<basicshowWindowScript>().broken)
        {
            
            cork.GetComponent<basicshowWindowScript>().SetWindowState(true, 6f, 0f, bouncetime+2);
            boom(true);
            return;
        }
        if (cork.CompareTag("Player") & !GameControllerScript.Instance.debugMode & !GameControllerScript.Instance.player.titlecard && IsEnemy)
        {
            GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Remove, DamageToNpc / GameControllerScript.Instance.player.PlayerDmgResistance, 0.75f, false, true, false);
            GameControllerScript.Instance.player.PushPlayer(GameControllerScript.Instance.player.GetPlayerPushDirection(transform.position), 32f*(bouncetime+1), 0.5f);
            boom(true);
            return;
        }
        if (cork.GetComponent<NPC>() != null && !IsEnemy)
        {
            NPC enpe = cork.GetComponent<NPC>();
            enpe.Stun(NpcStuntime);
            enpe.DrainHp(DamageToNpc);
            enpe.PushNpc(enpe.GetNPCPushDirection(transform.forward),NpcPushVal, 1f);
            boom(true);
            return;
        }
        if (ZerullClassic.Instance.realBossStarted && ZerullClassic.Instance.health != 1 && !IsEnemy)
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
            Instantiate(prefa, transform.position, transform.rotation);
            Destroy(gameObject, 0f);
            return;
        }
        iframe += 0.25f;
        fallvelocity += fallvelo;
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
        if (lifeSpan < 0f) boom(true);
    }
    #endregion

    #region Serialized Configuration
    public bool IsEnemy,GoThoughWalls;
    [Header("Movement Settings")]
    public float speed;
    [SerializeField] private float fallvelocity,iframe,VerticalGrav,HorizontalGrav;
    [SerializeField] private int bouncetime,DamageToNpc;
    [SerializeField] private float NpcStuntime,NpcPushVal,BounceVelocity;

    [Header("Lifespan Settings")]
    public float lifeSpan;

    [Header("Rotation Settings")]
    [SerializeField] private bool shouldRotate;
    public GameObject prefa;
    #endregion
    #region Internal References
    private Rigidbody rb;
    private Vector3 direction;
    #endregion
}