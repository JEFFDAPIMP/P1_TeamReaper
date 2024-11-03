using StarterAssets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Handles game pausing, place this script on the player object
/// </summary>

[RequireComponent(typeof(StarterAssetsInputs))]
[RequireComponent(typeof(FirstPersonController))]
[RequireComponent(typeof(PlayerAttack))]
public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the pause menu UI panel
    public GameObject pauseButton; // Reference to the pause button
    private bool isPaused = false;
    private bool buttonDown = false;
    private GameObject currentSelection;

    [Header("Player StarterAssetsInputs Reference")]
    [SerializeField] private StarterAssetsInputs starterAssetsInputs;
    [SerializeField] private FirstPersonController firstPersonController;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private EventSystem eventSystem;



    /// <summary>
    /// Awake called on object is initialised, regardless of whether or not the script is enabled.
    /// </summary>
    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        firstPersonController = GetComponent<FirstPersonController>();
        playerAttack = GetComponent<PlayerAttack>();

        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        // Toggle pause on pressing the Escape key
        if (starterAssetsInputs.pause)
        {
            if(buttonDown == false)
            {
                buttonDown = true;
                if (isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }
        else
        {
            buttonDown = false;
        }

        if (isPaused)
        {
            if (eventSystem.currentSelectedGameObject == null)
            {
                eventSystem.SetSelectedGameObject(pauseButton);
            }
            else
            {
                currentSelection = eventSystem.currentSelectedGameObject;
            }

            if (playerInput.devices[0].name == "Keyboard")
            {
                handleControllerInput();
            }

            if (playerInput.devices[0].name != "Keyboard")
            {
                handleControllerInput();
            }
        }

        
    }

    private void handleMouseInput()
    {
        starterAssetsInputs.cursorLocked = false;
        starterAssetsInputs.cursorInputForLook = false;
    }

    private void handleControllerInput()
    {
        starterAssetsInputs.cursorLocked = true;
        starterAssetsInputs.cursorInputForLook = true;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        firstPersonController.enabled = false;
        playerAttack.enabled = false;
        isPaused = true;
        starterAssetsInputs.shoot = false;


        playerInput.SwitchCurrentActionMap("UI");
        eventSystem.firstSelectedGameObject = pauseButton;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        firstPersonController.enabled = true;
        playerAttack.enabled = true;
        isPaused = false;
        starterAssetsInputs.shoot = false;
        starterAssetsInputs.pause = false;

        playerInput.SwitchCurrentActionMap("Player");
        starterAssetsInputs.cursorLocked = true;
        starterAssetsInputs.cursorInputForLook = true;
    }
}
