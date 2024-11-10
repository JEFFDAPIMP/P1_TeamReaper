using UnityEngine;
using UnityEngine.InputSystem;

public class InputDeviceLogger : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.onControlsChanged += OnControlsChanged;
        LogCurrentDevice();
    }

    private void OnControlsChanged(PlayerInput input)
    {
        LogCurrentDevice();
    }

    private void LogCurrentDevice()
    {
        InputDevice currentDevice = playerInput.devices[0];
        Debug.Log("Current Input Device: " + currentDevice.displayName);
    }
}
