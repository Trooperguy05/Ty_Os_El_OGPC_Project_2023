using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Interactable
{
    public GameObject explosion;

    protected override void Interact() {
        //Debug.Log("Interacted with " + gameObject.name);

        // destroy the battery after depleting the battery charge
        BatteryLight bL = GameObject.Find("Battery").GetComponent<BatteryLight>();
        if (bL.batteryCharge >= 2) {
            bL.updateCharge(-2f);
            destroyMine();
        }
    }

    // method that destroys the mine with particles and audio :)
    public void destroyMine() {
        MineSFX MineSFX = GetComponent<MineSFX>();
        MineSFX.explodeMine();
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
