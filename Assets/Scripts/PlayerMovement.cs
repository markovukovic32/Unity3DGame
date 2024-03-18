using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    private Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    bool isClimbing;

    private Animator playerAnimator;
    private bool isWalking = false;
    private bool isRunning = false;
    private bool isWalkingBackwards = false;
    private float distToGround;

    private GameObject akFPS;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        orientation = GameObject.FindWithTag("Orientation").transform;
        playerAnimator = gameObject.GetComponent<Animator>();
        akFPS = GameObject.FindWithTag("AKfps");
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);

        MyInput();
        SpeedControl();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        if (isClimbing)
        {
            Climbing();
            akFPS.layer = 2;
        }
        else
        {
            MovePlayer();
            akFPS.layer = 0;
        }
        if (IsPlayerStayingStill())
        {
            playerAnimator.SetBool("isIdle", true);
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) && !isRunning)
        {
            playerAnimator.SetBool("isRunning", true);
            isRunning = true;
        }
        else if (Input.GetKeyDown(KeyCode.W) && !isWalking)
        {
            playerAnimator.SetBool("isWalking", true);
            isWalking = true;
        }
        else if (Input.GetKeyDown(KeyCode.S) && !isWalkingBackwards)
        {
            playerAnimator.SetBool("isWalkingBackwards", true);
            isWalkingBackwards = true;
        }

        if (!IsPlayerStayingStill())
        {
            playerAnimator.SetBool("isIdle", false);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            playerAnimator.SetBool("isRunning", false);
            isRunning = false;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            playerAnimator.SetBool("isWalking", false);
            isWalking = false;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            playerAnimator.SetBool("isWalkingBackwards", false);
            isWalkingBackwards = false;
        }
    }

    private bool IsPlayerStayingStill()
    {
        return rb.velocity.magnitude == 0f;
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        float currentMoveSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * 2f : moveSpeed;

        if (grounded)
            rb.AddForce(moveDirection.normalized * currentMoveSpeed * 10f, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * currentMoveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        float currentMoveSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * 2f : moveSpeed;

        if (flatVel.magnitude > currentMoveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * currentMoveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            rb.useGravity = false; 
            rb.velocity = Vector3.zero; 

            isClimbing = true;
        }
        if (other.gameObject.CompareTag("LadderEnd"))
        {
            Debug.Log("Ladder end trigger");
            rb.useGravity = true; 
            isClimbing = false;
        }
    }

    private void Climbing()
    {
        float climbInput = Input.GetAxisRaw("Vertical");

        Vector3 climbDirection = new Vector3(0f, climbInput, 0f);

        rb.MovePosition(transform.position + climbDirection.normalized * moveSpeed * Time.deltaTime);
    }
}
