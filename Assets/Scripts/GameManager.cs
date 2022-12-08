using System;
using System.Collections;
using System.Threading;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    private bool levelIsCompleted = false;

    public GameObject mainMenu;

    public static bool isPaused = false;
    public GameObject pauseMenuUI;

    public GameObject levelStartText;
    public int levelStartTextSeconds;

    public GameObject levelCompletedText;
    public int levelCompletedDelaySeconds;

    public static bool levelIsCompletedStatic => instance.levelIsCompleted;

    public GameObject loadingScreen;

    public AudioSource ambientSound;

    public AudioSource christmasSongs;

    public static void GoToNextLevel() => instance.StartCoroutine(instance.CompleteLevel());

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
            ambientSound.Pause();

            string activeSceneName = GetActiveSceneName();
            if (activeSceneName == "Level 3")
            {
                christmasSongs.Pause();
            }

            EnableMouseCursor();
        }
        else
        {
            ambientSound.UnPause();

            string activeSceneName = GetActiveSceneName();
            if (activeSceneName == "Level 3")
            {
                christmasSongs.UnPause();
            }

            DisableMouseCursor();
        }
    }

    public void PressMainMenu()
    {
        PauseToggle();
        GoToMainMenu();
    }

    private void Awake()
    {
        instance = this;

        string activeSceneName = GetActiveSceneName();
        if (activeSceneName != "Main menu")
        {
            instance.StartCoroutine(instance.ShowLevelStartText());
        }
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
            if (!isPaused && !instance.levelIsCompleted)
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
    }

    private IEnumerator CompleteLevel()
    {
        levelCompletedText.SetActive(true);
        instance.levelIsCompleted = true;

        yield return instance.StartCoroutine(instance.WaitForLevelToLoad());

        levelCompletedText.SetActive(false);

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

    private IEnumerator ShowLevelStartText()
    {
        levelStartText.SetActive(true);

        yield return new WaitForSeconds(levelStartTextSeconds);

        levelStartText.SetActive(false);
    }

    private IEnumerator WaitForLevelToLoad()
    {
        yield return new WaitForSeconds(levelCompletedDelaySeconds);
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("Main menu");
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