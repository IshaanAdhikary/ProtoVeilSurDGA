using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Tracks player state shared across systems (crouching, notebook open/closed)
/// and player functions like opening notebook.
/// </summary>
[RequireComponent(typeof(FirstPersonController))]
public class PlayerStateController : MonoBehaviour
{
    [SerializeField] private GameObject notebookMenu;

    private PlayerControls controls;
    private FirstPersonController firstPersonController;

    private bool notebookOpen = false;
    private bool isCrouching = false;
    private bool inPhotoMode = false;
    private bool playerHasControl = true;

    private void Awake()
    {
        controls = new PlayerControls();
        firstPersonController = GetComponent<FirstPersonController>();
    }

    private void Start()
    {
        notebookMenu.SetActive(false);
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

    public bool GetPhotoMode()
    {
        return inPhotoMode;
    }

    public void SetPhotoMode(bool value)
    {
        inPhotoMode = value;
    }

    public bool GetPlayerHasControl()
    {
        return playerHasControl;
    }

    public void SetPlayerHasControl(bool value)
    {
        playerHasControl = value;
        firstPersonController.enabled = value;
    }

    /// <summary>
    /// When toggle notebook button is pressed, either open or close the notebook
    /// depending on if it is already open.
    /// </summary>
    private void ToggleNotebook(InputAction.CallbackContext ctx)
    {
        notebookOpen = !notebookOpen;
        notebookMenu.SetActive(notebookOpen);
        SetPlayerHasControl(!notebookOpen);

        Cursor.lockState = notebookOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = notebookOpen;
    }
}
