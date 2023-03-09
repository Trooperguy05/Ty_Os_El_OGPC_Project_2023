using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryLight : MonoBehaviour
{
    private Material mat;
    private PlayerInteract pI;
    private float glowIntensity = 2f;
    [SerializeField] private float batteryCharge = 10f;

    void Awake() {
        mat = GetComponent<Renderer>().material;
        pI = GameObject.Find("Player").GetComponent<PlayerInteract>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E) && pI.holdingBattery) {
            updateCharge(-1f);
        }
    }

    private void updateCharge(float n) {
        batteryCharge += n;
        glowIntensity += n;
        mat.SetColor("_EmissionColor", new Color(0, 191, 71) * glowIntensity);
        if (batteryCharge <= 0) {
            pI.holdingBattery = false;
            Destroy(gameObject);
        }
    }
}
