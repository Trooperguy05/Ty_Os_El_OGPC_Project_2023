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
    [Header("GameObjects")]
    public GameObject audioSettings;
    public GameObject pauseMenu;

    [Header("Sliders")]
    public Slider masterVolumeSlider;
    public Slider playerVolumeSlider;
    public Slider monsterVolumeSlider;
    public Slider ambienceVolumeSlider;

    [Header("Floats")]
    private float currentMasterVolume = 80;
    private float currentPlayerVolume = 80;
    private float currentMonsterVolume = 80;
    private float currentAmbienceVolume = 80;

    // Awake is called before Start 
    void Awake()
    {
        // Load the playerSFX FMOD bank on awake \\
        FMODUnity.RuntimeManager.LoadBank(playerSFX, true);
        // Load the enemySFX FMOD bank on awake \\
        FMODUnity.RuntimeManager.LoadBank(enemySFX, true);
    }
    
    // Start is called before the first frame update
    void Start()
    {
         // Set all volume slider values \\
        masterVolumeSlider.value = currentMasterVolume;
        playerVolumeSlider.value = currentPlayerVolume;
        monsterVolumeSlider.value = currentMonsterVolume;
        ambienceVolumeSlider.value = currentAmbienceVolume;
    }

    // Toggles the audio settings menu \\
    public void toggleAudioSettings() {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        audioSettings.SetActive(!audioSettings.activeSelf);
    }

    // Changes the master bus fader \\
    public void changeMasterVolume() {
        currentMasterVolume = masterVolumeSlider.value;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Global Volume", currentMasterVolume);
    }

    // Changes the player bus fader \\
    public void changePlayerVolume() {
        currentPlayerVolume = playerVolumeSlider.value;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Player Volume", currentPlayerVolume);
    }

    // Changes the monster bus fader \\
    public void changeMonsterVolume() {
        currentMonsterVolume = monsterVolumeSlider.value;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Monster Volume", currentMonsterVolume);
    }

    // Changes the ambience bus fader \\
    public void changeAmbienceVolume() {
        currentAmbienceVolume = ambienceVolumeSlider.value;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Ambience Volume", currentAmbienceVolume);
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
