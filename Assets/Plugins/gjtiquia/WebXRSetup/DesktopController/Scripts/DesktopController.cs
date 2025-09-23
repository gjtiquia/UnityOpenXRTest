using UnityEngine;
using UnityEngine.InputSystem;

public class DesktopController : MonoBehaviour
{
    [Header("Desktop Input")]
    [SerializeField] private InputActionAsset _inputActionAsset;
    [SerializeField] private InputActionReference _xTranslateAction;
    [SerializeField] private InputActionReference _zTranslateAction;
    [SerializeField] private InputActionReference _toggleMouseAction;
    [SerializeField] private InputActionReference _mouseRotationDeltaAction;
    [SerializeField] private InputActionReference _keyboardRotationAction;

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _mouseSensitivity = 2f;
    [SerializeField] private float _keyboardRotationSensitivity = 100f;

    private Camera _mainCamera;
    private CharacterController _characterController;
    private float _cameraPitch = 0f;
    private float _cameraYaw = 0f;

    private void Awake()
    {
        _inputActionAsset.Enable();
    }

    private void OnDestroy()
    {
        _inputActionAsset.Disable();
    }

    public void OnActivate(Camera mainCamera, CharacterController characterController)
    {
        _mainCamera = mainCamera;
        _characterController = characterController;

        Vector3 eulerAngles = _mainCamera.transform.eulerAngles;
        _cameraPitch = eulerAngles.x;
        _cameraYaw = eulerAngles.y;
    }

    private void Update()
    {
        HandleMovement();
        HandleCameraRotation();
    }

    private void HandleMovement()
    {
        float xValue = _xTranslateAction.action.ReadValue<float>();
        float zValue = _zTranslateAction.action.ReadValue<float>();

        Vector3 move = new Vector3(xValue, 0, zValue).normalized;

        // Rotate movement vector to match character's current yaw rotation
        move = Quaternion.Euler(0f, _cameraYaw, 0f) * move;

        _characterController.Move(move * _moveSpeed * Time.deltaTime);
    }

    private void HandleCameraRotation()
    {
        Vector2 rotationInput = Vector2.zero;
        
        // Handle mouse rotation input (right-click drag)
        bool isRightMouseHeld = _toggleMouseAction.action.ReadValue<float>() > 0f;
        if (isRightMouseHeld)
        {
            Vector2 mouseDelta = _mouseRotationDeltaAction.action.ReadValue<Vector2>();
            rotationInput = mouseDelta * _mouseSensitivity;
        }
        
        // Handle keyboard rotation input (arrow keys)
        Vector2 keyboardRotation = _keyboardRotationAction.action.ReadValue<Vector2>();
        if (keyboardRotation != Vector2.zero)
        {
            rotationInput += keyboardRotation * _keyboardRotationSensitivity * Time.deltaTime;
        }
        
        // Exit if no rotation input
        if (rotationInput == Vector2.zero)
            return;

        // Yaw affects both character controller and camera (x component of input)
        _cameraYaw += rotationInput.x;

        // Pitch only affects camera (y component of input, inverted for intuitive control)
        _cameraPitch -= rotationInput.y;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -90f, 90f);

        // Rotate character controller with yaw
        _characterController.transform.rotation = Quaternion.Euler(0f, _cameraYaw, 0f);

        // Apply full rotation to camera (pitch + yaw)
        _mainCamera.transform.rotation = Quaternion.Euler(_cameraPitch, _cameraYaw, 0f);
    }
}
