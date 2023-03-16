using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSoundDetection : MonoBehaviour
{
    public bool inEarshot = false;
    public float soundSensitivity = 5f;
    public Vector3 pointOfSound;

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
