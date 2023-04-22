using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPedestal : Interactable
{
    protected override void Interact() {
        BatteryGenerator bG = GameObject.Find("Battery Generator").GetComponent<BatteryGenerator>();
        MonsterSuspicion mS = GameObject.Find("Monster").GetComponent<MonsterSuspicion>();

        // spawn a battery at the cost of suspicion
        bG.spawnBattery(gameObject);
        StartCoroutine(mS.updateSuspicion(30));
    }
}
