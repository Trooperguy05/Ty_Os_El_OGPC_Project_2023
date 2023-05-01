using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Why : MonoBehaviour
{
    [Header("Particle Systems")]
    public ParticleSystem topParticles;
    public ParticleSystem bottomParticles;
    [Header("Materials")]
    public Material[] mats;
    [Header("ExitPedistalCharge")]
    public ExitPedestalCharge ePC;

    void Awake()
    {
        mats[0] = GameObject.Find("Battery.001").GetComponent<Renderer>().material;
        mats[1] = GameObject.Find("Battery.002").GetComponent<Renderer>().material;
        mats[2] = GameObject.Find("Battery.003").GetComponent<Renderer>().material;
        mats[3] = GameObject.Find("Battery.004").GetComponent<Renderer>().material;
        mats[4] = GameObject.Find("Battery.005").GetComponent<Renderer>().material;
        foreach (Material mat in mats) {
            mat.SetColor("_EmissionColor", new Color(0, 191, 71) * 0f);
        }
    }
    // Play particles and update intuitive charge indicator
    void Angy()
    {
        topParticles.Play();
        bottomParticles.Play();
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
            if (netCharge == 0.0f) netCharge = step;
            mats[1].SetColor("_EmissionColor", new Color(0, 191, 71) * netCharge);
        }
        else if (totalCharge <= 30) {
            mats[1].SetColor("_EmissionColor", new Color(0, 191, 71) * step);
            float batteryDiff = 30-totalCharge;
            float netCharge = step/(2*batteryDiff);
            if (netCharge == 0.0f) netCharge = step;
            mats[2].SetColor("_EmissionColor", new Color(0, 191, 71) * netCharge);
        }
        else if (totalCharge <= 40) {
            mats[2].SetColor("_EmissionColor", new Color(0, 191, 71) * step);
            float batteryDiff = 40-totalCharge;
            float netCharge = step/(2*batteryDiff);
            if (netCharge == 0.0f) netCharge = step;
            mats[3].SetColor("_EmissionColor", new Color(0, 191, 71) * netCharge);
        }
        else {
            mats[3].SetColor("_EmissionColor", new Color(0, 191, 71) * step);
            float batteryDiff = 50-totalCharge;
            float netCharge = step/(2*batteryDiff);
            if (netCharge == 0.0f) netCharge = step;
            mats[4].SetColor("_EmissionColor", new Color(0, 191, 71) * netCharge);
        }
        if (ePC.charge >= ePC.chargeMax) {
            StartCoroutine(ePC.openDoors());
        }
    }
}
