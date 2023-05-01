using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitPedestalCharge : MonoBehaviour
{
    public float charge;
    public int batteriesInserted;
    public float chargeMax;
    public bool chargedPedestal;
    public Slider cSlider;
    public Material mat;
    public GameObject battery;
    private Animator exitDoorsAnim;
    private Animator pedestalPress;
    private Animator batteryHolderAnim;
    private PlayerInteract pI;
    private MapGeneralAudio mGA;

    // get stuff
    void Start() {
        pI = GameObject.Find("Player").GetComponent<PlayerInteract>();
        mGA = GameObject.Find("FMOD Manager").GetComponent<MapGeneralAudio>();
        exitDoorsAnim = GameObject.Find("Exit Doors").GetComponent<Animator>();
        pedestalPress = transform.GetChild(0).GetComponent<Animator>();
        batteryHolderAnim = GameObject.Find("BatteryHolder").GetComponent<Animator>();
        mat = battery.GetComponent<MeshRenderer>().material;

        setCompletionBar();
    }

    // checking if the player charged the pedestal
    void Update() {
        if (charge >= chargeMax && !chargedPedestal) {
            chargedPedestal = true;
            //StartCoroutine(openDoors());
            Debug.LogError("Fully Charged Pedestal!");
        }

        /*
        if (Input.GetKeyDown(KeyCode.Y)) {
            StartCoroutine(openDoors());
        }
        */
    }

    /*
    // method that presses button when interacted with
    public void buttonPress() {
        StartCoroutine(mGA.playPedestalEvent());
        pedestalPress.SetTrigger("press");
    */

    // method that opens the doors after a set duration
    public IEnumerator openDoors() {
        yield return new WaitForSeconds(1f);
        exitDoorsAnim.SetTrigger("openDoors");
    }

    // method that sets up the completion bar at the start of the game
    private void setCompletionBar() {
        cSlider.maxValue = chargeMax;
        cSlider.value = charge;
    }

    // method that updates the completion bar 
    public IEnumerator updateCompletionBar(float value) {
        charge += value;
        for (int i = 0; i < value; i++) {
            cSlider.value++;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void insertBattery(Color color) {
        mat.SetColor("_EmissionColor", color);
        batteryHolderAnim.SetTrigger("InsertBattery");
    }

}
