using UnityEngine.AI;
using UnityEngine;
using System;
public class basicshowWindowScript : MonoBehaviour // when am i going to put this to use
{
    [Header("PLEASE IGNORE, this is just for other script to access these variables\n")]
    public bool enableOffMeshScript;
    public bool broken;
    public int durability,ogDurability;
    [Header("\nwindow side stuff\n")]
    public MeshRenderer window_In;
    public MeshRenderer window_Out;
    public MeshCollider meshCollider_In,meshCollider_Out;
    public GameObject windo, windo2;
    [Header("\nwindow map icon\n")]
    public GameObject windoIcon1;
    public GameObject windoIcon2;
    [Header("\nmisc setting\n")]
    public bool UseCustomBoxCollider = false;
    [Header("\nmain stuff\n")]
    public WindowData WinData;
    private Texture Window_unbroken,Window_broken;
    public void Start()
    {
        audioDevice = GetComponent<AudioManagerLiveReaction>();
        durability = WinData.durability;
        ogDurability = WinData.durability;
        Window_unbroken = WinData.normalWinMats[0].GetTexture("_SecondTex");
        Window_broken = WinData.normalWinMats[2].GetTexture("_SecondTex");
        window_In.material = WinData.normalWinMats[0];
        window_Out.material = WinData.normalWinMats[1];
        window_In.material.SetInt("_UseGlitch", 1);
        window_Out.material.SetInt("_UseGlitch", 1);
        
    }
    public void SetWindowState(bool sound = false, float soundval = 0, float soundvalCracke = 0, int howmanyduraLost = 0, bool setdura = false, int DuraSet = 1)
    {
        if (!setdura) durability -= howmanyduraLost;
        else if (setdura) durability = DuraSet;
        if (durability < 0) durability = 0;
        if (durability == 0)
        {
            meshCollider_In.enabled = false;
            meshCollider_Out.enabled = false;
            windoIcon1.SetActive(false);
            windoIcon2.SetActive(false);
            broken = true;
            if (sound)
            {
                foreach (MaxcipalScript maxi in GameControllerScript.Instance.maxiScr) if (maxi.isActiveAndEnabled) maxi.callToSMTH(this.transform.position);
                Singleton<OtherMainStuffManager>.Instance.HearingShit(soundval, this.transform, new Vector3(0f,0f,0f), "all",false);
            }
            if (WinData.sounds[0] != null) audioDevice.PlaySingleClip(WinData.sounds[0]);
            else Debug.Log($"le sounds data array number 1 (broken window sound object) is null!!!! go assign it dumbass");
            if (Window_broken != null) 
            {
                window_In.material.SetTexture("_SecondTex", Window_broken);
                window_Out.material.SetTexture("_SecondTex", Window_broken);
            }
            else Debug.Log("the broken window texture is NULL, how did this happened wawa...????");
            /*if (WinData.normalWinMats[2] != null) window_In.material = WinData.normalWinMats[2];
            else Debug.Log($"le normalwinmats data array 3 (broken window side 1) is null!!!! go assign it dumbass");
            if (WinData.normalWinMats[3] != null) window_Out.material = WinData.normalWinMats[3];
            else Debug.Log($"le normalwinmats data array 4 (broken window side 2) is null!!!! go assign it dumbass");*/
            if (WinData.particlPrefab[0] != null) Instantiate(WinData.particlPrefab[0], transform.position, Quaternion.identity);
            else Debug.Log($"le particle Prefab data array 1 (broken window particle prefab) is null!!!! go assign it dumbass");
            
        }
        else if (durability >= 1)
        {
            meshCollider_In.enabled = true;
            meshCollider_Out.enabled = true;
            windoIcon1.SetActive(true);
            windoIcon2.SetActive(true);
            broken = false;
            if (sound)
            {
                foreach (MaxcipalScript maxi in GameControllerScript.Instance.maxiScr) if (maxi.isActiveAndEnabled) maxi.callToSMTH(this.transform.position);
                Singleton<OtherMainStuffManager>.Instance.HearingShit(soundval, this.transform, new Vector3(0f,0f,0f), "all",false);
            }
            if (durability == ogDurability)
            {
                if (WinData.sounds[1] != null) audioDevice.PlaySingleClip(WinData.sounds[1]);
                else Debug.Log($"le sounds data array number 2 (window repair sound object) is null!!!! go assign it dumbass");
                /*if (WinData.normalWinMats[0] != null) window_In.material = WinData.normalWinMats[0];
                else Debug.Log($"le normalwinmats data array 1 (normal window side 1) is null!!!! go assign it dumbass");
                if (WinData.normalWinMats[1] != null) window_Out.material = WinData.normalWinMats[1];
                else Debug.Log($"le normalwinmats data array 2 (normal window side 2) is null!!!! go assign it dumbass");*/
                if (Window_unbroken != null) 
                {
                    window_In.material.SetTexture("_SecondTex", Window_unbroken);
                    window_Out.material.SetTexture("_SecondTex", Window_unbroken);
                }
                else Debug.Log("the unbroken window texture is NULL, how did this happened wawa...????");
                if (WinData.particlPrefab[1] != null) Instantiate(WinData.particlPrefab[1], transform.position, Quaternion.identity);
                else Debug.Log($"le particle Prefab data array 2 (window repair particle prefab) is null!!!! go assign it dumbass");
            }
            if (WinData.crackWinEnable)
            {
                if (durability != ogDurability)
                {
                    if (!WinData.uniqueCrackSound)
                    {
                        if (WinData.CrackedWindowSounds != null) audioDevice.PlaySingleClip(WinData.CrackedWindowSounds);
                        else Debug.Log($"le cracked window sounds object data is null!!!! go assign it dumbass");
                        if (WinData.crackParticlPrefab != null) Instantiate(WinData.crackParticlPrefab, transform.position, Quaternion.identity);
                        else Debug.Log($"le crack Particle Prefab data is null!!!! go assign it dumbass");
                    }
                    else 
                    {
                        if (WinData.CrackedWindowSoundsButItsAnArray[durability-1] != null) audioDevice.PlaySingleClip(WinData.CrackedWindowSoundsButItsAnArray[durability-1]);
                        else Debug.Log($"le cracked window sounds object data array {durability} is null!!!! go assign it dumbass");
                        if (WinData.crackParticlPrefabArra[durability-1] != null) Instantiate(WinData.crackParticlPrefabArra[durability-1], transform.position, Quaternion.identity);
                        else Debug.Log($"le crack Particle Prefab data array {durability} is null!!!! go assign it dumbass");
                    }
                }
                if (WinData.cracWindowTextur[durability-1] != null) 
                {
                    window_In.material.SetTexture("_SecondTex", WinData.cracWindowTextur[durability-1]);
                    window_Out.material.SetTexture("_SecondTex", WinData.cracWindowTextur[durability-1]);
                }
                else Debug.Log($"le cracked window texture data array {durability-1} is null!!!! go assign it dumbass");
            }
            else
            {
                if (WinData.sounds[0] != null) audioDevice.PlaySingleClip(WinData.sounds[0]);
                else Debug.Log($"le sounds data array number 1 (broken window sound object) is null!!!! go assign it dumbass");
            }
        }
    }
    public void Update()
    {
        window_In.material.SetFloat("_VertexGlitchSeed", Singleton<VertexGlitchManager>.Instance.global_VertexGlitchSeed);
        window_In.material.SetFloat("_VertexGlitchIntensity", Singleton<VertexGlitchManager>.Instance.global_VertexGlitchIntensity);
        window_In.material.SetInt("_ValueX", Singleton<VertexGlitchManager>.Instance.global_glitchColorRvalue);
        window_In.material.SetInt("_ValueY", Singleton<VertexGlitchManager>.Instance.global_glitchColorGvalue);
        window_In.material.SetInt("_ValueZ", Singleton<VertexGlitchManager>.Instance.global_glitchColorBvalue);
        window_Out.material.SetFloat("_VertexGlitchSeed", Singleton<VertexGlitchManager>.Instance.global_VertexGlitchSeed);
        window_Out.material.SetFloat("_VertexGlitchIntensity", Singleton<VertexGlitchManager>.Instance.global_VertexGlitchIntensity);
        window_Out.material.SetInt("_ValueX", Singleton<VertexGlitchManager>.Instance.global_glitchColorRvalue);
        window_Out.material.SetInt("_ValueY", Singleton<VertexGlitchManager>.Instance.global_glitchColorGvalue);
        window_Out.material.SetInt("_ValueZ", Singleton<VertexGlitchManager>.Instance.global_glitchColorBvalue);
        this.gameObject.layer = broken ? LayerMask.NameToLayer("Ignore Raycast") : LayerMask.NameToLayer("Windows");
        if (GameControllerScript.Instance.mode != "famished" && GameControllerScript.Instance.mode != "zerullclassic")
        {
            windo.layer = broken ? LayerMask.NameToLayer("Ignore Raycast") : LayerMask.NameToLayer("Windows");
            windo2.layer = broken ? LayerMask.NameToLayer("Ignore Raycast") : LayerMask.NameToLayer("Windows");
        }
        else
        {
            windo.layer = LayerMask.NameToLayer("Ignore Raycast");
            windo2.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
        if (broken) this.gameObject.GetComponent<NavMeshObstacle>().enabled = false;
        else
        {
            if (enableOffMeshScript)
            {
                this.gameObject.GetComponent<NavMeshObstacle>().enabled = false;
                if (!UseCustomBoxCollider) this.gameObject.GetComponent<BoxCollider>().size = new Vector3(10f, 10f, 1f);
            }
            else
            {
                this.gameObject.GetComponent<NavMeshObstacle>().enabled = true;
                if (!UseCustomBoxCollider) this.gameObject.GetComponent<BoxCollider>().size = new Vector3(10f, 10f, 1f);
            }
            this.gameObject.layer = LayerMask.NameToLayer("Default");
        }


        //sonium 
    }
    public void OnTriggerEnter(Collider play)
    {
        if (broken && play.CompareTag("Player") & !GameControllerScript.Instance.debugMode & !GameControllerScript.Instance.player.titlecard) DamagPlaye(UnityEngine.Random.Range(0,2));
    }
    public void DamagPlaye(int rando)
    {
        string dific = PlayerPrefs.GetString("CurDifficulity", "normal");
        int RandomRangeMult = dific == "easy" ? 0 : dific == "normal" ? 1 : dific == "hard" ? 2 : dific == "expert" ? 3 : dific == "maniac" ? 4 : 1;
        if (rando == 1) GameControllerScript.Instance.player.SetHP(PlayerScript.HealthChangeMode.Remove, (int)UnityEngine.Random.Range(1,5*RandomRangeMult), 1f, false, true, false);
    }

    private AudioManagerLiveReaction audioDevice;
}
