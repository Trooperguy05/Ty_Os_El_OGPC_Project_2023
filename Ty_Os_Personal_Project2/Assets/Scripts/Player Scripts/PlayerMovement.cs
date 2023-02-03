using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;
    public bool isRunning;
    public float footstepSpeed;
    private float hI;
    private float vI;
    private Vector3 moveDir;
    private Rigidbody rb;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool canJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    public float groundDrag;
    public bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        footstepSpeed = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);

        // input
        input();

        // handle drag
        if (grounded) rb.drag = groundDrag;
        else rb.drag = 0;
    }

    void FixedUpdate() {
        movePlayer();
    }

    private void input() {
        hI = Input.GetAxisRaw("Horizontal");
        vI = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && canJump && grounded) {
            canJump = false;
            jump();
            Invoke(nameof(resetJump), jumpCooldown);
        }
    }

    private void movePlayer() {
        moveDir = orientation.forward * vI + orientation.right * hI;

        if (grounded) rb.AddForce(moveDir * moveSpeed * 10f, ForceMode.Force);
        else if (!grounded) {
            rb.AddForce(moveDir * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void speedControl() {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed) {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = limitedVel;
        }
    }

    private void jump() {
        // reset y vel
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void resetJump() { canJump = true; }
}
