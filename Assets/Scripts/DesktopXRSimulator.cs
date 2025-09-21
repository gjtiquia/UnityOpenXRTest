using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

public class DesktopXRSimulator : MonoBehaviour
{
    [Header("Desktop Input")]
    [SerializeField] private InputActionAsset _inputActionAsset;
    [SerializeField] private InputActionReference _keyboardXTranslateAction;
    [SerializeField] private InputActionReference _keyboardYTranslateAction;

    [Header("Simulated Input")]
    [SerializeField] private InputActionReference _leftControllerMoveAction;

    private XRSimulatedController m_LeftControllerDevice;

    private void Awake()
    {
        _inputActionAsset.Enable();
        CreateSimulatedController();
    }

    private void OnDestroy()
    {
        _inputActionAsset.Disable();
        RemoveSimulatedController();
    }

    private void CreateSimulatedController()
    {
        if (m_LeftControllerDevice == null)
        {
            var descLeftHand = new InputDeviceDescription
            {
                product = nameof(XRSimulatedController),
                capabilities = new XRDeviceDescriptor
                {
                    deviceName = $"{nameof(XRSimulatedController)} - {UnityEngine.InputSystem.CommonUsages.LeftHand}",
                    characteristics = XRInputTrackingAggregator.Characteristics.leftController,
                }.ToJson(),
            };

            m_LeftControllerDevice = InputSystem.AddDevice(descLeftHand) as XRSimulatedController;
            if (m_LeftControllerDevice != null)
                InputSystem.SetDeviceUsage(m_LeftControllerDevice, UnityEngine.InputSystem.CommonUsages.LeftHand);
            else
                Debug.LogError($"Failed to create {nameof(XRSimulatedController)} for {UnityEngine.InputSystem.CommonUsages.LeftHand}.", this);
        }
        else
        {
            InputSystem.AddDevice(m_LeftControllerDevice);
        }
    }

    private void RemoveSimulatedController()
    {
        if (m_LeftControllerDevice != null)
        {
            InputSystem.RemoveDevice(m_LeftControllerDevice);
        }
    }

    private void Update()
    {
        // Poll for keyboard input every frame
        float xValue = _keyboardXTranslateAction.action.ReadValue<float>();
        float yValue = _keyboardYTranslateAction.action.ReadValue<float>();

        // Update simulated controller with keyboard input
        Vector2 moveInput = new Vector2(xValue, yValue);

        InputEventPtr eventPtr;
        using (StateEvent.From(m_LeftControllerDevice, out eventPtr))
        {
            ((Vector2Control)m_LeftControllerDevice["primary2DAxis"]).WriteValueIntoEvent(moveInput, eventPtr);
            InputSystem.QueueEvent(eventPtr);
        }

        // Debug log when there's input
        if (xValue != 0f || yValue != 0f)
        {
            Debug.Log($"Simulated Controller Input: X={xValue}, Y={yValue}");
        }

        // Debug: Read current value from left controller move action
        Vector2 leftControllerValue = _leftControllerMoveAction.action.ReadValue<Vector2>();
        if (leftControllerValue != Vector2.zero)
        {
            Debug.Log($"Left Controller Move Action Value: {leftControllerValue}");
        }
    }
}
