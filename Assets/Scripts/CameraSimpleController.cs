using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSimpleController : MonoBehaviour
{
    private Camera _camera;
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
        

        _camera = GetComponentInChildren<Camera>();
        _targetZoom = _camera.orthographicSize;
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
        _targetPosition = arg1.transform.position;
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
        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * movementTime);
    }


    private void HandleZoom()
    {
        _movementSpeedZoomAmplifier = Math.Clamp(_camera.orthographicSize / zoomMax, 0.2f, 1f);
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _targetZoom, Time.deltaTime * zoomSpeed);
    }

    private void HandleMovementInput()
    { 
        Vector2 fromInput = _movement.ReadValue<Vector2>();

        if (!(fromInput.sqrMagnitude > 0.1f)) return;

        Vector3 moveValue = (IsometricRight * (fromInput.x) + IsometricForward * fromInput.y) *
                            (movementSpeed * _movementSpeedZoomAmplifier);
        _targetPosition += moveValue;
    }


    private void HandleScreenEdgeMovement()
    {
        //mouse position is in pixels
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        if (Application.isFocused && edgeMovementEnabled)
        {
            //horizontal scrolling
            if (mousePosition.x < 0.015f * Screen.width)
                moveDirection += -IsometricRight;
            else if (mousePosition.x > (1f - 0.015f) * Screen.width)
                moveDirection += IsometricRight;

            //vertical scrolling
            if (mousePosition.y < 0.015f * Screen.height)
                moveDirection += -IsometricForward;
            else if (mousePosition.y > (1f - 0.015f) * Screen.height)
                moveDirection += IsometricForward;

            _targetPosition += moveDirection * (0.5f * movementSpeed * _movementSpeedZoomAmplifier);
        }
        
    }
    
    
}