using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundRadius : MonoBehaviour
{
    [Header("Sound Radius Collider")]
    public SphereCollider soundCollider;

    [Header("Sound Values")]
    public float soundValue;
    public float walkValue;
    public float runValue;
    public float stompValue;
    private float variableSoundValue;

    [Header("Sound Radi(?)")]
    public float walkRadius;
    public float runRadius;
    public float stompRadius;

    [Header("Player Movement")]
    public bool inAir;
    public bool stomped;
    public float stompLength;
    private PlayerMovementTest pM;

    // get the player movement test script
    void Awake() {
        pM = GetComponent<PlayerMovementTest>();
    }

    void Update()
    {
        // variable sound values so movement states aren't always one value
        bool playerIsMoving = (pM.state != PlayerMovementTest.PlayerState.air && pM.state != PlayerMovementTest.PlayerState.crouch && pM.state != PlayerMovementTest.PlayerState.stand);
        if (playerIsMoving) variableSoundValue = Random.Range(-1f, 1f);

        // checking if the player landed from a jump or fall \\
        // check for jump input
        if (pM.state == PlayerMovementTest.PlayerState.air) {
            inAir = true;
        }
        // check if they landed
        if (inAir && pM.grounded) {
            inAir = false;
            stomped = true;
            StartCoroutine(endStomp());
        }
        // create noise from landing
        if (stomped) {
            soundValue = stompValue + variableSoundValue;
            soundCollider.radius = stompRadius;
        }
        else {
            /// other high noise activities \\\
            // if player is walking
            if (pM.state == PlayerMovementTest.PlayerState.walk) {
                soundValue = walkValue + variableSoundValue;
                soundCollider.radius = walkRadius;
            }
            // if player is running
            else if (pM.state == PlayerMovementTest.PlayerState.run) {
                soundValue = runValue + variableSoundValue;
                soundCollider.radius = runRadius;
            }
            /// low noise activities \\\
            // if player is standing still, in air, or crouching
            else {
                soundValue = 0f;
                soundCollider.radius = 0.01f;
            }
        }
    }

    // method that waits a moment to end the stomp sound
    public IEnumerator endStomp() {
        yield return new WaitForSeconds(stompLength);
        stomped = false;
    }
}
