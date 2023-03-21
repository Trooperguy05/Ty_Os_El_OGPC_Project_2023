using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPedestalCharge : MonoBehaviour
{
    public float charge;
    public bool chargedPedestal;
    private Animator exitDoorsAnim;
    private Animator pedestalPress;
    private PlayerInteract pI;

    // get stuff
    void Start() {
        pI = GameObject.Find("Player").GetComponent<PlayerInteract>();
        exitDoorsAnim = GameObject.Find("Exit Doors").GetComponent<Animator>();
        pedestalPress = transform.GetChild(0).GetComponent<Animator>();
    }

    // checking if the player charged the pedestal
    void Update() {
        if (charge >= 100f && !chargedPedestal) {
            chargedPedestal = true;
            StartCoroutine(openDoors());
            Debug.LogError("Fully Charged Pedestal!");
        }

        if (Input.GetKeyDown(KeyCode.Y)) {
            StartCoroutine(openDoors());
        }
    }

    // method that presses button when interacted with
    public void buttonPress() {
        pedestalPress.SetTrigger("press");
    }

    // method that opens the doors after a set duration
    public IEnumerator openDoors() {
        yield return new WaitForSeconds(1f);
        exitDoorsAnim.SetTrigger("openDoors");
    }
}
