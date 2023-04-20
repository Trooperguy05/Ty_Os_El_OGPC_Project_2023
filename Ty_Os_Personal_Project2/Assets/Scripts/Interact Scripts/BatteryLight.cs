using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryLight : MonoBehaviour
{
    private Material mat;
    private PlayerInteract pI;
    private BatteryGenerator bG;
    public GameObject explosion;
    public float batteryCharge = 10f;

    void Awake() {
        mat = GetComponent<Renderer>().material;
        pI = GameObject.Find("Player").GetComponent<PlayerInteract>();
        bG = GameObject.Find("Battery Generator").GetComponent<BatteryGenerator>();
    }

    /*
    void Update() {
        if (Input.GetKeyDown(KeyCode.E) && pI.holdingBattery) {
            updateCharge(-1f);
        }
    }
    */

    // updates the emission light of the battery based on the charge
    public void updateCharge(float n) {
        batteryCharge += n;

        float batteryDiff = 10-batteryCharge;
        float step = 0.015625f;
        float netCharge = step/(2*batteryDiff);

        mat.SetColor("_EmissionColor", new Color(0, 191, 71) * netCharge);
        if (batteryCharge <= 0) {
            destroyBattery();
        }
    }

    // method that destroys the battery with **lights***
    public void destroyBattery() {
        pI.holdingBattery = false;
        bG.spawnedBattery= false;
        GameObject foo = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
