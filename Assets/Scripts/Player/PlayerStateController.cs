using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateController : MonoBehaviour
{
    [SerializeField] private GameObject notebookMenu;

    private PlayerControls controls;
    private FirstPersonController firstPersonController;

    private bool notebookOpen = false;
    private bool isCrouching = false;

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

    /// <summary>
    /// When toggle notebook button is pressed, either open or close the notebook
    /// depending on if it is already open.
    /// </summary>
    private void ToggleNotebook(InputAction.CallbackContext ctx)
    {
        notebookOpen = !notebookOpen;
        notebookMenu.SetActive(notebookOpen);
        firstPersonController.enabled = !notebookOpen;

        Cursor.lockState = notebookOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = notebookOpen;
    }
}
