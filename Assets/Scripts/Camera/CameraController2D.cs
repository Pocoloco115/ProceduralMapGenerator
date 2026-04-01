using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class CameraController2D_NewInput : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField] private float baseZoomSpeed = 3f;
    [SerializeField] private float minOrthoSize = 2f;

    [Header("Escala de zoom (solo para zoom)")]
    [SerializeField] private float minZoomScale = 0.2f;
    [SerializeField] private float maxZoomScale = 4.0f;

    [Header("Escala de zoom (solo para movimiento)")]
    [SerializeField] private float minMoveScale = 0.2f;
    [SerializeField] private float maxMoveScale = 4.0f;

    [Header("Pan con ratón")]
    [SerializeField] private float dragSpeed = 1f;

    [Header("Movimiento con teclado (base)")]
    [SerializeField] private float baseMoveSpeed = 8f;

    private Camera cam;
    private float maxOrthoSize;
    private float initialOrthoSize;

    private bool isDragging;
    private Vector2 lastMousePosScreen;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        if (!cam.orthographic)
            cam.orthographic = true;

        initialOrthoSize = cam.orthographicSize;
        maxOrthoSize = initialOrthoSize * 3f;
    }

    private void Update()
    {
        HandleZoom();
        HandleMouseDragPan();
        HandleKeyboardMovement();
    }

    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null || Mouse.current == null)
            return false;

        var eventData = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    private float GetZoomScale()
    {
        float t = cam.orthographicSize / initialOrthoSize;
        float curved = t * t;
        return Mathf.Clamp(curved, minZoomScale, maxZoomScale);
    }

    private float GetMoveScale()
    {
        float t = cam.orthographicSize / initialOrthoSize;
        float curved = t * t;
        return Mathf.Clamp(curved, minMoveScale, maxMoveScale);
    }

    private void HandleZoom()
    {
        if (Mouse.current == null) return;
        if (IsPointerOverUI()) return;

        float scroll = Mouse.current.scroll.ReadValue().y;
        if (Mathf.Abs(scroll) > 0.0001f)
        {
            float scrollNormalized = scroll / 120f;

            float scale = GetZoomScale();
            float zoomSpeed = baseZoomSpeed * scale;

            float newSize = cam.orthographicSize - scrollNormalized * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(newSize, minOrthoSize, maxOrthoSize);
        }
    }

    private void HandleMouseDragPan()
    {
        if (Mouse.current == null) return;

        var mouse = Mouse.current;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            if (IsPointerOverUI())
                return;

            isDragging = true;
            lastMousePosScreen = mouse.position.ReadValue();
        }

        if (mouse.leftButton.wasReleasedThisFrame)
        {
            isDragging = false;
        }

        if (isDragging && mouse.leftButton.isPressed)
        {
            if (IsPointerOverUI())
                return;

            Vector2 mousePos = mouse.position.ReadValue();
            Vector2 deltaScreen = mousePos - lastMousePosScreen;

            Vector3 worldA = cam.ScreenToWorldPoint(lastMousePosScreen);
            Vector3 worldB = cam.ScreenToWorldPoint(lastMousePosScreen + deltaScreen);
            Vector3 deltaWorld = worldA - worldB;

            transform.position += new Vector3(deltaWorld.x, deltaWorld.y, 0f) * dragSpeed;

            lastMousePosScreen = mousePos;
        }
    }

    private void HandleKeyboardMovement()
    {
        if (Keyboard.current == null) return;

        Vector2 dir = Vector2.zero;
        var k = Keyboard.current;

        if (k.wKey.isPressed || k.upArrowKey.isPressed) dir.y += 1f;
        if (k.sKey.isPressed || k.downArrowKey.isPressed) dir.y -= 1f;
        if (k.aKey.isPressed || k.leftArrowKey.isPressed) dir.x -= 1f;
        if (k.dKey.isPressed || k.rightArrowKey.isPressed) dir.x += 1f;

        if (dir.sqrMagnitude > 0.0001f)
        {
            dir.Normalize();

            float moveScale = GetMoveScale();
            float moveSpeed = baseMoveSpeed * moveScale;

            transform.position += new Vector3(dir.x, dir.y, 0f) * moveSpeed * Time.deltaTime;
        }
    }
}