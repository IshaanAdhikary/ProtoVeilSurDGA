using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateController : MonoBehaviour
{
    [SerializeField] private GameObject notebook;

    private PlayerControls controls;
    private FirstPersonController firstPersonController;

    private bool notebookOpen = false;
    private bool isCrouching = false;

    private void Awake()
    {
        controls = new PlayerControls();
        firstPersonController = GetComponent<FirstPersonController>();
    }

    private void OnEnable()
    {
        controls.General.Enable();

        controls.General.Notebook.performed += ToggleNotebook;
    }

    private void OnDisable()
    {
        controls.General.Notebook.performed -= ToggleNotebook;

        controls.General.Disable();
    }

    public bool GetCrouching()
    {
        return isCrouching;
    }

    public void SetCrouching(bool value)
    {
        isCrouching = value;
    }

    private void ToggleNotebook(InputAction.CallbackContext ctx)
    {

    }
}
