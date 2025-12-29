using UnityEngine.AI;
using UnityEngine;
using System;
public class MultiDurabilityWindow : MonoBehaviour // when am i going to put this to use
{
    private void Start()
    {
        gc = FindObjectOfType<GameControllerScript>();
    }
    [Header("tooltips are here incase i fogo how all this work haha")]
    public bool enableOffMeshScript;
    public bool broken;
    public MeshRenderer window_In, window_Out;
    public MeshCollider meshCollider_In,meshCollider_Out;
    public int durability;
    [Tooltip("array 0 and 1 respectively is for in and out side of the window, array 2 and 3 is for broken window particl and repair prefab")] public GameObject[] amthing;
    [Tooltip("used for mostly cracked window sprite")] public Material[] cracWindowMats;
    [Tooltip("array num 0 and 1 are broken window mats on in and out side respectively, same for unbroken windows mat but on array 2 or 3")] public Material[] normalWinMats;
    [Tooltip("array num 0 is broken window subtitles object, array num 1 is window repair subtitles object,array 2 and above is just for cracking subtitles object cuz multidurability haha fuck you")]public subsScriptableObject[] subtitlesObject;
    [Tooltip("array 0 and 1 respectively is for broken and repair sound of the window, array 2 and above is for you guessed it crackeddddd window sound")] public AudioClip[] sounds;
    public void SetWindowState(bool sound = false, bool subtitlesAlt = false, bool uniqueDurabSound = false, float soundval = 0, float soundvalCracke = 0, int durabilityeal = 0)
    {
        if (durabilityeal == 0)
        {
            broken = true;
            if (sound)
            {
                if (gc.baldiScrpt.isActiveAndEnabled)
                {
                    gc.baldiScrpt.Hear(transform.position, soundval);
                }
                if (gc.famishScrpt.isActiveAndEnabled)
                {
                    gc.famishScrpt.Hear(transform.position, soundval);
                }
                if (gc.zerulscrpt.isActiveAndEnabled)
                {
                    gc.zerulscrpt.Hear(transform.position, soundval);
                }
                if (gc.muchoing.isActiveAndEnabled)
                {
                    gc.muchoing.Hear(transform.position, soundval);
                }
            }
            audioDevice.PlayOneShot(sounds[0]);
            window_In.material = normalWinMats[2];
            window_Out.material = normalWinMats[3];
            meshCollider_In.enabled = false;
            meshCollider_Out.enabled = false;
            broken = true;
            GameControllerScript.Instance.SubsManager.summonLeSubtitle(subtitlesObject[0].subtitleOption, subtitlesObject[0], audioDevice);
            Instantiate(amthing[2], transform.position, Quaternion.identity);
        }
        if (durabilityeal >= 1)
        {
            broken = false;
            if (sound)
            {
                if (gc.baldiScrpt.isActiveAndEnabled)
                {
                    gc.baldiScrpt.Hear(transform.position, soundvalCracke);
                }
                if (gc.famishScrpt.isActiveAndEnabled)
                {
                    gc.famishScrpt.Hear(transform.position, soundvalCracke);
                }
                if (gc.zerulscrpt.isActiveAndEnabled)
                {
                    gc.zerulscrpt.Hear(transform.position, soundvalCracke);
                }
                if (gc.muchoing.isActiveAndEnabled)
                {
                    gc.muchoing.Hear(transform.position, soundvalCracke);
                }
            }
            if (durabilityeal == durability)
            {
                audioDevice.PlayOneShot(sounds[1]);
                window_In.material = normalWinMats[0];
                window_Out.material = normalWinMats[1];
            }
            if (durabilityeal != durability)
            {
                if (!uniqueDurabSound)
                {
                    audioDevice.PlayOneShot(sounds[2]);
                }
                if (uniqueDurabSound)
                {
                    audioDevice.PlayOneShot(sounds[durabilityeal + 2]);
                }
                window_In.material = cracWindowMats[durabilityeal];
                window_Out.material = cracWindowMats[durabilityeal + durability];
            }
            if (!subtitlesAlt)
            {
                GameControllerScript.Instance.SubsManager.summonLeSubtitle(subtitlesObject[1].subtitleOption, subtitlesObject[1], audioDevice);
            }
            if (subtitlesAlt)
            {
                if (!uniqueDurabSound)
                {
                    GameControllerScript.Instance.SubsManager.summonLeSubtitle(subtitlesObject[2].subtitleOption, subtitlesObject[2], audioDevice);
                }
                if (uniqueDurabSound)
                {
                    GameControllerScript.Instance.SubsManager.summonLeSubtitle(subtitlesObject[2].subtitleOption, subtitlesObject[2], audioDevice);
                }
            }
            meshCollider_In.enabled = true;
            meshCollider_Out.enabled = true;
            Instantiate(amthing[3], transform.position, Quaternion.identity);
        }
    }
    public void FixedUpdate()
    {
        if (broken)
        {
            this.gameObject.GetComponent<NavMeshObstacle>().enabled = false;
            this.gameObject.layer = LayerMask.NameToLayer("Broken Windows");
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
    public AudioSource audioDevice;
}
