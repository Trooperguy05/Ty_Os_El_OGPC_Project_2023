using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPedestal : Interactable
{
    protected override void Interact() {
        // check for battery
        GameObject grip = GameObject.Find("Object Grip");
        if (grip.transform.childCount == 0) return;
        GameObject battery = grip.transform.GetChild(0).gameObject;
        BatteryLight bL = battery.GetComponent<BatteryLight>();

        // add battery charge to pedestal
        StartCoroutine(GetComponent<ExitPedestalCharge>().updateCompletionBar(bL.batteryCharge));
        //GetComponent<ExitPedestalCharge>().buttonPress();
        bL.destroyBattery();
        GetComponent<ExitPedestalCharge>().insertBattery();
    }
}