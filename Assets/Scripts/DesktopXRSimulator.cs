using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

public class DesktopXRSimulator : MonoBehaviour
{
    [Header("Desktop Input")]
    [SerializeField] private InputActionAsset _inputActionAsset;
    [SerializeField] private InputActionReference _keyboardXTranslateAction;
    [SerializeField] private InputActionReference _keyboardYTranslateAction;

    [Header("Simulated Input")]
    [SerializeField] private InputActionReference _leftControllerMoveAction;

    private void Awake()
    {
        _inputActionAsset.Enable();
    }

    private void OnDestroy()
    {
        _inputActionAsset.Disable();
    }

    private void Update()
    {
        // Poll for keyboard input every frame
        float xValue = _keyboardXTranslateAction.action.ReadValue<float>();
        float yValue = _keyboardYTranslateAction.action.ReadValue<float>();

        // Create a new Vector2 with the current values
        Vector2 newValue = new(xValue, yValue);

        // Get the device and control from the left controller move action
        var device = _leftControllerMoveAction.action.controls[0].device;
        var control = _leftControllerMoveAction.action.controls[0];

        // Create state event and write the value
        using (StateEvent.From(device, out InputEventPtr eventPtr))
        {
            ((AxisControl)control).WriteValueIntoEvent(newValue, eventPtr);
            InputSystem.QueueEvent(eventPtr);
        }

        // Debug log when there's input
        if (xValue != 0f || yValue != 0f)
        {
            Debug.Log($"Input: X={xValue}, Y={yValue}");
        }
    }
}
