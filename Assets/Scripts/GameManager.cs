using System;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;

    public static void GoToNextLevel()
    {
        int activeSceneBuildIndex = GetActiveSceneBuildIndex();

        bool isLastLevel = activeSceneBuildIndex + 1 == SceneManager.sceneCountInBuildSettings;
        if (!isLastLevel)
        {
            SceneManager.LoadScene(activeSceneBuildIndex + 1);
        }
        else
        {
            GoToMainMenu();
        }
    }

    public void PressPlay() => SceneManager.LoadScene("Level 1"); // Load Level 1 scene

    public void PressRestart()
    {
        PauseToggle();

        int activeSceneBuildIndex = GetActiveSceneBuildIndex();
        RestartLevel(activeSceneBuildIndex);
    }

    public void PressQuit() => Application.Quit();

    public void PauseToggle()
    {
        isPaused = !isPaused;
        pauseMenuUI.SetActive(isPaused);
        Time.timeScale = Convert.ToSingle(!isPaused); // false = 0 | true = 1
        Start();
    }

    public void PressMainMenu()
    {
        PauseToggle();
        GoToMainMenu();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None; // Unlocks the cursor from the middle of the screen
        Cursor.visible = true; // Makes the cursor visible
    }

    private void Update()
    {
        string activeSceneName = GetActiveSceneName();

        if (activeSceneName != "Main Menu" && Input.GetKeyDown(KeyCode.Escape))
        {
            PauseToggle();
        }
    }

    private static void GoToMainMenu() => SceneManager.LoadScene("Main Menu");

    private static void RestartLevel(int activeSceneBuildIndex) => SceneManager.LoadScene(activeSceneBuildIndex);

    private static string GetActiveSceneName() => SceneManager.GetActiveScene().name;

    private static int GetActiveSceneBuildIndex() => SceneManager.GetActiveScene().buildIndex;
}