using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TransistionManager : MonoBehaviour
{
    public string detailsthing, detailsthing2, statething, statething2, largimag, largtext;
    public void Start()
    {
        DiscordRPC_stuff.current.UpdateStatus(detailsthing, statething, largimag, largtext);
    }
    public void Transition()
    {
        Time.timeScale = 1f;
        StartCoroutine(TransitionEnumerator());
    }

    public IEnumerator TransitionEnumerator()
    {
        DiscordRPC_stuff.current.UpdateStatus("transistioning,.,.,.", "mwah", largimag, largtext);
        Cursor.LockCursor();
        if (transistype == TransistionType.CircleInOut)
        {
            if (!fastTransistion) yield return new WaitForSecondsRealtime(0.7f);
            circleIOFrom.SetTrigger("nooo");
            if (changeMusik) menumusi.SmoothlyPlayUHHHH(theme,smoothFadeDuration);
            yield return new WaitForSecondsRealtime(1.55f);
            to.transform.parent.gameObject.SetActive(true);
            circleIOTo.SetTrigger("yooo");
            yield return new WaitForSecondsRealtime(0.2f);
            yield return new WaitForSecondsRealtime(0.8f);
            from.transform.parent.gameObject.SetActive(false);
        }
        if (transistype == TransistionType.Dither)
        {
            yield return new WaitForEndOfFrame();
            ditherFrom.SetTrigger("close");
            to.transform.parent.gameObject.SetActive(true);
            ditherTo.SetTrigger("open");
            if (changeMusik) menumusi.SmoothlyPlayUHHHH(theme,smoothFadeDuration);
            yield return new WaitForSecondsRealtime(0.6f);
            from.transform.parent.gameObject.SetActive(false);
        }
        Cursor.UnlockCursor();
        if (AfterTransisCompleteEvent != null) AfterTransisCompleteEvent.Invoke();
        DiscordRPC_stuff.current.UpdateStatus(detailsthing2, statething2, largimag, largtext);
    }

    public GameObject from,to;
    public Animator ditherFrom,ditherTo,circleIOFrom,circleIOTo;
    public bool changeMusik;
    public SmoothMusicTransis menumusi;
    public float smoothFadeDuration;
    public AudioObjectyeah theme;
    [SerializeField] private CursorControllerScript Cursor;
    public bool fastTransistion;
    public TransistionType transistype = TransistionType.CircleInOut;
    public UnityEvent AfterTransisCompleteEvent;
    public enum TransistionType
    {
        Dither,
        CircleInOut
        
    }
}