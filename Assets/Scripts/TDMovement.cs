using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TDMovement : MonoBehaviour
{
    public static TDMovement instance;
    
    
    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float sprintSpeed = 10f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float jumpForce = 5f;

    [Header("References")]
    [SerializeField] Transform cameraTransform;

    [Header("Slope")]
    [SerializeField] float maxSlopeAngle;
    private RaycastHit slopeHit;
    
    private float currentSpeed;
    private bool isSprinting = false;
    private PlayerInput inputActions;
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool jumpPressed;
    private Animator animator;

    void Awake()
    {
        if (instance != this && instance != null) return;
        instance = this;


        rb = GetComponent<Rigidbody>();
        inputActions = new PlayerInput();

        rb.drag = 0f;
        rb.angularDrag = 0f;

        animator = GetComponent<Animator>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (respawnPoint == Vector3.zero)
        respawnPoint = transform.position;
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Sprint.started += ctx => isSprinting = true;
        inputActions.Player.Sprint.canceled += ctx => isSprinting = false;
        inputActions.Player.Jump.performed += ctx => jumpPressed = true;
    }

    void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Jump.performed -= ctx => jumpPressed = true;
        inputActions.Player.Disable();
    }

    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Move();
        HandleJump();

        if (transform.position.y < -10f)
        {
            Die();
            Debug.Log("Dead");
        }

        animator.SetFloat("Speed", ((moveDir + rb.position) - rb.position).magnitude);
        
        grounded = IsGrounded();
        bool isInAir = !grounded;
        animator.SetBool("IsJumping", isInAir && rb.velocity.y > 0.1f);
        animator.SetBool("IsFalling", isInAir && rb.velocity.y < -0.1f);
    }
    
    Vector3 moveDir;
    bool grounded;
    void Move()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        moveDir = forward * moveInput.y + right * moveInput.x;

        currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        Vector3 targetPos = rb.position + moveDir * currentSpeed * Time.fixedDeltaTime;
        Vector3 smoothPos = Vector3.MoveTowards(rb.position, targetPos, currentSpeed * Time.fixedDeltaTime);
        //rb.MovePosition(targetPos);

        //SlopeMovement

        if (OnSlope())
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            targetPos = rb.position + SlopeMoveDir() * currentSpeed * Time.fixedDeltaTime;
            smoothPos = Vector3.MoveTowards(rb.position, targetPos, currentSpeed * Time.fixedDeltaTime);
            rb.MovePosition(smoothPos);
        }
        else
        {
            rb.MovePosition(smoothPos);
        }
        
        float speed = (new Vector3 (rb.velocity.x, 0f, rb.velocity.z).normalized * currentSpeed).magnitude;
        //Debug.Log(speed);
        //Debug.Log(((moveDir + rb.position) - rb.position).magnitude);
        RaycastHit hit;
        grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 1.2f);

        // Vector3 slopeNormal = hit.normal;
        // Vector3 slopeDir = Vector3.ProjectOnPlane(moveDir, slopeNormal).normalized;

        // Vector3 finalMove = grounded ? slopeDir : moveDir;
        // Vector3 newPos = rb.position + finalMove * currentSpeed * Time.fixedDeltaTime;

        // rb.MovePosition(newPos);

        // if (grounded && moveInput == Vector2.zero)
        // {
        //     rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        // }
        
        //end of slope movement
        if (moveDir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            Quaternion rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(rotation);
        }
    }
    
    private bool OnSlope()
    {
        float playerHeight = transform.position.y + 1f;

        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            //Debug.Log(angle);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 SlopeMoveDir()
    {
        return Vector3.ProjectOnPlane(moveDir, slopeHit.normal).normalized;
    }

    void HandleJump()
    {
        if (jumpPressed && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        jumpPressed = false;
    }

    [SerializeField] Vector3 respawnPoint;

    public void SetRespawnPoint(Vector3 newPoint)
    {
        respawnPoint = newPoint;
        Debug.Log("Respawn to" + newPoint);
    }

    void Die()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = respawnPoint + Vector3.up * 2;
        Debug.Log("Respawn");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

}
