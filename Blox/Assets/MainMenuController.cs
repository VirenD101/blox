using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public GameObject mainMenuPanel;
    public GameObject creditsPanel;
    
    [SerializeField] private string firstLevelName = "Level1";

    public void PlayGame() {
        SceneManager.LoadScene(firstLevelName);
    }

    public void OpenCredits()
    {
        // Hide the main menu buttons and show your credits
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        // Hide the credits and bring back the main menu buttons
        mainMenuPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }

    public void QuitGame() {
        Debug.Log("Application closed successfully.");
        Application.Quit();
    }
}