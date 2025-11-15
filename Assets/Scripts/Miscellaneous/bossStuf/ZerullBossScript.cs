using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZerullBossScript : MonoBehaviour
{
    [Header("Audio"), SerializeField]
    private AudioQueueScript audioDevice;

    [SerializeField] public AudioClip hit, bossIntro, bossIntro_Loop,totemSound, bossStart,ChairHit,ChairStart;
    [SerializeField] private subsScriptableObject bossHit_Captions, chairHit_Captions,bossyapStart_captions, bossyapIntro_captions,bossyapIntroloop_captions,totem_Captions;

    [Header("References"), SerializeField]
    private NavMeshAgent agent;
    private Transform target;

    [SerializeField] private SpriteRenderer normalSprite;
    [SerializeField] private PlayerScript pscript;
    [SerializeField] private Sprite ChairSprite;

    private MaterialPropertyBlock spriteProperties;

    private void Start()
    {
        bool chair = PlayerPrefsExtension.GetBool("BeatedUpZerull");
        if (chair)
        {
            normalSprite.sprite = ChairSprite;
            hit = ChairHit;
            bossStart = ChairStart;
        }
        spriteProperties = new MaterialPropertyBlock();
        normalSprite.GetPropertyBlock(spriteProperties);
        spriteProperties.SetFloat("_Seed", 0f);
        spriteProperties.SetFloat("_Percent", 0f);
    }

    private void Update()
    {
        if (iframedown)
        {
            iframes -= Time.deltaTime;
        }
        if (iframes < 0f)
        {
            iframedown = false;
        }
        foreach (WindowScript w in FindObjectsOfType<WindowScript>())
        {
            w.enableOffMeshScript = true;
            if (!w.broken)
            {
                if (Vector3.Distance(transform.position, w.transform.position) <= 10)
                {
                    w.Window(true, false, 0f);
                }
            }
        }
        if (target != null && agent.enabled && gameObject.activeSelf)
        {
            agent.SetDestination(target.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!ZerullClassic.Instance.BossStarted && other.CompareTag("bosSpawn"))
        {
            if (other.GetComponent<bosTrig>().IsEnterBossTrigger())
            {
                ZerullClassic.Instance.Encounter();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!ZerullClassic.Instance.debug && !ZerullClassic.Instance.debugMode && ZerullClassic.Instance.BossStarted && other.CompareTag("Player"))
        {
            GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Remove, 50 / GameControllerScript.Instance.player.PlayerDmgResistance, 2f, false, true, false);
            return;
        }
    }
    public NavMeshAgent Agent
    {
        get
        {
            return agent;
        }

        set
        {
            agent = value;
        }
    }

    public SpriteRenderer sprite
    {
        get
        {
            return normalSprite;
        }
    }

    public Transform Target
    {
        set
        {
            target = value;
        }
    }

    public bool DrumsMidi
    {
        set 
        {
            midiDrums = value;
        }
    }
    public void Hit(bool firstHit, float time, float hp = 1f)
    {
        if (iframes > 0f) return;
        hitted = true;
        audioDevice.ClearQueue();
        
        bool chairh = PlayerPrefsExtension.GetBool("BeatedUpZerull");
        if (!chairh)
        {
            audioDevice.QueueAudio(hit, bossHit_Captions);
        }
        if (chairh)
        {
            audioDevice.QueueAudio(ChairHit, chairHit_Captions);
        }
        if (!ZerullClassic.Instance.RealBossStarted)
        {
            audioDevice.QueueAudio(bossStart, bossyapStart_captions);
        }
        ToggleState(false, firstHit, true);
        StartCoroutine(Stun(hp, time, firstHit));
    }
    public void totem()
    {
        audioDevice.ClearQueue();
        audioDevice.QueueAudio(totemSound, totem_Captions);
        iframes = 10f;
        iframedown = true;
    }
    private IEnumerator Stun(float hp, float time, bool firstHit)
    {
        while (time > 0f)
        {
            time -= Time.deltaTime;
            spriteProperties.SetFloat("_Percent", 0.9f);
            spriteProperties.SetFloat("_Seed", Random.Range(0f, 4096f));
            normalSprite.SetPropertyBlock(spriteProperties);
            yield return null;
        }
        gng(hp,firstHit);
    }
    public void gng(float hp, bool firsthit)
    {
        spriteProperties.SetFloat("_Percent", 0f);
        spriteProperties.SetFloat("_Seed", 0f);
        normalSprite.SetPropertyBlock(spriteProperties);
        ToggleState(true, firsthit, false);
        AfterHit(hp);
    }

    private void AfterHit(float hp = 1f)
    {
        agent.speed += 1.05f * hp;
        GameControllerScript.Instance.player.DefaultWalkSpeed += 1 * hp;
        GameControllerScript.Instance.player.DefaultRunSpeed += 1 * hp;
        ZerullClassic.Instance.AfterHit();
        hitted = false;
    }

    public void StartBossIntro()
    {
        audioDevice.ClearQueue();
        audioDevice.QueueAudio(bossIntro,bossyapIntro_captions);
        audioDevice.QueueAudio(bossIntro_Loop,bossyapIntroloop_captions);
        //audioDevice.SetLoop(true);
    }

    public void ToggleState(bool toggle, bool firstHit, bool delay)
    {
        if (firstHit)
        {
            return;
        }
        if (delay)
        {
            StopCoroutine(ToggleDelay(toggle));
            StartCoroutine(ToggleDelay(toggle));
        }
        else
        {
            agent.isStopped = !toggle;
        }
    }

    private IEnumerator ToggleDelay(bool toggle)
    {
        yield return new WaitForSeconds(0.2f);
        agent.isStopped = !toggle;
    }

    [Header("Chase Music")]
    [SerializeField] private AudioSource audioChase;
    private bool midiDrums,iframedown;
    private float iframes = 10f;
    [HideInInspector] public bool hitted,totemready;
}
