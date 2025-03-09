using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance

    // Game settings
    public bool isEdgePanningEnabled = true; // Toggle for camera edge panning
    public float gameSpeed = 1.0f; // Game speed control (1x, 2x, etc.)
    public bool isPaused = false; // Game pause state

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make sure GameManager persists across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Example: Pause/Resume toggle
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void ToggleEdgePanning()
    {
        isEdgePanningEnabled = !isEdgePanningEnabled;
        Debug.Log("Edge Panning: " + (isEdgePanningEnabled ? "Enabled" : "Disabled"));
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : gameSpeed;
        Debug.Log("Game " + (isPaused ? "Paused" : "Resumed"));
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pauses the game
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = gameSpeed; // Resumes the game
    }

    public void SetGameSpeed(float speed)
    {
        gameSpeed = speed;
        if (!isPaused)
        {
            Time.timeScale = gameSpeed;
        }
        Debug.Log("Game Speed set to " + gameSpeed + "x");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
