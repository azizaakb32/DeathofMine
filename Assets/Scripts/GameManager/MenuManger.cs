using UnityEngine;
using UnityEngine.SceneManagement;

// =====================================================================
// MenuManager.cs
// -----------------------------------------------------------------
// This script is attached to the "MenuManager" object and controls
// all menu panels (MainMenuPanel, PauseMenu, YouDie, Escaped, GameMenu)
// and their buttons (Play, Quit, Resume, etc.).
//
// HOW IT WORKS:
//  - Only ONE panel is ever visible at a time, the rest stay disabled
//  - Each "Show..." function first disables everything,
//    then enables the required panel
//  - Buttons call these functions via OnClick() in the Inspector
// =====================================================================

public class MenuManager : MonoBehaviour
{
    [Header("Panels (drag the matching objects here from the Hierarchy)")]
    [SerializeField] private GameObject mainMenuPanel;   // "MainMenuPanel"
    [SerializeField] private GameObject pauseMenuPanel;  // "PauseMenu"
    [SerializeField] private GameObject youDiePanel;      // "YouDie"
    [SerializeField] private GameObject escapedPanel;     // "Escaped"
    [SerializeField] private GameObject gameMenuPanel;    // "GameMenu" (in-game HUD)

    private void Start()
    {
        // Only the main menu is visible when the game starts
        ShowMainMenu();
    }

    // ---------------------------------------------------------------
    // CURSOR STATE HANDLING
    // ---------------------------------------------------------------
    // menuOpen = true  -> cursor is visible and free to move (for clicking buttons)
    // menuOpen = false -> cursor is hidden and locked to the screen center (for the FPS camera)
    private void SetCursorState(bool menuOpen)
    {
        if (menuOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // ---------------------------------------------------------------
    // PANEL SWITCHING FUNCTIONS
    // ---------------------------------------------------------------

    // Helper function that disables all panels.
    // Each reference is null-checked, so no error is thrown even if
    // a panel hasn't been assigned in the Inspector yet - it's just skipped.
    private void CloseAllPanels()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (youDiePanel != null) youDiePanel.SetActive(false);
        if (escapedPanel != null) escapedPanel.SetActive(false);
        if (gameMenuPanel != null) gameMenuPanel.SetActive(false);
    }

    public void ShowMainMenu()
    {
        Time.timeScale = 1f;
        CloseAllPanels();
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        SetCursorState(true);
    }

    public void ShowGameMenu()
    {
        Time.timeScale = 1f;
        CloseAllPanels();
        if (gameMenuPanel != null) gameMenuPanel.SetActive(true);
        SetCursorState(false);
    }

    public void ShowPauseMenu()
    {
        Time.timeScale = 0f;
        CloseAllPanels();
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        SetCursorState(true);
    }

    public void ShowYouDie()
    {
        Time.timeScale = 0f;
        CloseAllPanels();
        if (youDiePanel != null) youDiePanel.SetActive(true);
        SetCursorState(true);
    }

    public void ShowEscaped()
    {
        Time.timeScale = 0f;
        CloseAllPanels();
        if (escapedPanel != null) escapedPanel.SetActive(true);
        SetCursorState(true);
    }

    // ---------------------------------------------------------------
    // BUTTON FUNCTIONS (wired to OnClick() in the Inspector)
    // ---------------------------------------------------------------

    // === Buttons inside MainMenuPanel ===

    public void PlayButton()
    {
        ShowGameMenu();
    }

    public void QuitButton()
    {
        Application.Quit();

#if UNITY_EDITOR
        // Application.Quit() doesn't work in the Unity Editor,
        // so this line stops Play mode instead (Editor only)
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // === Buttons inside PauseMenu ===

    public void ResumeButton()
    {
        ShowGameMenu();
    }

    // === Buttons inside YouDie ===

    // Reloads the current scene
    public void RestartButton()
    {
        Time.timeScale = 1f; // Reset time scale, otherwise the reloaded scene stays paused too
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // === Buttons inside Escaped ===

    public void ContinueButton()
    {
        // Next level/stage logic goes here.
        // For now it just returns to the game menu:
        ShowGameMenu();
    }

    // Quit button on the Escaped panel (same behavior as QuitButton() above)
    public void QuiteButton()
    {
        QuitButton();
    }

    // === Shared button across multiple panels ===

    // "MainMenuButton" is used by PauseMenu, YouDie, and Escaped panels,
    // all wired to this single function
    public void MainMenuButton()
    {
        ShowMainMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // Toggles pause state on/off
    public void TogglePause()
    {
        if (pauseMenuPanel != null && pauseMenuPanel.activeSelf)
        {
            ShowGameMenu();
        }
        else if (gameMenuPanel != null && gameMenuPanel.activeSelf)
        {
            ShowPauseMenu();
        }
    }
}
