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
    public IEnumerator timeounaleshit(AudioClip clipped,subsScriptableObject subtitlObjec)
	{
		yield return new WaitForSeconds(Singleton<TimeOutManagerFUCKYEA>.Instance.timeoutTVDurationStuffIncaseReachingFinaleLmfao);
        StartCoroutine(Television.StartTVSequence(clipped,subtitlObjec));
    }
    public IEnumerator transiskill(GameObject subject)
	{
        subject.GetComponent<MathGameScript>().enabled = false;
		subject.GetComponent<MathGameScript>().DitherTransis.SetTrigger("close");
		yield return new WaitForSecondsRealtime(1f);
        UnityEngine.Object.Destroy(subject);
    }
    public void Update()
    {
        learnMusic.ignoreListenerPause = learningActive;
        if (!learningActive)learnMusic.Stop();
    }

    public void DeactivateLearningGame(GameObject subject,int allAnswerWrong = 0)
    {
        if (!gc.spoopMode && gc.mode == "story")
        {
            gc.schoolMusic.Play();
        }
        scoreSystemManager.Instance.AddScore(gc.mode == "zerullclassic" ? 1375 : 750, true,true);
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
        if (gc.notebooks == 1 && !gc.spoopMode && gc.mode == "story")
        {
            Tutor.tutorSource.Stop();
            quarter.SetActive(true);
            Tutor.tutorSource.PlayOneShot(aud_Prize);
            gc.SubsManager.hideSub(Tutor.TutorSub);
            gc.SubsManager.summonLeSubtitle(prizeSubs.subtitleOption,prizeSubs,Tutor.tutorSource);
        }
        if (gc.notebooks == 2 && !gc.spoopMode && gc.mode == "story")
        {
            gc.SubsManager.hideSub(prizeSubs);
        }
        if (gc.notebooks == 2)
        {
            gc.ActivateSpoopMode();
        }
        Singleton<OtherMainStuffManager>.Instance.AngerShit(1.1f, 0f,false, "all");
        Singleton<OtherMainStuffManager>.Instance.AngerShit(0.1f, 0f,false, "famished");
        if (gc.notebooks > 2 && gc.mode == "zerullclassic")
        {
            Singleton<OtherMainStuffManager>.Instance.AngerShit(0.9f, 0f,false, "zerull");
        }
        if (allAnswerWrong == 1)
        {
            Singleton<OtherMainStuffManager>.Instance.AngerShit(0.3f, 0f,false, "zerull");
            Singleton<OtherMainStuffManager>.Instance.AngerShit(0, 0.3f,true, "zerull");
        }
        if (gc.notebooks == gc.maxNotebooks && gc.mode != "endless" && gc.mode != "LappingOfAsylum")
        {
            if (gc.mode != "story")
            {
                gc.Gatesrea.ForEach(g => g.Down(false));
            }
            if (gc.mode == "story")
            {
                if (AdditionalGameCustomizer.Instance != null &&
                    AdditionalGameCustomizer.Instance.FinalModeTV)
                {
                    //Television.baldingit = true;
                    //StartCoroutine(timeounaleshit(aud_AllNotebooks,balSubs));
                    Television.TeacherJerryingIt = true;
                    StartCoroutine(timeounaleshit(aud_TeacherJerryAllCheese,jerSubs));
                }
                else
                {
                    gc.audioDevice.PlayOneShot(aud_AllNotebooks, 0.8f);
                }
                if (gc.warrealest || gc.timeout)
                {
                    gc.ElevdorRea.ForEach(ed => ed.Opendor = true);
                    gc.Gatesrea.ForEach(g => g.Down(false));
                    gc.finaleMode = true;
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
                                    gc.ElevdorRea.ForEach(ed => ed.Opendor = true);
                                    gc.Gatesrea.ForEach(g => g.Down(false));
                                    gc.finaleMode = true;
                                    break;
                                case AdditionalGameCustomizer.EscapeFunsies.Taldi:
                                    gc.escapeMusic.clip = gc.TaldiEscape;
                                    gc.escapeMusic.loop = true;
                                    gc.escapeMusic.Play();
                                    gc.ElevdorRea.ForEach(ed => ed.Opendor = true);
                                    gc.Gatesrea.ForEach(g => g.Down(false));
                                    gc.finaleMode = true;
                                    break;
                                case AdditionalGameCustomizer.EscapeFunsies.Daldi:
                                    StartCoroutine(gc.ambatudaldi());
                                    gc.ElevdorRea.ForEach(ed => ed.Opendor = true);
                                    gc.Gatesrea.ForEach(g => g.Down(false));
                                    gc.finaleMode = true;
                                    break;
                                case AdditionalGameCustomizer.EscapeFunsies.TBS:
                                    gc.Gatesrea.ForEach(g => g.Down());
                                    StartCoroutine(Singleton<MusicShitass>.Instance.basicShowMusicShit(0));
                                    break;
                            }
                        }
                    }
                    if (gc.FinaleSecret)
                    {
                        StartCoroutine(Singleton<MusicShitass>.Instance.truerfinale(0));
                        gc.ElevdorRea.ForEach(ed => ed.Opendor = true);
                        gc.Gatesrea.ForEach(g => g.Down(false));
                        gc.finaleMode = true;
                    }
                }
            }
        }
    }
    #endregion

    #region References
    [Header("References")]
    public AudioSource learnMusic;
    public AudioClip aud_AllNotebooks,aud_TeacherJerryAllCheese,aud_Timeout, aud_Prize;
    public subsScriptableObject prizeSubs,famSubs,balSubs,jerSubs,balSubsTIMEOUT;
    public GameObject quarter;


    [Header("Scripts")]
    [SerializeField] private KeyFunctions KF;
    public BaldTVyea Television;
    public TutorScript Tutor;
    #endregion

    #region PrivateStates
    private GameControllerScript gc;
    private AudioQueueScript audioQueue;
    [HideInInspector] public bool learningActive;
    #endregion
}