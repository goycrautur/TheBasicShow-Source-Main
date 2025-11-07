using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UISplashScreenManager : MonoBehaviour
{
    #region Unity Lifecycle
    private void Awake()
    {
        SetSplashImageYPosition(enableLoadingBar ? 10f : 0f);

        if (splashImage == null)
        {
            Debug.LogError("Assign Splash Image in inspector!");
        }

        if (warningPanel != null)
        {
            warningPanel.SetActive(false);
        }

        LoadingBar.gameObject.SetActive(enableLoadingBar);
        LoadingBar.value = 0f;
        if (PercentageText != null) PercentageText.text = "0%";

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        SetImageAlpha(0f);
        splashImage.gameObject.SetActive(true);
    }

    private void Start()
    {
        Singleton<Options>.Instance.GetVolume();
        Singleton<Options>.Instance.GetVSync();

        if (skipSplash)
        {
            splashImage.gameObject.SetActive(false);
            ShowWarning();
            return;
        }

        StartCoroutine(PlaySplashScreens());
    }
    #endregion

    #region Splash Logic
    private IEnumerator PlaySplashScreens()
    {
        float totalDuration = 0f;
        foreach (var splash in splashScreens)
        {
            totalDuration += splash.displayDuration + splash.fadeDuration;
        }

        if (enableLoadingBar && LoadingBar != null)
        {
            StartCoroutine(UpdateLoadingBar(totalDuration));
        }

        foreach (var splash in splashScreens)
        {
            if (splash.logoSprite == null) continue;

            splashImage.sprite = splash.logoSprite;
            SetImageAlpha(1f);

            if (splash.soundEffect != null)
            {
                audioSource.clip = splash.soundEffect;
                audioSource.Play();
            }

            yield return new WaitForSeconds(splash.displayDuration);
            yield return StartCoroutine(FadeImageAlpha(1f, 0f, splash.fadeDuration));
        }

        if (LoadingBar != null)
            LoadingBar.gameObject.SetActive(false);

        splashImage.gameObject.SetActive(false);
        ShowWarning();
    }
    #endregion

    #region UI Behavior
    private IEnumerator UpdateLoadingBar(float totalDuration)
    {
        float startTime = Time.time;
        int progress = 0;

        int[] stallPoints = { 25, 55, 80 };
        int currentStallIndex = 0;

        float nextMessageTime = 0f;
        string lastMessage = "";

        while (true)
        {
            float elapsed = Time.time - startTime;
            float normalized = Mathf.Clamp01(elapsed / totalDuration);
            int targetProgress = Mathf.RoundToInt(normalized * 100f);

            if (currentStallIndex < stallPoints.Length && targetProgress >= stallPoints[currentStallIndex])
            {
                yield return new WaitForSeconds(Random.Range(0.25f, 0.6f));
                currentStallIndex++;
            }

            if (targetProgress > progress) progress = targetProgress;

            LoadingBar.value = progress;
            PercentageText.text = progress + "%";

            if (Time.time >= nextMessageTime && loadingSteps.Length > 0)
            {
                string randomMessage;
                do
                {
                    randomMessage = loadingSteps[Random.Range(0, loadingSteps.Length)];
                }
                while (randomMessage == lastMessage && loadingSteps.Length > 1);

                lastMessage = randomMessage;

                if (LoadingDetailsText != null)
                    LoadingDetailsText.text = randomMessage;

                nextMessageTime = Time.time + 0.75f;
            }

            if (elapsed >= totalDuration) break;

            yield return null;
        }

        LoadingBar.value = 100;
        PercentageText.text = "100%";
        LoadingDetailsText.text = "Loading complete!";
    }

    private IEnumerator FadeImageAlpha(float fromAlpha, float toAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = splashImage.color;
        color.a = fromAlpha;
        splashImage.color = color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(fromAlpha, toAlpha, elapsed / duration);
            splashImage.color = color;
            yield return null;
        }

        color.a = toAlpha;
        splashImage.color = color;
    }

    private void SetImageAlpha(float alpha)
    {
        Color color = splashImage.color;
        color.a = alpha;
        splashImage.color = color;
    }

    private void SetSplashImageYPosition(float y)
    {
        var rt = splashImage.rectTransform;
        Vector3 pos = rt.localPosition;
        pos.y = y;
        rt.localPosition = pos;
    }

    private void ShowWarning()
    {
        warningPanel.SetActive(true);
        splash.SetActive(false);
    }
    #endregion

    #region Structs
    [System.Serializable]
    public struct SplashScreen
    {
        public Sprite logoSprite;
        public float fadeDuration;
        public float displayDuration;
        public AudioClip soundEffect;
    }
    #endregion

    #region Inspector Fields
    [Header("UI References")]
    [SerializeField] private Image splashImage;
    [SerializeField] private GameObject warningPanel, splash;
    [SerializeField] private Slider LoadingBar;
    [SerializeField] private TMP_Text PercentageText, LoadingDetailsText;

    [Header("Splash Screens")]
    [SerializeField] private List<SplashScreen> splashScreens;

    [Header("Settings")]
    [SerializeField] private bool skipSplash = false;
    [SerializeField] private bool enableLoadingBar = true;
    #endregion

    #region Private Fields
    private AudioSource audioSource;
    public readonly string[] loadingSteps = new string[]
    {
        "welcome old",
        "lobotomy sound hits hard",
        "hi revive | he died aga",
        "TIMES SECRET KEY SELLS FOR 4999 YTP LMFAOOO",
        "i'm ashleying it - ashley",
        "starrified - personally id just put some weird stuff therw",
        "wait reminds me     THEY SAY MY HITBOX IS A-",
        "https://www.youtube.com/watch?v=W6zKyDvidOc",
        "'You are grounded' - Boris 2018"
    };
    #endregion
}