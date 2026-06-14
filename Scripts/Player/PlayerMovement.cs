using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Скорости")]
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 8f;
    [SerializeField] private float _crouchSpeed = 2f;

    [Header("Прыжок и гравитация")]
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _gravity = -9.81f;

    [Header("Настройки приседания")]
    [SerializeField] private float _crouchHeight = 0.5f;
    [SerializeField] private float _standHeight = 2f;
    [SerializeField] private float _crouchSmoothTime = 0.2f;

    [Header("Проверка земли")]
    [SerializeField] private float _groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Камера")]
    [SerializeField] private Transform _cameraRoot;
    [SerializeField] private float _mouseSensitivity = 2f;
    [SerializeField] private float _minPitch = -80f;
    [SerializeField] private float _maxPitch = 80f;

    // Приватные поля
    private CharacterController _controller;
    private PlayerControls _controls;
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private Vector3 _velocity;
    private float _rotationX;
    private bool _isGrounded;
    private bool _isSprinting;
    private bool _isCrouching;
    private float _currentHeight;
    private float _currentSpeed;
    private float _heightVelocity;
    private float _lastGroundCheckTime;
    private const float GroundCheckInterval = 0.1f;
    private bool _isMovementLocked;
    private bool _isCameraLocked;
    private bool _isInspecting;
    private float _originalMouseSensitivity;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _controls = new PlayerControls();

        _standHeight = _controller.height;
        _currentSpeed = _walkSpeed;

        // Настройка ввода
        _controls.Gameplay.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _controls.Gameplay.Move.canceled += ctx => _moveInput = Vector2.zero;

        _controls.Gameplay.Look.performed += ctx => _lookInput = ctx.ReadValue<Vector2>();
        _controls.Gameplay.Look.canceled += ctx => _lookInput = Vector2.zero;

        _controls.Gameplay.Jump.performed += _ => Jump();
        _controls.Gameplay.Sprint.performed += _ => { _isSprinting = true; UpdateSpeed();};
        _controls.Gameplay.Sprint.canceled += _ =>  { _isSprinting = false; UpdateSpeed();};
        _controls.Gameplay.Crouch.performed += _ => { _isCrouching = true; UpdateSpeed();};
        _controls.Gameplay.Crouch.canceled += _ =>  { _isCrouching = false; UpdateSpeed();};
    }

    private void OnEnable() => _controls.Gameplay.Enable();
    private void OnDisable() => _controls.Gameplay.Disable();

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _originalMouseSensitivity = _mouseSensitivity; // Сохраняем исходную чувствительность
    }

    private void Update()
    {
        CheckGround();
        HandleMovement();
        HandleRotation();
        HandleGravity();
        Crouching();
    }

    private void CheckGround()
    {
        Vector3 rayOrigin = transform.position + _controller.center;
        if (Physics.CapsuleCast(
            rayOrigin - Vector3.up * _controller.radius,
            rayOrigin + Vector3.up * _controller.radius,
            _controller.radius,
            Vector3.down,
            out _,
            _groundCheckDistance,
            _groundLayer))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }

    private void HandleMovement()
    {
        if (_isMovementLocked) return;

        _moveInput = _controls.Gameplay.Move.ReadValue<Vector2>();
        Vector3 inputDirection = new Vector3(_moveInput.x, 0, _moveInput.y);
        if (inputDirection.sqrMagnitude > 0.01f)
        {
            Vector3 moveDirection = _cameraRoot.TransformDirection(inputDirection.normalized);
            _controller.Move(moveDirection * _currentSpeed * Time.deltaTime);
        }
    }
    private void HandleRotation()
    {

        if (_isCameraLocked)
        {
            _lookInput = Vector2.zero; // Игнорируем ввод мыши
            return;
        }

        _rotationX -= _lookInput.y * _mouseSensitivity;
        _rotationX = Mathf.Clamp(_rotationX, _minPitch, _maxPitch);
        _cameraRoot.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.Rotate(Vector3.up * _lookInput.x * _mouseSensitivity);
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
    }
    
    private void HandleGravity()
    {
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -0.5f;
        }
        else
        {
            _velocity.y += _gravity * Time.deltaTime;
        }

        _controller.Move(_velocity * Time.deltaTime);
    }

    private void Crouching()
    {
        float targetHeight = _isCrouching ? _crouchHeight : _standHeight;
        
        // Плавное изменение высоты
        _currentHeight = Mathf.SmoothDamp(_currentHeight, targetHeight, ref _heightVelocity, _crouchSmoothTime);

        _controller.height = _currentHeight;
    }

    private void UpdateSpeed() 
    {
        if (_isCrouching)
        {
            _currentSpeed = _crouchSpeed;
        }
        else if (_isSprinting)
        {
            _currentSpeed = _sprintSpeed;
        }
        else
        {
            _currentSpeed = _walkSpeed;
        }
    }
    public void SetMovementLock(bool locked)
    {
        _isMovementLocked = locked;
        if (locked) _moveInput = Vector2.zero; // Сбрасываем ввод движения
    }

    public void SetCameraLock(bool locked)
    {
        _isCameraLocked = locked;
        _mouseSensitivity = locked ? 0f : _originalMouseSensitivity; // Обнуляем чувствительность
    }

}
