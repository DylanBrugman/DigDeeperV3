namespace Core.Camera {
/*
* This script provides a 2D camera controller for Unity,
* suitable for use with tilemaps or other 2D scenes.
*
* Features:
* - WASD or Arrow Keys for direct camera movement.
* - Right Mouse Button + Drag for panning.
* - Mouse Scroll Wheel for zooming in/out.
*/

using UnityEngine;

public class CameraController : MonoBehaviour
{
    // --- Public Variables (Adjustable in Unity Inspector) ---
    [Header("Movement Settings")]
    [Tooltip("Speed at which the camera moves with WASD/Arrow keys.")]
    public float moveSpeed = 5f;
    [SerializeField] private bool useRawInputAxis;

    [Header("Zoom Settings")]
    [Tooltip("Speed at which the camera zooms in/out with the scroll wheel.")]
    public float zoomSpeed = 5f;
    [Tooltip("Minimum orthographic size (zoom in limit).")]
    public float minZoom = 2f;
    [Tooltip("Maximum orthographic size (zoom out limit).")]
    public float maxZoom = 10f;

    // --- Private Variables ---
    private Camera mainCamera; // Reference to the main camera component
    private Vector3 dragOrigin; // Stores the mouse position when panning starts

    // Called when the script instance is being loaded.
    void Awake()
    {
        // Get a reference to the Camera component attached to this GameObject.
        // It's crucial that this script is attached to the Camera itself.
        mainCamera = GetComponent<Camera>();

        // Ensure the camera is orthographic for 2D viewing.
        if (mainCamera != null && !mainCamera.orthographic)
        {
            Debug.LogWarning("Camera is not orthographic. Setting to Orthographic for 2D control.");
            mainCamera.orthographic = true;
        }
    }

    // Update is called once per frame.
    void Update()
    {
        // Handle camera movement using keyboard input (WASD or Arrow Keys)
        HandleKeyboardMovement();

        // Handle camera panning using right mouse button drag
        HandleMousePanning();

        // Handle camera zooming using the mouse scroll wheel
        HandleMouseZoom();
    }

    /// <summary>
    /// Handles camera movement based on WASD or Arrow key input.
    /// </summary>
    void HandleKeyboardMovement()
    {
        Vector3 inputDirection = Vector3.zero;

        if (!useRawInputAxis) {
            // Get horizontal input (A/D or Left/Right Arrow)
            inputDirection.x = Input.GetAxis("Horizontal");
            // Get vertical input (W/S or Up/Down Arrow)
            inputDirection.y = Input.GetAxis("Vertical");
        } else {
            // Get horizontal input (A/D or Left/Right Arrow)
            inputDirection.x = Input.GetAxisRaw("Horizontal");
            // Get vertical input (W/S or Up/Down Arrow)
            inputDirection.y = Input.GetAxisRaw("Vertical");
        }

        // Normalize the direction vector to prevent faster diagonal movement.
        // Multiply by moveSpeed and Time.deltaTime for frame-rate independent movement.
        transform.position += inputDirection.normalized * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Handles camera panning when the right mouse button is held down and dragged.
    /// </summary>
    void HandleMousePanning()
    {
        // Check if the right mouse button is pressed down (first frame)
        if (Input.GetMouseButtonDown(1)) // 1 is for the right mouse button
        {
            // Store the initial mouse position in ecsWorld coordinates.
            // ScreenToWorldPoint converts screen pixel coordinates to ecsWorld coordinates.
            // We keep the z-coordinate of the camera to ensure the point is on the same plane as the camera.
            dragOrigin = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        // Check if the right mouse button is being held down
        if (Input.GetMouseButton(1))
        {
            // Calculate the current mouse position in ecsWorld coordinates.
            Vector3 currentMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // Calculate the difference between the current mouse position and the drag origin.
            // This difference represents how much the camera should move.
            Vector3 difference = dragOrigin - currentMousePosition;

            // Move the camera by the calculated difference.
            transform.position += difference;
        }
    }

    /// <summary>
    /// Handles camera zooming using the mouse scroll wheel.
    /// </summary>
    void HandleMouseZoom()
    {
        // Get the scroll wheel input (positive for forward scroll, negative for backward)
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // If there's scroll input
        if (Mathf.Abs(scroll) > 0.01f) // Check for a meaningful scroll amount
        {
            // Adjust the camera's orthographic size.
            // Subtracting scroll zooms in when scrolling forward, adding zooms out.
            // Multiply by zoomSpeed for control and Time.deltaTime for smoothness.
            mainCamera.orthographicSize -= scroll * zoomSpeed;

            // Clamp the orthographic size within the defined min and max zoom limits.
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minZoom, maxZoom);
        }
    }
}

}