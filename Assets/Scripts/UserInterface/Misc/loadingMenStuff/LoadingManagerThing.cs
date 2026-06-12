using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
public class LoadingManagerThing : MonoBehaviour
{
    public static LoadingManagerThing Instance;
    public CanvasGroup loadMenMain,loadMenCompo1;
    private float AlphaVal = 0f;
    public Animator circl;
    public bool IsInLoadTransistion;
    public TMP_Text PercentText;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            SetLoadingAlphaValue(0f);
            CircleTransistionStuff(true);
        }
        else Destroy(this);
    }
    private void Update()
    {
        loadMenMain.alpha = Mathf.Lerp(loadMenMain.alpha,AlphaVal, 5f * Time.unscaledDeltaTime);
    }
    public void SetLoadingAlphaValue(float val)
    {
        AlphaVal = val;
    }
    public void CircleTransistionStuff(bool close)
    {
        if (close) circl.SetTrigger("nooo");
        else circl.SetTrigger("yooo");
    }
    public void LoadSceneAsyncUHHH(string sceneName,float Delay = 0f,bool CursorVisible = true)
    {
        StartCoroutine(loadingyummers(sceneName,Delay,CursorVisible));
    }
    private IEnumerator loadingyummers(string sceneName,float delaySec,bool CursorVisible)
    {
        Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
        yield return new WaitForSecondsRealtime(delaySec);
        IsInLoadTransistion = true;
        PercentText.text = "loadin...";
        Time.timeScale = 0f;
        SetLoadingAlphaValue(1f);
        yield return new WaitForSecondsRealtime(1f);
        CircleTransistionStuff(false);
        yield return new WaitForSecondsRealtime(1f);
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName); 
        while (!load.isDone)
        {
            PercentText.text = $"hold on its loading, its at {(float)load.progress}%";
            yield return null;
        }
        PercentText.text = "done!";
        yield return new WaitForSecondsRealtime(0.5f);
        CircleTransistionStuff(true);
        yield return new WaitForSecondsRealtime(1.2f);
        SetLoadingAlphaValue(0);
        Time.timeScale = 1f;
        IsInLoadTransistion = false;
        if (AudioListener.pause == true) AudioListener.pause = false;
        Cursor.lockState = CursorLockMode.None;
		Cursor.visible = CursorVisible;
        

        Debug.Log("its/./////DONEEEE");
    }
}
