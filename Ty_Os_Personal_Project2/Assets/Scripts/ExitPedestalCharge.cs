using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPedestalCharge : MonoBehaviour
{
    public float charge;
    public bool chargedPedestal;
    private GameObject exitDoors;
    private PlayerInteract pI;

    void Start() {
        pI = GameObject.Find("Player").GetComponent<PlayerInteract>();
        exitDoors = GameObject.Find("Exit Doors");
    }

    // checking if the player charged the pedestal
    void Update() {
        if (charge >= 100f && !chargedPedestal) {
            chargedPedestal = true;
            Debug.LogError("Fully Charged Pedestal!");
        }
    }
}
