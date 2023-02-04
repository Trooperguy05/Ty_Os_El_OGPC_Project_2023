using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode runKey = KeyCode.LeftShift;

    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;
    public float footstepSpeed;
    public bool isRunning;
    public bool isMoving;
    [SerializeField] private bool ableToRun;
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
        ableToRun = true;
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
        movePlayer(isRunning);
    }

    private void input() {
        hI = Input.GetAxisRaw("Horizontal");
        vI = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(runKey)) {
            if (isRunning) {
                footstepSpeed = 1.0f;
                StopCoroutine(runningStamina(2.0f));
                isRunning = false;
                StartCoroutine(runningCooldown(3.0f));
            }
            else if (ableToRun) {
                footstepSpeed = 0.5f;
                StartCoroutine(runningStamina(2.0f));
            }
        }
        // Stop running and start cooldown if player stops moving \\
        if (isRunning && !isMoving) {
            footstepSpeed = 1.0f;
            StopCoroutine(runningStamina(2.0f));
            isRunning = false;
            StartCoroutine(runningCooldown(3.0f));
        }

        if (Input.GetKey(jumpKey) && canJump && grounded) {
            canJump = false;
            jump();
            Invoke(nameof(resetJump), jumpCooldown);
        }
    }

    private void movePlayer(bool running) {
        moveDir = orientation.forward * vI + orientation.right * hI;
        
        // Checks if player is moving \\
        if (rb.velocity == new Vector3(0, 0, 0)) isMoving = false;
        else isMoving = true;

        if (grounded && !running) rb.AddForce(moveDir * moveSpeed * 10f, ForceMode.Force);
        else if (grounded && running) rb.AddForce(moveDir * moveSpeed * 20f, ForceMode.Force);
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

    // Sprinting \\
    public IEnumerator runningStamina(float sprintDuration) {
        // Change bools \\
        ableToRun = false;
        isRunning = true;
        // allow player to sprint for "sprintDuration" \\
        float timer = 0.0f;
        while (isRunning) {
            if (timer > sprintDuration) {
                isRunning = false;
                ableToRun = false;
                footstepSpeed = 1.0f;
            }
            else { timer += Time.deltaTime; }
            yield return null;
        }
        // Start running cooldown \\
        StartCoroutine(runningCooldown(3.0f));
    }

    // sprint cooldown \\
    public IEnumerator runningCooldown(float sprintCooldown) {
        float timer = 0.0f;
        while (timer < sprintCooldown) {
            timer += Time.deltaTime;
            yield return null;
        }
        // Change bool \\
        ableToRun = true;
    }
}
