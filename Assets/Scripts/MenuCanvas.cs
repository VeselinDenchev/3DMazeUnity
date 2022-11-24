using System;

using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCanvas : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;

    public void PressPlay() // Method for play button
    {
        SceneManager.LoadScene("Level 1"); // Load Level 1 scene
    }

    public void PressRestart()
    {
        PauseToggle();

        string activeSceneName = GetActiveSceneName();
        SceneManager.LoadScene(activeSceneName);
    }

    public void PressQuit() // Method for quit Button
    {
        Application.Quit(); // Quits the game
    }

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
        SceneManager.LoadScene("Main Menu");
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

    private string GetActiveSceneName() => SceneManager.GetActiveScene().name;
}