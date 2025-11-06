using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FamishedModeController : MonoBehaviour
{
    #region SingletonSetup
    private void Awake() => Instance = this;
    public static FamishedModeController Instance;
    #endregion
    private void Start()
    {
        if (gc.mode == "famished")
        {
            gc.UpdateNotebookCount();
            StartCoroutine(easing(new Color(0.9803922f, 0.5019608f, 0.4470589f, 1f), 0, 2, 0));
            gc.WindowLayermask = "Ignore Raycast";
        }
    }
    /*public void FixedUpdate()
    {
        float shadvalh = 2f;
        if (shadvalh <= 2f)
        {
            shadvalh -= Time.deltaTime;
            Shader.SetGlobalFloat("_VertexGlitchIntensity", shadvalh);
        }
    }*/
    public void manualUpdate()
    {
        if (gc.notebooks == 2)
        {
            gc.voxLight.ambientLight = Color.black;
            StartCoroutine(easing(new Color(0.45f, 0.45f, 0.45f, 1f), 0, 2, 2));
            angerMultipler = 2.5f;
            StartCoroutine(ohgodwhat());
            butch.SetActive(true);
            gc.notebookCount.color = Color.Lerp(Color.white, new Color(0.55f, 0.55f, 0.55f, 1f), 1 - Mathf.Repeat(1f, 0.2f));
            ItemManager.Instance.ItemNameText.color = Color.Lerp(Color.white, new Color(0.55f, 0.55f, 0.55f, 1f), 1 - Mathf.Repeat(1f, 0.2f));
        }
        if (gc.notebooks == 3)
        {
            angerMultipler = 1.9f;
        }
        if (gc.notebooks == 4)
        {
            angerMultipler = 1.75f;
        }
        if (gc.notebooks == 5)
        {
            angerMultipler = 1.55f;
        }
        if (gc.notebooks == 6)
        {
            angerMultipler = 1.35f;
        }
        if (gc.notebooks == 7)
        {
            gc.voxLight.ambientLight = new Color(0.05f, 0.05f, 0.05f, 0f);
            StartCoroutine(easing(new Color(0.35f, 0.35f, 0.35f, 1f), 0, 1, 1));
            StopCoroutine(ohgodwhat());
            StartCoroutine(imdespairingit());
            angerMultipler = 1.65f;
            gc.famishScrpt.activatewindowbreak = true;
        }
        if (gc.notebooks == 8)
        {
            angerMultipler = 1.15f;
        }
        if (gc.notebooks == 14 && gc.exitsReached == 0)
        {
            gc.voxLight.ambientLight = new Color(1f, 0f, 0f, 1f);
            StartCoroutine(easing(new Color(0.25f, 0.25f, 0.25f, 1f), 0, 3, 3));
            angerMultipler = 1.35f;
            corspesspawn = true;
            funnyaudiotuff.clip = despairloopfinale1;
            funnyaudiotuff.loop = true;
            funnyaudiotuff.Play();
        }
        if (gc.exitsReached == 1)
        {
            funnyaudiotuff.clip = despairloopfinale2;
            funnyaudiotuff.loop = true;
            funnyaudiotuff.Play();
            StartCoroutine(easing(new Color(0.2f, 0.2f, 0.2f, 1f), 0, 3, 3));
        }
        if (gc.exitsReached == 3)
        {
            StartCoroutine(finale());
            StartCoroutine(easing(new Color(0.15f, 0.15f, 0.15f, 1f), 0, 3, 3));
        }
        if (gc.exitsReached == 5)
        {
            angerMultipler = 0.3f;
            StopCoroutine(finale());
            StartCoroutine(finaleaga());
            StartCoroutine(easing(new Color(0.65f, 0.65f, 0.65f, 1f), 0, 3, 3));
        }
    }
    public IEnumerator ohgodwhat()
    {
        funnyaudiotuff.clip = despairIntro;
        funnyaudiotuff.Play();
        yield return new WaitForSeconds(despairIntro.length);
        funnyaudiotuff.clip = despairloop1;
        funnyaudiotuff.loop = true;
        funnyaudiotuff.Play();
    }
    public IEnumerator imdespairingit()
    {
        funnyaudiotuff.clip = despairIntro2;
        funnyaudiotuff.Play();
        yield return new WaitForSeconds(despairIntro2.length);
        funnyaudiotuff.clip = despairloop2;
        funnyaudiotuff.loop = true;
        funnyaudiotuff.Play();
    }
    public IEnumerator finale()
    {
        funnyaudiotuff.clip = despairSuddenstop;
        funnyaudiotuff.Play();
        yield return new WaitForSeconds(despairSuddenstop.length);
        funnyaudiotuff.clip = despairloopfinale3;
        funnyaudiotuff.loop = true;
        funnyaudiotuff.Play();
    }
    public IEnumerator finaleaga()
    {
        funnyaudiotuff.clip = despairSuddenstop2;
        funnyaudiotuff.Play();
        yield return new WaitForSeconds(despairSuddenstop2.length);
        funnyaudiotuff.clip = despairloopfinale4;
        funnyaudiotuff.loop = true;
        funnyaudiotuff.Play();
    }

    public IEnumerator easing(Color kolor, float a, float b, float duration)
    {
        Color start = gc.voxLight.ambientLight;

        for (float t = a; t < b; t += Time.deltaTime / duration)
        {
            gc.voxLight.ambientLight = Color.Lerp(start, kolor, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        gc.voxLight.ambientLight = kolor;
    }
    [SerializeField] private GameControllerScript gc;
    public bool corspesspawn;
    public float angerMultipler;
    public GameObject butch;
    public AudioSource funnyaudiotuff;
    public AudioClip despairIntro, despairloop1, despairIntro2, despairloop2, despairloopfinale1, despairloopfinale2, despairSuddenstop, despairloopfinale3, despairSuddenstop2,despairloopfinale4;
}