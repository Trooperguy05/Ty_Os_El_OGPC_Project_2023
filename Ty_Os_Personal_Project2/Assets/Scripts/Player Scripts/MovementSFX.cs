using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementSFX : MonoBehaviour
{
    private FMOD.Studio.EventInstance footsteps;
    private FMOD.Studio.EventInstance runningFootsteps;
    private PlayerMovement playerMovement;
    private PlayerMovementTest playerMovementTest;
    private float timer = 0.0f;
    [Header("Event References")]
    public FMODUnity.EventReference footstepsReference;
    public FMODUnity.EventReference runningFootstepsReference;
    // Start is called before the first frame update
    void Start()
    // Get Stuff \\
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMovementTest = GetComponent<PlayerMovementTest>();
    }

    // Update is called once per frame
    void Update()
    {   
        // Time running footsteps \\
        if (playerMovement.isRunning && playerMovement.grounded && playerMovement.isMoving) {
            if (timer > playerMovement.footstepSpeed) {
                PlayFootstep(true);
                timer = 0.0f;
            }
        }
        // Time walking footsteps \\
        else if (/*playerMovement.grounded && playerMovement.isMoving && */playerMovementTest.state == PlayerMovementTest.PlayerState.walk) {
            if (timer > /*playerMovement.footstepSpeed*/ 1.0f) {
                PlayFootstep(false);
                timer = 0.0f;
            }
        }
        
        timer += Time.deltaTime;
    }

    // Play footsteps event \\
    public void PlayFootstep(bool isRunning) {
        // Play running footsteps \\
        if (isRunning) {
            runningFootsteps = FMODUnity.RuntimeManager.CreateInstance(runningFootstepsReference);
            runningFootsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            runningFootsteps.start();
            runningFootsteps.release();
        }
        // Play walking footsteps \\
        else {
            footsteps = FMODUnity.RuntimeManager.CreateInstance(footstepsReference);
            footsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            footsteps.start();
            footsteps.release();
        }
    }
}
