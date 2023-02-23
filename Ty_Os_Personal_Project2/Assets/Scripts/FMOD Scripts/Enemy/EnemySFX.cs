using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySFX : MonoBehaviour
{
    [Header("Event References")]
    public FMODUnity.EventReference runningFootstepsReference;
    public FMODUnity.EventReference walkingFootstepsReference;

    private FMOD.Studio.EventInstance runningFootsteps;
    private FMOD.Studio.EventInstance walkingFootsteps;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (true) { // Needs to be set to if Enemy is running
            if (timer > null) {
                playEnemyFootsteps(true);
                timer = 0.0f;
            }
        }
        else if (false) {
            if (timer > null) { // Needs to be set to if Enemy is not running
                playEnemyFootsteps(false);
                timer = 0.0f;
            }
        }

        timer += Time.deltaTime;
    }

    public void playEnemyFootsteps(bool isRunning) {
        // Play running footsteps if running \\
        if (isRunning) {
            runningFootsteps = FMODUnity.RuntimeManager.CreateInstance(runningFootstepsReference);
            runningFootsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            runningFootsteps.start();
            runningFootsteps.release();
        }
        // Play walking footsteps if not running \\
        else {
            walkingFootsteps = FMODUnity.RuntimeManager.CreateInstance(walkingFootstepsReference);
            walkingFootsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            walkingFootsteps.start();
            walkingFootsteps.release();
        }
    }
}
