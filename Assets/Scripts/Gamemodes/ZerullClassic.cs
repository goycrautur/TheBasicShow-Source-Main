using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using BrunoMikoski.AnimationSequencer;
using MidiPlayerTK;

public class ZerullClassic : MonoBehaviour
{
    private bool replaceALLSAKES;
    [SerializeField] private GameControllerScript gc;
    public GameObject zer;
    #region SingletonSetup
    private void Awake() => Instance = this;
    public static ZerullClassic Instance;
    #endregion
    [Header("Health"), SerializeField]
    public float maxHealth = 10f;

    public float health = 10f,ExhaustedJerMaxHp = 15f;

    public float Health => health;

    [Header("Null Settings")]

    [Tooltip("Speed of Boss (null/baldloon), when Boss Started")] public float BossSpeed = 20f;

    [Tooltip("Speed of Player, when Boss Started")] public float PlayerSpeed = 20f;

    [Header("Projectile"), SerializeField]
    private Transform[] projectilespawns;

    public GameObject[] projectileprefabs;

    [HideInInspector] public GameObject currentProjectile;

    private bool projectilesDoNotExist,ok,AllowProjectileSpawn;

    public int maxObjects = 5;

    [HideInInspector] public int objects = 0;

    [HideInInspector] public float spawnCooldown,curHealthValueForLerping,thrownDelay;

    [Header("Blockages")]
    public bool spawnBlockagesDuringTheBossfight;
    public GameObject blockages;
    public GameObject[] RandomBlockages;
    [Tooltip("Boss variant of null"), SerializeField] public ZerullBossScript zs;
    public Slider healthSlider;
    public RectTransform ZerullIcon;
    public AnimationSequencerController HealthbarAnimAppear,HealthbarAnimDissapear;
    private Vector3 ogZerullIconSize;
    private Vector2 ogZerullIconPivot;

    [Header("Audio"), SerializeField, Tooltip("Boss Music")]
    private AudioObjectyeah[] Boss_Music;
    [SerializeField, Tooltip("Music")] private AudioManagerLiveReaction musicAudio, musicLoop;

    [Header("the fucking midi tempo"), Tooltip("do i have to elaborate about what this does")] public float midiTempo = 1f;

    [HideInInspector] public bool debug, AlreadyCleared = false;
    [Header("Bonus")]
    [SerializeField, Tooltip("Does the walls must shake during the bossfight.")] private bool VertexShake = true;
    [SerializeField, Tooltip("Does the game will show a health slider")] private bool ShowHealthSlider = false;

    [Tooltip("Does the game will play a sound, when projectile thrown. [SET THE SOUND IN PROJECTILE PREFABS]")] public bool playSoundWhenProjectileThrown = false;

    [Tooltip("Debug Mode")] public bool debugMode = false;

    [SerializeField, Tooltip("Does the start & hit song is a midi version.")] private bool StartSongsIsMidi = true;

    [Tooltip("Does the loop song is a midi version.")] public bool Midi = true;

    [Tooltip("BTM of the Midi")] public float Midi_BTM = 120f;

    [Tooltip("Boss Intro MIDI Name String")] public string midiIntro;

    [Tooltip("Boss Start MIDI Name String")] public string midiStart;

    [Tooltip("Boss Loop MIDI Name String")] public string midiLoop;
    [Tooltip("Exhausted boss MIDI intro Name String")] public string exhamidiIntro;

    [Tooltip("Exhausted boss MIDI hit Name String")] public string exhamidiStart;

    [Tooltip("Exhausted boss MIDI loop Name String")] public string exhamidiLoop;
    [Tooltip("Boss Loop MIDI the bloxyver Name String")] public string bloxyLoop1,bloxyLoop2;

