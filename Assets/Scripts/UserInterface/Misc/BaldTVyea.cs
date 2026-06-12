using UnityEngine;
using System.Collections;

public class BaldTVyea : MonoBehaviour
{
    public void StopTvSequence()
    {
        if (TvCorou != null)
        {
            StopCoroutine(TvCorou);
            WarningMarks.SetActive(false);
            BaldiTVAnimator.Rebind();
            famished.SetActive(false);
            teacherJer.SetActive(false);
            Baldi.SetActive(false);
            Static.SetActive(false);
            TvCorou = null;
        }
    }
    public void TvSequencer(AudioObjectyeah baldiClip,bool BypassDeltatime = false)
    {
        TvCorou = StartCoroutine(StartTVSequence(baldiClip,BypassDeltatime));
    }
    public IEnumerator StartTVSequence(AudioObjectyeah baldiClip,bool BypassDeltatime = false)
    {
        BaldiTVAnimator.updateMode = BypassDeltatime ? AnimatorUpdateMode.UnscaledTime : AnimatorUpdateMode.Normal;
        Statics.updateMode = BypassDeltatime ? AnimatorUpdateMode.UnscaledTime : AnimatorUpdateMode.Normal;
        exclama.updateMode = BypassDeltatime ? AnimatorUpdateMode.UnscaledTime : AnimatorUpdateMode.Normal;
        yield return StartCoroutine(StartLoweringTV(BypassDeltatime));

        yield return StartCoroutine(PlayBaldiClip(baldiClip,BypassDeltatime));

        yield return StartCoroutine(StartLiftingTV(BypassDeltatime));
        
    }

    public IEnumerator StartLoweringTV(bool WaitRealTime = false)
    {
        bool showMarkings = Markings;
        float delayTimer = showMarkings ? 3f : 0.75f;

        if (showMarkings)
        {
            WarningMarks.SetActive(true);
            TelevisionDevice.PlaySingleClip(markingSound == MarkingSoundType.Alert ? mus_Alert : aud_TimesOutBell);
        }

        BaldiTVAnimator.Rebind();
        BaldiTVAnimator.Play("TV_LowerDown", -1, 0f);

        if (!WaitRealTime) yield return new WaitForSeconds(delayTimer);
        else yield return new WaitForSecondsRealtime(delayTimer);

        if (showMarkings) WarningMarks.SetActive(false);

        Static.SetActive(true);
        if (!WaitRealTime) yield return new WaitForSeconds(0.25f);
        else yield return new WaitForSecondsRealtime(0.25f);
        Static.SetActive(false);
        if (famishingit) famished.SetActive(true);
        if (TeacherJerryingIt) teacherJer.SetActive(true);
        if (baldingit) Baldi.SetActive(true);
    }

    public IEnumerator PlayBaldiClip(AudioObjectyeah clip,bool WaitRealTime = false)
    {
        if (clip == null) yield break;

        if (famishingit) famAudDevice.PlaySingleClip(clip);
        if (TeacherJerryingIt) teacherJerAudDevice.PlaySingleClip(clip);
        if (baldingit) BaldiDevice.PlaySingleClip(clip);
        
        float timer = 0f;
        bool bypassPauseCountdown = WaitRealTime;
        Debug.Log("it bypass");

        while (timer < clip.audClip.length)
        {   
            if (!AudioListener.pause || bypassPauseCountdown)timer += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    public IEnumerator StartLiftingTV(bool WaitRealTime = false)
    {
        Debug.Log("lift");
        Baldi.SetActive(false);
        famished.SetActive(false);
        teacherJer.SetActive(false);
        Static.SetActive(true);
        if (!WaitRealTime) yield return new WaitForSeconds(0.25f);
        else yield return new WaitForSecondsRealtime(0.25f);
        Static.SetActive(false);

        BaldiTVAnimator.Rebind();
        BaldiTVAnimator.Play("TV_RiseUp", -1, 0f);
    }

    [Header("Serialized References")]
    [SerializeField] private Animator BaldiTVAnimator;
    [SerializeField] private Animator Statics,exclama;
    [SerializeField] private GameObject Static, Baldi, WarningMarks, famished, teacherJer;
    [SerializeField] private AudioManagerLiveReaction TelevisionDevice, BaldiDevice,famAudDevice, teacherJerAudDevice;
    [SerializeField] private AudioObjectyeah mus_Alert, aud_TimesOutBell;
    [HideInInspector] public Coroutine TvCorou;
    [Header("Extras")]
    public bool Markings;
    public bool baldingit, famishingit,TeacherJerryingIt;
    [SerializeField] private MarkingSoundType markingSound = MarkingSoundType.Alert;
    public enum MarkingSoundType { Alert, Bell };
}