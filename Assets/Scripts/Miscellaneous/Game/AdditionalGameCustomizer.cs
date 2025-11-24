using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class AdditionalGameCustomizer : MonoBehaviour
{
    #region UnityCallbacks
    private void Awake() => Instance = this;

    private void Start()
    {
        InitializeCustomAdditions();
        SkyBoxHandling();
        ScrambleItems();
        InitializeGameTuff();

    }
    public bool unloc, iteinfo,captio;
    private void InitializeGameTuff()
    {
        unloc = PlayerPrefsExtension.GetBool("thonkPad");
        iteinfo = PlayerPrefsExtension.GetBool("ItemInfo");
        captio = PlayerPrefsExtension.GetBool("Captions");
        NoYCTP = !unloc;
        ItemInfoShit = iteinfo;
        Subtitles = captio;
        ItemInfostuffahah.SetActive(ItemInfoShit);
        subtitlesCanvas.SetActive(Subtitles);
        bool chair = PlayerPrefsExtension.GetBool("BeatedUpZerull");
        if (GameControllerScript.Instance.mode == "endless")
        {
            modesText.text = "Endless Mode";
        }
        if (GameControllerScript.Instance.mode == "story")
        {
            modesText.text = "Story Mode";
            ModifierText.text = "Modifier used: mucho";
        }
        if (GameControllerScript.Instance.mode == "famished")
        {
            NoYCTP = true;
            modesText.text = "Famished Butch Takeover";
        }
        if (GameControllerScript.Instance.mode == "zerullclassic")
        {
            NoYCTP = false;
            modesText.text = chair ? "c  h  a  i  r" : "?eru?? M0D3";
        }
        if (GameControllerScript.Instance.mode == "LappingOfAsylum")
        {
            modesText.text = "Lapping Of Asylum";
        }
    }

    private void Update()
    {
        therainbo();
        CameraShaking();
        FlashlightCode();
        StaminaStyleHandling();
        KeyFunctions();
        CurrencySystem();
        PercentageSystemShit();
        speedtextmf.text = Singleton<InputManager>.Instance.GetActionKey(InputAction.Run) && GameControllerScript.Instance.player.stamina > 0f ? ""+GameControllerScript.Instance.player.runSpeed : ""+GameControllerScript.Instance.player.walkSpeed;
        spee.sprite = Singleton<InputManager>.Instance.GetActionKey(InputAction.Run) && GameControllerScript.Instance.player.stamina > 0f ? run2 : run1;
        defmultText.text = GameControllerScript.Instance.player.PlayerDmgResistance+"X";
    }
    #endregion
    public void therainbo()
    {
        if (!rainbowTime)
        {
            rainboCanv.color = donthaveanamelmfao;
        }
        if (rainbowTime) // haha fuck you groomcradia im gonna stole your code
        {
            if (rainboCanv.color.a < 0.1f)
            {
                rainboCanv.color = new Color(1f, 0f, 0f, rainboCanv.color.a + (rainboSpee * Time.deltaTime));
            }
            Color.RGBToHSV(rainboCanv.color, out huehuehue, out saturati, out brignes);
            huehuehue += rainboSpee * Time.deltaTime;
            if (huehuehue > 1f)
            {
                huehuehue = 0f;
            }
            rainboCanv.color = Color.HSVToRGB(huehuehue, saturati, brignes);
            rainboCanv.color = new Color(rainboCanv.color.r, rainboCanv.color.g, rainboCanv.color.b, transparenci);
        }
    }
    private void PercentageSystemShit()
    {
        if (StaminaPercentage && !ZerullClassic.Instance.BossStarted)
        {
            percentageText.text = (int)GameControllerScript.Instance.player.stamina + "%";
            if (GameControllerScript.Instance.player.stamina <= 15f)
            {
                percentageText.color = Color.red;
            }
            else
            {
                percentageText.color = Color.black;
            }
        }
        if (ZerullClassic.Instance.BossStarted || ZerullClassic.Instance.RealBossStarted)
        {
            percentageText.text = "∞%";
        }
        if (HealthPercentage)
        {
            healthPercentageText.text = GameControllerScript.Instance.player.health + "/" + GameControllerScript.Instance.player.maxHealth;
            if (GameControllerScript.Instance.player.health <= 15f)
            {
                healthPercentageText.color = Color.red;
            }
            else
            {
                healthPercentageText.color = Color.white;
            }
        }
    }

    #region Initialization
    private void InitializeCustomAdditions()
    {
        TMP.SetActive(OldDetentionTimer);
        Clock.SetActive(!OldDetentionTimer);
        GaugeManager.SetActive(Gauges);
        staminapercent.SetActive(StaminaPercentage);
        healthpercent.SetActive(HealthPercentage);
    }
    #endregion

    #region VisualEffects
    private void CameraShaking()
    {
        GameObject cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        if (cameraObject != null)
        {
            Camera cameraComponent = cameraObject.GetComponent<Camera>();
            if (cameraComponent != null)
            {
                
                cameraComponent.fieldOfView = CameraShake ? Random.Range(58, 62) : Mathf.Lerp(cameraComponent.fieldOfView, FovAmmount, 5f * Time.deltaTime);
            }
        }
    }

    private void FlashlightCode()
    {
        GameObject cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        if (cameraObject != null)
        {
            Light light = cameraObject.GetComponent<Light>();
            if (light != null)
            {
                light.enabled = isFlashlightOn;
            }
        }
    }
    #endregion

    #region StaminaManagement
    private void StaminaStyleHandling()
    {
        var staminaMap = new Dictionary<StaminaDisplay, GameObject>
        {
            { StaminaDisplay.Old, OldStamina},
            { StaminaDisplay.PreOld, PreOldStamina},
            { StaminaDisplay.Normal, NewStamina},
            { StaminaDisplay.Vertical, VerticalStamina },
            { StaminaDisplay.Circle, CircleStamina }
        };

        OldStamina.SetActive(false);
        PreOldStamina.SetActive(false);
        NewStamina.SetActive(false);
        VerticalStamina.SetActive(false);
        CircleStamina.SetActive(false);

        if (staminaMap.ContainsKey(StaminaStyle))
        {
            staminaMap[StaminaStyle].SetActive(true);
        }

        if (StaminaStyle == StaminaDisplay.Old)
        {
            bool YouNeedRest = GameControllerScript.Instance.player.stamina < 0f;
            if (warning.activeSelf != YouNeedRest)
            {
                warning.SetActive(YouNeedRest);
            }
        }
    }
    #endregion

    #region InputHandling
    private void KeyFunctions()
    {
        if (Time.timeScale == 0f) return;

        if (Input.GetKeyDown(KeyCode.R) && ItemDropping)
        {
            int selectedSlot = ItemManager.Instance.ItemSelection;
            if (ItemManager.Instance.Inventory[selectedSlot].ItemInstance != null)
            {
                ItemManager.Instance.DropItem(selectedSlot);
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Singleton<VertexGlitchManager>.Instance.Glitch();
        }

        if (FlashLight && Input.GetKeyDown(KeyCode.F))
        {
            isFlashlightOn = !isFlashlightOn;
        }
    }
    #endregion

    #region SkyboxManagement
    private void SkyBoxHandling()
    {
        switch (SetSkybox)
        {
            case SkyboxStyle.Default:
                RenderSettings.skybox = DefaultSky;
                currentSkybox = SkyboxStyle.Default;
                break;
            case SkyboxStyle.Day:
                RenderSettings.skybox = NormalSky;
                currentSkybox = SkyboxStyle.Day;
                break;
            case SkyboxStyle.Sunset:
                RenderSettings.skybox = TwilightSky;
                currentSkybox = SkyboxStyle.Sunset;
                break;
            case SkyboxStyle.Night:
                RenderSettings.skybox = NightSky;
                currentSkybox = SkyboxStyle.Night;
                break;
        }
    }
    #endregion

    #region Currency
    private void CurrencySystem()
    {
        if (ReworkedCurrency)
        {
            Counter.SetActive(true);
            AudioSource audioDevice = GameControllerScript.Instance.audioDevice;
            currencyCounter.text = "$" + Cash.ToString("F2");

            if (Cash >= 0.25)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (Sych.ScreenRaycastMatchesTag("VendingMachine", out RaycastHit hit, GameControllerScript.Instance.player.LocalRange))
                    {
                        var vendingMachine = hit.collider.GetComponent<VendingMachineScript>();
                        if (vendingMachine != null)
                        {
                            if (!ItemManager.Instance.IsInventoryFull())
                            {
                                Cash = Cash - 0.25;
                                audioDevice.PlayOneShot(aud_Drop);
                                vendingMachine.DispenseItem();
                            }
                        }
                    }
                    else if (Sych.ScreenRaycastMatchesTag("Phone", out hit, GameControllerScript.Instance.player.LocalRange))
                    {
                        var tapePlayer = hit.collider.GetComponent<TapePlayerScript>();
                        if (tapePlayer != null)
                        {
                            Cash = Cash - 0.25;
                            audioDevice.PlayOneShot(aud_Drop);
                            tapePlayer.Play();
                        }
                    }
                }
            }
        }
        else
        {
            Counter.SetActive(false);
        }
    }
    #endregion

    #region RandomizedItems
    private void ScrambleItems()
    {
        if (RandomizeItems)
        {
            List<Vector3> list = new List<Vector3>();

            foreach (PickupScript pickupScript in FindObjectsOfType<PickupScript>())
            {
                if (pickupScript.gameObject != quarter && !pickupScript.SpawnAtRandom)
                {
                    list.Add(pickupScript.transform.position);
                }
            }
            foreach (PickupScript pickupScript2 in FindObjectsOfType<PickupScript>())
            {
                if (pickupScript2.gameObject != quarter && !pickupScript2.SpawnAtRandom)
                {
                    int index = Random.Range(0, list.Count);
                    pickupScript2.transform.position = list[index];
                    list.RemoveAt(index);
                }
            }
        }
    }
    #endregion

    #region SerializedFields
    [Header("Gameplay Addons")]
    public EscapeFunsies EscapeMusicFunsies = EscapeFunsies.BBCR;
    public bool RandomizeJumps;
    public bool rainbowTime;
    public Color donthaveanamelmfao, darkencanva, canvascolormain, zaColor;
    public bool NoYCTP, DetentionAfterScissorUse, AnOldRule, ItemDropping, SkipCraftersAttack, ReworkedCurrency, RandomizeItems, ItemInfoShit,Subtitles;
    public Image spee;
    public Sprite run1, run2, invincibl, dorMapLockedSprite, dorMapSprite;
    public Texture2D itemMapSprite,SpecialItemMapSprite;

    [Header("Visual Addons")]
    public StaminaDisplay StaminaStyle = StaminaDisplay.Normal;
    public bool RandomizeBookColor, Indicator, FinalModeTV, Gauges, OldDetentionTimer, FlashLight, CameraShake, StaminaPercentage, HealthPercentage;
    public SkyboxStyle SetSkybox = SkyboxStyle.Day;

    [Header("Serialized References")]
    public Image[] ExitImages;
    public Image rainboCanv;
    public float rainboSpee, huehuehue, saturati, brignes, transparenci, FovAmmount;
    public Sprite[] BookColors;
    public Material NormalSky, NormalRedSky, NightSky, RedNightSky, TwilightSky, RedTwilightSky, DefaultSky;
    [SerializeField] private GameObject warning, Clock, TMP, OldStamina, PreOldStamina, NewStamina, VerticalStamina, CircleStamina, GaugeManager, Counter, staminapercent, healthpercent;
    [SerializeField] private TMP_Text currencyCounter, percentageText, healthPercentageText, modesText, ModifierText, speedtextmf,defmultText;
    [SerializeField] private AudioClip aud_Drop;
    [SerializeField] private GameObject quarter,ItemInfostuffahah,subtitlesCanvas;
    #endregion

    #region RuntimeVariables
    private bool isFlashlightOn = false;
    public static AdditionalGameCustomizer Instance;
    [HideInInspector] public SkyboxStyle currentSkybox;
    public double Cash = 0.00;
    #endregion

    #region Enums
    public enum SkyboxStyle { Default, Day, Sunset, Night }
    public enum EscapeFunsies { BBCR, Daldi }
    public enum StaminaDisplay { Old, PreOld, Normal, Vertical, Circle }
    #endregion
    [Header("doing This Cuz I Cant Be Bothered Spamming Shit")]
    public GameObject[] ItemBackgroundsGameObj;
    public GameObject[] ItemSlotsGameObj,ItemSlotsImagesGameObj;
    public Sprite[] ItemSlotsSprites;
    public List<Image> ItemImageSlots = new List<Image>();


    [Header("item slot")]

    [SerializeField] public List<RawImage> ItemImages1slot = new List<RawImage>();
    [SerializeField] public List<RawImage> ItemImages2slot = new List<RawImage>();
    [SerializeField] public List<RawImage> ItemImages3slot = new List<RawImage>();
    [SerializeField] public List<RawImage> ItemImages4slot = new List<RawImage>();
    [SerializeField] public List<RawImage> ItemImages5slot = new List<RawImage>();
    [SerializeField] public List<RawImage> ItemImages6slot = new List<RawImage>();
    [SerializeField] public List<RawImage> ItemImages7slot = new List<RawImage>();
    [SerializeField] public List<RawImage> ItemImages8slot = new List<RawImage>();
    [SerializeField] public List<RawImage> ItemImages9slot = new List<RawImage>();

    [Header("images background slots")]

    [SerializeField] public List<Image> ItemImageBGs1slot = new List<Image>();
    [SerializeField] public List<Image> ItemImageBGs2slot = new List<Image>();
    [SerializeField] public List<Image> ItemImageBGs3slot = new List<Image>();
    [SerializeField] public List<Image> ItemImageBGs4slot = new List<Image>();
    [SerializeField] public List<Image> ItemImageBGs5slot = new List<Image>();
    [SerializeField] public List<Image> ItemImageBGs6slot = new List<Image>();
    [SerializeField] public List<Image> ItemImageBGs7slot = new List<Image>();
    [SerializeField] public List<Image> ItemImageBGs8slot = new List<Image>();
    [SerializeField] public List<Image> ItemImageBGs9slot = new List<Image>();
    public HeldItem[] Inventory1slot, Inventory2slot, Inventory3slot, Inventory4slot, Inventory5slot, Inventory6slot, Inventory7slot, Inventory8slot, Inventory9slot;
}