    [Tooltip("Length of Start MIDI")] public float midiStartLength = 7f;
    public GameObject[] tweenOutitems;
    public Transform[] StartingBossfightProjectileSpawnLocaltion;
    public bool ableToStart,bossStarted, realBossStarted,switchToBloxyb;
    public Animator yourflashbang;
    public bool taggedonetime,oneHPfailsave,ExhaustedJerBoss;

    [Header("Extremely temporarily raaargh")]
    public bool ForceMethhouseBossOst;
    public AudioObjectyeah[] MethHouse_BossMusic;
    public string BossMidiLoopMethhouse;
    

    public bool BossStarted
    {
        get
        {
            return bossStarted;
        }

        set
        {
            bossStarted = value;
        }
    }

    public bool RealBossStarted
    {
        get
        {
            return realBossStarted;
        }

        set
        {
            realBossStarted = value;
        }
    }
    private void Start()
    {
        /*Singleton<MusicManagerMaes>.Instance.PlayMidi(ExhaustedJerBoss ? exhamidiStart : midiStart, loop: true);
        if (StartSongsIsMidi) Singleton<MusicManagerMaes>.Instance.QueueMidi(ExhaustedJerBoss ? exhamidiLoop : (switchToBloxyb ? bloxyLoop1 : midiLoop), true);*/
        ogZerullIconSize = ZerullIcon.localScale;
        ogZerullIconPivot = ZerullIcon.pivot;
        if (ExhaustedJerBoss) maxHealth = ExhaustedJerMaxHp;
        string dific = PlayerPrefs.GetString("CurDifficulity", "normal");
        int extraHealth = dific == "easy" ? 5 : dific == "normal" ? 12 : dific == "hard" ? 18 : dific == "expert" ? 25 : dific == "maniac" ? 30 : 5;
        maxHealth += extraHealth;
        health = maxHealth;
        bool shakeswitchFR = PlayerPrefsExtension.GetBool("WallShakeSwitch");
        bool shake = PlayerPrefsExtension.GetBool("WallShake");
        bool blox = PlayerPrefsExtension.GetBool("bloxy");
        if(!shakeswitchFR) PlayerPrefsExtension.SetBool("WallShake", true);
        VertexShake = !shakeswitchFR ? true : shake;
        switchToBloxyb = blox;
        Singleton<VertexGlitchManager>.Instance.mustGlitch = false;
        if (Midi) if (zs != null) zs.DrumsMidi = true;
        oneHPfailsave = true;
    }
    public void OnEnable()
	{
		MusicManagerMaes.OnMidiEvent += MidiEvent;
		MusicManagerMaes.OnMidiTransitionComplete += MidiTransitionComplete;
	}

	public void OnDisable()
	{
		MusicManagerMaes.OnMidiEvent -= MidiEvent;
		MusicManagerMaes.OnMidiTransitionComplete -= MidiTransitionComplete;
	}

    private void tryTOStartBoss()
    {
        if (StartSongsIsMidi)
        {
            if (ableToStart && !taggedonetime)
            {
                BossBegin();
                taggedonetime = true;
                return;
            }
        }
    }
    private void MidiTransitionComplete()
    {
        ableToStart = true;
    }
    private void MidiEvent(MPTKEvent midiEvent)
	{
		if (ableToStart && midiEvent.Command == MPTKCommand.MetaEvent && midiEvent.Meta == MPTKMeta.TextEvent)
        {
            //Debug.Log($"midi event info: {midiEvent.Info}");
            if (midiEvent.Info == "HeavyShake")
            {
                ZerullIcon.localScale = ogZerullIconSize * 1.45f;
                ZerullIcon.pivot = new Vector2(0.75f,0.25f);
                AdditionalGameCustomizer.Instance.ExtraFovAmmount = -60f;
                Singleton<VertexGlitchManager>.Instance.Glitch();
                CameraScript.Instance.TempShakeAmount = 0.35f;
            }
            else
            {
                ZerullIcon.localScale = ogZerullIconSize * 1.25f;
                ZerullIcon.pivot = new Vector2(0.75f,0.25f);
                AdditionalGameCustomizer.Instance.ExtraFovAmmount = -30f;
                Singleton<VertexGlitchManager>.Instance.Glitch();
            }
        }
	}

