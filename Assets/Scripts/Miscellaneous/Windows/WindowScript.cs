using UnityEngine.AI;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
public class WindowScript : MonoBehaviour
{
    private void Start()
    {
        gc = FindObjectOfType<GameControllerScript>();
        windoIcon1.SetActive(true);
        windoIcon2.SetActive(true);
    }
    public ParticleSystem shattersPrefab, restorePrefab;
    public bool enableOffMeshScript,broken;
    public MeshRenderer window_In,window_Out;
    public MeshCollider meshCollider_In,meshCollider_Out;
    public GameObject windo, windo2, windoIcon1, windoIcon2;
    public Material[] WindowMats;
    public void Window(bool broke, bool sound, float soundval)
    {
        if (!broken && broke)
        {
            /*foreach (PrincipalScript prin in  GameControllerScript.Instance.prinScr)
            {
                if (prin.isActiveAndEnabled)
                {
                    prin.callToSMTH(this.transform.position);
                }
            }*/
            foreach (MaxcipalScript maxi in  gc.maxiScr)
            {
                if (maxi.isActiveAndEnabled)
                {
                    maxi.callToSMTH(this.transform.position);
                }
            }
            if (sound)
            {
                Singleton<OtherMainStuffManager>.Instance.HearingShit(soundval, this.transform, new Vector3(0f,0f,0f), "all",false);
            }
            audioDevice.clip = brokesound;
            audioDevice.Play();
            windoIcon1.SetActive(false);
            windoIcon2.SetActive(false);
            window_In.material = WindowMats[2];
            window_Out.material = WindowMats[3];
            meshCollider_In.enabled = false;
            meshCollider_Out.enabled = false;
            broken = true;
            gc.SubsManager.summonLeSubtitle(gc.subtitlesScriptableObject[2].subtitleOption,gc.subtitlesScriptableObject[2],audioDevice);
            Instantiate(shattersPrefab, transform.position, Quaternion.identity);
        }
        if (broken && !broke)
        {
            audioDevice.clip = restore;
            audioDevice.Play();
            windoIcon1.SetActive(true);
            windoIcon2.SetActive(true);
            window_In.material = WindowMats[0];
            window_Out.material = WindowMats[1];
            meshCollider_In.enabled = true;
            meshCollider_Out.enabled = true;
            broken = false;
            gc.SubsManager.summonLeSubtitle(gc.subtitlesScriptableObject[3].subtitleOption,gc.subtitlesScriptableObject[3],audioDevice);
            Instantiate(restorePrefab, transform.position, Quaternion.identity);
        }
    }
    public void Update()
    {
        windo.layer = LayerMask.NameToLayer(gc.WindowLayermask);
        windo2.layer = LayerMask.NameToLayer(gc.WindowLayermask);
        if (broken)
        {
            this.gameObject.GetComponent<NavMeshObstacle>().enabled = false;
            this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
        else
        {
            if (enableOffMeshScript)
            {
                this.gameObject.GetComponent<NavMeshObstacle>().enabled = false;
                if (!UseCustomBoxCollider)
                {
                this.gameObject.GetComponent<BoxCollider>().size = new Vector3(10f, 10f, 1f);
                }
            }
            else
            {
                this.gameObject.GetComponent<NavMeshObstacle>().enabled = true;
                if (!UseCustomBoxCollider)
                {
                this.gameObject.GetComponent<BoxCollider>().size = new Vector3(10f, 10f, 1f);
                }
            }
            this.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
    public void OnTriggerEnter(Collider play)
    {
        if (broken && play.CompareTag("Player") & !gc.debugMode & !gc.player.titlecard)
        {
            //DamagPlaye(UnityEngine.Random.Range(0,2));
        }
    }
    public void DamagPlaye(int rando)
    {
        if (rando == 1)
        {
            gc.player.SetHP(PlayerScript.HealthChangeMode.Remove, UnityEngine.Random.Range(0,5), 1f, false, true, false);
        }
    }

    [SerializeField] public GameControllerScript gc;
    public AudioClip brokesound, restore;
    public AudioSource audioDevice;
    public bool UseCustomBoxCollider = true;
}
