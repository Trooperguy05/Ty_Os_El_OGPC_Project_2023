using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTest : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float runSpeed;
    public Transform orientation;
    private float hI;
    private float vI;
    private Vector3 moveDir;
    private Rigidbody rb;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool canJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    public float groundDrag;
    public bool grounded;

    [Header("States? idk")]
    public PlayerState state;
    public enum PlayerState {
        none,
        walk,
        run,
        crouch,
        air
    }

    // get stuff
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        startYScale = transform.localScale.y;
    }

    // calls everything
    void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, groundLayer);

        // input
        input();
        speedControl();
        stateHandler();

        // handle drag
        if (grounded) rb.drag = groundDrag;
        else rb.drag = 0;
    }

    // move the player through fixed update
    void FixedUpdate() {
        movePlayer();
    }

    // method that handles player input
    private void input() {
        // walk/run input
        hI = Input.GetAxisRaw("Horizontal");
        vI = Input.GetAxisRaw("Vertical");
        
        // jump input
        if (Input.GetKey(jumpKey) && canJump && grounded) {
            canJump = false;
            jump();
            Invoke(nameof(resetJump), jumpCooldown);
        }

        // crouch input
        if (Input.GetKeyDown(crouchKey)) {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        else if (Input.GetKeyUp(crouchKey)) {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    // method that handles different player movement states
    private void stateHandler() {
        // crouch
        if (Input.GetKey(crouchKey)) {
            state = PlayerState.crouch;
            moveSpeed = crouchSpeed;
        }
        // when not crouching
        else {
            // sprinting
            if (grounded && Input.GetKey(runKey)) {
                state = PlayerState.run;
                moveSpeed = runSpeed;
            }
            // walking
            else if (grounded) {
                state = PlayerState.walk;
                moveSpeed = walkSpeed;
            }
            // air
            else {
                state = PlayerState.air;
            }
        }
    }

    // method that moves the player
    private void movePlayer() {
        moveDir = orientation.forward * vI + orientation.right * hI;

        if (grounded) rb.AddForce(moveDir * moveSpeed * 10f, ForceMode.Force);
        else rb.AddForce(moveDir * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }
    
    // method that limits the player's speed
    private void speedControl() {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed) {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    // method that makes the player jump
    private void jump() {
        // reset y vel
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    // method that resets ability to jump
    private void resetJump() { canJump = true; }
}
