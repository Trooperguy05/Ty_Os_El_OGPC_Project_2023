using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSoundDetection : MonoBehaviour
{
    public bool inEarshot = false;
    public float soundSensitivity = 5f;
    //private MonsterMovement mM;
    private PlayerSoundRadius pSR;

    // get stuff
    void Awake() {
        //mM = transform.parent.gameObject.GetComponent<MonsterMovement>();
        pSR = GameObject.Find("Sound Radius").GetComponent<PlayerSoundRadius>();
    }

    // sound detection
    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "player sound") {
            inEarshot = true;
        }
    }
    void OnTriggerExit(Collider col) {
        if (col.gameObject.tag == "player sound") {
            inEarshot = false;
        }
    }
}
