using UnityEngine;
using System.Collections;

public class BaldTVyea : MonoBehaviour
{
    public IEnumerator StartTVSequence(AudioObjectyeah baldiClip)
    {
        yield return StartCoroutine(StartLoweringTV());

        yield return StartCoroutine(PlayBaldiClip(baldiClip));

        yield return StartCoroutine(StartLiftingTV());
    }

    public IEnumerator StartLoweringTV()
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

        yield return new WaitForSeconds(delayTimer);

        if (showMarkings) WarningMarks.SetActive(false);

        Static.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        Static.SetActive(false);
        if (famishingit) famished.SetActive(true);
        if (TeacherJerryingIt) teacherJer.SetActive(true);
        if (baldingit) Baldi.SetActive(true);
    }

    public IEnumerator PlayBaldiClip(AudioObjectyeah clip)
    {
        if (clip == null) yield break;

        if (famishingit) famAudDevice.PlaySingleClip(clip);
        if (TeacherJerryingIt) teacherJerAudDevice.PlaySingleClip(clip);
        if (baldingit) BaldiDevice.PlaySingleClip(clip);
        
        float timer = 0f;

        while (timer < clip.audClip.length)
        {
            if (!AudioListener.pause) timer += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    public IEnumerator StartLiftingTV()
    {
        Baldi.SetActive(false);
        famished.SetActive(false);
        teacherJer.SetActive(false);
        Static.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        Static.SetActive(false);

        BaldiTVAnimator.Rebind();
        BaldiTVAnimator.Play("TV_RiseUp", -1, 0f);
    }

    [Header("Serialized References")]
    [SerializeField] private Animator BaldiTVAnimator;
    [SerializeField] private GameObject Static, Baldi, WarningMarks, famished, teacherJer;
    [SerializeField] private AudioManagerLiveReaction TelevisionDevice, BaldiDevice,famAudDevice, teacherJerAudDevice;
    [SerializeField] private AudioObjectyeah mus_Alert, aud_TimesOutBell;
    [Header("Extras")]
    public bool Markings;
    public bool baldingit, famishingit,TeacherJerryingIt;
    [SerializeField] private MarkingSoundType markingSound = MarkingSoundType.Alert;
    public enum MarkingSoundType { Alert, Bell };
}