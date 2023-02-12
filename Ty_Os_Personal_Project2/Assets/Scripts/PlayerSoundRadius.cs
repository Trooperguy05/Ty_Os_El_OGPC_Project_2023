using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundRadius : MonoBehaviour
{
    [Header("Sound Radius Collider")]
    public SphereCollider soundCollider;

    [Header("Sound Radi(?)")]
    public float walkRadius;
    public float runRadius;
    public float crouchRadius;

    [Header("Player Movement")]
    private PlayerMovementTest pM;

    // get the player movement test script
    void Awake() {
        pM = GetComponent<PlayerMovementTest>();
    }

    // 
    void Update()
    {
        /// high noise activities \\\
        // if player is walking
        if (pM.state == PlayerMovementTest.PlayerState.walk) {
            soundCollider.radius = walkRadius;
        }
        // if player is running
        else if (pM.state == PlayerMovementTest.PlayerState.run) {
            soundCollider.radius = runRadius;
        }
        /// low noise activities \\\
        // if player is standing still, in air, or crouching
        else {
            soundCollider.radius = 0.01f;
        }
    }
}
