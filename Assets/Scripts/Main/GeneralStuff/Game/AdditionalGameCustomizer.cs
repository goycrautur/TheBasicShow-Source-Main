using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class AdditionalGameCustomizer : MonoBehaviour
{
    #region UnityCallbacks
    private void Awake() => Instance = this;

    private void Start()
    {
        InitializeCustomAdditions();
        SkyBoxHandling();
        
        //InitializeGameTuff();

    }
    public bool unloc, iteinfo,captio;
    public void InitializeGameTuff()
    {
        GameControllerScript.Instance.mode = PlayerPrefs.GetString("CurrentMode");
        bool chair = PlayerPrefsExtension.GetBool("BeatedUpZerull");
        bool unloc = PlayerPrefsExtension.GetBool("thonkPad");
        NoYCTP = unloc;
        if (GameControllerScript.Instance.mode == "endless") modesText.text = "Endless Mode";
        if (GameControllerScript.Instance.mode == "story")
        {
            modesText.text = "Story Mode";
            Sych.SetGameWindowTitle("The Basic Show - Story Mode");
            ModifierText.text = "Modifier used: mucho";
        }
        if (GameControllerScript.Instance.mode == "famished")
        {
            NoYCTP = true;
            Sych.SetGameWindowTitle("The Basic Show - ?????");
            modesText.text = "Famished Butch Takeover";
        }
        if (GameControllerScript.Instance.mode == "wegaChallenge")
        {
            NoYCTP = true;
            Sych.SetGameWindowTitle("how did you get here");
            modesText.text = "WEGA CHALLENGE";
        }
        if (GameControllerScript.Instance.mode == "zerullclassic")
        {
            NoYCTP = false;
            modesText.text = chair ? "c  h  a  i  r" : "?eru?? M0D3";
            Sych.SetGameWindowTitle(chair ? "c  h  a  i  r" : "His domain | When are you gonna add *The Pit* for this guy brah - someone, def not me(gray)");
        }
        if (GameControllerScript.Instance.mode == "LappingOfAsylum") 
        {
            NoYCTP = true;
            Sych.SetGameWindowTitle("The Basic Show - asylum of the larper");
            modesText.text = "Lapping Of Asylum";
        }
        if (GameControllerScript.Instance.mode == "minusb") 
        {
            NoYCTP = true;
            Sych.SetGameWindowTitle("The Basic Show - ?????");
            modesText.text = "why the fuck is minus b in basic show";
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
        iteinfo = PlayerPrefsExtension.GetBool("ItemInfo");
        captio = PlayerPrefsExtension.GetBool("Captions");
        ItemInfoShit = iteinfo;
        Subtitles = captio;
        ItemInfostuffahah.SetActive(ItemInfoShit);
        subtitlesCanvas.SetActive(Subtitles);
        
        speedtextmf.text = Math.Round(GameControllerScript.Instance.player.playerSpeed,2).ToString();
        spee.sprite = Singleton<InputManager>.Instance.GetActionKey(InputAction.Run) && GameControllerScript.Instance.player.stamina > 0f ? run2 : run1;
        defmultText.text = GameControllerScript.Instance.player.PlayerDmgResistance+"X";
    }
    #endregion
    public void therainbo()
    {
        if (!rainbowTime) rainboCanv.color = donthaveanamelmfao;
        if (rainbowTime) // haha fuck you groomcradia im gonna stole your code
        {
            if (rainboCanv.color.a < 0.1f) rainboCanv.color = new Color(1f, 0f, 0f, rainboCanv.color.a + (rainboSpee * Time.deltaTime));
            Color.RGBToHSV(rainboCanv.color, out huehuehue, out saturati, out brignes);
            huehuehue += rainboSpee * Time.deltaTime;
            if (huehuehue > 1f) huehuehue = 0f;
            rainboCanv.color = Color.HSVToRGB(huehuehue, saturati, brignes);
            rainboCanv.color = new Color(rainboCanv.color.r, rainboCanv.color.g, rainboCanv.color.b, transparenci);
        }
    }
    private void PercentageSystemShit()
    {
        if (StaminaPercentage)
        {
            what = Mathf.Lerp(what,GameControllerScript.Instance.player.stamina, 20*Time.deltaTime);
            percentageText.text = (int)what + "%";
            if (GameControllerScript.Instance.player.stamina <= 15f) percentageText.color = Color.red;
            else percentageText.color = Color.black;
        }
        //if (ZerullClassic.Instance.BossStarted || ZerullClassic.Instance.RealBossStarted) percentageText.text = "∞%";
        if (HealthPercentage)
        {
            healthPercentageText.text = GameControllerScript.Instance.player.health + "/" + GameControllerScript.Instance.player.maxHealth;
            if (GameControllerScript.Instance.player.health <= 15f) healthPercentageText.color = Color.red;
            else healthPercentageText.color = Color.white;
        }
    }

    #region Initialization
    private void InitializeCustomAdditions()
    {
        DefaultFovAmmount = FovAmmount;
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
        CameraScript.Instance.MainCamera.fieldOfView = CameraShake ? UnityEngine.Random.Range(58, 62) : Mathf.Lerp(CameraScript.Instance.MainCamera.fieldOfView, FovAmmount + ExtraFovAmmount, 5f * Time.deltaTime);
        CameraScript.Instance.XrayCamera.fieldOfView = CameraShake ? UnityEngine.Random.Range(58, 62) : Mathf.Lerp(CameraScript.Instance.XrayCamera.fieldOfView, FovAmmount + ExtraFovAmmount, 5f * Time.deltaTime);
    }

    private void FlashlightCode()
    {
        GameObject cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        if (cameraObject != null)
        {
            Light light = cameraObject.GetComponent<Light>();
            if (light != null) light.enabled = isFlashlightOn;
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

        if (staminaMap.ContainsKey(StaminaStyle)) staminaMap[StaminaStyle].SetActive(true);

        if (StaminaStyle == StaminaDisplay.Old)
        {
            bool YouNeedRest = GameControllerScript.Instance.player.stamina < 0f;
            if (warning.activeSelf != YouNeedRest) warning.SetActive(YouNeedRest);
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
            if (ItemManager.Instance.Inventory[selectedSlot].ItemInstance != null && !ItemManager.Instance.Inventory[selectedSlot].ItemInstance.undropable) 
            {
                ItemManager.Instance.DropItem(selectedSlot);
            }
        }
        if (Input.GetKeyDown(KeyCode.G)) Singleton<VertexGlitchManager>.Instance.Glitch();
        if (FlashLight && Input.GetKeyDown(KeyCode.F)) isFlashlightOn = !isFlashlightOn;
        if (Input.GetKeyDown(KeyCode.T)) Singleton<TimeOutManagerFUCKYEA>.Instance.TimeDuratiOk = 0;
        if (Input.GetKeyDown(KeyCode.U)) 
        {
            foreach (basicshowWindowScript wind in FindObjectsOfType<basicshowWindowScript>()) 
            {
                if (wind.broken) wind.SetWindowState(false,0f,0f,0,true,wind.ogDurability);
            }
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
            currencyCounter.text = ": " + Cash.ToString();

            if (Cash >= 1)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (SendRayShit("VendingMachine", out RaycastHit hit, GameControllerScript.Instance.player.LocalRange))
                    {
                        var vendingMachine = hit.collider.GetComponent<VendingMachineScript>();
                        if (vendingMachine != null) if (!ItemManager.Instance.IsInventoryFull()) vendingMachine.DispenseItem();
                    }
                    else if (SendRayShit("Phone", out hit, GameControllerScript.Instance.player.LocalRange))
                    {
                        var tapePlayer = hit.collider.GetComponent<TapePlayerScript>();
                        if (tapePlayer != null)
                        {
                            Cash = Cash - 1;
                            GameControllerScript.Instance.lbams.MainSource3.PlaySingleClip(GameControllerScript.Instance.lbams.quarterDrop);
                            tapePlayer.Play();
                        }
                    }
                }
            }
        }
        else Counter.SetActive(false);
    }
    #endregion
    #region Helpers
    public bool SendRayShit(string tag, out RaycastHit rayHit, float range = 10f)
    {
        rayHit = default;

        if (Sych.ScreenCenterRaycast(out RaycastHit hit))
        {
            bool withinRange = hit.transform.IsWithinDistance(range);
            bool tagMatch = string.IsNullOrEmpty(tag) || hit.collider.CompareTag(tag);

            if (withinRange && tagMatch)
            {
                rayHit = hit;
                return true;
            }
        }
        return false;
    }
    #endregion

    #region RandomizedItems
    public void ScrambleItems()
    {
        if (RandomizeItems && !ActuallyRandomizeItems)
        {

            List<Vector3> list = new List<Vector3>();

            foreach (PickupScript pickupScript in FindObjectsOfType<PickupScript>())
            {
                if (pickupScript.ID != 5 && pickupScript.ID != 34 && !pickupScript.SpawnAtRandom) list.Add(pickupScript.transform.position);
            }
            foreach (PickupScript pickupScript2 in FindObjectsOfType<PickupScript>())
            {
                if (pickupScript2.ID != 5 && pickupScript2.ID != 34 && !pickupScript2.SpawnAtRandom)
                {
                    int index = UnityEngine.Random.Range(0, list.Count);
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
    public bool NoYCTP, DetentionAfterScissorUse, AnOldRule, ItemDropping, SkipCraftersAttack, ReworkedCurrency, RandomizeItems,ActuallyRandomizeItems, ItemInfoShit,Subtitles;
    public Image spee;
    public Sprite run1, run2, invincibl,dimcraab, dorMapLockedSprite, dorMapSprite;
    public Texture2D itemMapSprite,SpecialItemMapSprite;

    [Header("Visual Addons")]
    public StaminaDisplay StaminaStyle = StaminaDisplay.Normal;
    public bool RandomizeBookColor, Indicator, FinalModeTV, Gauges, OldDetentionTimer, FlashLight, CameraShake, StaminaPercentage, HealthPercentage;
    public SkyboxStyle SetSkybox = SkyboxStyle.Day;

    [Header("Serialized References")]
    public Image[] ExitImages;
    public Image rainboCanv;
    public float rainboSpee, huehuehue, saturati, brignes, transparenci, FovAmmount,ExtraFovAmmount,DefaultFovAmmount,what;
    public Sprite[] BookColors;
    public Material NormalSky, NormalRedSky, NightSky, RedNightSky, TwilightSky, RedTwilightSky, DefaultSky;
    [SerializeField] private GameObject warning, Clock, TMP, OldStamina, PreOldStamina, NewStamina, VerticalStamina, CircleStamina, GaugeManager, Counter, staminapercent, healthpercent;
    [SerializeField] private TMP_Text currencyCounter, percentageText, healthPercentageText, modesText, ModifierText, speedtextmf,defmultText;
    [SerializeField] public AudioClip aud_Drop;
    [SerializeField] private GameObject ItemInfostuffahah,subtitlesCanvas;
    #endregion

    #region RuntimeVariables
    private bool isFlashlightOn = false;
    public static AdditionalGameCustomizer Instance;
    [HideInInspector] public SkyboxStyle currentSkybox;
    public List<PickupScript> pickup = new List<PickupScript>();
    public int Cash = 0;
    #endregion

    #region Enums
    public enum SkyboxStyle { Default, Day, Sunset, Night }
    public enum EscapeFunsies { BBCR, Daldi, TBS,Taldi}
    public enum StaminaDisplay { Old, PreOld, Normal, Vertical, Circle }
    #endregion

}