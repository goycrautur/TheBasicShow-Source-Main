using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using System.Reflection;
using DG.Tweening;

public class LappingOfAsylumController : MonoBehaviour
{
    #region SingletonSetup
    private void Awake() => LapInstance = this;
    public static LappingOfAsylumController LapInstance;
    #endregion
    public void Update()
    {
        if (vanishScore) scoreDecreaseTimer -= Time.deltaTime;
        if (scoreDecreaseTimer < 0f)
		{
            scoreSystemManager.Instance.AddScore(-15*CurrentLap);
            scoreDecreaseTimer = 1f/CurrentLap;
        }
    }
    private void Start()
    {
        realCurrentMaxNoteboo = gc.maxNotebooks;
        CurrentMaxNotebooks = gc.maxNotebooks;
        LapPortals.ForEach(lap => lap.SetActive(false));
        for (int i = 0; i < lappingHi.Length; i++)
        {
            stupidText[i] = PlayerPrefs.GetString($"Lap {i+1} Music", "Default");
            if (stupidText[i] != "Default") lappingHi[i].LapMusik.audClip = Sych.LoadSound(stupidText[i]);
        }
    }
    public void doShit(string stuff = "beginning")
    {
        if (stuff == "beginning")
        {
            gc.UnlockAmount = 0;
            gc.ObjectsToEnable.ForEach(o => o.SetActive(true));
            gc.Math.quarter.SetActive(true);
            string mode = PlayerPrefs.GetString("CurrentMode");
            if (mode != "LappingOfAsylum") LapPortals.ForEach(lap => Destroy(lap));
            var audLapVal = (int)UnityEngine.Random.Range(0f, RandomizedPrelapMusic.Length);
            LapSound.ClearQueue(true);
            LapSound.QueueAudio(RandomizedPrelapMusic[audLapVal]);
            LapSound.SetLoop(true);
            if (gc.warrealest) gc.LapManag.MeepTimer.SetActive(true);
        }
        if (stuff == "exitstuff")
        {
            if (CurrentLap < MaxLap) LapPortals.ForEach(lap => lap.SetActive(true));
            gc.Gatesrea.ForEach(g => g.Down(false));
            gc.ElevdorRea.ForEach(ed => ed.Opendor = true);
            for (int e = 0; e < AdditionalGameCustomizer.Instance.ExitImages.Length; ++e) StartCoroutine(gc.tweeniconSolo(Color.black, 0, 1, 1f, e));
            elevatorTriggers.ForEach(et => et.SetActive(true));
            allowClosElev = true;
        }
    }
    public void UpdateManually()
    {
        ChaosCheeseCount++;
        if (ChaosCheeseCount == 7)
        {
            bool ChaosMode = PlayerPrefsExtension.GetBool("Chaos");
            if (ChaosMode)
            {
                if (gc.mode == "LappingOfAsylum")
                {
                    foreach (GameObject obj in gc.ObjectsToEnable)
                    {
                        if (obj != null)
                        {
                            GameObject clone = Instantiate(obj, obj.transform.position, obj.transform.rotation);
                            clone.name = obj.name;
                            clone.SetActive(true);
                        }
                    }
                }
            }
            ChaosCheeseCount = 0;
        }
        if (Meeptimar.isActiveAndEnabled && Lap5CheeseCount != 7) meepTimerScript.Instance.AddTime(gc.warrealest && CurrentLap < 4 ? 18/(CurrentLap+1) : 12.5f, Color.green);
        if (LapFamishShit) FamishCheeseCount++;
        if (Lap5TimingStuff) Lap5CheeseCount++;
        if (Lap5CheeseCount == 7)
        {
            if (Meeptimar.isActiveAndEnabled) meepTimerScript.Instance.AddTime(25f, Meeptimar.startingTime <= 25f ? Color.green : Color.red, true);
            /*LapSound.clip = lap5Aud1;
            LapSound.Play();
            LapSound.loop = true;*/
            Lap5TimingStuff = false;
            Lap5CheeseCount = 0;
        }
        if (FamishCheeseCount == 3) gc.fmc.angerMultipler = 1.25f;
        if (FamishCheeseCount == 4) gc.fmc.angerMultipler = 1.35f;
        if (FamishCheeseCount == 5) gc.fmc.angerMultipler = 1.25f;
        if (FamishCheeseCount == 6) gc.fmc.angerMultipler = 0.9f;
        if (FamishCheeseCount == 7)
        {
            gc.fmc.angerMultipler = 0.65f;
            gc.famishScrpt.activatewindowbreak = true;
            LapFamishShit = false;
            FamishCheeseCount = 0;
        }
        if (CurrentLap >= 3)
        {
            Chaos3CheeseCount++;
            if (Chaos3CheeseCount == 7)
            {
                bool ChaosMode = PlayerPrefsExtension.GetBool("Chaos");
                if (ChaosMode)
                {
                    if (gc.mode == "LappingOfAsylum")
                    {
                        if (gc.fmc.butch != null)
                        {
                            GameObject clone = Instantiate(gc.fmc.butch, gc.fmc.butch.transform.position, gc.fmc.butch.transform.rotation);
                            clone.name = gc.fmc.butch.name;
                            clone.GetComponent<FamishedScript>().famishedSpd = 0.5f;
                            clone.SetActive(true);
                        }
                        /*if (gc.zerull.zer != null)
                        {
                            GameObject clone = Instantiate(gc.zerull.zer, gc.zerull.zer.transform.position, gc.zerull.zer.transform.rotation);
                            clone.name = gc.zerull.zer.name;
                            clone.GetComponent<zerullscript>().Anger = 0.5f;
                            clone.SetActive(true);
                        }*/
                        if (gc.wegchal.WEGA != null)
                        {
                            GameObject clone = Instantiate(gc.wegchal.WEGA, gc.wegchal.WEGA.transform.position, gc.wegchal.WEGA.transform.rotation);
                            clone.name = gc.wegchal.WEGA.name;
                            clone.SetActive(true);
                        }
                    }
                }
                Chaos3CheeseCount = 0;
            }
        }
        if (gc.notebooks == CurrentMaxNotebooks)
        {
            if (CurrentLap == 0) StartCoroutine(zawadro());
            else doShit("exitstuff");
        }
    }
    public IEnumerator zawadro()
    {
        gc.lbams.MainSource2.PlaySingleClip(zawarudo);
        vanishScore = false;
        LapSound.ClearQueue(true);
        gc.player.DisableCamMove = true;
        gc.player.movementLocked = true;
        gc.player.titlecard = true;
        gc.playerCollider.enabled = false;
        gc.npcCloneList.ForEach(o => o.SetActive(false));
        AdditionalGameCustomizer.Instance.donthaveanamelmfao = AdditionalGameCustomizer.Instance.zaColor;
        yield return new WaitForSeconds(zawarudo.audClip.length);
        gc.player.transform.position = EndingManager.Instance.SecretWarpPoint.transform.position + Vector3.up * gc.player.height;
        lowBudgetAudioManagementShit.Instance.TpSource.PlaySingleClip(lowBudgetAudioManagementShit.Instance.evilLeafTP);
        yield return new WaitForSeconds(0.1f);
        gc.player.maxHealth += 50;
        gc.player.totemshit(false);
        gc.maxNotebooks += realCurrentMaxNoteboo;
        CurrentMaxNotebooks += realCurrentMaxNoteboo;
        for (int i = 0; i < noteboos.Length; ++i) if (noteboos[i].hidden) noteboos[i].Respawn();
        gc.UpdateNotebookCount();
        yield return new WaitForSeconds(zawarudo.audClip.length / 2);
        UnityEngine.Debug.Log(LapSound.audioDevice.timeSamples);
        LapSound.ClearQueue(true);
        LapSound.QueueAudio(lappingHi[CurrentLap].LapMusik);
        LapSound.SetLoop(true);
        gc.lbams.MainSource2.PlaySingleClip(BellSoundLapping);
        CurrentLap++;
        gc.player.walkSpeedMultipler += 0.2f;
        gc.player.runSpeedMultipler += 0.2f;
        gc.player.movementLocked = false;
        gc.player.titlecard = false;
        gc.playerCollider.enabled = true;
        gc.player.DisableCamMove = false;
        gc.npcCloneList.ForEach(o => o.SetActive(true));
        AdditionalGameCustomizer.Instance.donthaveanamelmfao = AdditionalGameCustomizer.Instance.canvascolormain;
        vanishScore = true;
        Singleton<TimeOutManagerFUCKYEA>.Instance.InitializeTimeoutStuff(lappingHi[CurrentLap-1].LapMusik.audClip.length + lappingHi[CurrentLap].LapMusik.audClip.length);
        bool shrinky = PlayerPrefsExtension.GetBool("shrink");
        if (shrinky) Singleton<OtherMainStuffManager>.Instance.ChangeItemSlot(Singleton<OtherMainStuffManager>.Instance.realMaxSlotsAmmou);
        foreach (MuchoScript muc in GameControllerScript.Instance.muchscr) if (muc.isActiveAndEnabled) muc.MuchoSpeedScale += 0.1f;
        yield return null;
        yield break;
    }
    public IEnumerator flagmove()
    {
        lapFlagImages.sprite = lappingHi[CurrentLap].LapFlag;
        float ratioy = (float)Screen.width / 360f;
        LapFlag.transform.DOMoveY(ratioy * 160, 1f);
        yield return new WaitForSeconds(3);
        LapFlag.transform.DOMoveY(ratioy * 300, 1f);
        yield break;
    }
    public IEnumerator LapPortal()
    {
        if (CurrentLap >= 1)
        {
            if (!inportalALREADY)
            {
                vanishScore = false;
                scoreSystemManager.Instance.AddScore(5500*CurrentLap);
                if (Meeptimar.isActiveAndEnabled) meepTimerScript.Instance.AddTime(55f,Color.green);
                gc.player.maxHealth += 25;
                gc.player.totemshit(false);
                gc.ElevdorRea.ForEach(ed => ed.Close());
                gc.ElevdorRea.ForEach(ed => ed.finaleActivated = false);
                for (int e = 0; e < AdditionalGameCustomizer.Instance.ExitImages.Length; ++e) StartCoroutine(gc.tweeniconSolo(new Color(0, 0, 0, 0), 0, 1, 1f, e));
                for (int i = 0; i < noteboos.Length; ++i) if (noteboos[i].hidden) noteboos[i].Respawn();
                gc.exitsReached = 0;
                gc.maxNotebooks += realCurrentMaxNoteboo;
                CurrentMaxNotebooks += realCurrentMaxNoteboo;
                allowClosElev = false;
                inportalALREADY = true;
                gc.player.movementLocked = true;
                gc.player.titlecard = true;
                gc.playerCollider.enabled = false;
                gc.lbams.MainSource2.PlaySingleClip(PortalEnteringSound);
                gc.npcCloneList.ForEach(o => o.SetActive(false));
                gc.CirclAnimator.SetTrigger("nooo");
                //StartCoroutine(FadeSound(LapSound,1.5f,"fadeOut"));
                StartCoroutine(LapSound.FadeOut(1.5f));
                yield return new WaitForSeconds(PortalEnteringSound.audClip.length + 1);
                gc.player.titlecard = false;
                gc.player.movementLocked = false;
                gc.playerCollider.enabled = true;
                gc.lbams.MainSource2.PlaySingleClip(PortalExitingSound);
                gc.player.transform.position = EndingManager.Instance.SecretWarpPoint.transform.position + Vector3.up * gc.player.height;
                gc.npcCloneList.ForEach(o => o.SetActive(true));
                gc.CirclAnimator.SetTrigger("yooo");
                yield return new WaitForSeconds(PortalExitingSound.audClip.length);
                LapSpecificsStuff();
                gc.lbams.MainSource2.PlaySingleClip(BellSoundLapping);
                gc.player.walkSpeedMultipler += 0.2f;
                gc.player.runSpeedMultipler += 0.2f;
                CurrentLap++;
                gc.UpdateNotebookCount();
                if (CurrentLap > MaxLap - 1) LapPortals.ForEach(lap => lap.SetActive(false));
                inportalALREADY = false;
                gc.Gatesrea.ForEach(g => g.Down(false));
                LapPortals.ForEach(lap => lap.SetActive(false));
                bool shrinky = PlayerPrefsExtension.GetBool("shrink");
                if (shrinky) Singleton<OtherMainStuffManager>.Instance.ChangeItemSlot(Singleton<OtherMainStuffManager>.Instance.realMaxSlotsAmmou);
                yield return null;
            }
        }
        yield break;
    }
    public void LapSpecificsStuff()
    {
        vanishScore = true;
        if (CurrentLap == 2)
        {
            gc.ItemsToRespawn.ForEach(item => item.SetActive(true));
            gc.ItemsToRespawn.ForEach(item => item.GetComponent<PickupScript>().ItemRespawning());
            gc.MachinesToRestock.ForEach(machine => machine?.RestockVendingMachine(true));
            Singleton<TimeOutManagerFUCKYEA>.Instance.TimeDuratiOk = 0;
            gc.fmc.butch.SetActive(true);
            //gc.zerull.zer.SetActive(true);
            gc.wegchal.WEGA.SetActive(true);
            gc.wegchal.globalWegaSpeed = 30;
            LapFamishShit = true;
        }
        if (CurrentLap == 3)
        {
            gc.fmc.angerMultipler = 0.45f;
            gc.wegchal.globalWegaSpeed = 35;
            MeepTimer.SetActive(true);
        }
        if (CurrentLap == 4)
        {
            gc.wegchal.globalWegaSpeed = 40;
            ZerullClassic.Instance.health = 20;
            if (ZerullClassic.Instance.spawnBlockagesDuringTheBossfight) ZerullClassic.Instance.blockages.SetActive(true);
            gc.ObjectsToEnable.ForEach(o => o.SetActive(false));
            mucho.SetActive(true);
            Lap5TimingStuff = true;
            
        }
        
        LapSound.ClearQueue(true);
        LapSound.QueueAudio(lappingHi[CurrentLap].LapMusik);
        StartCoroutine(LapSound.FadeIn(1.5f,lappingHi[CurrentLap].LapMusik.volume));
        LapSound.SetLoop(true);
        if (lappingHi[CurrentLap].usesFlag) StartCoroutine(flagmove());
        foreach (MuchoScript muc in GameControllerScript.Instance.muchscr) if (muc.isActiveAndEnabled) muc.MuchoSpeedScale += 0.1f;
    }

    [SerializeField] private GameControllerScript gc;
    public List<GameObject> LapPortals, elevatorTriggers = new List<GameObject>();
    public AudioObjectyeah[] RandomizedPrelapMusic;
    public GameObject LapFlag, mucho, MeepTimer;
    public meepTimerScript Meeptimar;
    public Image lapFlagImages;
    public booksInteract[] noteboos;
    public AudioManagerLiveReaction LapSound;
    public AudioObjectyeah BellSoundLapping,PortalEnteringSound,PortalExitingSound,zawarudo, lap5Aud1;
    public int CurrentLap, MaxLap, CurrentMaxNotebooks, realCurrentMaxNoteboo,FamishCheeseCount,Lap5CheeseCount,ChaosCheeseCount,Chaos3CheeseCount;
    public float scoreDecreaseTimer,Lap1SongTimer;
    public bool inportalALREADY, h, allowClosElev, LapFamishShit, Lap5TimingStuff, vanishScore,lap1TimerReach1Min,lap1NearEndThing;
    public string[] stupidText;
    public lapVariablesStuff[] lappingHi;
    [Serializable]
	public class lapVariablesStuff
    {
        public bool usesFlag;
        public AudioObjectyeah LapMusik;
		public Sprite LapFlag;
	}
}