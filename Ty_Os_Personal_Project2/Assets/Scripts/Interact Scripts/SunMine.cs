using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SunMine : MonoBehaviour
{
    // sound variables
    public bool inEarshot = false;
    public float baseSoundSensitivity;
    [SerializeField] private float soundSensitivity;
    private PlayerSoundRadius pSR;
    private GameObject player;

    // get objects
    void Start() {
        player = GameObject.Find("Player");
        pSR = player.GetComponent<PlayerSoundRadius>();
    }

    // Update is called once per frame
    void Update()
    {
        // sound sensitivity decreases as the player gets close to the mine
        if (inEarshot) {
            soundSensitivity = baseSoundSensitivity + Vector3.Distance(transform.position, player.transform.position);
        }
        else {
            soundSensitivity = baseSoundSensitivity;
        }

        // if the player is within earshot and they make too much noise
        if (inEarshot && pSR.soundValue >= soundSensitivity) {
            Mine m = transform.parent.GetComponent<Mine>();
            // player loses
            GameObject.Find("Player").GetComponent<PlayerDead>().playerFail_wrapper();
            m.destroyMine();
        }
    }

    // inearshot calcs
    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player") {
            inEarshot = true;
        }
    }
    void OnTriggerExit(Collider col) {
        if (col.gameObject.tag == "Player") {
            inEarshot = false;
        }
    }
}
