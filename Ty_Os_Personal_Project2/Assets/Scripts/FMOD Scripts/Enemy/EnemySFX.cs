using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySFX : MonoBehaviour
{
    // FMOD Variables \\
    [Header("Event References")]
    public FMODUnity.EventReference footstepsReference;
    [Header("Event Instances")]
    private FMOD.Studio.EventInstance footsteps;
    
    // Unity Variables \\
    private float timer;
    private MonsterMovement monsterMovement;
    // Start is called before the first frame update
    void Start()
    {
        monsterMovement = GameObject.Find("Monster").GetComponent<MonsterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (monsterMovement.stateMachine.currentState.name != "StandState") {
            if (monsterMovement.stateMachine.currentState.name == "WanderState") {
                if (timer > 1.0f) {
                    playEnemyFootsteps(false);
                    timer = 0.0f;
                }
            }

            else if (monsterMovement.stateMachine.currentState.name == "ChaseState") {
                if (timer > 0.5f) {
                    playEnemyFootsteps(true);
                    timer = 0;
                }
            }
        }

        timer += Time.deltaTime;
    }

    public void playEnemyFootsteps(bool isChasing) {
        // Play Enemy Footstep Event \\
        footsteps = FMODUnity.RuntimeManager.CreateInstance(footstepsReference);
        footsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        footsteps.start();
        footsteps.release();
        
    }
}
