using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySFX : MonoBehaviour
{
    // FMOD Variables \\
    [Header("Event References")]
    public FMODUnity.EventReference movementReference;
    public FMODUnity.EventReference enemyAmbientReference;
    public FMODUnity.EventReference stingerReference;

    [Header("Event Instances")]
    private FMOD.Studio.EventInstance movement;
    private FMOD.Studio.EventInstance enemyAmbient;
    private FMOD.Studio.EventInstance stinger;

    [Header("Snapshot Instances")]
    private FMOD.Studio.EventInstance stingerSnapshot;
    
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
        // Start Stuff \\
        playEnemyAmbient();
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

    // Play the enemy ambience event \\
    private void playEnemyAmbient() {
        // Create and play the EnemyAmbient event \\
        enemyAmbient = FMODUnity.RuntimeManager.CreateInstance(enemyAmbientReference);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(enemyAmbient, gameObject.transform);
        enemyAmbient.start();
    }

    // Play the stinger event \\
    private void playStinger() {
        // Create and play the stinger event instance \\
        stinger = FMODUnity.RuntimeManager.CreateInstance(stingerReference);
        stinger.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        stinger.start();
        // Start the stinger snapshot \\
        StartCoroutine(playStingerSnapshot());
        // Release the stinger event from memory \\
        stinger.release();
    }

    // Use the stinger snapshot to bring down the volume of all other events \\
    private IEnumerator playStingerSnapshot() {
        // Create and play the stinger snapshot instance \\
        stingerSnapshot = FMODUnity.RuntimeManager.CreateInstance("snapshot:/Monster Stinger");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(stingerSnapshot, gameObject.transform);
        stingerSnapshot.start();
        // Check if the stinger event is done playing \\
        FMOD.Studio.PLAYBACK_STATE playbackState = FMOD.Studio.PLAYBACK_STATE.PLAYING;
        while (playbackState != FMOD.Studio.PLAYBACK_STATE.STOPPED) {
            // Wait until the stinger is done playing \\
            stinger.getPlaybackState(out playbackState);
            yield return null;
        }
        // Once the stinger event is finished stop the snapshot \\
        stingerSnapshot.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        // Release the snapshot from memory \\
        stingerSnapshot.release();
    }
}
