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
        this.LoadScene(1); // Load Level 1 scene 
        mainMenu.SetActive(false);
    }

    public void PressRestart()
    {
        this.PauseToggle();

        int activeSceneBuildIndex = GetActiveSceneBuildIndex();
        RestartLevel(activeSceneBuildIndex);
    }

    public void PressQuit() => Application.Quit();

    public void PauseToggle()
    {
        isPaused = !isPaused;
        this.pauseMenuUI.SetActive(isPaused);
        Time.timeScale = Convert.ToSingle(!isPaused); // false = 0 | true = 1

        if (isPaused)
        {
            this.ambientSound.Pause();

            string activeSceneName = GetActiveSceneName();
            if (activeSceneName == "Level 3")
            {
                this.christmasSongs.Pause();
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
            base.StartCoroutine(this.ShowLevelStartText());
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
            if (!isPaused && !this.levelIsCompleted)
            {
                this.PauseToggle();
            }
        }
    }

    private void LoadScene(int sceneBuildIndex)
    {
        base.StartCoroutine(this.LoadSceneAsync(sceneBuildIndex));
    }

    private IEnumerator LoadSceneAsync(int sceneBuildIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneBuildIndex);

        this.loadingScreen.SetActive(true);
        LockMouse();

        while (!asyncOperation.isDone) yield return null;
    }

    private IEnumerator CompleteLevel()
    {
        this.levelCompletedText.SetActive(true);
        this.levelIsCompleted = true;

        yield return instance.StartCoroutine(instance.WaitForLevelToLoad());

        this.levelCompletedText.SetActive(false);

        int activeSceneBuildIndex = GetActiveSceneBuildIndex();

        bool isLastLevel = activeSceneBuildIndex + 1 == SceneManager.sceneCountInBuildSettings;
        if (!isLastLevel)
        {
            this.LoadScene(activeSceneBuildIndex + 1);
        }
        else
        {
            this.GoToMainMenu();
        }
    }

    private IEnumerator ShowLevelStartText()
    {
        this.levelStartText.SetActive(true);

        yield return new WaitForSeconds(this.levelStartTextSeconds);

        this.levelStartText.SetActive(false);
    }

    private IEnumerator WaitForLevelToLoad()
    {
        yield return new WaitForSeconds(this.levelCompletedDelaySeconds);
    }

    private void GoToMainMenu()
    {
        this.LoadScene(0);

        if (this.levelStartText.activeSelf)
        {
            this.levelStartText.SetActive(false);
        }

        EnableMouseCursor();
    }

    private static void RestartLevel(int activeSceneBuildIndex) 
    {
        if (instance.levelStartText.activeSelf)
        {
            instance.levelStartText.SetActive(false);
        }

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