    private void Update()
    {
        
        curHealthValueForLerping = Mathf.Lerp(curHealthValueForLerping,health, 5*Time.deltaTime);
        ZerullIcon.localScale = Vector3.Lerp(ZerullIcon.localScale,ogZerullIconSize, 20*Time.deltaTime);
        ZerullIcon.pivot = Vector2.Lerp(ZerullIcon.pivot,ogZerullIconPivot, 20*Time.deltaTime);
        AdditionalGameCustomizer.Instance.ExtraFovAmmount = Mathf.Lerp(AdditionalGameCustomizer.Instance.ExtraFovAmmount,0, 25*Time.deltaTime);
        if (thrownDelay > 0f)thrownDelay -= Time.deltaTime;
        if (!taggedonetime) tryTOStartBoss();
        if (replaceALLSAKES)
        {
            for (int i = 0; i < ItemManager.Instance.Inventory.Length; i++)
            {
                if (ItemManager.Instance.Inventory[i].ItemID == 12)
                {
                    ItemManager.Instance.CollectItem(16);
                    ItemManager.Instance.RemoveItemFromInventory(ItemManager.Instance.GetItem(12));
                    return;
                }
            }
        }
        if (ShowHealthSlider)
        {
            healthSlider.maxValue = maxHealth - 1f;
            if (healthSlider != null) if (healthSlider.value != curHealthValueForLerping) healthSlider.value = curHealthValueForLerping;
        }
        if (realBossStarted)
        {
            Singleton<MusicManagerMaes>.Instance.ReservedPlayer.MPTK_ChannelVolumeSet(9, Mathf.Clamp(1f - (Vector3.Distance(zs.transform.position, GameControllerScript.Instance.player.transform.position) - 75f) / 75f, 0f, 1f));
        }
        if (AllowProjectileSpawn)
        {
            if (objects > maxObjects) objects = maxObjects;
            if (spawnCooldown > 0f) spawnCooldown -= Time.deltaTime;
            if (spawnCooldown <= 0f)
            {
                //if (objects < maxObjects) SpawnProjectile(false, false);
                spawnCooldown = 25f;
            }
            if (!projectilesDoNotExist && objects == 0)
            {
                projectilesDoNotExist = true;
                spawnCooldown = 5f;
            }
        }
    }

