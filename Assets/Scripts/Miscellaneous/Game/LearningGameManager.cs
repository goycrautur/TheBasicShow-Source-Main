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
        string dific = PlayerPrefs.GetString("CurDifficulity", "normal");
        angerMult = dific == "easy" ? 0.7f : dific == "normal" ? 1 : dific == "hard" ? 1.4f : dific == "expert" ? 1.7f : dific == "maniac" ? 2.2f : 1;
        tempAngerMult = dific == "easy" ? 0.4f : dific == "normal" ? 0.8f : dific == "hard" ? 1.2f : dific == "expert" ? 1.6f : dific == "maniac" ? 2 : 0.8f;
        gc = GetComponent<GameControllerScript>();
        audioQueue = GetComponent<AudioManagerLiveReaction>();
    }
    #endregion

    #region LearningStateManagement
    public void ActivateLearningGame()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        learningActive = true;
        KF.UnlockMouse();
        Tutor.tutorSource.ClearQueue(true);

        if (!gc.spoopMode)
        {
            gc.lbams.SchoolMusic.ClearQueue(true);
            learnMusic.QueueAudio(LearnAudio);
            learnMusic.SetLoop(true);
        }
    }
    public IEnumerator timeounaleshit(AudioObjectyeah clipped)
	{
		yield return new WaitForSeconds(Singleton<TimeOutManagerFUCKYEA>.Instance.timeoutTVDurationStuffIncaseReachingFinaleLmfao);
        StartCoroutine(Television.StartTVSequence(clipped));
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
        learnMusic.audioDevice.ignoreListenerPause = learningActive;
        learnMusic.SetLoop(learningActive);
        if (!learningActive)learnMusic.ClearQueue(true);
    }

    public void DeactivateLearningGame(GameObject subject,int allAnswerWrong = 0)
    {
        lowBudgetAudioManagementShit lbams = lowBudgetAudioManagementShit.Instance;
        if (!gc.spoopMode && gc.mode == "story") 
        {
            lbams.SchoolMusic.QueueAudio(lbams.schoolClip);
            learnMusic.SetLoop(true);
        }
        scoreSystemManager.Instance.AddScore(gc.mode == "zerullclassic" ? 1375 : 1000, true,true);
        gc.lbams.SchoolMusic.SetIgnoreListenerPause(false);
        AudioListener.pause = false;
        Time.timeScale = 1f;
        gc.PlayerCamera.cullingMask = gc.cullingMask;
        learningActive = false;
        KF.LockMouse();
        lbams.PlayClip(lbams.MainSource2,subject.GetComponent<MathGameScript>().nbScri.cheeseCollect,true);
        gc.Icon.Rebind();
        gc.Icon.Play("IconSpin", -1, 0f);
        StartCoroutine(transiskill(subject));


        if (gc.player.stamina < 100f) gc.player.SetStamina(PlayerScript.StaminaChangeMode.Set, 100f);
        if (gc.player.stamina > 100f) gc.player.SetStamina(PlayerScript.StaminaChangeMode.Add, 50f);
        if (gc.notebooks == 1 && !gc.spoopMode && gc.mode == "story")
        {
            Tutor.tutorSource.ClearQueue(true);
            quarter.SetActive(true);
            Tutor.tutorSource.PlaySingleClip(aud_Prize);
        }
        if (gc.notebooks == 2) gc.ActivateSpoopMode();
        Singleton<OtherMainStuffManager>.Instance.AngerShit(1.1f*angerMult, 0f,false, "all");
        Singleton<OtherMainStuffManager>.Instance.AngerShit(0.1f*angerMult, 0f,false, "famished");
        if (gc.notebooks > 2 && gc.mode == "zerullclassic") Singleton<OtherMainStuffManager>.Instance.AngerShit(0.9f*angerMult, 0f,false, "zerull");
        if (allAnswerWrong == 1)
        {
            Singleton<OtherMainStuffManager>.Instance.AngerShit(0.3f*angerMult, 0f,false, "zerull");
            Singleton<OtherMainStuffManager>.Instance.AngerShit(0, 0.3f*tempAngerMult,true, "zerull");
        }
        if (gc.notebooks == gc.maxNotebooks && gc.mode != "endless" && gc.mode != "LappingOfAsylum")
        {
            if (gc.mode != "story") gc.Gatesrea.ForEach(g => g.Down(false));
            if (gc.mode == "story")
            {
                if (AdditionalGameCustomizer.Instance != null &&
                    AdditionalGameCustomizer.Instance.FinalModeTV)
                {
                    //Television.baldingit = true;
                    //StartCoroutine(timeounaleshit(aud_AllNotebooks,balSubs));
                    Television.TeacherJerryingIt = true;
                    if (!gc.FinaleSecret) StartCoroutine(timeounaleshit(aud_TeacherJerryAllCheese));
                }
                else lbams.MainSource3.PlaySingleClip(aud_AllNotebooks);
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
                                    gc.lbams.EscapeMusic.ClearQueue(true);
                                    gc.lbams.PlayClip(gc.lbams.EscapeMusic,gc.lbams.SchoolhouseEscape,true);
                                    gc.ElevdorRea.ForEach(ed => ed.Opendor = true);
                                    gc.Gatesrea.ForEach(g => g.Down(false));
                                    gc.finaleMode = true;
                                    break;
                                case AdditionalGameCustomizer.EscapeFunsies.Taldi:
                                    gc.lbams.EscapeMusic.ClearQueue(true);
                                    gc.lbams.PlayClip(gc.lbams.EscapeMusic,gc.lbams.TaldiEscape,true);
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
                                    StartCoroutine(gc.basicShowMusicShit());
                                    break;
                            }
                        }
                    }
                    if (gc.FinaleSecret)
                    {
                        gc.lbams.EscapeMusic.ClearQueue(true);
                        gc.lbams.EscapeMusic.QueueAudio(gc.lbams.EvapV2Finale[0]);
                        gc.lbams.EscapeMusic.QueueAudio(gc.lbams.EvapV2FinaleIntros[0]);
                        gc.lbams.EscapeMusic.SetLoop(true);
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
    public float angerMult;
    public float tempAngerMult,ScoreDifMult;
    public AudioManagerLiveReaction learnMusic;
    public AudioObjectyeah LearnAudio,aud_AllNotebooks,aud_TeacherJerryAllCheese,aud_Timeout, aud_Prize;
    public GameObject quarter;


    [Header("Scripts")]
    [SerializeField] private KeyFunctions KF;
    public BaldTVyea Television;
    public TutorScript Tutor;
    #endregion

    #region PrivateStates
    private GameControllerScript gc;
    private AudioManagerLiveReaction audioQueue;
    [HideInInspector] public bool learningActive;
    #endregion
}