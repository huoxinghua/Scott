using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputManager inputManager;
    private GroundCheck groundCheck;
    [Header("move")]
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveDirection;
   // [HideInInspector]
    public bool isSprinting = false;

    [SerializeField] private float sprintMultiplier = 1.5f;
    [Header("jump")]
    [SerializeField] private float jumpStrength = 2f;
    private Rigidbody rb;
    [SerializeField] private float fallMultiplier = 4f;

    private Vector3 originalPos;
    private int safePosition = -7;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = GetComponentInChildren<GroundCheck>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Start()
    {
        originalPos = transform.position;
    }

    private void OnEnable()
    {
        inputManager = GetComponentInChildren<PlayerInputManager>();

        if (inputManager != null)
        {
            inputManager.OnMoveInput += Move;
            inputManager.OnJumpInput += Jump;
            inputManager.OnSprintInputStart += SprintStart;
            inputManager.OnSprintInputCancel += SprintCancel;
        }
        else
        {
            Debug.Log("input manager is null ");
        }
    }

  

    private void OnDisable()
    {
        inputManager = GetComponent<PlayerInputManager>();
        if (inputManager != null)
        {
            inputManager.OnMoveInput -= Move;
            inputManager.OnJumpInput -= Jump;
            inputManager.OnSprintInputStart -= SprintStart;
            inputManager.OnSprintInputCancel -= SprintCancel;
        }
        else
        {
            Debug.Log("input manager is null ");
        }
    }
    private void SprintStart()
    {
        isSprinting = true;
    }
    private void SprintCancel()
    {
        isSprinting = false;
    }
    private void FixedUpdate()
    {
        //move
        Vector3 velocity = rb.linearVelocity;
        var currentSpeed = moveSpeed;
        PlayerLook camera = Camera.main.GetComponent<PlayerLook>();
        if(isSprinting )
        {
            currentSpeed = moveSpeed * sprintMultiplier;
        }
        else
        {
            currentSpeed = moveSpeed;
        }
        velocity.x = moveDirection.x * currentSpeed;
        velocity.z = moveDirection.y * currentSpeed;
        rb.linearVelocity = transform.rotation * (velocity + direction);

        // extra gravity when falling
        if (!groundCheck.isGrounded && rb.linearVelocity.y < 0)
        {
          
            rb.AddForce(Vector3.down * fallMultiplier, ForceMode.Acceleration);
        }

        if(transform.position.y <= safePosition)
        {
            transform.position = originalPos;
        }
    }
    Vector3 direction;
    public bool isMoving =false;
    public void Move(Vector2 dir)
    {
        moveDirection = dir;
      
        if (dir != Vector2.zero)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }
    public void Jump()
    {
        if (groundCheck && groundCheck.isGrounded)
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            //Debug.Log("player jump");
        }
    }


    internal void SetBonusSpeed(float bonus)
    {
        Debug.Log("before upgrade speed:" + moveSpeed + "bonus:" + bonus);
       
      
        moveSpeed = moveSpeed * bonus;
        Debug.Log("after upgrade speed:" + moveSpeed);
    }
}