    public void BossIntro()
    {
        scoreSystemManager.Instance.stopUpdatingTSDiscord = true;
        Prepare();
        if (debugMode) Debug.Log("Bossfight");
        zs.sprite.gameObject.SetActive(true);
        debug = true; // Makes that null doesn't able to kill player
        zs.Agent.speed = 140f; // Make null very fast for time
    }
    public void Prepare()
    {
        zs.gameObject.SetActive(true); // Enable null
    }
    public void Encounter()
    {
        Sych.SetGameWindowTitle("The Basic Show - Confrontation");
        gc.modeState = "oh no he got last exit what do i do";
        PlayerPrefsExtension.SetBool("WallShakeSwitch", true);
        foreach (basicshowWindowScript w in FindObjectsOfType<basicshowWindowScript>()) if (!w.broken) w.SetWindowState(false, 6f, 0f, 0, true, 0);
        if (GameControllerScript.Instance.LapManag.Meeptimar.isActiveAndEnabled)
        {
            meepTimerScript.Instance.AddTime(25f, Color.green);
            meepTimerScript.Instance.canTime = false;
        }
        if (debugMode) Debug.Log("Encounter");

        debug = true; // Makes that null doesn't able to kill player 

        bossStarted = true; // Boss Started bool enabled
        for (int i = 0; i < StartingBossfightProjectileSpawnLocaltion.Length; ++i)
        {
            SpawnProjectile(StartingBossfightProjectileSpawnLocaltion[i], true, Random.Range(0, projectileprefabs.Length));
        }
        zs.Agent.speed = 0f; // Set Null speed to 0. (make null doesn't able to move)
        zs.Agent.isStopped = true; // Fully stop Null agent
        zs.sprite.gameObject.SetActive(true); // Enable Null Sprite
        GameControllerScript.Instance.player.DefaultWalkSpeed += PlayerSpeed-GameControllerScript.Instance.player.DefaultWalkSpeed;
        GameControllerScript.Instance.player.DefaultRunSpeed += PlayerSpeed-GameControllerScript.Instance.player.DefaultRunSpeed;
        if (!ForceMethhouseBossOst)
        {
            if (!StartSongsIsMidi)
            {
                musicAudio.ClearQueue(true);
                musicAudio.SetLoop(true);
                musicAudio.QueueAudio(Boss_Music[0]);
            }
            else
            {
                Singleton<MusicManagerMaes>.Instance.PlayMidi(ExhaustedJerBoss ? exhamidiIntro : midiIntro, loop: true);
                Singleton<MusicManagerMaes>.Instance.SetSpeed(midiTempo);
            }
        }
        else
        {
            musicAudio.ClearQueue(true);
            musicAudio.SetLoop(true);
            musicAudio.QueueAudio(MethHouse_BossMusic[0]);
        }
        zs.StartBossIntro();
    }
    private void RemoveProjectiles()
    {
        foreach (ProjectileScript projectile in FindObjectsOfType<ProjectileScript>())
        {
            Destroy(projectile.gameObject);
            objects = 0;
        }
    }
    private void RemoveItems()
    {
        foreach (PickupScript pickup in FindObjectsOfType<PickupScript>())
        {
            Destroy(pickup.gameObject);
            objects = 0;
        }
    }
    public bool alreadyHit,isbroyapping; // why not
    private void StartHit()
    {
        Sych.SetGameWindowTitle("@!@!!@#M#%%^^*****^($%)^))^)");
        gc.modeState = "!!?!?!@?!?@!?@?!??!@!?@!?@!?@? HE GOT HURT";
        if (debugMode)
            Debug.Log("Boss Pissed");

        isbroyapping = true;

        RemoveProjectiles(); // Remove projectiles

        Singleton<VertexGlitchManager>.Instance.sourceToSyncIn = musicLoop.audioDevice; // Make vertex shake sync to music
        if (!ForceMethhouseBossOst)
        {
            if (!StartSongsIsMidi)
            {
                musicAudio.ClearQueue(true);
                musicAudio.QueueAudio(Boss_Music[0]);
            }
            else
            {
                Singleton<MusicManagerMaes>.Instance.StopMidi();
                Singleton<MusicManagerMaes>.Instance.PlayMidi(ExhaustedJerBoss ? exhamidiStart : midiStart, loop: true);
                if (StartSongsIsMidi) Singleton<MusicManagerMaes>.Instance.QueueMidi(ExhaustedJerBoss ? exhamidiLoop : (switchToBloxyb ? bloxyLoop1 : midiLoop), true);
                Singleton<MusicManagerMaes>.Instance.SetSpeed(midiTempo);
            }
        }
        else
        {
            musicAudio.ClearQueue(true);
            musicAudio.QueueAudio(MethHouse_BossMusic[1]);
        }

        GameControllerScript.Instance.player.DefaultWalkSpeed += PlayerSpeed-GameControllerScript.Instance.player.DefaultWalkSpeed;
        GameControllerScript.Instance.player.DefaultRunSpeed += PlayerSpeed-GameControllerScript.Instance.player.DefaultRunSpeed;

        StartCoroutine(BeforeBossBegin());
    }
    public void PlaySomeMidi()
    {
        zs.agent.isStopped = true;
        debug = true;
        Singleton<MusicManagerMaes>.Instance.PlayMidi("bbcmboss End", false);
        
        StartCoroutine(BeforeBossAfterTotem());
        
    }
    private IEnumerator BeforeBossAfterTotem()
    {
        Singleton<MusicManagerMaes>.Instance.QueueMidi(switchToBloxyb && !ExhaustedJerBoss ? bloxyLoop1 : midiLoop, true);
        yield return new WaitForSeconds(12f); // Wait until the music will end
        health = maxHealth/3;
        Sych.SetGameWindowTitle("The fight Continues. yeah");
        SpawnProjectile(false, false);
        Singleton<MusicManagerMaes>.Instance.SetSpeed(midiTempo);
        if (spawnBlockagesDuringTheBossfight) blockages.SetActive(true);
        zs.totemAfterStun();
    }

