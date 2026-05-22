using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    
    [Header("Scene Configuration")]
    [Tooltip("The exact name of your first playable level scene.")]
    [SerializeField] private string firstLevelName = "Level1";

    public void PlayGame() {
        // Option A: Loads by scene string identifier name
        SceneManager.LoadScene(firstLevelName);

        // Option B (Alternative): Loads the very next scene index in your Build Settings map
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        Debug.Log("Application closed successfully.");
        
        // Closes the built standalone execution application (.exe / .app)
        Application.Quit();
    }
}