using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Interactable
{
    protected override void Interact() {
        //Debug.Log("Interacted with " + gameObject.name);

        // destroy the battery after depleting the battery charge
        BatteryLight bL = GameObject.Find("Battery").GetComponent<BatteryLight>();
        SunMine sM = transform.GetChild(0).GetComponent<SunMine>();
        if (bL.batteryCharge >= 2) {
            bL.updateCharge(-2f);
            sM.destroyMine();
        }
    }
}
