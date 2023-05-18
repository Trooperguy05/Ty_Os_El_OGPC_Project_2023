using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMine : MonoBehaviour
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

        // if the player is within earshot and makes too much noise
        if (inEarshot && pSR.soundValue >= soundSensitivity) {
            // increase the monster suspicion
            Mine m = transform.parent.GetComponent<Mine>();
            GameObject.Find("Monster").GetComponent<MonsterSuspicion>().updateSuspicion_wrapper(20);
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
