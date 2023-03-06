using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySFX : MonoBehaviour
{
    // FMOD Variables \\
    [Header("Event References")]
    public FMODUnity.EventReference movementReference;
    [Header("Event Instances")]
    private FMOD.Studio.EventInstance movement;
    
    // Unity Variables \\
    [Header("Scripts")]
    private MonsterMovement monsterMovement;
    [Header("Movement Vars")]
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        // Get Stuff \\
        monsterMovement = GameObject.Find("Monster").GetComponent<MonsterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if monster is moving \\
        if (monsterMovement.stateMachine.currentState.name != "StandState") {
            // Check if monster is wandering \\
            if (monsterMovement.stateMachine.currentState.name == "WanderState") {
                // call playEnemyMovement() at the right time \\
                if (timer > 1.0f) {
                    playEnemyMovement(false);
                    timer = 0.0f;
                }
            }

            // Check if monster is chasing the player \\
            else if (monsterMovement.stateMachine.currentState.name == "ChaseState") {
                // call playEnemyMovement() at the right time \\
                if (timer > 0.5f) {
                    playEnemyMovement(true);
                    timer = 0;
                }
            }
        }

        // Increment timer \\
        timer += Time.deltaTime;
    }

    // Play the enemy moving sound effect(s) \\
    private void playEnemyMovement(bool isChasing) {
        // Set FMOD event parameter \\
        if (isChasing) {} // Set Parameter \\
        else {} // Set Parameter \\
        // Play the movement sound effect \\
        movement = FMODUnity.RuntimeManager.CreateInstance(movementReference);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(movement, gameObject.transform);
        movement.start();
        movement.release();
    }

    // Get the playback state of an instance \\
    private FMOD.Studio.PLAYBACK_STATE checkPlaybackState(FMOD.Studio.EventInstance instance) {
        // Create a new temporary variable \\
        FMOD.Studio.PLAYBACK_STATE playbackState;
        // Get the instances playback state and output into playbackState temp variable \\
        instance.getPlaybackState(out playbackState);
        // Return the instances playback state \\
        return playbackState;
    }

}
