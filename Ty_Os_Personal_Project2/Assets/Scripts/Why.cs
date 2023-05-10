using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Why : MonoBehaviour
{
    [Header("FMOD Studio Event Emitters")]
    public FMODUnity.StudioEventEmitter explosionEmitter;
    [Header("Particle Systems")]
    public ParticleSystem topParticles;
    public ParticleSystem bottomParticles;
    [Header("Materials")]
    public Material[] mats;
    [Header("ExitPedistalCharge")]
    public ExitPedestalCharge ePC;

    void Awake()
    {
        // Cronge
        for (int i = 0; i < 5; i++) {
            mats[i] = GameObject.Find("Battery.00" + (i + 1)).GetComponent<Renderer>().material;
        }

        foreach (Material mat in mats) {
            mat.SetColor("_EmissionColor", new Color(0, 191, 71) * 0f);
        }
        explosionEmitter = gameObject.transform.GetChild(1).gameObject.GetComponent<FMODUnity.StudioEventEmitter>();
    }
    // Play particles and update intuitive charge indicator
    void Angy()
    {
        // Play particle effects
        topParticles.Play();
        bottomParticles.Play();
        explosionEmitter.Play();
        // Update charge indicator based on charge
        float totalCharge = ePC.charge;
        float step = 0.015625f;
        if (totalCharge <= 10) {
            float batteryDiff = 10-totalCharge;
            float netCharge = step/(2*batteryDiff);
            Debug.Log(netCharge);
            if (netCharge == Mathf.Infinity) netCharge = step;
            mats[0].SetColor("_EmissionColor", new Color(0, 191, 71) * netCharge);
        }
        else if (totalCharge <= 20) {
            mats[0].SetColor("_EmissionColor", new Color(0, 191, 71) * step);
            float batteryDiff = 20-totalCharge;
            float netCharge = step/(2*batteryDiff);
            if (netCharge == Mathf.Infinity) netCharge = step;
            mats[1].SetColor("_EmissionColor", new Color(0, 191, 71) * netCharge);
        }
        else if (totalCharge <= 30) {
            mats[1].SetColor("_EmissionColor", new Color(0, 191, 71) * step);
            float batteryDiff = 30-totalCharge;
            float netCharge = step/(2*batteryDiff);
            if (netCharge == Mathf.Infinity) netCharge = step;
            mats[2].SetColor("_EmissionColor", new Color(0, 191, 71) * netCharge);
        }
        else if (totalCharge <= 40) {
            mats[2].SetColor("_EmissionColor", new Color(0, 191, 71) * step);
            float batteryDiff = 40-totalCharge;
            float netCharge = step/(2*batteryDiff);
            if (netCharge == Mathf.Infinity) netCharge = step;
            mats[3].SetColor("_EmissionColor", new Color(0, 191, 71) * netCharge);
        }
        else {
            mats[3].SetColor("_EmissionColor", new Color(0, 191, 71) * step);
            float batteryDiff = 50-totalCharge;
            float netCharge = step/(2*batteryDiff);
            if (netCharge == Mathf.Infinity) netCharge = step;
            mats[4].SetColor("_EmissionColor", new Color(0, 191, 71) * netCharge);
            if (ePC.charge >= ePC.chargeMax) {
                StartCoroutine(ePC.openDoors());
            }
        }
    }
}
