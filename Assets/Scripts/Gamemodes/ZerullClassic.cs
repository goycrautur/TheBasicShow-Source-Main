using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using FluidMidi;
using TMPro;
using DG.Tweening;

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

    public float health = 10f;

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

    private float spawnCooldown,curHealthValueForLerping;

    [Header("Blockages")]
    public bool spawnBlockagesDuringTheBossfight;
    public GameObject blockages;
    public GameObject[] RandomBlockages;
    [Tooltip("Boss variant of null"), SerializeField] public ZerullBossScript zs;
    [Tooltip("If it's set to null, game will use optimized version")] public Slider healthSlider;

    [Header("Audio"), SerializeField, Tooltip("Boss Music (Boss Clips located at NullScript object)")]
    private AudioClip[] Boss_Music;
    [SerializeField, Tooltip("Music")] private AudioSource musicAudio, musicLoop;

    [Header("the fucking midi tempo"), Tooltip("do i have to elaborate about what this does")] public float midiTempo = 1f;

    [HideInInspector] public bool debug;
    [Header("Bonus")]
    [SerializeField, Tooltip("Does the walls must shake during the bossfight.")] private bool VertexShake = true;
    [SerializeField, Tooltip("Does the game will show a health slider")] private bool ShowHealthSlider = false;

    [Tooltip("Does the game will play a sound, when projectile thrown. [SET THE SOUND IN PROJECTILE PREFABS]")] public bool playSoundWhenProjectileThrown = false;

    [Tooltip("Debug Mode")] public bool debugMode = false;

    [SerializeField, Tooltip("Does the start & hit song is a midi version.")] private bool StartSongsIsMidi = true;

    [SerializeField, Tooltip("Does the loop song is a midi version.")] private bool Midi = true;

    [Tooltip("BTM of the Midi")] public float Midi_BTM = 120f;

    [Tooltip("Boss Intro MIDI version")] public StreamingAsset midiIntro;

    [Tooltip("Boss Start MIDI version")] public StreamingAsset midiStart;

    [Tooltip("Boss Loop MIDI version")] public StreamingAsset midiLoop;
    [Tooltip("Boss Loop MIDI the bloxyver")] public StreamingAsset bloxyLoop1,bloxyLoop2;

    [Tooltip("Length of Start MIDI")] public float midiStartLength = 7f;
    public GameObject[] tweenOutitems,tweenitemsAlt;
    public Transform[] StartingBossfightProjectileSpawnLocaltion;

    [Header("Shaders")]
    [SerializeField] private Material CellingMat;
    [SerializeField] private Material NoLightCeilingMat,WallMat, FloorMat, FenceMat, CarpetMat, NoLightCeilingMatGlitch, CellingMatGlitch, WallMatGlitch, FloorMatGlitch, FenceMatGlitch, CarpetMatGlitch;
    public SongPlayer normalMidiPlayer, drumsMidiPlayer, normalMidiPlayerLoop;
    public bool bossStarted, realBossStarted,switchToBloxyb;
    public Animator yourflashbang;

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
        health = maxHealth;
        bool shakeswitchFR = PlayerPrefsExtension.GetBool("WallShakeSwitch");
        bool shake = PlayerPrefsExtension.GetBool("WallShake");
        bool blox = PlayerPrefsExtension.GetBool("bloxy");
        if(!shakeswitchFR )
        {
        PlayerPrefsExtension.SetBool("WallShake", true);
        }
        VertexShake = !shakeswitchFR ? true : shake;
        switchToBloxyb = blox;
        Singleton<VertexGlitchManager>.Instance.mustGlitch = false;
        if (Midi)
        {
            if (zs != null)
            {
                zs.DrumsMidi = true;
            }
            Singleton<VertexGlitchManager>.Instance.Midi = true;
        }
    }

    private void Update()
    {
        curHealthValueForLerping = Mathf.Lerp(curHealthValueForLerping,health, 5*Time.deltaTime);
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
            if (healthSlider != null)
            {
                if (healthSlider.value != curHealthValueForLerping)
                {
                    healthSlider.value = curHealthValueForLerping;
                }
            }
        }
        if (AllowProjectileSpawn)
        {
            if (objects > maxObjects)
            {
                objects = maxObjects;
            }
            if (spawnCooldown > 0f)
            {
                spawnCooldown -= Time.deltaTime;
            }
            if (spawnCooldown <= 0f)
            {
                if (objects < maxObjects)
                {
                    SpawnProjectile(false, false);
                }
                spawnCooldown = 5f;
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
        if (debugMode)
        {
            Debug.Log("Bossfight");
        }
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
        gc.modeState = "oh no he got last exit what do i do";
        PlayerPrefsExtension.SetBool("WallShakeSwitch", true);
        foreach (WindowScript w in FindObjectsOfType<WindowScript>())
		{
			if (!w.broken)
			{
				w.Window(true, false, 0f);
			}
		}
        if (GameControllerScript.Instance.LapManag.Meeptimar.isActiveAndEnabled)
        {
            meepTimerScript.Instance.AddTime(25f, Color.green);
            meepTimerScript.Instance.canTime = false;
        }
        if (debugMode)
        {
            Debug.Log("Encounter");
        }

        debug = true; // Makes that null doesn't able to kill player 

        bossStarted = true; // Boss Started bool enabled
        for (int i = 0; i < StartingBossfightProjectileSpawnLocaltion.Length; ++i)
        {
            SpawnProjectile(StartingBossfightProjectileSpawnLocaltion[i], true, Random.Range(2, projectileprefabs.Length));
        }
        zs.Agent.speed = 0f; // Set Null speed to 0. (make null doesn't able to move)
        zs.Agent.isStopped = true; // Fully stop Null agent
        zs.sprite.gameObject.SetActive(true); // Enable Null Sprite
        GameControllerScript.Instance.player.DefaultWalkSpeed += PlayerSpeed-GameControllerScript.Instance.player.DefaultWalkSpeed;
        GameControllerScript.Instance.player.DefaultRunSpeed += PlayerSpeed-GameControllerScript.Instance.player.DefaultRunSpeed;
        if (!StartSongsIsMidi)
        {
            PlayMusic(musicAudio, Boss_Music[0], true);
        }
        else
        {
            normalMidiPlayer = Singleton<MusicManager>.Instance.PlayNormalMidi(midiIntro, true);
            //Singleton<MusicManager>.Instance.SetSpeed(midiTempo, normalMidiPlayer);
            Singleton<MusicManager>.Instance.SetSpeed(midiTempo, normalMidiPlayer, null);
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
        gc.modeState = "!!?!?!@?!?@!?@?!??!@!?@!?@!?@? HE GOT HURT";
        if (debugMode)
            Debug.Log("Boss Pissed");

        isbroyapping = true;
        GlitchShaders(true);

        RemoveProjectiles(); // Remove projectiles

        Singleton<VertexGlitchManager>.Instance.sourceToSyncIn = musicLoop; // Make vertex shake sync to music

        if (!StartSongsIsMidi)
        {
            PlayMusic(musicAudio, Boss_Music[1], false);
        }
        else
        {
            Singleton<MusicManager>.Instance.StopMidi(false, normalMidiPlayer, null);

            normalMidiPlayer = Singleton<MusicManager>.Instance.PlayNormalMidi(midiStart, false);
            Singleton<MusicManager>.Instance.SetSpeed(midiTempo, normalMidiPlayer, null);
        }

        GameControllerScript.Instance.player.DefaultWalkSpeed += PlayerSpeed-GameControllerScript.Instance.player.DefaultWalkSpeed;
        GameControllerScript.Instance.player.DefaultRunSpeed += PlayerSpeed-GameControllerScript.Instance.player.DefaultRunSpeed;

        StartCoroutine(BeforeBossBegin());
    }

    private IEnumerator BeforeBossBegin()
    {
        gc.modeState = "he pissed";
        yield return new WaitForSeconds(StartSongsIsMidi ? (midiStartLength/midiTempo) : Boss_Music[1].length); // Wait until the music will end
        BossBegin(); // Start boss
    }

    private void BossBegin()
    {
        AllowProjectileSpawn = true;
        gc.modeState = "in bossfight - " + health +"/"+ maxHealth+"hp";
        if (GameControllerScript.Instance.LapManag.Meeptimar.isActiveAndEnabled)
        {
            meepTimerScript.Instance.AddTime(25f, Color.green);
            meepTimerScript.Instance.canTime = true;
        }
        isbroyapping = false;
        if (debugMode)
        {
            Debug.Log("Boss Loop");
        }

        debug = false; // Disable debug to make null able to kill player


        spawnCooldown = 1f; // Set spawn cooldown to 1

        GameControllerScript.Instance.player.walkSpeed = PlayerSpeed;
        GameControllerScript.Instance.player.runSpeed = PlayerSpeed;

        zs.Agent.speed = BossSpeed; // Set Null speed to BossSpeed (20)
        zs.Agent.isStopped = false; // Unstop Null agent
        zs.Target = GameControllerScript.Instance.player.transform; // Set Null target to Player

        if (VertexShake) // If VertexShake bool enabled, start Vertex Shaking (wall shake)
        {
            Singleton<VertexGlitchManager>.Instance.BossStartShake(); // Start null angry vertex shake
            Singleton<VertexGlitchManager>.Instance.mustGlitch = true; // Make walls to shake
            if (Midi)
            {
                Singleton<VertexGlitchManager>.Instance.MidiBTM = Midi_BTM * midiTempo;
            }
        }

        realBossStarted = true; // Real Boss Started

        musicAudio.Stop(); // Stop Intro/Hit music

        if (!Midi) // If Midi bool is not enabled
        {
            PlayMusic(musicLoop, Boss_Music[2], true); // Play normal music
        }
        else // If Midi bool is enabled
        {
            if (musicLoop != null) // Play midi music
            {
                musicLoop.Stop();
            }
            if (StartSongsIsMidi)
            {
                Singleton<MusicManager>.Instance.StopMidi(false, normalMidiPlayer, null);
            }
            normalMidiPlayerLoop = Singleton<MusicManager>.Instance.PlayMidi(switchToBloxyb ? bloxyLoop1 : midiLoop, true);
            drumsMidiPlayer = Singleton<MusicManager>.Instance.PlayDrumsMidi(switchToBloxyb ? bloxyLoop1 : midiLoop, true, normalMidiPlayerLoop);
            Singleton<MusicManager>.Instance.SetSpeed(midiTempo, normalMidiPlayerLoop, drumsMidiPlayer);
            Singleton<MusicManager>.Instance.GainDrums(zs.transform, drumsMidiPlayer);
            Singleton<MusicManager>.Instance.SeekToDrums(normalMidiPlayerLoop, drumsMidiPlayer);
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
        if (spawnBlockagesDuringTheBossfight)
            blockages.SetActive(true);
        for (int a = 0; a < tweenOutitems.Length; ++a)
        {
            tweenOutitems[a].transform.DOMoveX(-700, 3f);
        }
        float ratioy = (float)Screen.width / 360f;
        tweenitemsAlt[0].transform.DOMoveY(ratioy*175, 3f);
        tweenitemsAlt[1].transform.DOMoveY(ratioy*45, 3f);
    }
    public void OnHit(float tiem, float hp = 1f) // When player hit null by a projectile
    {

        if (alreadyHit && !realBossStarted || (zs.iframes > 0f))
            return;

        zs.Hit(!realBossStarted, tiem, zs.totemready ? 1 : hp);
        if (health <= maxHealth / 2 && !ok && switchToBloxyb)
        {
            AdditionalGameCustomizer.Instance.FovAmmount = 80;
            Singleton<MusicManager>.Instance.StopMidi(true, null, null);
            normalMidiPlayerLoop = Singleton<MusicManager>.Instance.PlayMidi(bloxyLoop2, true);
            drumsMidiPlayer = Singleton<MusicManager>.Instance.PlayDrumsMidi(bloxyLoop2, true, normalMidiPlayerLoop);
            Singleton<MusicManager>.Instance.SetSpeed(midiTempo, normalMidiPlayerLoop, drumsMidiPlayer);
            Singleton<MusicManager>.Instance.GainDrums(zs.transform, drumsMidiPlayer);
            Singleton<MusicManager>.Instance.SeekToDrums(normalMidiPlayerLoop, drumsMidiPlayer);
            gc.ObjectsToEnable.ForEach(o => o.SetActive(true));
            
            StartCoroutine(easingeee(new Color(1f, 0.92f, 0.016f, 1f), 0, 2, 2));
            ok = true;
            RemoveProjectiles();
            yourflashbang.Rebind();
		    yourflashbang.Play("flashAnim", -1, 0f);
        }
        if (realBossStarted && Midi)
        {
            midiTempo += (!switchToBloxyb ? 0.02f : 0.03f) * (zs.totemready ? 1 : hp);
            Singleton<MusicManager>.Instance.SetSpeed(0.001f, normalMidiPlayerLoop, null);
        }
        if (GameControllerScript.Instance.LapManag.Meeptimar.isActiveAndEnabled)
        {
            meepTimerScript.Instance.AddTime(zs.totemready ? 10f : 5f * hp,Color.green);
        }
        scoreSystemManager.Instance.AddScore(zs.totemready ? 275 : 275*(int)hp);
        debug = true; // Enable debug bool, to make null not able to kill player
        health -= zs.totemready ? 1 : hp; // Decreases null health
        gc.modeState = "in bossfight - " + health +"/"+ maxHealth+"hp";
        Singleton<OtherMainStuffManager>.Instance.AngerShit(1.5f * (zs.totemready ? 1 : hp), 0f,false, "mucho");
            
        if (health <= 0) // If health is zero or less, game will load results after zerull/chair used totem
        {
            if (!zs.totemready)
            {
                zs.totem();
                health = 10;
                zs.totemready = true;
                gc.modeState = "in bossfight - " + health +"/"+ maxHealth+"hp || oh no he used totem of undying";
            }
            else
            BossEnd();
        }
        return;
    }
    private void BossEnd()
    {
        gc.modeState = "you did it";
        zs.gameObject.SetActive(false);
        gc.ObjectsToEnable.ForEach(o => o.SetActive(false));

        musicAudio.Stop();
        musicLoop.Stop();

        GlitchShaders(false);
        Singleton<VertexGlitchManager>.Instance.mustGlitch = false;
        Shader.SetGlobalFloat("_VertexGlitchIntensity", 0f);
        Shader.SetGlobalFloat("_VertexGlitchSeed", 0f);

        if (spawnBlockagesDuringTheBossfight)
            blockages.SetActive(false);

        if (Midi)
        {
            Singleton<MusicManager>.Instance.StopMidi(true, null, null);
        }
        Singleton<VertexGlitchManager>.Instance.mustGlitch = false;
        if (Midi)
        {
            Singleton<VertexGlitchManager>.Instance.Midi = false;
        }
        gc.ElevdorRea.ForEach(ed => ed.finaleActivated = false);
        gc.Gatesrea.ForEach(g => g.Down(false));
        gc.ElevdorRea.ForEach(ed => ed.Opendor = true);
        if (gc.LapManag.Meeptimar.isActiveAndEnabled)
        {
            gc.LapManag.Meeptimar.AddTime(60f, Color.green);
            gc.LapManag.Meeptimar.inEnding = true;
            gc.LapManag.Meeptimar.canTime = false;
            gc.warmusic.Stop();
        }
        PlayerPrefsExtension.SetBool("BeatedUpZerull", true);
        AllowProjectileSpawn = false;
        GameControllerScript.Instance.player.DefaultWalkSpeed += PlayerSpeed-GameControllerScript.Instance.player.DefaultWalkSpeed;
        GameControllerScript.Instance.player.DefaultRunSpeed += PlayerSpeed - GameControllerScript.Instance.player.DefaultRunSpeed;
        float ratioy = (float)Screen.width / 360f;
        tweenitemsAlt[0].transform.DOMoveY(ratioy*425, 3f);
    }
    public void AfterHit()
    {
        if (!realBossStarted)
        {
            StartHit(); // Piss the boss
            return;
        }
        else
        {
            debug = false;
            if (Midi)
            {
                if (health > 1)
                {
                    Singleton<MusicManager>.Instance.SetSpeed(midiTempo, normalMidiPlayerLoop, drumsMidiPlayer);
                    Singleton<VertexGlitchManager>.Instance.MidiBTM = Midi_BTM * midiTempo;
                    Singleton<MusicManager>.Instance.SeekToDrums(normalMidiPlayerLoop, drumsMidiPlayer);
                }
                if (health == 1)
                {
                    Singleton<MusicManager>.Instance.SetSpeed(0.001f, normalMidiPlayerLoop, null);
                    Singleton<VertexGlitchManager>.Instance.MidiBTM = Midi_BTM * midiTempo;
                }
            }

            if (health == 1)
            {
                spawnCooldown = 5f;
                maxObjects = 5;
                RemoveProjectiles();
                RemoveItems();
            }
        }
    }
    private void PlayMusic(AudioSource source, AudioClip clip, bool loop = false)
    {
        source.clip = clip;
        source.loop = loop;
        source.Play();
    }
    public GameObject SpawnProjectile(Transform transform, bool noRandom = false, int projectileVal = 0) => Instantiate<GameObject>(projectileprefabs[noRandom ? projectileVal : Random.Range(0, projectileprefabs.Length)], transform.position, Quaternion.identity);

    public void SpawnProjectile(bool destroyOthers, bool leftOnlyOne)
    {
        if (destroyOthers)
        {
            RemoveProjectiles();
            RemoveItems();
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
    public void GlitchShaders(bool toggle)
    {
        Material noligCeliMat = toggle ? NoLightCeilingMatGlitch : NoLightCeilingMat;
        Material CelingMat = toggle ? CellingMatGlitch : CellingMat;
        Material WalMat = toggle ? WallMatGlitch : WallMat;
        Material FlorMat = toggle ? FloorMatGlitch : FloorMat;
        Material fencMat = toggle ? FenceMatGlitch : FenceMat;
        Material carpMat = toggle ? CarpetMatGlitch : CarpetMat;
        MeshRenderer[] renderers = Object.FindObjectsOfType<MeshRenderer>(true);

        foreach (MeshRenderer renderer in renderers)
        {
            Material currentMat = renderer.sharedMaterial ?? renderer.material;
            if (currentMat.name.StartsWith(NoLightCeilingMat.name))
            {
                renderer.sharedMaterial = noligCeliMat;
            }
            if (currentMat.name.StartsWith(CellingMat.name))
            {
                renderer.sharedMaterial = CelingMat;
            }
            else if (currentMat.name.StartsWith(WallMat.name))
            {
                renderer.sharedMaterial = WalMat;
            }
            else if (currentMat.name.StartsWith(FloorMat.name))
            {
                renderer.sharedMaterial = FlorMat;
            }
            else if (currentMat.name.StartsWith(FenceMat.name))
            {
                renderer.sharedMaterial = fencMat;
            }
            else if (currentMat.name.StartsWith(CarpetMat.name))
            {
                renderer.sharedMaterial = carpMat;
            }
        }
    }
    public void dosomeupdatebitch()
    {
        bool ChaosMode = PlayerPrefsExtension.GetBool("Chaos");
        if (gc.mode == "zerullclassic")
        {
            GlitchShaders(false);
            zer.SetActive(true);
            //if (!ChaosMode)
            //{
                StartCoroutine(easingeee(new Color(0.5f, 0.5f, 0.5f, 1f), 0, 2, 2));

                gc.notebookCount.color = Color.Lerp(Color.white, new Color(0.55f, 0.55f, 0.55f, 1f), 1 - Mathf.Repeat(1f, 0.2f));
                ItemManager.Instance.ItemNameText.color = Color.Lerp(Color.white, new Color(0.55f, 0.55f, 0.55f, 1f), 1 - Mathf.Repeat(1f, 0.2f));
            //}
            if (gc.warrealest)
            {
                gc.LapManag.MeepTimer.SetActive(true);
            }
        }
    }
    public void jusUpdatebr()
    {
        if (gc.notebooks == 1)
        {
            gc.Gatesrea.ForEach(g => g.Down());
        }
        if (gc.exitsReached == 3)
        {
            replaceALLSAKES = true;
        }
        if (gc.exitsReached == 5)
        {
            replaceALLSAKES = false;
            for (int i = 0; i < ItemManager.Instance.Inventory.Length; i++)
            {
                zer.SetActive(false);
            }
        }
    }
    public IEnumerator easingeee(Color kolor, float a, float b, float duration)
    {
        Color start = gc.voxLight.ambientLight;

        for (float t = a; t < b; t += Time.deltaTime / duration)
        {
            gc.voxLight.ambientLight = Color.Lerp(start, kolor, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        gc.voxLight.ambientLight = kolor;
    }
}
