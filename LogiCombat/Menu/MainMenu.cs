using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsMenu; // Reference to the options menu UI

    //public void StartGame()
    //{
    //    Debug.Log("Starting Game...");
    //    SceneManager.LoadScene("GameScene"); // Replace "GameScene" with your actual game scene name
    //}

    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
        gameObject.SetActive(false); // Hide the main menu
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
