using UnityEngine;
using UnityEngine.InputSystem;

public class DesktopController : MonoBehaviour
{
    [Header("Desktop Input")]
    [SerializeField] private InputActionAsset _inputActionAsset;
    [SerializeField] private InputActionReference _keyboardXTranslateAction;
    [SerializeField] private InputActionReference _keyboardZTranslateAction;

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 3f;

    private CharacterController _characterController;

    private void Awake()
    {
        _inputActionAsset.Enable();
    }

    private void OnDestroy()
    {
        _inputActionAsset.Disable();
    }

    public void OnActivate(CharacterController characterController)
    {
        _characterController = characterController;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Read input values
        float xValue = _keyboardXTranslateAction.action.ReadValue<float>();
        float zValue = _keyboardZTranslateAction.action.ReadValue<float>();

        Vector3 move = new Vector3(xValue, 0, zValue).normalized;

        // Apply movement to character controller
        _characterController.Move(move * _moveSpeed * Time.deltaTime);
    }
}
