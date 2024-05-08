using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;

    [HideInInspector]public float moveSpeed;

    public float groundDrag;

    PlayerInput playerInput;
    Vector3 moveVector;
    public bool running;

    [Header("Stamina")]
    public Slider staminaBar;
    public float maxStamina, stamina;

    public float runningCost;

    [Header("Other")]
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    #region movment
    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.Player.Movement.performed += OnMovementPerform;
        playerInput.Player.Movement.canceled += OnMovementCancelled;
        playerInput.Player.Running.performed += OnRunningPerform;
        playerInput.Player.Running.canceled += OnRunningCancelled;
    }

    private void OnDisable()
    {
        playerInput.Disable();
        playerInput.Player.Movement.performed -= OnMovementPerform;
        playerInput.Player.Movement.canceled -= OnMovementCancelled;
        playerInput.Player.Running.performed -= OnRunningPerform;
        playerInput.Player.Running.canceled -= OnRunningCancelled;
    }

    void OnMovementPerform(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector3>();
    }

    void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector3.zero;
    }

    void OnRunningPerform(InputAction.CallbackContext value)
    {
        if (stamina > 0)
        {
            moveSpeed = sprintSpeed;
            running = true;
        }
        else
        {
            running = false;
            moveSpeed = walkSpeed;
        }
    }

    void OnRunningCancelled(InputAction.CallbackContext value)
    {
        running = false;
        moveSpeed = walkSpeed;
    }
    #endregion 

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        StaminaBar();
        SpeedControl();
        rb.drag = groundDrag;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void StaminaBar()
    {
        if (running)
        {
            stamina -= runningCost*Time.deltaTime;
            if (stamina < 0)
                stamina = 0;                
            staminaBar.fillAmount = stamina / maxStamina;
        }
        else
        {
            if (stamina < maxStamina)
            {
                stamina += runningCost * Time.deltaTime;
                if (stamina > maxStamina)
                    stamina = maxStamina;
                staminaBar.fillAmount = stamina / maxStamina;
            }
        }
        if (stamina <= 0)
        {
            running = false;
            moveSpeed = walkSpeed;
        }
    }

    private void MovePlayer()
    {
        moveDirection = moveVector.z * orientation.forward + moveVector.x * orientation.right;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
