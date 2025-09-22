using UnityEngine;
using UnityEngine.UI;
using WebXR;

public class DesktopControllerUI : MonoBehaviour
{
    [SerializeField] private Button _activateDesktopControllerButton;

    private void Awake()
    {
        _activateDesktopControllerButton.onClick.AddListener(OnActivateDesktopControllerButtonPressed);
    }

    private void Start()
    {
        var isARSupported = WebXRManager.Instance.isSupportedAR;
        var isVRSupported = WebXRManager.Instance.isSupportedVR;

        if (isARSupported || isVRSupported)
            _activateDesktopControllerButton.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _activateDesktopControllerButton.onClick.RemoveListener(OnActivateDesktopControllerButtonPressed);
    }

    private void OnActivateDesktopControllerButtonPressed()
    {
        DesktopControllerManager.Instance.ActivateDesktopController();
        _activateDesktopControllerButton.gameObject.SetActive(false);
    }
}
