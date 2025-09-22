using UnityEngine;
using UnityEngine.UI;

public class DesktopControllerUI : MonoBehaviour
{
    [SerializeField] private Button _activateDesktopControllerButton;

    private void Awake()
    {
        _activateDesktopControllerButton.onClick.AddListener(OnActivateDesktopControllerButtonPressed);
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
