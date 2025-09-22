using UnityEngine;
using UnityEngine.InputSystem;

public class DesktopController : MonoBehaviour
{
    [Header("Desktop Input")]
    [SerializeField] private InputActionAsset _inputActionAsset;
    [SerializeField] private InputActionReference _keyboardXTranslateAction;
    [SerializeField] private InputActionReference _keyboardZTranslateAction;
    [SerializeField] private InputActionReference _manipulateHeadAction;
    [SerializeField] private InputActionReference _mouseDeltaAction;

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _mouseSensitivity = 2f;

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
        float xValue = _keyboardXTranslateAction.action.ReadValue<float>();
        float zValue = _keyboardZTranslateAction.action.ReadValue<float>();

        Vector3 move = new Vector3(xValue, 0, zValue).normalized;
        
        // Rotate movement vector to match character's current yaw rotation
        move = Quaternion.Euler(0f, _cameraYaw, 0f) * move;

        _characterController.Move(move * _moveSpeed * Time.deltaTime);
    }

    private void HandleCameraRotation()
    {
        bool isRightMouseHeld = _manipulateHeadAction.action.ReadValue<float>() > 0f;
        if (!isRightMouseHeld)
            return;
            
        Vector2 mouseDelta = _mouseDeltaAction.action.ReadValue<Vector2>();
        
        // Yaw affects both character controller and camera
        _cameraYaw += mouseDelta.x * _mouseSensitivity * Time.deltaTime;
        
        // Pitch only affects camera
        _cameraPitch -= mouseDelta.y * _mouseSensitivity * Time.deltaTime;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -90f, 90f);
        
        // Rotate character controller with yaw
        _characterController.transform.rotation = Quaternion.Euler(0f, _cameraYaw, 0f);
        
        // Apply full rotation to camera (pitch + yaw)
        _mainCamera.transform.rotation = Quaternion.Euler(_cameraPitch, _cameraYaw, 0f);
    }
}
