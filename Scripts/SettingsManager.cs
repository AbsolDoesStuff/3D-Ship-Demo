using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public GameObject settingsCanvas;
    public Slider musicSlider;
    public Slider sfxSlider;

    private bool openedFromPauseMenu = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (settingsCanvas != null)
        {
            settingsCanvas.SetActive(false);
            Debug.Log("Settings canvas initialized (hidden).");
        }
        else
        {
            Debug.LogError("SettingsCanvas not assigned in SettingsManager!");
        }

        if (musicSlider != null)
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }
        else
        {
            Debug.LogWarning("MusicSlider not assigned in SettingsManager!");
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
        else
        {
            Debug.LogWarning("SFXSlider not assigned in SettingsManager!");
        }
    }

    public void OpenSettings(bool fromPauseMenu)
    {
        openedFromPauseMenu = fromPauseMenu;
        Debug.Log($"Opening settings menu. Opened from pause menu: {openedFromPauseMenu}");

        // Show the settings menu
        if (settingsCanvas != null)
        {
            settingsCanvas.SetActive(true);
            Debug.Log("Settings canvas shown.");
        }
        else
        {
            Debug.LogError("Cannot open settings menu: SettingsCanvas is null!");
            return;
        }

        // Hide the menu we came from
        if (fromPauseMenu)
        {
            if (PauseManager.Instance != null && PauseManager.Instance.pauseMenuCanvas != null)
            {
                PauseManager.Instance.pauseMenuCanvas.SetActive(false);
                Debug.Log("Hiding pause menu canvas.");
            }
            else
            {
                Debug.LogWarning("PauseManager or pauseMenuCanvas is null!");
            }

            // Ensure main menu is hidden
            if (GameManager.Instance != null && GameManager.Instance.menuScreen != null)
            {
                GameManager.Instance.menuScreen.SetActive(false);
                Debug.Log("Ensuring main menu is hidden.");
            }
        }
        else
        {
            if (GameManager.Instance != null && GameManager.Instance.menuScreen != null)
            {
                GameManager.Instance.menuScreen.SetActive(false);
                Debug.Log("Hiding main menu screen.");
            }
            else
            {
                Debug.LogWarning("GameManager or menuScreen is null!");
            }

            // Ensure pause menu is hidden
            if (PauseManager.Instance != null && PauseManager.Instance.pauseMenuCanvas != null)
            {
                PauseManager.Instance.pauseMenuCanvas.SetActive(false);
                Debug.Log("Ensuring pause menu is hidden.");
            }
        }
    }

    public void CloseSettings()
    {
        if (settingsCanvas != null)
        {
            settingsCanvas.SetActive(false);
            Debug.Log("Settings menu closed.");
        }

        if (openedFromPauseMenu)
        {
            if (PauseManager.Instance != null && PauseManager.Instance.pauseMenuCanvas != null)
            {
                PauseManager.Instance.pauseMenuCanvas.SetActive(true);
                Debug.Log("Showing pause menu canvas.");
            }
            if (GameManager.Instance != null && GameManager.Instance.menuScreen != null)
            {
                GameManager.Instance.menuScreen.SetActive(false);
                Debug.Log("Ensuring main menu screen is hidden.");
            }
        }
        else
        {
            if (GameManager.Instance != null && GameManager.Instance.menuScreen != null)
            {
                GameManager.Instance.menuScreen.SetActive(true);
                Debug.Log("Showing main menu screen.");
            }
            if (PauseManager.Instance != null && PauseManager.Instance.pauseMenuCanvas != null)
            {
                PauseManager.Instance.pauseMenuCanvas.SetActive(false);
                Debug.Log("Ensuring pause menu canvas is hidden.");
            }
        }
    }

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetMusicVolume(volume);
        }
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetSFXVolume(volume);
        }
    }
}