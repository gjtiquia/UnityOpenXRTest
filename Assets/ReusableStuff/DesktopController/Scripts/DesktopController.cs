using UnityEngine;

public class DesktopController : MonoBehaviour
{
    private CharacterController _characterController;

    public void OnActivate(CharacterController characterController)
    {
        _characterController = characterController;
    }
}
