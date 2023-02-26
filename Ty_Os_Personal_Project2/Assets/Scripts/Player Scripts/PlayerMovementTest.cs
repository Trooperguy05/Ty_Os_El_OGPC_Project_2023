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

    [Header("Stamina")]
    private Stamina playerStamina;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool canJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Slope Vars")]
    public float maxAngle;
    private RaycastHit slopeHit;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    public float groundDrag;
    public bool grounded;

    [Header("States? idk")]
    public PlayerState state;
    public enum PlayerState {
        none,
        stand,
        walk,
        run,
        crouch,
        air
    }

    // get stuff
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerStamina = GetComponent<Stamina>();

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
            /// sprinting
            if (grounded && Input.GetKeyDown(runKey)) {
                // toggle on running
                if (state != PlayerState.run && playerStamina.currentStamina > 0) {
                    state = PlayerState.run;
                    moveSpeed = runSpeed;
                    playerStamina.startRunning();
                }
                // toggle off running
                else if (state == PlayerState.run) {
                    state = PlayerState.walk;
                    moveSpeed = walkSpeed;
                    playerStamina.stopRunning();
                }
            }
            /// if player is running
            else if (state == PlayerState.run) {
                // standing
                if (grounded && (vI == 0 && hI == 0)) {
                    state = PlayerState.stand;
                    playerStamina.stopRunning();
                }
                // air
                else if (!grounded) {
                    state = PlayerState.air;
                    playerStamina.stopRunning();
                }
            }
            /// if player is not running
            if (state != PlayerState.run) {
                /// walking
                if (grounded && (vI != 0 || hI != 0)) {
                    state = PlayerState.walk;
                    moveSpeed = walkSpeed;
                }
                /// ground/stand
                else if (grounded) {
                    state = PlayerState.stand;
                }
                /// air
                else {
                    state = PlayerState.air;
                }
            }
        }
    }

    // method that moves the player
    private void movePlayer() {
        moveDir = orientation.forward * vI + orientation.right * hI;

        // if on a slope
        if (onSlope()) {
            rb.AddForce(getSlopeMoveDirection() * moveSpeed * 10f, ForceMode.Force);
        }
        // if not on slope
        else {
            if (grounded) rb.AddForce(moveDir * moveSpeed * 10f, ForceMode.Force);
            else rb.AddForce(moveDir * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        // turn off gravity on a slope
        rb.useGravity = !onSlope();
    }
    
    // method that limits the player's speed
    private void speedControl() {
        // limit speed on slope
        if (onSlope()) {
            if (rb.velocity.magnitude > moveSpeed) {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        // limit speed on ground and air
        else {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed) {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
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

    // method that checks if player is on a slope
    private bool onSlope() {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f)) {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxAngle && angle != 0;
        }
        return false;
    }

    // method that gets a move direction based on slope angle
    private Vector3 getSlopeMoveDirection() {
        return Vector3.ProjectOnPlane(moveDir, slopeHit.normal).normalized;
    }
}
