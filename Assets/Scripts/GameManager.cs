using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public GameObject mainMenu;

    public static bool isPaused = false;
    public GameObject pauseMenuUI;

    public GameObject loadingScreen;

    public static void GoToNextLevel()
    {
        int activeSceneBuildIndex = GetActiveSceneBuildIndex();

        bool isLastLevel = activeSceneBuildIndex + 1 == SceneManager.sceneCountInBuildSettings;
        if (!isLastLevel)
        {
            instance.LoadScene(activeSceneBuildIndex + 1);
        }
        else
        {
            instance.GoToMainMenu();
        }
    }

    public void PressPlay()
    {
        instance.LoadScene(1); // Load Level 1 scene 
        mainMenu.SetActive(false);
    }

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

        if (isPaused)
        {
            EnableMouseCursor();
        }
        else
        {
            DisableMouseCursor();
        }
    }

    public void PressMainMenu()
    {
        PauseToggle();
        GoToMainMenu();
    }

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        string activeSceneName = GetActiveSceneName();
        EnableMouseCursor();
    }

    private void Update()
    {
        string activeSceneName = GetActiveSceneName();

        if (activeSceneName != "Main menu" && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseToggle();
            }
        }
    }

    private void LoadScene(int sceneBuildIndex)
    {
        instance.StartCoroutine(instance.LoadSceneAsync(sceneBuildIndex));
    }

    private IEnumerator LoadSceneAsync(int sceneBuildIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneBuildIndex);

        instance.loadingScreen.SetActive(true);
        LockMouse();

        while (!asyncOperation.isDone) yield return null;

        instance.loadingScreen.SetActive(false);
        UnlockMouse();
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
        EnableMouseCursor();
    }

    private static void RestartLevel(int activeSceneBuildIndex) 
    {
        instance.LoadScene(activeSceneBuildIndex);
    }

    private static string GetActiveSceneName() => SceneManager.GetActiveScene().name;

    private static int GetActiveSceneBuildIndex() => SceneManager.GetActiveScene().buildIndex;

    private static void EnableMouseCursor()
    {
        UnlockMouse();
        Cursor.visible = true;
    }

    private static void DisableMouseCursor()
    {
        LockMouse();
        Cursor.visible = false;
    }

    private static void LockMouse() => Cursor.lockState = CursorLockMode.Locked;

    private static void UnlockMouse() => Cursor.lockState = CursorLockMode.None;
}