using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FMODManager : MonoBehaviour
{
    // FMOD Variables \\
    [Header("FMOD Bank References")]
    [FMODUnity.BankRef]
    public string playerSFX;
    [FMODUnity.BankRef]
    public string enemySFX;

    // Unity Variables \\
    [Header("Sliders")]
    private Slider volumeSlider;

    // Start is called before the first frame update
    void Awake()
    {
        // Load the playerSFX FMOD bank on awake \\
        FMODUnity.RuntimeManager.LoadBank(playerSFX, true);
        // Load the enemySFX FMOD bank on awake \\
        FMODUnity.RuntimeManager.LoadBank(enemySFX, true);
    }

    // Changes the master bus volume \\
    public void changeVolume() {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Global Volume", volumeSlider.value);
    }

    // Loads a specified FMOD bank \\
    public IEnumerator LoadBank(string bank) {
        FMODUnity.RuntimeManager.LoadBank(bank);
        Debug.Log(bank + " is loading");
        yield return null;
    }
    
    // Overload coroutine to accept a list \\
    public IEnumerator LoadBank(List<string> banks) {
        foreach (string bank in banks){
            FMODUnity.RuntimeManager.LoadBank(bank);
            Debug.Log(bank + " is loading");
            yield return null;
        }
    }

    // Unloads a specified FMOD bank \\
    public IEnumerator UnloadBank(string bank) {
        FMODUnity.RuntimeManager.UnloadBank(bank);
        Debug.Log(bank + " is unloading");
        yield return null;
    }

    // Overload coroutine to accept a list \\
    public IEnumerator UnloadBank(List<string> banks) {
        foreach (string bank in banks) {
            FMODUnity.RuntimeManager.UnloadBank(bank);
            Debug.Log(bank + " is unloading");
            yield return null;
        }
    }

}
