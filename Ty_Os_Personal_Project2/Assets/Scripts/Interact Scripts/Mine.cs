using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Interactable
{
    public GameObject explosion;

    protected override void Interact() {
        // find if the player has a battery in their hand
        GameObject foo = GameObject.Find("Object Grip");
        if (foo.transform.childCount == 0) {
            return;
        }
        // destroy the battery & mine after depleting the battery charge
        BatteryLight bL = foo.transform.GetChild(0).GetComponent<BatteryLight>();
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
