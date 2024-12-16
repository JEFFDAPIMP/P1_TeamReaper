using UnityEngine;
using UnityEngine.InputSystem;

public class ActiveActionMapLogger : MonoBehaviour
{
    private PlayerInput playerInput;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {   
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found.");
            return;
        }

        // Subscribe to the onActionTriggered event to detect active action maps
        playerInput.onActionTriggered += context =>
        {
            Debug.Log("Active Action Map: " + playerInput.currentActionMap.name);
        };

        // Log initial active action map
        Debug.Log("Initial Active Action Map: " + playerInput.currentActionMap.name);
    }
}
