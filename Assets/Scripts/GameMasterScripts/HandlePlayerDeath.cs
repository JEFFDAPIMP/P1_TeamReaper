using StarterAssets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class HandlePlayerDeath : MonoBehaviour
{
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject resetButton;

    [SerializeField] private FirstPersonController firstPersonController;
    [SerializeField] private PlayerInventorySwitcher playerInventorySwitcher;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private PlayerAttack playerAttack;

    private string playerTag = "Player";

    private void Start()
    {
        deathMenu.SetActive(false);

        if(firstPersonController == null)
        {
            firstPersonController = GameObject.FindGameObjectWithTag(playerTag).GetComponent<FirstPersonController>();
        }

        if (playerInventorySwitcher == null)
        {
            playerInventorySwitcher = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerInventorySwitcher>();
        }

        if (pauseManager == null)
        {
            pauseManager = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PauseManager>();
        }

        if (playerAttack == null)
        {
            playerAttack = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerAttack>();
        }
    }

    public void DisplayDeathMenu()
    {
        CurserLock(false);
        DisablePlayerScripts(false);

        deathMenu.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        eventSystem.SetSelectedGameObject(resetButton);
    }

    public void ResetCurrentScene()
    {
        CurserLock(true);
        DisablePlayerScripts(true);

        deathMenu.SetActive(false);
        Time.timeScale = 1f; // Pause the game

        // Get the active scene and reload it
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void DisablePlayerScripts(bool input)
    {
        firstPersonController.enabled = input;
        playerInventorySwitcher.enabled = input;
        pauseManager.enabled = input;
        playerAttack.enabled = input;
    }

    private void CurserLock(bool input)
    {
        if (!input)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}