using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    public GameObject pauseMenuCanvas;
    private bool isPaused = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false);
            Debug.Log("Pause menu canvas initialized (hidden).");
        }
        else
        {
            Debug.LogWarning("pauseMenuCanvas not assigned in PauseManager!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
            Debug.Log("Game paused.");
        }

        // Hide other menus
        if (GameManager.Instance != null && GameManager.Instance.menuScreen != null)
        {
            GameManager.Instance.menuScreen.SetActive(false);
            Debug.Log("Main menu hidden during pause.");
        }
        if (SettingsManager.Instance != null && SettingsManager.Instance.settingsCanvas != null)
        {
            SettingsManager.Instance.settingsCanvas.SetActive(false);
            Debug.Log("Settings menu hidden during pause.");
        }
    }

    public void ResumeGame()
    {
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
            Debug.Log("Game resumed.");
        }

        if (GameManager.Instance != null && GameManager.Instance.menuScreen != null)
        {
            GameManager.Instance.menuScreen.SetActive(false);
            Debug.Log("Ensuring main menu is hidden on resume.");
        }
        if (SettingsManager.Instance != null && SettingsManager.Instance.settingsCanvas != null)
        {
            SettingsManager.Instance.settingsCanvas.SetActive(false);
            Debug.Log("Ensuring settings menu is hidden on resume.");
        }
    }

    public void QuitToMenu()
    {
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false);
        }
        if (GameManager.Instance != null && GameManager.Instance.menuScreen != null)
        {
            GameManager.Instance.menuScreen.SetActive(true);
            Time.timeScale = 0f;
            isPaused = false;
            Debug.Log("Quit to main menu.");
        }
        if (SettingsManager.Instance != null && SettingsManager.Instance.settingsCanvas != null)
        {
            SettingsManager.Instance.settingsCanvas.SetActive(false);
            Debug.Log("Settings menu hidden on quit to menu.");
        }
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}