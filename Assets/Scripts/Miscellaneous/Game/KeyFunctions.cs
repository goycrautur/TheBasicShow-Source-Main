using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyFunctions : MonoBehaviour
{
    #region UnityCallbacks
    private void Start() => LockMouse();
    private void Awake() => hi = this;
    public static KeyFunctions hi;
    private void Update()
    {
        if (!gamePaused)
        {
            ItemCollecting();
        }
        PauseAndExit();
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
        if (gc.Math.learningActive || gc.player.gameOver || gc.youCantPause) return;

        if (Singleton<InputManager>.Instance.GetActionKey(InputAction.PauseOrCancel) && !gc.progress.GetResults)
        {
            ToggleGamePause(!gamePaused);
        }

        if (gamePaused)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                ExitGame();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetGame();
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                ToggleGamePause(false);
            }
        }
    }

    public void ToggleGamePause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
        Singleton<MusicManager>.Instance.PauseMidi(isPaused ? true : false);
        audballs.ignoreListenerPause = isPaused;
        AudioListener.pause = isPaused;
        gamePaused = isPaused;
        pauseMenu.SetActive(isPaused);

        if (isPaused)
        {
            UnlockMouse();
        }
        else
        {
            LockMouse();
        }
    }

    public void ExitGame()
    {
        AudioListener.pause = false;
        SceneManager.LoadSceneAsync("MainMenu");
        Singleton<MusicManager>.Instance.PauseMidi(false);
    }
    public void ResetGame()
    {
        AudioListener.pause = false;
        SceneManager.LoadSceneAsync("GameArea");
        Singleton<MusicManager>.Instance.PauseMidi(false);
    }
    #endregion

    #region ItemInteraction
    private void ItemCollecting()
    {
        if (Time.timeScale == 0f) return;

        if (Input.GetMouseButtonDown(0) || Singleton<InputManager>.Instance.GetActionKey(InputAction.Interact))
        {
            if (Sych.ScreenCenterRaycast(out RaycastHit hit) && hit.transform.IsWithinDistance(gc.player.LocalRange))
            {
                if (hit.collider.TryGetComponent(out Interactable interactable))
                {
                    interactable.Interact();
                }
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
    #endregion

    #region SerializedFields
    [SerializeField] private GameObject pauseMenu, reticle,Minimap;
    [SerializeField] private GameControllerScript gc;
    [SerializeField] private CursorControllerScript cursorController;
    [SerializeField] private AudioSource audballs;
    #endregion

    #region PublicState
    [HideInInspector] public bool mouseLocked, gamePaused,minimapActive;
    #endregion
}