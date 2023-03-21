using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPedestalCharge : MonoBehaviour
{
    public float charge;
    public bool chargedPedestal;
    private Animator exitDoorsAnim;
    private PlayerInteract pI;

    // get stuff
    void Start() {
        pI = GameObject.Find("Player").GetComponent<PlayerInteract>();
        exitDoorsAnim = GameObject.Find("Exit Doors").GetComponent<Animator>();
    }

    // checking if the player charged the pedestal
    void Update() {
        if (charge >= 100f && !chargedPedestal) {
            chargedPedestal = true;
            exitDoorsAnim.SetTrigger("openDoors");
            Debug.LogError("Fully Charged Pedestal!");
        }

        if (Input.GetKeyDown(KeyCode.Y)) {
            exitDoorsAnim.SetTrigger("openDoors");
        }
    }
}
