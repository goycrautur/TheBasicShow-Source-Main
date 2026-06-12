using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;
public class KeyFunctions : MonoBehaviour
{
    #region UnityCallbacks
    private void Start() 
    {
        PlaceholdCutscenAlpha = 0f;
        NoCutscenesQuestionmark.alpha = 0f;
        LockMouse();
    }

    private void Awake() => hi = this;
    private void whatever() => NoCutscenesQuestionmark.alpha = Mathf.Lerp(NoCutscenesQuestionmark.alpha,PlaceholdCutscenAlpha, 3f * Time.unscaledDeltaTime);
    public static KeyFunctions hi;
    private void Update()
    {
        if (!gamePaused)
        {
            ItemCollecting();
        }
        PauseAndExit();
        whatever();
    }
    
    private void LateUpdate()
    {
        if (!gamePaused)
        {
            if (Input.GetKey(KeyCode.Tab))
            {
                Minimap.SetActive(true);
                minimapActive = true;
            }
            else
            {
                Minimap.SetActive(false);
                minimapActive = false;
            }
        }
    }
    #endregion

    #region PauseAndExitManagement
    private void PauseAndExit()
    {
        if (gc.Math.learningActive || gc.player.gameOver || gc.youCantPause || LoadingManagerThing.Instance.IsInLoadTransistion) return;
        if (Singleton<InputManager>.Instance.GetActionKey(InputAction.PauseOrCancel) && !gc.progress.GetResults) ToggleGamePause(!gamePaused);
        if (gamePaused)
        {
            if (Input.GetKeyDown(KeyCode.Y)) ExitGame();
            if (Input.GetKeyDown(KeyCode.R))  ResetGame();
            else if (Input.GetKeyDown(KeyCode.N)) ToggleGamePause(false);
        }
    }

    public void ToggleGamePause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
        Singleton<MusicManagerMaes>.Instance.PauseMidi(isPaused);
        audballs.SetIgnoreListenerPause(isPaused);
        AudioListener.pause = isPaused;
        gamePaused = isPaused;
        pauseMenu.SetActive(isPaused);

        if (isPaused) UnlockMouse();
        else LockMouse();
    }

    public void ExitGame()
    {
        Singleton<TimeOutManagerFUCKYEA>.Instance.ResetTimeoutStuff();
        LoadingManagerThing.Instance.LoadSceneAsyncUHHH("MainMenu");
        Singleton<MusicManagerMaes>.Instance.PauseMidi(false);
    }
    public void ResetGame()
    {
        Singleton<TimeOutManagerFUCKYEA>.Instance.ResetTimeoutStuff();
        LoadingManagerThing.Instance.LoadSceneAsyncUHHH("GameArea",0,false);
        Singleton<MusicManagerMaes>.Instance.PauseMidi(false);
    }
    #endregion

    #region ItemInteraction
    private void ItemCollecting()
    {
        if (Time.timeScale == 0f) return;

        if (Input.GetMouseButtonDown(0) || Singleton<InputManager>.Instance.GetActionKey(InputAction.Interact))
        {
            if (Sych.ScreenCenterRaycast(out RaycastHit hit,PlayerClickablesLayer.value) && hit.transform.IsWithinDistance(gc.player.LocalRange))
            {
                if (hit.collider.TryGetComponent(out Interactable interactable)) interactable.Interact();
            }
        }
    }
    #endregion

    #region CursorControl
    public void LockMouse()
    {
        if (!gc.Math.learningActive)
        {
            cursorController.LockCursor();
            mouseLocked = true;
            reticle.SetActive(true);
        }
    }

    public void UnlockMouse()
    {
        cursorController.UnlockCursor();
        mouseLocked = false;
        reticle.SetActive(false);
    }
    public void PlaceholdCutscene(float WaitDuration = 0f,bool ForcePause = true,UltEvents.UltEvent StartingEvent = null, UltEvents.UltEvent AfterDuraEvent = null)
    {
        if (PlaceholdCutsceneCouro != null) 
        {
            StopCoroutine(PlaceholdCutsceneCouro);
            PlaceholdCutscenAlpha = 0f;
            AfterCutsceneDone(AfterDuraEvent);
            PlaceholdCutsceneCouro = null;
            return;
        }
        PlaceholdCutsceneCouro = StartCoroutine(CutsceneAhahaHelpMEFUCK(WaitDuration,ForcePause,StartingEvent,AfterDuraEvent));
    }
    
    public IEnumerator CutsceneAhahaHelpMEFUCK(float WaitDuration,bool ForcePause,UltEvents.UltEvent StartingEvent, UltEvents.UltEvent AfterDuraEvent)
    {
        float time = WaitDuration;
        if (StartingEvent != null) StartingEvent.Invoke();
        AudioListener.pause = true;
        PlaceholdCutsceneActive = true;
        gc.youCantPause = true;
        PlaceholdCutscenAlpha = 1f;
        Time.timeScale = ForcePause ? 0f : 1f;
        while (time > 0f)
        {
            if (PlaceholdCutsceneActive && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
            {
                PlaceholdCutscenAlpha = 0f;
                time = 0.01f;
                yield return null;
            }
            time -= !ForcePause ? Time.deltaTime : Time.unscaledDeltaTime;
            yield return null;
        }
        PlaceholdCutscenAlpha = 0f;
        AfterCutsceneDone(AfterDuraEvent);
        yield return null;
    }
    private void AfterCutsceneDone(UltEvents.UltEvent AfterDuraEvent)
    {
        AudioListener.pause = false;
        PlaceholdCutsceneActive = false;
        Time.timeScale = 1f;
        gc.youCantPause = false;
        if (AfterDuraEvent != null) AfterDuraEvent.Invoke();
    }
    #endregion

    #region SerializedFields
    public CanvasGroup NoCutscenesQuestionmark;
    [SerializeField] private float PlaceholdCutscenAlpha;
    [HideInInspector] public Coroutine PlaceholdCutsceneCouro;
    public bool PlaceholdCutsceneActive;
    [SerializeField] private GameObject pauseMenu, reticle,Minimap;
    [SerializeField] private GameControllerScript gc;
    [SerializeField] private CursorControllerScript cursorController;
    [SerializeField] private AudioManagerLiveReaction audballs;
    public LayerMask PlayerClickablesLayer;
    #endregion

    #region PublicState
    [HideInInspector] public bool mouseLocked, gamePaused,minimapActive;
    #endregion
}