    private IEnumerator BeforeBossBegin()
    {
        Sych.SetGameWindowTitle("The Basic Show - isnt it obvious hes charging his ult");
        gc.modeState = "he pissed";
        if (!ForceMethhouseBossOst)
        {
            if (!StartSongsIsMidi) 
            {
                yield return new WaitForSeconds(Boss_Music[1].audClip.length); // Wait until the music will end
                BossBegin(); // Start boss
            }
        }
        else
        {
            yield return new WaitForSeconds(MethHouse_BossMusic[1].audClip.length -0.01f);
            ableToStart = true;
        }
    }

    private void BossBegin()
    {
        Sych.SetGameWindowTitle("The fight begins. preferably now");
        AllowProjectileSpawn = true;
        SpawnProjectile(false, false);
        SpawnProjectile(false, false);
        gc.modeState = "in bossfight - " + health +"/"+ maxHealth+"hp";
        if (GameControllerScript.Instance.LapManag.Meeptimar.isActiveAndEnabled)
        {
            meepTimerScript.Instance.AddTime(25f, Color.green);
            meepTimerScript.Instance.canTime = true;
        }
        isbroyapping = false;
        if (debugMode) Debug.Log("Boss Loop");
        debug = false; // Disable debug to make null able to kill player
        spawnCooldown = 1f; // Set spawn cooldown to 1
        GameControllerScript.Instance.player.walkSpeed = PlayerSpeed;
        GameControllerScript.Instance.player.runSpeed = PlayerSpeed;
        if (zs.isActiveAndEnabled)
        {
            zs.Agent.speed = BossSpeed; // Set Null speed to BossSpeed (20)
            zs.Agent.isStopped = false; // Unstop Null agent
            zs.Target = GameControllerScript.Instance.player.transform; // Set Null target to Player
        }
        if (VertexShake) // If VertexShake bool enabled, start Vertex Shaking (wall shake)
        {
            Singleton<VertexGlitchManager>.Instance.BossStartShake(); // Start null angry vertex shake
            Singleton<VertexGlitchManager>.Instance.mustGlitch = true; // Make walls to shake
        }
        if (ForceMethhouseBossOst)
        {
            midiTempo = 1;
            Singleton<MusicManagerMaes>.Instance.QueueMidi(BossMidiLoopMethhouse, true,true);
            Singleton<MusicManagerMaes>.Instance.SetSpeed(midiTempo);
            realBossStarted = true; 
            musicAudio.ClearQueue(true);
            ShowCustomThings();
            return;
        }
        realBossStarted = true; // Real Boss Started
        musicAudio.ClearQueue(true); // Stop Intro/Hit music

        if (!Midi) // If Midi bool is not enabled
        {
            musicAudio.ClearQueue(true);
            musicAudio.SetLoop(true);
            musicAudio.QueueAudio(Boss_Music[2]);
             // Play normal music
        }
        else // If Midi bool is enabled
        {
            if (musicLoop != null) musicLoop.ClearQueue(true);
        }
        ShowCustomThings();
    }
    private void ShowCustomThings()
    {
        if (ShowHealthSlider) // If ShowHealthSlider bool enabled, show a health slider
        {
            if (healthSlider != null)
                healthSlider.gameObject.SetActive(true);
        }
        HealthbarAnimAppear.Play();
        if (spawnBlockagesDuringTheBossfight)
            blockages.SetActive(true);
        for (int a = 0; a < tweenOutitems.Length; ++a)
        {
            tweenOutitems[a].transform.DOMoveX(-3600, 5f);
        }
    }
    public void OnHit(float tiem, float hp = 1f, bool spawnExtraProjectile = true, bool DontClear = false) // When player hit null by a projectile
    {

        if (alreadyHit && !realBossStarted || (zs.iframes > 0f)) return;

        zs.Hit(!realBossStarted, tiem, (zs.totemready || !realBossStarted) && hp != 0 && hp != 1 ? 1 : hp);
        if (health <= maxHealth / 2 && !ok && switchToBloxyb)
        {
            AdditionalGameCustomizer.Instance.FovAmmount += 20;
            Singleton<MusicManagerMaes>.Instance.StopMidi();
            Singleton<MusicManagerMaes>.Instance.PlayMidi(bloxyLoop2, loop: true);
            Singleton<MusicManagerMaes>.Instance.SetSpeed(midiTempo);
            gc.ObjectsToEnable.ForEach(o => o.SetActive(true));
            
            StartCoroutine(easingeee(new Color(1f, 0.92f, 0.016f, 1f), 0, 2, 2));
            ok = true;
            RemoveProjectiles();
            yourflashbang.Rebind();
		    yourflashbang.Play("flashAnim", -1, 0f);
        }
        if (realBossStarted && Midi) midiTempo += (!switchToBloxyb ? 0.015f : 0.025f) * (zs.totemready && hp != 1 ? 1 : hp);
        if (GameControllerScript.Instance.LapManag.Meeptimar.isActiveAndEnabled) meepTimerScript.Instance.AddTime(zs.totemready ? 5f : 5f * hp,Color.green);
        Singleton<MusicManagerMaes>.Instance.SetSpeed(midiTempo);
        scoreSystemManager.Instance.AddScore(zs.totemready && hp != 0 && hp != 1 ? 275 : 275*(int)hp);
        debug = true; // Enable debug bool, to make null not able to kill player
        health -= (zs.totemready || !realBossStarted) && hp != 0 && hp != 1 ? 1 : hp; // Decreases null health
        gc.modeState = "in bossfight - " + health +"/"+ maxHealth+"hp";
        Singleton<OtherMainStuffManager>.Instance.AngerShit(1.5f * (zs.totemready && hp != 0 && hp != 1 ? 1 : hp)*LearningGameManager.Instance.angerMult, 0f,false, "mucho");
        Singleton<MusicManagerMaes>.Instance.HangMidi(true,true);
        if (realBossStarted && health > 1)
        {
            zs.ThrowProjectileFromBoss(!zs.totemready ? 3*((int)hp) : 5*(int)hp, !zs.totemready ? 0.5f/hp : 0.25f/hp);
            if (spawnExtraProjectile) SpawnProjectile(false, false);
        }
        if (health == 1)
        {
            oneHPfailsave = false;
            spawnCooldown = 5f;
            maxObjects = 5;
            zs.ThrowProjectileFromBoss(10,0.125f);
            Sych.SetGameWindowTitle(!zs.totemready ? "Last. Hit.  ?" : "His final chance");
            if (!DontClear)
            {
                RemoveProjectiles();
                SpawnProjectile(false, false);
            }
        }
            
        if (health <= 0) // If health is zero or less, game will load results after zerull/chair used totem
        {
            if (oneHPfailsave)
            {
                health = 1;
                oneHPfailsave = false;
                spawnCooldown = 5f;
                maxObjects = 5;
                zs.ThrowProjectileFromBoss(25,0.125f);
                Sych.SetGameWindowTitle(!zs.totemready ? "Last. Hit.  ?" : "His final chance");
                if (!DontClear)
                {
                    RemoveProjectiles();
                    SpawnProjectile(false, false);
                }
                return;
            }
            if (!zs.totemready)
            {
                Sych.SetGameWindowTitle("oh shit");
                zs.totem();
                zs.totemready = true;
                Singleton<MusicManagerMaes>.Instance.KillMidi();
                GameControllerScript.Instance.player.DefaultWalkSpeed += PlayerSpeed-GameControllerScript.Instance.player.DefaultWalkSpeed;
                GameControllerScript.Instance.player.DefaultRunSpeed += PlayerSpeed - GameControllerScript.Instance.player.DefaultRunSpeed;
                gc.modeState = "in bossfight - " + health +"/"+ maxHealth+"hp || oh no he used totem of undying";
                blockages.SetActive(false);
                return;
            }
            else
            BossEnd();
        }
        if (!realBossStarted)
        {
            StartHit(); // Piss the boss
            return;
        }
        return;
    }
    
