using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class FMODManager : MonoBehaviour
{
    // FMOD Variables \\
    [Header("FMOD Bank References")]
    [FMODUnity.BankRef]
    public List<string> banks = new List<string>();

    [Header("FMOD Event Instances")]
    private FMOD.Studio.EventInstance volumeTest;
    private FMODUnity.StudioEventEmitter menuMusic;

    [ Header("FMOD Event References")]
    public FMODUnity.EventReference masterVolumeTest;
    public FMODUnity.EventReference playerVolumeTest;
    public FMODUnity.EventReference monsterVolumeTest;
    public FMODUnity.EventReference ambienceVolumeTest;

    // Unity Variables \\
    [Header("GameObjects")]
    public GameObject audioSettings;
    public GameObject pauseMenu;

    [Header("FMODSettings")]
    private FMODSettings FMODSettings;

    [Header("Sliders")]
    public Slider masterVolumeSlider;
    public Slider playerVolumeSlider;
    public Slider monsterVolumeSlider;
    public Slider ambienceVolumeSlider;
    public Slider musicVolumeSlider;

    [Header("Floats")]
    private float masterVolume;
    private float playerVolume;
    private float monsterVolume;
    private float ambienceVolume;
    private float musicVolume;


    // Awake is called before Start 
    void Awake()
    {
        // Load all required FMOD banks
        StartCoroutine(LoadBank(banks));
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Get FMODSettings script \\
        FMODSettings = GameObject.Find("FMOD Settings").GetComponent<FMODSettings>();
        // Get FMOD Studio Event Emitter
        menuMusic = GameObject.Find("Main Camera").GetComponent<FMODUnity.StudioEventEmitter>();
        // Set all current volumes \\
        masterVolume = FMODSettings.masterVolume;
        playerVolume = FMODSettings.playerVolume;
        monsterVolume = FMODSettings.monsterVolume;
        ambienceVolume = FMODSettings.ambienceVolume;
        musicVolume = FMODSettings.musicVolume;
        // Set all volume slider values \\
        masterVolumeSlider.value = masterVolume;
        playerVolumeSlider.value = playerVolume;
        monsterVolumeSlider.value = monsterVolume;
        ambienceVolumeSlider.value = ambienceVolume;
        musicVolumeSlider.value = musicVolume;
        // Play the main menu music (if in main menu scene)
        if (SceneManager.GetActiveScene().buildIndex == 0) {
            menuMusic.Play();
        }
    }

    // Sets the FMOD parameter to add the 3rd stereo track into the menu music \\
    public void alterMusic(bool buttonHighlighted) {
        if (buttonHighlighted) {
            menuMusic.SetParameter("Highlighted Button", 1.0f);
        }
        else {
            menuMusic.SetParameter("Highlighted Button", 0.0f);
        }
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
        musicVolume = 80;
        FMODSettings.masterVolume = 80;
        FMODSettings.playerVolume = 80;
        FMODSettings.monsterVolume = 80;
        FMODSettings.ambienceVolume = 80;
        FMODSettings.musicVolume = 80;
        // Set volume slider values \\
        masterVolumeSlider.value = masterVolume;
        playerVolumeSlider.value = playerVolume;
        monsterVolumeSlider.value = monsterVolume;
        ambienceVolumeSlider.value = ambienceVolume;
        musicVolumeSlider.value = musicVolume;
        // Set FMOD parameter values \\
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Global Volume", masterVolume);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Player Volume", playerVolume);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Monster Volume", monsterVolume);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Ambience Volume", ambienceVolume);
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Music Volume", musicVolume);
        // Play test audio \\
        testAudio(masterVolumeTest);
        
    }

    // Changes the master bus fader \\
    public void changeMasterVolume() {
        masterVolume = masterVolumeSlider.value;
        FMODSettings.masterVolume = masterVolume;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Global Volume", masterVolume);
        testAudio(masterVolumeTest);
    }

    // Changes the player bus fader \\
    public void changePlayerVolume() {
        playerVolume = playerVolumeSlider.value;
        FMODSettings.playerVolume = playerVolume;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Player Volume", playerVolume);
        testAudio(playerVolumeTest);
    }

    // Changes the monster bus fader \\
    public void changeMonsterVolume() {
        monsterVolume = monsterVolumeSlider.value;
        FMODSettings.monsterVolume = monsterVolume;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Monster Volume", monsterVolume);
        testAudio(monsterVolumeTest);
    }

    // Changes the ambience bus fader \\
    public void changeAmbienceVolume() {
        ambienceVolume = ambienceVolumeSlider.value;
        FMODSettings.ambienceVolume = ambienceVolume;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Ambience Volume", ambienceVolume);
        testAudio(ambienceVolumeTest);
    }

    // Changes the music bus fader \\
    public IEnumerator changeMusicVolume() {
        while (true) {
            musicVolume = musicVolumeSlider.value;
            FMODSettings.musicVolume = musicVolume;
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Music Volume", musicVolume);
            yield return null;
        }
    }

    // Wrapper for the changeMusicVolume coroutine \\
    public void changeMusicVolume(bool stop) {
        if (stop) StopCoroutine(changeMusicVolume());
        else StartCoroutine(changeMusicVolume());
    }

    // Fade the music down while interacting with another slider \\
    public IEnumerator fadeMusic(bool fadeUp) {
        if (fadeUp) {
            for (float i = 0; i < musicVolume; i += 1.0f) {
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Music Volume", i);
                yield return null;
            }
        }
        else {
            for (float i = musicVolume; i > 0; i -= 1.0f) {
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Music Volume", i);
                yield return null;
            }
        }
    }

    // Wrapper for the slider to interact with \\
    public void fadeMusicWrapper(bool fadeUp) {
        if (fadeUp) {
            StartCoroutine(fadeMusic(true));
        }
        else {
            StartCoroutine(fadeMusic(false));
        }
    }

    // Create the test audio instance \\
    private void testAudio(FMODUnity.EventReference reference) {
            volumeTest = FMODUnity.RuntimeManager.CreateInstance(reference);
            volumeTest.start();
            volumeTest.release();
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
