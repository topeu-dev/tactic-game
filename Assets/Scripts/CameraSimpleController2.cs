using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSimpleController2 : MonoBehaviour
{
    private CinemachineCamera _virtualCamera;
    private InputAction _movement;

    [Header("Zoom")]
    [SerializeField]
    private float zoomMin;

    [SerializeField]
    private float zoomMax;

    [SerializeField]
    private float zoomStep;

    [SerializeField]
    private float zoomSpeed;

    [Header("Movement")]
    [SerializeField]
    private float movementTime;

    [SerializeField]
    private float movementSpeed;

    [Header("Edge Movement")]
    [SerializeField]
    private bool edgeMovementEnabled;

    private Vector3 _targetPosition = Vector3.zero;
    private float _movementSpeedZoomAmplifier;
    private float _targetZoom;

    private static readonly Vector3 IsometricForward = new Vector3(1, 0, 1).normalized;
    private static readonly Vector3 IsometricRight = new Vector3(1, 0, -1).normalized * 0.6f;

    private void Awake()
    {
        _movement = InputActionSingleton.GeneralInputActions.Camera.CameraMovement;
        InputActionSingleton.GeneralInputActions.Camera.Zoom.performed += ZoomCamera;

        // Получаем компонент CinemachineVirtualCamera
        _virtualCamera = GetComponentInChildren<CinemachineCamera>();
        if (_virtualCamera == null)
        {
            Debug.LogError("CinemachineVirtualCamera не найдена.");
        }
        else
        {
            _targetZoom = _virtualCamera.Lens.OrthographicSize;
        }
    }

    private void OnEnable()
    {
        InputActionSingleton.GeneralInputActions.Camera.Zoom.Enable();
        InputActionSingleton.GeneralInputActions.Camera.CameraMovement.Enable();
        EventManager.CameraEvent.OnPlayableCharacterFocusEvent += OnPlayableCharacterFocusEvent;
    }

    private void OnDisable()
    {
        InputActionSingleton.GeneralInputActions.Camera.Zoom.Disable();
        InputActionSingleton.GeneralInputActions.Camera.CameraMovement.Disable();
        EventManager.CameraEvent.OnPlayableCharacterFocusEvent -= OnPlayableCharacterFocusEvent;
    }

    private void OnPlayableCharacterFocusEvent(Component arg0, GameObject arg1)
    {
        // var adjustTarget = new Vector3(arg1.transform.position.x - 18f, arg1.transform.position.y, arg1.transform.position.z + 14f);
        // _targetPosition = adjustTarget;
        var asd = _virtualCamera.ResolveLookAt(arg1.transform);
        print("Initial pos " + arg1.transform.position + "LookAt " +asd.transform.position);
        _targetPosition = asd.position;
        // _targetPosition = arg1.transform.position;
    }

    void Update()
    {
        HandleMovementInput();
        MoveCameraToTargetSmooth();
        HandleScreenEdgeMovement();
        HandleZoom();
    }

    private void ZoomCamera(InputAction.CallbackContext context)
    {
        float zoomDir = context.ReadValue<float>();
        _targetZoom = zoomDir < 0
            ? Mathf.Clamp(_targetZoom + zoomStep, zoomMin, zoomMax)
            : Mathf.Clamp(_targetZoom - zoomStep, zoomMin, zoomMax);
    }

    private void MoveCameraToTargetSmooth()
    {
        if (_virtualCamera != null)
        {
            // _virtualCamera.transform.position = Vector3.Lerp(_virtualCamera.transform.position, _targetPosition,
            //     Time.deltaTime * movementTime);

            _virtualCamera.transform.position = _targetPosition;
        }
    }

    private void HandleZoom()
    {
        if (_virtualCamera != null)
        {
            _movementSpeedZoomAmplifier = Mathf.Clamp(_virtualCamera.Lens.OrthographicSize / zoomMax, 0.2f, 1f);
            _virtualCamera.Lens.OrthographicSize = Mathf.Lerp(_virtualCamera.Lens.OrthographicSize, _targetZoom,
                Time.deltaTime * zoomSpeed);
        }
    }

    private void HandleMovementInput()
    {
        Vector2 fromInput = _movement.ReadValue<Vector2>();

        if (!(fromInput.sqrMagnitude > 0.1f)) return;

        Vector3 moveValue = (IsometricRight * fromInput.x + IsometricForward * fromInput.y) *
                            (movementSpeed * _movementSpeedZoomAmplifier);
        _targetPosition += moveValue;
    }

    private void HandleScreenEdgeMovement()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        if (Application.isFocused && edgeMovementEnabled)
        {
            if (mousePosition.x < 0.015f * Screen.width)
                moveDirection += -IsometricRight;
            else if (mousePosition.x > (1f - 0.015f) * Screen.width)
                moveDirection += IsometricRight;

            if (mousePosition.y < 0.015f * Screen.height)
                moveDirection += -IsometricForward;
            else if (mousePosition.y > (1f - 0.015f) * Screen.height)
                moveDirection += IsometricForward;

            _targetPosition += moveDirection * (0.5f * movementSpeed * _movementSpeedZoomAmplifier);
        }
    }
}