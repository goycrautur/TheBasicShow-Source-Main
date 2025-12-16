using UnityEngine;
using System.Collections;

public class BaldTVyea : MonoBehaviour
{
    public IEnumerator StartTVSequence(AudioClip baldiClip,subsScriptableObject subtitlObjec)
    {
        yield return StartCoroutine(StartLoweringTV());

        yield return StartCoroutine(PlayBaldiClip(baldiClip,subtitlObjec));

        yield return StartCoroutine(StartLiftingTV());
    }

    public IEnumerator StartLoweringTV()
    {
        bool showMarkings = Markings;
        float delayTimer = showMarkings ? 3f : 0.75f;

        if (showMarkings)
        {
            WarningMarks.SetActive(true);
            TelevisionDevice.PlayOneShot(markingSound == MarkingSoundType.Alert ? mus_Alert : aud_TimesOutBell);
            GameControllerScript.Instance.SubsManager.summonLeSubtitle2D(belltimeou.subtitleOption,belltimeou,new Vector3(0f,40.5f,0f),TelevisionDevice);
        }

        BaldiTVAnimator.Rebind();
        BaldiTVAnimator.Play("TV_LowerDown", -1, 0f);

        yield return new WaitForSeconds(delayTimer);

        if (showMarkings) WarningMarks.SetActive(false);

        Static.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        Static.SetActive(false);
        if (famishingit)
        {
            famished.SetActive(true);
        }
        if (baldingit)
        {
            Baldi.SetActive(true);
        }
    }

    public IEnumerator PlayBaldiClip(AudioClip clip,subsScriptableObject subtitlObjec)
    {
        if (clip == null) yield break;

        BaldiDevice.PlayOneShot(clip);
        GameControllerScript.Instance.SubsManager.summonLeSubtitle2D(subtitlObjec.subtitleOption,subtitlObjec,new Vector3(0f,40.5f,0f),BaldiDevice);
        float timer = 0f;

        while (timer < clip.length)
        {
            if (!AudioListener.pause) timer += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    public IEnumerator StartLiftingTV()
    {
        Baldi.SetActive(false);
        famished.SetActive(false);
        Static.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        Static.SetActive(false);

        BaldiTVAnimator.Rebind();
        BaldiTVAnimator.Play("TV_RiseUp", -1, 0f);
    }

    [Header("Serialized References")]
    [SerializeField] private Animator BaldiTVAnimator;
    [SerializeField] private GameObject Static, Baldi, WarningMarks, famished;
    [SerializeField] private AudioSource TelevisionDevice, BaldiDevice;
    [SerializeField] private AudioClip mus_Alert, aud_TimesOutBell;
    [SerializeField] private subsScriptableObject belltimeou;
    [Header("Extras")]
    public bool Markings, baldingit, famishingit;
    [SerializeField] private MarkingSoundType markingSound = MarkingSoundType.Alert;
    public enum MarkingSoundType { Alert, Bell };
}