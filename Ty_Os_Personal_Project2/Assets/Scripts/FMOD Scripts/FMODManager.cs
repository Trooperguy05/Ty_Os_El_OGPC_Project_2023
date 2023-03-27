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
    private float masterVolume;
    private float playerVolume;
    private float monsterVolume;
    private float ambienceVolume;

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
        // Get FMODSettings script \\
        FMODSettings FMODSettings = GameObject.Find("FMOD Settings").GetComponent<FMODSettings>();
        // Set all current volumes \\
        masterVolume = FMODSettings.masterVolume;
        playerVolume = FMODSettings.playerVolume;
        monsterVolume = FMODSettings.monsterVolume;
        ambienceVolume = FMODSettings.ambienceVolume;
        // Set all volume slider values \\
        masterVolumeSlider.value = masterVolume;
        playerVolumeSlider.value = playerVolume;
        monsterVolumeSlider.value = monsterVolume;
        ambienceVolumeSlider.value = ambienceVolume;
    }

    // Toggles the audio settings menu \\
    public void toggleAudioSettings() {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        audioSettings.SetActive(!audioSettings.activeSelf);
    }

    // Reset all volume values to base values \\
    public void resetVolumeSettings() {
        // Set base values \\
        masterVolume = 80;
        playerVolume = 80;
        monsterVolume = 80;
        ambienceVolume = 80;
        // Set volume slider values \\
        masterVolumeSlider.value = masterVolume;
        playerVolumeSlider.value = playerVolume;
        monsterVolumeSlider.value = monsterVolume;
        ambienceVolumeSlider.value = ambienceVolume;
        // Set FMOD parameter values \\
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Global Volume", masterVolume);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Player Volume", playerVolume);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Monster Volume", monsterVolume);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Ambience Volume", ambienceVolume);
        
    }

    // Changes the master bus fader \\
    public void changeMasterVolume() {
        masterVolume = masterVolumeSlider.value;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Global Volume", masterVolume);
    }

    // Changes the player bus fader \\
    public void changePlayerVolume() {
        playerVolume = playerVolumeSlider.value;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Player Volume", playerVolume);
    }

    // Changes the monster bus fader \\
    public void changeMonsterVolume() {
        monsterVolume = monsterVolumeSlider.value;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Monster Volume", monsterVolume);
    }

    // Changes the ambience bus fader \\
    public void changeAmbienceVolume() {
        ambienceVolume = ambienceVolumeSlider.value;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Ambience Volume", ambienceVolume);
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
