using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPedestal : Interactable
{
    protected override void Interact() {
        Debug.LogError("pain");

        // check for battery
        GameObject battery = GameObject.Find("Battery");
        if (battery == null) return;

        // add battery charge to pedestal
        GetComponent<ExitPedestalCharge>().charge += (int) battery.GetComponent<BatteryLight>().batteryCharge;
    }
}