    private void BossEnd()
    {
        HealthbarAnimDissapear.Play();
        Sych.SetGameWindowTitle("found dead in 2763 thing");
        gc.modeState = "you did it";
        
        Instantiate(zs.ExplodePrefab, zs.transform.position, zs.transform.rotation);
        zs.gameObject.SetActive(false);
        gc.ObjectsToEnable.ForEach(o => o.SetActive(false));

        musicAudio.ClearQueue(true);
        musicLoop.ClearQueue(true);

        Singleton<VertexGlitchManager>.Instance.mustGlitch = false;
        Shader.SetGlobalFloat("_VertexGlitchIntensity", 0f);
        Shader.SetGlobalFloat("_VertexGlitchSeed", 0f);

        blockages.SetActive(false);

        if (Midi) Singleton<MusicManagerMaes>.Instance.KillMidi();
        Singleton<VertexGlitchManager>.Instance.mustGlitch = false;
        Singleton<VertexGlitchManager>.Instance.Midi = false;
        gc.ElevdorRea.ForEach(ed => ed.finaleActivated = false);
        gc.Gatesrea.ForEach(g => g.Down(false));
        gc.ElevdorRea.ForEach(ed => ed.Opendor = true);
        if (gc.LapManag.Meeptimar.isActiveAndEnabled)
        {
            gc.LapManag.Meeptimar.AddTime(60f, Color.green);
            gc.LapManag.Meeptimar.inEnding = true;
            gc.LapManag.Meeptimar.canTime = false;
        }
        PlayerPrefsExtension.SetBool("BeatedUpZerull", true);
        AllowProjectileSpawn = false;
        RemoveProjectiles();
        RemoveItems();
        GameControllerScript.Instance.player.DefaultWalkSpeed += PlayerSpeed-GameControllerScript.Instance.player.DefaultWalkSpeed;
        GameControllerScript.Instance.player.DefaultRunSpeed += PlayerSpeed - GameControllerScript.Instance.player.DefaultRunSpeed;
    }
    public void ReviveHim()
    {
        PlayerPrefsExtension.SetBool("BeatedUpZerull", false);
    }
    public void AfterHit()
    {
        debug = false;
        if (health != 1) Singleton<MusicManagerMaes>.Instance.HangMidi(false);
        else 
        {
            //Singleton<MusicManagerMaes>.Instance.StopMidi();
            Singleton<MusicManagerMaes>.Instance.HangMidi(stop: true, keepDrums: true);
            //Singleton<MusicManagerMaes>.Instance.QueueMidi(switchToBloxyb ? bloxyLoop1 : midiLoop, true);
            //Singleton<MusicManagerMaes>.Instance.SetSpeed(midiTempo);
        }
    }
    public GameObject SpawnProjectile(Transform transform, bool noRandom = false, int projectileVal = 0) => Instantiate<GameObject>(projectileprefabs[noRandom ? projectileVal : Random.Range(0, projectileprefabs.Length)], transform.position, Quaternion.identity);

