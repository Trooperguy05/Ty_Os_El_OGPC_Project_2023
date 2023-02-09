using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementSFX : MonoBehaviour
{
    private FMOD.Studio.EventInstance footsteps;
    private FMOD.Studio.EventInstance runningFootsteps;
    private FMOD.Studio.EventInstance heavyBreathing;
    private PlayerMovement playerMovement;
    private PlayerMovementTest playerMovementTest;
    private Stamina stamina;
    private float timer = 0.0f;
    [SerializeField] private float runningFootstepsSpeed = 0.3f;
    [SerializeField] private float footstepsSpeed = 0.5f;
    public bool isBreathing;
    [Header("Event References")]
    public FMODUnity.EventReference footstepsReference;
    public FMODUnity.EventReference runningFootstepsReference;
    public FMODUnity.EventReference heavyBreathingReference;
    // Start is called before the first frame update
    void Start()
    // Get Stuff \\
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMovementTest = GetComponent<PlayerMovementTest>();
        stamina = GetComponent<Stamina>();
        heavyBreathing = FMODUnity.RuntimeManager.CreateInstance(heavyBreathingReference);
    }

    // Update is called once per frame
    void Update()
    {   
        // Time running footsteps \\
        if (playerMovementTest.state == PlayerMovementTest.PlayerState.run) {
            if (timer > runningFootstepsSpeed) {
                PlayFootstep(true);
                timer = 0.0f;
            }
        }
        // Time walking footsteps \\
        else if (playerMovementTest.state == PlayerMovementTest.PlayerState.walk) {
            if (timer > footstepsSpeed) {
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

    // Starts playing heavyBreathing event Instance \\
    public void startHeavyBreathing() {
        if (!isBreathing) {
            heavyBreathing.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            heavyBreathing.start();
        }
        isBreathing = true;
    }

    // Stops playing heavyBreathing event Instance \\
    public void stopHeavyBreathing() { 
        heavyBreathing.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        isBreathing = false;
    }
}
