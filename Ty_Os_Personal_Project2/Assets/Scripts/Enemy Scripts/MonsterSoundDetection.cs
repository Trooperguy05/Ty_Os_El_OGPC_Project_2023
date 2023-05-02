using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSoundDetection : MonoBehaviour
{
    public bool inEarshot = false;
    public float baseSoundSensitivity;
    public float soundSensitivity;
    public Vector3 pointOfSound;
    private GameObject player;

    void Start() {
        // Get Stuff \\
        player = GameObject.Find("Player");
    }

    void Update() {
        // sound sensitivity decreases as the player gets close to the mine
        if (inEarshot) {
            soundSensitivity = baseSoundSensitivity + Vector3.Distance(transform.position, player.transform.position);
        }
        else {
            soundSensitivity = baseSoundSensitivity;
        }
    }

    // sound detection
    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "player sound") {
            inEarshot = true;

            pointOfSound = col.ClosestPoint(transform.position);
        }
    }
    void OnTriggerExit(Collider col) {
        if (col.gameObject.tag == "player sound") {
            inEarshot = false;
        }
    }
}