    public void SpawnProjectile(bool destroyOthers, bool leftOnlyOne)
    {
        if (destroyOthers)
        {
            RemoveProjectiles();
            if (leftOnlyOne)
            {
                maxObjects = 1;
                objects = 1;
            }
        }
        int projectileID = Random.Range(0, projectilespawns.Length);
        if (debugMode)
        {
            Debug.Log(string.Format("Projectile appeared at projectile point {0}!", projectileID));
        }
        GameObject projectile = SpawnProjectile(projectilespawns[projectileID]);
        projectilesDoNotExist = false;
        if (!leftOnlyOne)
        {
            objects++;
        }
    }
    public ZerullBossScript GetBoss() => zs;
    public void dosomeupdatebitch()
    {
        bool ChaosMode = PlayerPrefsExtension.GetBool("Chaos");
        if (gc.mode == "zerullclassic")
        {
            zer.SetActive(true);
            //if (!ChaosMode)
            //{
                StartCoroutine(easingeee(new Color(0.5f, 0.5f, 0.5f, 1f), 0, 2, 2));

                gc.notebookCount.color = Color.Lerp(Color.white, new Color(0.55f, 0.55f, 0.55f, 1f), 1 - Mathf.Repeat(1f, 0.2f));
                ItemManager.Instance.ItemNameText.color = Color.Lerp(Color.white, new Color(0.55f, 0.55f, 0.55f, 1f), 1 - Mathf.Repeat(1f, 0.2f));
            //}
            if (gc.warrealest) gc.LapManag.MeepTimer.SetActive(true);
        }
    }
    public void jusUpdatebr()
    {
        Vector3 DropPosition = GameControllerScript.Instance.player.dropItemPos.position;
        DropPosition.y = 4;
        Vector3 iguess = new Vector3(DropPosition.x - 1.5f, DropPosition.y, DropPosition.z - 1.5f);
        Vector3 iguess2 = new Vector3(DropPosition.x - 1.5f, DropPosition.y, DropPosition.z + 1.5f);
        if (gc.notebooks == 1)
        {
            Sych.SetGameWindowTitle("Locked in his zone");
            gc.Gatesrea.ForEach(g => g.Down());
        }
        if (gc.notebooks == 2)  Sych.SetGameWindowTitle("Sych.MessageFromHim(true) >= Welcome to your personal hell, i guess");
        if (gc.exitsReached < 1) 
        {
            if (gc.notebooks == 3 || gc.notebooks == 5 || gc.notebooks == 8 || gc.notebooks == 11 || gc.notebooks == 12 || gc.notebooks == 13 || gc.notebooks == 14) ItemManager.Instance.CreateDroppedItem(2,DropPosition,true,true);
            ItemManager.Instance.CreateDroppedItem(5,DropPosition);
        }
        if (gc.notebooks == gc.maxNotebooks && gc.exitsReached < 6) 
        {
            ItemManager.Instance.CreateDroppedItem(2,DropPosition,true,true);
            ItemManager.Instance.CreateDroppedItem(2,DropPosition,true,true);
            ItemManager.Instance.CreateDroppedItem(5,iguess);
            ItemManager.Instance.CreateDroppedItem(5,DropPosition);
            ItemManager.Instance.CreateDroppedItem(5,iguess2);
        }
        if (gc.exitsReached == 3) replaceALLSAKES = true;
        if (gc.exitsReached == 5)
        {
            Sych.SetGameWindowTitle("You won............................. maybe?");
            replaceALLSAKES = false;
            zer.SetActive(false);
        }
    }
    public IEnumerator easingeee(Color kolor, float a, float b, float duration)
    {
        Color start = RenderSettings.ambientLight;

        for (float t = a; t < b; t += Time.deltaTime / duration)
        {
            RenderSettings.ambientLight = Color.Lerp(start, kolor, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        RenderSettings.ambientLight = kolor;
    }

}
