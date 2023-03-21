using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPedestal : Interactable
{
    protected override void Interact() {
        // check for battery
        GameObject battery = GameObject.Find("Battery");
        if (battery == null) return;
        BatteryLight bL = battery.GetComponent<BatteryLight>();

        // add battery charge to pedestal
        GetComponent<ExitPedestalCharge>().charge += bL.batteryCharge;
        GetComponent<ExitPedestalCharge>().buttonPress();
        bL.destroyBattery();
    }
}