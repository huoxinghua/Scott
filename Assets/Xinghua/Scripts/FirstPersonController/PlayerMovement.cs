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
    private Animator[]animators;
    public Animator playerAnim;
    public Animator gunAnim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = GetComponentInChildren<GroundCheck>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animators = GetComponentsInChildren<Animator>();
        playerAnim =animators[0];
       gunAnim = animators[1];


    }
    private bool isIdle = true;
    private void Start()
    {
        originalPos = transform.position;
        
        playerAnim.SetFloat("Speed", 0f);
 

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
        playerAnim.SetFloat("Speed", 1);
    }
    private void SprintCancel()
    {
        isSprinting = false;
        if(isMoving)
        {
            isIdle = false;
            playerAnim.SetFloat("Speed", 0.5f);
        }
        else
        {
            isIdle = true;
            playerAnim.SetFloat("Speed", 0f);
        }
       
    }
    private bool wasGrounded;
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


        playerAnim.SetBool("InAir", !groundCheck.isGrounded);
        playerAnim.SetFloat("YVelocity", rb.linearVelocity.y);
        if (groundCheck.isGrounded &&!wasGrounded  )
        {
            playerAnim.SetBool("isJump", false);
            playerAnim.SetBool("isGrounded", true);
        }
        else//in air
        {
            playerAnim.SetBool("isGrounded", false);
        }
        wasGrounded = groundCheck.isGrounded;
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
            playerAnim.SetBool("isJump",true);
           // playerAnim.SetBool("InAir", false);
            //playerAnim.SetFloat("YVelocity", rb.linearVelocity.y);
        }
 
      
    }
    public void SetBonusSpeed(float bonus)
    {
       if(bonus == 0)return;
     
        Debug.Log("before upgrade speed:" + moveSpeed + "bonus:" + bonus);
        float totalbonus = 0f;
        totalbonus += bonus;
        moveSpeed = moveSpeed * (1+ totalbonus);
        Debug.Log("after upgrade speed:" + moveSpeed);
    }
}
