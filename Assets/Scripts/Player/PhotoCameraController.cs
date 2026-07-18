using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Controls camera zoom, UI, and photo capture logic for
/// the player's photo camera.
/// </summary>
[RequireComponent(typeof(PlayerStateController))]
public class PhotoCameraController : MonoBehaviour
{
    [SerializeField] private Canvas cameraUI;
    [SerializeField] private GameObject photographPrefab;
    private Camera targetCamera;
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float zoomedFOV = 30f;
    [SerializeField] private float zoomSpeed = 10f;

    private PlayerControls controls;
    private PlayerStateController playerStateController;
    private GameObject notebookMenu;
    private float targetFOV;

    // TODO: remove this debug image
    private RawImage debugImage;

    private void Awake()
    {
        controls = new PlayerControls();
        playerStateController = GetComponent<PlayerStateController>();
        notebookMenu = playerStateController.GetNotebookMenu();

        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        targetFOV = normalFOV;
        cameraUI.gameObject.SetActive(false);

        // TODO: remove this debug image
        GameObject debugImageObject = new GameObject("DebugPhotoImage", typeof(RawImage));
        debugImageObject.transform.SetParent(cameraUI.transform, false);
        RectTransform debugImageRect = debugImageObject.GetComponent<RectTransform>();
        debugImageRect.anchorMin = new Vector2(0f, 1f);
        debugImageRect.anchorMax = new Vector2(0f, 1f);
        debugImageRect.pivot = new Vector2(0f, 1f);
        debugImageRect.anchoredPosition = Vector2.zero;
        debugImageRect.sizeDelta = new Vector2(300f, 300f);
        debugImage = debugImageObject.GetComponent<RawImage>();
    }

    private void OnEnable()
    {
        controls.PlayerMovement.Enable();

        controls.PlayerMovement.AimCamera.performed += OnAimCameraPerformed;
        controls.PlayerMovement.AimCamera.canceled += OnAimCameraCanceled;
        controls.PlayerMovement.Interact.performed += OnSnap;
    }

    private void OnDisable()
    {
        controls.PlayerMovement.AimCamera.performed -= OnAimCameraPerformed;
        controls.PlayerMovement.AimCamera.canceled -= OnAimCameraCanceled;
        controls.PlayerMovement.Interact.performed -= OnSnap;

        controls.PlayerMovement.Disable();
    }

    private void OnAimCameraPerformed(InputAction.CallbackContext ctx)
    {
        if (!playerStateController.GetPlayerHasControl())
        {
            return;
        }

        targetFOV = zoomedFOV;
        cameraUI.gameObject.SetActive(true);
        playerStateController.SetPhotoMode(true);
    }

    private void OnAimCameraCanceled(InputAction.CallbackContext ctx)
    {
        targetFOV = normalFOV;
        cameraUI.gameObject.SetActive(false);
        playerStateController.SetPhotoMode(false);
    }

    private void OnSnap(InputAction.CallbackContext ctx)
    {
        if (!playerStateController.GetPhotoMode())
        {
            return;
        }

        RenderTexture captureRT = RenderTexture.GetTemporary(Screen.width, Screen.height, 24);
        Texture2D photo = CapturePhoto(captureRT);
        RenderTexture.ReleaseTemporary(captureRT);

        GameObject photograph = Instantiate(photographPrefab, notebookMenu.transform);
        PhotoNote photoNote = photograph.GetComponent<PhotoNote>();
        photoNote.setBounds(notebookMenu.transform as RectTransform);
        photoNote.LoadImage(photo);

        // TODO: remove this debug image
        debugImage.texture = photo;
    }

    /// <summary>
    /// Returns a square Texture2D (not a Sprite) of the center of
    /// the screen of targetCamera, cropping the sides but keeping
    /// the full height.
    /// </summary>
    private Texture2D CapturePhoto(RenderTexture captureRT)
    {
        var previousTarget = targetCamera.targetTexture;

        targetCamera.targetTexture = captureRT;
        targetCamera.Render();

        int squareSize = captureRT.height;
        int xOffset = (captureRT.width - squareSize) / 2;

        RenderTexture.active = captureRT;
        var photo = new Texture2D(squareSize, squareSize, TextureFormat.RGB24, false);
        photo.ReadPixels(new Rect(xOffset, 0, squareSize, squareSize), 0, 0);
        photo.Apply();

        RenderTexture.active = null;
        targetCamera.targetTexture = previousTarget;
        return photo;
    }

    private void Update()
    {
        if (targetCamera == null)
        {
            return;
        }

        targetCamera.fieldOfView = Mathf.Lerp(targetCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }
}
