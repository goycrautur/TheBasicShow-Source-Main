using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LearningGameManager : MonoBehaviour
{
    #region SingletonSetup
    private void Awake() => Instance = this;
    public static LearningGameManager Instance;
    #endregion
    #region UnityCallbacks
    private void Start()
    {
        gc = GetComponent<GameControllerScript>();
        audioQueue = GetComponent<AudioQueueScript>();
    }
    #endregion

    #region LearningStateManagement
    public void ActivateLearningGame()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        learningActive = true;
        KF.UnlockMouse();
        Tutor.tutorSource.Stop();

        if (!gc.spoopMode)
        {
            StartCoroutine(audioQueue.FadeOut(gc.schoolMusic, 0.25f));
            learnMusic.Play();
        }
    }
    public IEnumerator transiskill(GameObject subject)
	{
        subject.GetComponent<MathGameScript>().enabled = false;
		subject.GetComponent<MathGameScript>().DitherTransis.SetTrigger("close");
		yield return new WaitForSecondsRealtime(1f);
        UnityEngine.Object.Destroy(subject);
    }

    public void DeactivateLearningGame(GameObject subject)
    {
        gc.schoolMusic.ignoreListenerPause = false;
        AudioListener.pause = false;
        Time.timeScale = 1f;
        gc.PlayerCamera.cullingMask = gc.cullingMask;
        learningActive = false;
        KF.LockMouse();
        gc.audioDevice.PlayOneShot(gc.aud_Collected);
        gc.Icon.Rebind();
        gc.Icon.Play("IconSpin", -1, 0f);
        StartCoroutine(transiskill(subject));


        if (gc.player.stamina < 100f)
        {
            gc.player.SetStamina(PlayerScript.StaminaChangeMode.Set, 100f);
        }
        if (gc.player.stamina > 100f)
        {
            gc.player.SetStamina(PlayerScript.StaminaChangeMode.Add, 50f);
        }
        if (gc.notebooks == 2)
        {
            gc.ActivateSpoopMode();
        }
        if (gc.spoopMode)
        {
            if (gc.baldiScrpt.isActiveAndEnabled)
            {
                gc.baldiScrpt.GetAngry(1.1f / 2);
            }
            if (gc.muchoing.isActiveAndEnabled)
            {
                gc.muchoing.GetAngry(1.1f / 2);
            }
            if (gc.famishScrpt.isActiveAndEnabled)
            {
                gc.famishScrpt.GetAngry(1.2f / 2);
            }
            if (gc.zerulscrpt.isActiveAndEnabled)
            {
                gc.zerulscrpt.GetAngry(1.5f / 2);
            }
        }

        if (!gc.spoopMode && gc.mode == "story")
        {
            gc.schoolMusic.Play();
            StartCoroutine(audioQueue.FadeOut(learnMusic, 0.25f));
        }

        if (gc.notebooks == 1 && !gc.spoopMode && gc.mode == "story")
        {
            quarter.SetActive(true);
            Tutor.tutorSource.PlayOneShot(aud_Prize);
        }
        if (gc.notebooks == 2 && !gc.spoopMode && gc.mode == "story")
        {
            Tutor.StartCoroutine(Tutor.captions());
        }
        if (gc.notebooks == gc.maxNotebooks && gc.mode != "endless" && gc.mode != "LappingOfAsylum")
        {
            gc.Gatesrea.ForEach(g => g.Down(false));
            if (gc.mode == "story")
            {
                if (AdditionalGameCustomizer.Instance != null &&
                    AdditionalGameCustomizer.Instance.FinalModeTV)
                {
                    Television.baldingit = true;
                    StartCoroutine(Television.StartTVSequence(aud_AllNotebooks));
                }
                else
                {
                    gc.audioDevice.PlayOneShot(aud_AllNotebooks, 0.8f);
                }
                if (!gc.warrealest)
                {
                    if (!gc.FinaleSecret)
                    {
                        if (AdditionalGameCustomizer.Instance != null)
                        {
                            switch (AdditionalGameCustomizer.Instance.EscapeMusicFunsies)
                            {
                                case AdditionalGameCustomizer.EscapeFunsies.BBCR:
                                    gc.escapeMusic.clip = gc.SchoolhouseEscape;
                                    gc.escapeMusic.loop = true;
                                    gc.escapeMusic.Play();
                                    break;
                                case AdditionalGameCustomizer.EscapeFunsies.Daldi:
                                    StartCoroutine(gc.ambatudaldi());
                                    break;
                            }
                        }
                    }
                    if (gc.FinaleSecret)
                    {
                        StartCoroutine(gc.truerfinale(0));
                    }
                }
            }
        }
    }
    #endregion

    #region References
    [Header("References")]
    public AudioSource learnMusic;
    public AudioClip aud_AllNotebooks, aud_Prize;
    public GameObject quarter;

    [Header("Scripts")]
    [SerializeField] private KeyFunctions KF;
    public FinalModeTV Television;
    public TutorScript Tutor;
    #endregion

    #region PrivateStates
    private GameControllerScript gc;
    private AudioQueueScript audioQueue;
    [HideInInspector] public bool learningActive;
    #endregion
}