using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pitHole : MonoBehaviour
{
    #region SingletonSetup
    private void Awake() => Instance = this;
    public static pitHole Instance;
    public void hiIgotChanged()
    {
        flor.SetActive(false);
        pitwind.SetActive(true);
    }
    #endregion
    public IEnumerator funnyportalaltAreWeDeadass()
    {
        Singleton<TimeOutManagerFUCKYEA>.Instance.ResetTimeoutStuff();
        AudioListener.pause = false;
        Singleton<MusicManager>.Instance.PauseMidi(false);
        EndingManager.Instance.black.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        PlayerPrefs.SetString("CurrentMode", "thePit");
        PlayerPrefsExtension.SetBool("pitGotUnlocked", true);
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadSceneAsync("GameArea");
    }
    public void switchmodeStuff()
    {
        if (!toggled)
        {
            toggled=true;
            StartCoroutine(funnyportalaltAreWeDeadass());
        }
    }
    public GameObject flor,pitwind;
    public bool toggled;
}
