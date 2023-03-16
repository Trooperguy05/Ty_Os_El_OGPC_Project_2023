using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject spotLight;

    [Header("Scripts")]
    private ConsoleMenu consoleMenu;

    [Header("FMOD Stuff")]
    public FMODUnity.EventReference clickOnReference;
    public FMODUnity.EventReference clickOffReference;
    private FMOD.Studio.EventInstance clickOn;
    private FMOD.Studio.EventInstance clickOff;
    
    [Header("Lighting Stuff")]
    private Light light;
    private bool flashlightActive;



    // Start is called before the first frame update
    void Start()
    {
        // Get Stuff \\
        light = spotLight.GetComponent<Light>();
        consoleMenu = GameObject.Find("Console Manager").GetComponent<ConsoleMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle flashlight \\
        if (Input.GetKeyDown(KeyCode.T) && !flashlightActive && !consoleMenu.isConsoleActive) flashlightOn();
        else if (Input.GetKeyDown(KeyCode.T) && flashlightActive && !consoleMenu.isConsoleActive) flashlightOff();
    }


    public void flashlightOn() {
        // Stop clickOff FMOD event instance \\
        clickOff.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        
        // Create and play clickOn FMOD event instance \\
        clickOn = FMODUnity.RuntimeManager.CreateInstance(clickOnReference);
        clickOn.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        clickOn.start();
        clickOn.release();
        
        // Turn the spotLight GameObject on \\
        spotLight.SetActive(true);
        flashlightActive = true;
    }

    public void flashlightOff() {
        // Stop clickOn FMOD event instance \\
        clickOn.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        // Create and play clickOff FMOD event instance \\
        clickOff = FMODUnity.RuntimeManager.CreateInstance(clickOffReference);
        clickOff.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        clickOff.start();
        clickOff.release();
        
        // Turn the spotLight GameObject off \\
        spotLight.SetActive(false);
        flashlightActive = false;
    }
    
}
