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
    private bool running;

    [Header("Stamina")]
    public Slider staminaBar;
    public Image staminaBarColor;
    Color greenStaminaBarColor;
    bool runOutOfStamina;
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
        if (stamina > 0 && !runOutOfStamina)
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
        staminaBar.maxValue = maxStamina;
        staminaBar.value = stamina;
        greenStaminaBarColor = staminaBarColor.color;
        runOutOfStamina = false;
        running=false;
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
        if (running && moveVector != Vector3.zero)
        {
            stamina -= runningCost*Time.deltaTime;
            if (stamina < 0)
                stamina = 0;                
            staminaBar.value = stamina;
        }
        else
        {
            if (stamina < maxStamina)
            {
                stamina += runningCost * Time.deltaTime;
                if (stamina > maxStamina)
                    stamina = maxStamina;
                staminaBar.value = stamina;
            }
        }
        if (stamina <= 0)
        {
            running = false;
            moveSpeed = walkSpeed;
            staminaBarColor.color = Color.red;
            runOutOfStamina = true;
        }
        if (stamina > maxStamina * 0.1f)
        {
            staminaBarColor.color = greenStaminaBarColor;
            runOutOfStamina = false;
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
