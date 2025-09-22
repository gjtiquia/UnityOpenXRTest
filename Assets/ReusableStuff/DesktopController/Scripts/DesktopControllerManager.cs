using UnityEngine;

public class DesktopControllerManager : MonoBehaviour
{
    public static DesktopControllerManager Instance;

    [Header("External References")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CharacterController _characterController;

    [Header("Prefabs")]
    [SerializeField] private DesktopController _desktopControllerPrefab;

    private DesktopController _desktopControllerInstance;

    private void Awake()
    {
        Instance = this;
    }

    public void ActivateDesktopController()
    {
        if (_desktopControllerInstance != null)
            return;

        var instance = Instantiate(_desktopControllerPrefab);
        instance.gameObject.name = nameof(DesktopController);
        instance.OnActivate(_mainCamera, _characterController);

        _desktopControllerInstance = instance;
    }

    // TODO : deactivate desktop controls
}
