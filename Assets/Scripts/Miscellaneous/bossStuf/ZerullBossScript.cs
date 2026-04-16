using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZerullBossScript : MonoBehaviour
{
    [Header("Audio"), SerializeField]
    private AudioManagerLiveReaction audioDevice;

    [SerializeField] public AudioObjectyeah hit, bossIntro, bossIntro_Loop,totemSound, bossStart,ChairHit,ChairStart;
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
        if (iframedown)iframes -= Time.deltaTime;
        if (iframes < 0f) iframedown = false;
        foreach (basicshowWindowScript w in FindObjectsOfType<basicshowWindowScript>()) 
        {
            w.enableOffMeshScript = true;
            if (!w.broken) if (Vector3.Distance(this.transform.position, w.transform.position) <= 10) w.SetWindowState(false, 6f, 0f, 0, true, 0);
        }
        if (target != null && agent.enabled && gameObject.activeSelf) agent.SetDestination(target.position);
        if (stuntiem > 0f) stuntiem -= Time.deltaTime * stunTimeMult;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!ZerullClassic.Instance.BossStarted && other.CompareTag("bosSpawn")) if (other.GetComponent<bosTrig>().IsEnterBossTrigger()) ZerullClassic.Instance.Encounter();
    }
    private void OnTriggerStay(Collider other)
    {
        if (!ZerullClassic.Instance.debug && !ZerullClassic.Instance.debugMode && ZerullClassic.Instance.BossStarted && other.CompareTag("Player"))
        {
            if (!GameControllerScript.Instance.debugMode & !GameControllerScript.Instance.player.titlecard)
            GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Remove, 50 / GameControllerScript.Instance.player.PlayerDmgResistance, 0.75f, false, true, false);
            GameControllerScript.Instance.player.killedbyhim = true;
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
        stuntiem = time;
        hitted = true;
        audioDevice.ClearQueue(true);
        
        bool chairh = PlayerPrefsExtension.GetBool("BeatedUpZerull");
        if (!chairh) audioDevice.QueueAudio(hit);
        else audioDevice.QueueAudio(ChairHit);
        if (!ZerullClassic.Instance.RealBossStarted)audioDevice.QueueAudio(bossStart);
        agent.speed += 0.75f * hp;
        GameControllerScript.Instance.player.DefaultWalkSpeed += 0.7f * hp;
        GameControllerScript.Instance.player.DefaultRunSpeed += 0.7f * hp;
        StartCoroutine(Stun(hp, firstHit));
    }
    public void totem()
    {
        audioDevice.ClearQueue(true);
        audioDevice.QueueAudio(totemSound);
        iframes = 10f;
        iframedown = true;
    }
    private IEnumerator Stun(float hp, bool firstHit)
    {
        while (stuntiem > 0f)
        {
            agent.isStopped = true;
            ZerullClassic.Instance.debug = true;
            stunTimeMult = 1f;
            spriteProperties.SetFloat("_Percent", 0.9f);
            spriteProperties.SetFloat("_Seed", Random.Range(0f, 4096f));
            normalSprite.SetPropertyBlock(spriteProperties);
            yield return null;
        }
        gng(hp,firstHit);
    }
    public void gng(float hp, bool firsthit)
    {
        stunTimeMult = 0f;
        spriteProperties.SetFloat("_Percent", 0f);
        spriteProperties.SetFloat("_Seed", 0f);
        normalSprite.SetPropertyBlock(spriteProperties);
        ZerullClassic.Instance.debug = firsthit;
        agent.isStopped = firsthit;
        
        ZerullClassic.Instance.AfterHit();
        hitted = false;
    }

    public void StartBossIntro()
    {
        bool chairhh = PlayerPrefsExtension.GetBool("BeatedUpZerull");
        audioDevice.ClearQueue(true);
        //audioDevice.SetLoop(true);
        if (!chairhh)
        {
            audioDevice.QueueAudio(bossIntro);
            audioDevice.QueueAudio(bossIntro_Loop);
        }
        
    }

    [Header("Chase Music")]
    private bool midiDrums,iframedown;
    public float iframes = 0f,stuntiem,stunTimeMult;
    [HideInInspector] public bool hitted,totemready;
}
