using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputManager inputManager;
    private GroundCheck groundCheck;
    [Header("move")]
    private float moveSpeed;
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
    private Animator[] animators;
    public Animator playerAnim;
    public Animator gunAnim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = GetComponentInChildren<GroundCheck>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animators = GetComponentsInChildren<Animator>();
        playerAnim = animators[0];
        gunAnim = animators[1];

    }
    private bool isIdle = true;
    private void Start()
    {
        originalPos = transform.position;
        if (UpgradeManager.Instance != null )
        {
            Debug.Log("UpgradeManager player speed:" + UpgradeManager.Instance.newSpeed);
            moveSpeed = UpgradeManager.Instance.newSpeed;
        }
        

        Debug.Log("player start speed:" + moveSpeed);
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
        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.OnPlayerDataUpgradeConfirm += SetSpeed;
        }
    }

    private void SetSpeed(float value)
    {
       moveSpeed = value;
        Debug.Log("set player speed to : "+moveSpeed);
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
        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.OnPlayerDataUpgradeConfirm -= SetSpeed;
        }
    }

    private void SprintStart()
    {
        if (!groundCheck.isGrounded) return;//if jump in air can not sprint
       
        isSprinting = true;
        playerAnim.SetFloat("Speed", 1);
    }
    private void SprintCancel()
    {
        isSprinting = false;
        if (isMoving)
        {
            isIdle = false;
            playerAnim.SetFloat("Speed", 0.5f);
            Debug.Log("speed" + moveSpeed);
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
        if (isSprinting)
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

        if (transform.position.y <= safePosition)
        {
            transform.position = originalPos;
        }


        playerAnim.SetBool("InAir", !groundCheck.isGrounded);
        playerAnim.SetFloat("YVelocity", rb.linearVelocity.y);
        if (groundCheck.isGrounded && !wasGrounded)
        {
            playerAnim.SetBool("isJump", false);
            //  playerAnim.SetBool("isGrounded", true);
        }
        /*  else//in air
          {
              playerAnim.SetBool("isGrounded", false);
          }*/
        wasGrounded = groundCheck.isGrounded;
    }
    Vector3 direction;
    public bool isMoving = false;

    public int UpdateData { get; private set; }

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
            playerAnim.SetBool("isJump", true);
        }
    }
    public void SetBonusSpeed(float totalBonus)
    {
        if (totalBonus == 0) return;
        Debug.Log("before upgrade speed:" + moveSpeed + "totalbonus:" + totalBonus);
        moveSpeed = moveSpeed * totalBonus;
        Debug.Log("after upgrade speed:" + moveSpeed);
    }
}
