using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class CircleInOutScript : MonoBehaviour
{
    #region SingletonSetup
    private void Awake() => Instance = this;
    public static CircleInOutScript Instance;
    #endregion
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
        from.GetComponent<Animator>().SetTrigger("nooo");
        if (changeMusik)
        {
            menumusi.ClearQueue(true);
            menumusi.QueueAudio(theme);
            menumusi.SetLoop(true);
        }
        yield return new WaitForSecondsRealtime(1.55f);
        to.transform.parent.gameObject.SetActive(true);
        to.GetComponent<Animator>().SetTrigger("yooo");
        yield return new WaitForSecondsRealtime(0.2f);
        yield return new WaitForSecondsRealtime(0.8f);
        from.transform.parent.gameObject.SetActive(false);
        Cursor.UnlockCursor();
        DiscordRPC_stuff.current.UpdateStatus(detailsthing2, statething2, largimag, largtext);
    }

    public GameObject from;
    public GameObject to;
    public bool changeMusik;
    public AudioManagerLiveReaction menumusi;
    public AudioObjectyeah theme;
    [SerializeField] private CursorControllerScript Cursor;
    public bool fastTransistion;
}