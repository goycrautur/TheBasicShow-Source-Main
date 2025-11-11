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
            if (sound)
            {
                gc.HearingShit(soundval, this.transform, new Vector3(0f,0f,0f), "all",false);
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
            gc.SubsManager.summonLeSubtitle(gc.subtitlesScriptableObject[2].subtitleOption,gc.subtitlesScriptableObject[2],0f,audioDevice);
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
            gc.SubsManager.summonLeSubtitle(gc.subtitlesScriptableObject[3].subtitleOption,gc.subtitlesScriptableObject[3],0f,audioDevice);
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
                this.gameObject.GetComponent<BoxCollider>().size = new Vector3(10f, 10f, 1f);
            }
            else
            {
                this.gameObject.GetComponent<NavMeshObstacle>().enabled = true;
                this.gameObject.GetComponent<BoxCollider>().size = new Vector3(10f, 10f, 1f);
            }
            this.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    [SerializeField] public GameControllerScript gc;
    public AudioClip brokesound, restore;
    public AudioSource audioDevice;
}
