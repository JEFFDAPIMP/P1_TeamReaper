using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string nextSceneName = "GameScene";

    /*
    public void OnStartGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartGame();
        }
    }

    public void OnQuitGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            QuitGame();
        }
    }
    */

    public void StartGame()
    {
        // Load the game scene
        SceneManager.LoadScene(nextSceneName);
    }

    public void QuitGame()
    {
        // Quit the game
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
