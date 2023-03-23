using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneralAudio : MonoBehaviour
{
    // FMOD Variables \\
    [Header("FMOD Banks")]
    [FMODUnity.BankRef]
    public string mapGeneralBank;

    [Header("Studio Event Emitters")]
    public FMODUnity.StudioEventEmitter leftDoorEmitter;
    public FMODUnity.StudioEventEmitter rightDoorEmitter;
    public FMODUnity.StudioEventEmitter pedestalEmitter;

    [Header("Event References")]
    public FMODUnity.EventReference batteryEventReference;

    [Header("Event Instances")]
    private FMOD.Studio.EventInstance batteryEventInstance;

    // Unity Variables \\
    [Header("Scripts")]
    private FMODManager FMODManager;

    // Start is called before the first frame update
    void Start()
    {
        FMODManager = GameObject.Find("FMOD Manager").GetComponent<FMODManager>();
    }
    
    // Play the open doors event for both doors and unload the bank once finished\\
    public IEnumerator playOpenDoorsEvent() {
        leftDoorEmitter.Play();
        rightDoorEmitter.Play();
        while (rightDoorEmitter.IsPlaying()) { yield return null; }
        FMODManager.UnloadBank(mapGeneralBank);
    }

    // Load the bank and play the pedestal event \\
    public void playPedestalEvent() {
        FMODManager.LoadBank(mapGeneralBank);
        pedestalEmitter.Play();
    }
}