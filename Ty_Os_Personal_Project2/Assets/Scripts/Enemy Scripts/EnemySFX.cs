using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySFX : MonoBehaviour
{
    [Header("Event References")]
    public FMODUnity.EventReference referencePlaceholder1;
    public FMODUnity.EventReference referencePlaceholder2;

    private FMOD.Studio.EventInstance instancePlaceholder1;
    private FMOD.Studio.EventInstance instancePlaceholder2;
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
            instancePlaceholder1 = FMODUnity.RuntimeManager.CreateInstance(referencePlaceholder1);
            instancePlaceholder1.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            instancePlaceholder1.start();
            instancePlaceholder1.release();
        }
        // Play walking footsteps if not running \\
        else {
            instancePlaceholder2 = FMODUnity.RuntimeManager.CreateInstance(referencePlaceholder2);
            instancePlaceholder2.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            instancePlaceholder2.start();
            instancePlaceholder2.release();
        }
    }
}
