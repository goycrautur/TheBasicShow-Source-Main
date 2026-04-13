using UnityEngine;

public class CraftersScript : NPC
{
    #region Initialization
    public override void OnStart()
    {
        base.OnStart();
        normalSprite = spriteImage.sprite;
    }
    #endregion

    #region Update & State Logic
    public override void OnUpdate()
    {
        if (forceShowTime > 0f) forceShowTime -= Time.deltaTime;
        if (chillBro > 0f) chillBro -= Time.deltaTime;
        if (AngryMeter == 5 & !angry)
        {
            angry = true;
            audioDevice.ClearQueue(true);
            audioDevice.SetLoop(true);
            audioDevice.QueueAudio(aud_Loop);
            audioDevice.QueueAudio(aud_Intro);
            spriteImage.sprite = angrySprite;
            AngryMeter = 0;
        }
        base.agentSpeed = base.DefaultAgentSpeed * base.agentSpeedScale;
        if (base.stun) agent.speed = 0f;
        if (base.StunTime < 0f) agent.speed = base.agentSpeed;
        speedAlt1 = defspeedAlt1 * base.agentSpeedScale;
        speedAlt2 = defspeedAlt2 * base.agentSpeedScale;

        if (!angry)
        {
            if (((transform.position - agent.destination).magnitude <= 20f & (transform.position - player.position).magnitude >= 60f) || forceShowTime > 0f)  spriteImage.sprite = normalSprite;
            else  spriteImage.sprite = invisibleSprite;
        }
        else
        {
            if (AdditionalGameCustomizer.Instance != null && AdditionalGameCustomizer.Instance.SkipCraftersAttack) agent.speed += speedAlt2 * Time.deltaTime;
            else if (agent.speed < 180f) agent.speed += speedAlt1 * Time.deltaTime;
            TargetPlayer();

        }
    }
    #endregion

    #region Visibility & Aggression Check
    public override void OnFixedUpdate()
    {
        CheckForPlayerAndVisibility();
    }

    private void CheckForPlayerAndVisibility()
    {
        if ((transform.position + Vector3.up * 2f).RaycastFromPosition(player.position - transform.position, out RaycastHit raycastHit))
        {
            if ((transform.position + Vector3.up * 2f).RaycastFromPosition(player.position - transform.position, out RaycastHit hitVape, QueryTriggerInteraction.UseGlobal))
            {
                if (hitVape.transform.gameObject.layer == 11) return;
            }
            if (raycastHit.transform.CompareTag("Player") && craftersRenderer.isVisible && spriteImage.sprite == normalSprite && chillBro < 0f)
            {
                if (!stopUpdate)
                {
                    stopUpdate = true;
                    AngryMeter += 1;
                    if (AngryMeter < 5) audioDevice.PlaySingleClip(angrySound);
                }
            }
            else stopUpdate = false;
        }
    }
    #endregion

    #region Movement & Targeting
    protected override void HandleMovement() { }

    protected override void TargetPlayer() => agent.SetDestination(player.position);

    public void GiveLocation(Vector3 location, bool flee)
    {
        if (!angry && agent.isActiveAndEnabled)
        {
            agent.SetDestination(location);
            if (flee) forceShowTime = 3f;
        }
    }
    #endregion

    #region Collision & Attack Trigger
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") & angry)
        {
            if (AdditionalGameCustomizer.Instance != null && AdditionalGameCustomizer.Instance.SkipCraftersAttack)GiveConsequence();
            else
            {
                GameObject attacker = Instantiate(attackingCrafters);
                attacker.transform.position = transform.position + Vector3.up * 4f;
                attacker.GetComponent<CraftersAttackerScript>().playerTransform = player;
                attacker.GetComponent<CraftersAttackerScript>().crafters = gameObject;
                attacker.GetComponent<CraftersAttackerScript>().craftersScript = this;
                attacker.GetComponent<CraftersAttackerScript>().Attack();
                attacker.GetComponent<CraftersAttackerScript>().Teleport = base.IsHitboxValid;
                gameObject.SetActive(false);
            }
        }
    }

    public void GiveConsequence(bool tp = true)
    {
        cc.enabled = true;
        if (tp)  gc.CraftersTeleport();
        chillBro = 20f;
        angry = false;
        agent.speed = base.agentSpeed;
        spriteImage.sprite = normalSprite;
        audioDevice.ClearQueue(true);
    }
    #endregion

    #region Serialized Inspector Variables
    [Header("States & Conditions")]
    [SerializeField] private float chillBro;
    [SerializeField] private bool angry, stopUpdate;

    [Header("References & Components")]
    [SerializeField] private CharacterController cc;
    [SerializeField] private Renderer craftersRenderer;
    [SerializeField] private SpriteRenderer spriteImage;

    [Header("Audio & Visuals")]
    [SerializeField] private AudioObjectyeah aud_Intro;
    [SerializeField] private AudioObjectyeah aud_Loop,angrySound;
    [SerializeField] private Sprite angrySprite, normalSprite, invisibleSprite;

    [Header("Movement & Speed")]
    [SerializeField] private GameObject attackingCrafters;
    [SerializeField] private float baseSpeed, speedAlt1,speedAlt2, defspeedAlt1,defspeedAlt2;

    [Header("Gameplay Flow")]
    [SerializeField] private int AngryMeter;
    public bool endless;
    #endregion

    #region Internal State
    public float forceShowTime;
    [SerializeField] private AudioManagerLiveReaction audioDevice;
    #endregion
}