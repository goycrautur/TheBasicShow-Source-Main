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
    private void Start()
    {
        realCurrentMaxNoteboo = gc.maxNotebooks;
        CurrentMaxNotebooks = gc.maxNotebooks;
        LapPortals.ForEach(lap => lap.SetActive(false));
    }
     public void doShit(string stuff = "beginning")
    {
        if (stuff == "beginning")
        {
            gc.UnlockAmount = 0;
            gc.ObjectsToEnable.ForEach(o => o.SetActive(true));
            gc.Math.quarter.SetActive(true);
            string mode = PlayerPrefs.GetString("CurrentMode");
            if (mode != "LappingOfAsylum")
            {
                LapPortals.ForEach(lap => Destroy(lap));
            }
            var audLapVal = (int)UnityEngine.Random.Range(0f, RandomizedPrelapMusic.Length);
            LapSound.clip = RandomizedPrelapMusic[audLapVal];
            LapSound.Play();
            LapSound.loop = true;
        }
        if (stuff == "exitstuff")
        {
            if (CurrentLap < MaxLap - 1)
            {
                LapPortals.ForEach(lap => lap.SetActive(true));
            }
            gc.Gatesrea.ForEach(g => g.Down(false));
            gc.ElevdorRea.ForEach(ed => ed.Opendor = true);
            for (int e = 0; e < AdditionalGameCustomizer.Instance.ExitImages.Length; ++e)
            {
                StartCoroutine(gc.tweeniconSolo(Color.black, 0, 1, 1f, e));
            }
            elevatorTriggers.ForEach(et => et.SetActive(true));
            allowClosElev = true;
        }
    }
    public void UpdateManually()
    {
        if (Meeptimar.isActiveAndEnabled && Lap5CheeseCount != 7)
        {
            meepTimerScript.Instance.AddTime(12.5f, Color.green);
        }
        if (LapFamishShit)
        {
            FamishCheeseCount++;
        }
        if (Lap5TimingStuff)
        {
            Lap5CheeseCount++;
        }
        if (Lap5CheeseCount == 7)
        {
            if (Meeptimar.isActiveAndEnabled)
            {
                meepTimerScript.Instance.AddTime(25f, Meeptimar.startingTime <= 25f ? Color.green : Color.red, true);
            }
            LapSound.clip = lap5Aud1;
            LapSound.Play();
            LapSound.loop = true;
            Lap5TimingStuff = false;
            Lap5CheeseCount = 0;
        }
        if (FamishCheeseCount == 3)
        {
            gc.fmc.angerMultipler = 1.25f;
        }
        if (FamishCheeseCount == 4)
        {
            gc.fmc.angerMultipler = 1.35f;
        }
        if (FamishCheeseCount == 5)
        {
            gc.fmc.angerMultipler = 1.25f;
        }
        if (FamishCheeseCount == 6)
        {
            gc.fmc.angerMultipler = 0.9f;
        }
        if (FamishCheeseCount == 7)
        {
            gc.voxLight.ambientLight = new Color(0.05f, 0.05f, 0.05f, 0f);
            StartCoroutine(gc.fmc.easing(new Color(0.35f, 0.35f, 0.35f, 1f), 0, 1, 1));
            gc.fmc.angerMultipler = 0.65f;
            gc.famishScrpt.activatewindowbreak = true;
            LapFamishShit = false;
            FamishCheeseCount = 0;
        }
        if (gc.notebooks == 1)
        {
            gc.Gatesrea.ForEach(g => g.Down());
        }
        if (gc.notebooks == CurrentMaxNotebooks)
        {
            if (CurrentLap == 0)
            {
                StartCoroutine(zawadro());
            }
            else
            {
                doShit("exitstuff");
            }
        }
    }
    public IEnumerator zawadro()
    {
        if (!h)
        {
            h = true;
            gc.audioDevice2.PlayOneShot(zawarudo);
            LapSound.Stop();
            gc.player.movementLocked = true;
            gc.player.titlecard = true;
            gc.playerCollider.enabled = false;
            gc.ObjectsToEnable.ForEach(o => o.SetActive(false));
            AdditionalGameCustomizer.Instance.donthaveanamelmfao = AdditionalGameCustomizer.Instance.zaColor;
            yield return new WaitForSeconds(zawarudo.length);
            gc.player.maxHealth += 50;
            gc.player.totemshit(false);
            gc.maxNotebooks += realCurrentMaxNoteboo;
            CurrentMaxNotebooks += realCurrentMaxNoteboo;
            for (int i = 0; i < noteboos.Length; ++i)
            {
                noteboos[i].Respawn();
            }
            gc.UpdateNotebookCount();
            yield return new WaitForSeconds(zawarudo.length / 2);
            LapSound.clip = lappingHi[CurrentLap].LapMusik;
            LapSound.Play();
            LapSound.loop = true;
            gc.audioDevice2.PlayOneShot(BellSoundLapping);
            CurrentLap++;
            gc.player.walkSpeedMultipler += 0.45f;
            gc.player.runSpeedMultipler += 0.4f;
            gc.player.movementLocked = false;
            gc.player.titlecard = false;
            gc.playerCollider.enabled = true;
            gc.ObjectsToEnable.ForEach(o => o.SetActive(true));
            AdditionalGameCustomizer.Instance.donthaveanamelmfao = AdditionalGameCustomizer.Instance.canvascolormain;
            yield return null;
        }
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
                if (Meeptimar.isActiveAndEnabled)
                {
                    meepTimerScript.Instance.AddTime(55f,Color.green);
                }
                gc.player.maxHealth += 25;
                gc.player.totemshit(false);
                gc.Gatesrea.ForEach(g => g.Down());
                gc.ElevdorRea.ForEach(ed => ed.Close());
                gc.ElevdorRea.ForEach(ed => ed.finaleActivated = false);
                for (int e = 0; e < AdditionalGameCustomizer.Instance.ExitImages.Length; ++e)
                {
                    StartCoroutine(gc.tweeniconSolo(new Color(0, 0, 0, 0), 0, 1, 1f, e));
                }
                for (int i = 0; i < noteboos.Length; ++i)
                {
                    noteboos[i].Respawn();
                }
                gc.exitsReached = 0;
                gc.maxNotebooks += realCurrentMaxNoteboo;
                CurrentMaxNotebooks += realCurrentMaxNoteboo;
                allowClosElev = false;
                inportalALREADY = true;
                gc.player.movementLocked = true;
                gc.player.titlecard = true;
                gc.playerCollider.enabled = false;
                gc.audioDevice2.PlayOneShot(PortalEnteringSound);
                gc.ObjectsToEnable.ForEach(o => o.SetActive(false));
                gc.CirclAnimator.SetTrigger("nooo");
                yield return new WaitForSeconds(PortalEnteringSound.length + 1);
                gc.player.titlecard = false;
                gc.player.movementLocked = false;
                gc.playerCollider.enabled = true;
                gc.audioDevice2.PlayOneShot(PortalExitingSound);
                gc.player.transform.position = EndingManager.Instance.SecretWarpPoint.transform.position + Vector3.up * gc.player.height;
                gc.ObjectsToEnable.ForEach(o => o.SetActive(true));
                gc.CirclAnimator.SetTrigger("yooo");
                yield return new WaitForSeconds(PortalExitingSound.length);
                LapSpecificsStuff();
                gc.audioDevice2.PlayOneShot(BellSoundLapping);
                gc.player.walkSpeedMultipler += 0.3f;
                gc.player.runSpeedMultipler += 0.3f;
                CurrentLap++;
                gc.UpdateNotebookCount();
                if (CurrentLap > MaxLap - 1)
                {
                    LapPortals.ForEach(lap => lap.SetActive(false));
                }
                inportalALREADY = false;
                yield return null;
            }
        }
        yield break;
    }
    public void LapSpecificsStuff()
    {
        if (CurrentLap == 1)
        {
            gc.ItemsToRespawn.ForEach(item => item.SetActive(true));
        }
        if (CurrentLap == 2)
        {
            gc.fmc.butch.SetActive(true);
            gc.zerull.zer.SetActive(true);
            LapFamishShit = true;
        }
        if (CurrentLap == 3)
        {
            gc.voxLight.ambientLight = Color.white;
            MeepTimer.SetActive(true);
            gc.ItemsToRespawn.ForEach(item => item.SetActive(true));
        }
        if (CurrentLap == 4)
        {
            gc.ObjectsToEnable.ForEach(o => o.SetActive(false));
            mucho.SetActive(true);
            Lap5TimingStuff = true;
        }
        LapSound.clip = lappingHi[CurrentLap].LapMusik;
        LapSound.Play();
        LapSound.loop = true;
        if (lappingHi[CurrentLap].usesFlag)
        {
            StartCoroutine(flagmove());
        }
    }

    [SerializeField] private GameControllerScript gc;
    public List<GameObject> LapPortals, elevatorTriggers = new List<GameObject>();
    public AudioClip[] RandomizedPrelapMusic;
    public GameObject LapFlag, mucho, MeepTimer;
    public meepTimerScript Meeptimar;
    public Image lapFlagImages;
    public booksInteract[] noteboos;
    public AudioSource LapSound;
    public AudioClip BellSoundLapping,PortalEnteringSound,PortalExitingSound,zawarudo, lap5Aud1;
    public int CurrentLap, MaxLap, CurrentMaxNotebooks, realCurrentMaxNoteboo,FamishCheeseCount,Lap5CheeseCount;
    public bool inportalALREADY, h, allowClosElev, LapFamishShit, Lap5TimingStuff;
    public lapVariablesStuff[] lappingHi;
    [Serializable]
	public class lapVariablesStuff
    {
        public bool usesFlag;
        public AudioClip LapMusik;
		public Sprite LapFlag;
	}
}