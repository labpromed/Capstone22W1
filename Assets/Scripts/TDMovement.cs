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
    
    private float currentSpeed;
    private bool isSprinting = false;
    private PlayerInput inputActions;
    private Rigidbody rb;
    private Vector2 moveInput;
    private bool jumpPressed;

    void Awake()
    {
        if (instance != this && instance != null) return;
        instance = this;


        rb = GetComponent<Rigidbody>();
        inputActions = new PlayerInput();

        rb.drag = 0f;
        rb.angularDrag = 0f;
    }

    void Start()
    {
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
        }
    }

    void Move()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * moveInput.y + right * moveInput.x;

        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        // Vector3 targetPos = rb.position + moveDir * currentSpeed * Time.fixedDeltaTime;
        // rb.MovePosition(targetPos);

        RaycastHit hit;
        bool grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 1.2f);

        Vector3 slopeNormal = hit.normal;
        Vector3 slopeDir = Vector3.ProjectOnPlane(moveDir, slopeNormal).normalized;

        Vector3 finalMove = grounded ? slopeDir : moveDir;
        Vector3 newPos = rb.position + finalMove * currentSpeed * Time.fixedDeltaTime;

        rb.MovePosition(newPos);

        if (grounded && moveInput == Vector2.zero)
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }

        if (moveDir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            Quaternion rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(rotation);
        }
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
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

}
