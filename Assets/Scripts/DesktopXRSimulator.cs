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
    private XRSimulatedControllerState m_LeftControllerState;

    private void Awake()
    {
        _inputActionAsset.Enable();
        CreateSimulatedController();
        InitializeControllerState();
    }

    private void InitializeControllerState()
    {
        m_LeftControllerState = new XRSimulatedControllerState();
        m_LeftControllerState.Reset();
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
            {
                InputSystem.SetDeviceUsage(m_LeftControllerDevice, UnityEngine.InputSystem.CommonUsages.LeftHand);
                Debug.Log($"Successfully created {nameof(XRSimulatedController)} for {UnityEngine.InputSystem.CommonUsages.LeftHand}. Device: {m_LeftControllerDevice.name}");
            }
            else
                Debug.LogError($"Failed to create {nameof(XRSimulatedController)} for {UnityEngine.InputSystem.CommonUsages.LeftHand}.", this);
        }
        else
        {
            InputSystem.AddDevice(m_LeftControllerDevice);
            Debug.Log($"Re-added existing {nameof(XRSimulatedController)} to Input System. Device: {m_LeftControllerDevice.name}");
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

        // Update the controller state directly (Unity XRDeviceSimulator approach)
        Vector2 moveInput = new Vector2(xValue, yValue);
        
        if (m_LeftControllerDevice != null && m_LeftControllerDevice.added)
        {
            // Update the controller state directly
            m_LeftControllerState.primary2DAxis = moveInput;
            
            // Apply the state to the device using InputState.Change
            InputState.Change(m_LeftControllerDevice, m_LeftControllerState);
            
            // Debug log when there's input
            if (xValue != 0f || yValue != 0f)
            {
                Debug.Log($"Updated controller state: X={xValue}, Y={yValue}");
            }
        }

        // Debug: Read current value from left controller move action
        Vector2 leftControllerValue = _leftControllerMoveAction.action.ReadValue<Vector2>();
        if (leftControllerValue != Vector2.zero)
        {
            Debug.Log($"Left Controller Move Action Value: {leftControllerValue}");
        }
    }
}
