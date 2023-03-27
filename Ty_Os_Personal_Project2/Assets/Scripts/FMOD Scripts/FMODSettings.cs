using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODSettings : MonoBehaviour
{
    // Audio settings to transer between scenes \\
    [Header("Volumes")]
    public float masterVolume;
    public float playerVolume;
    public float monsterVolume;
    public float ambienceVolume;

    [Header("Master Bus")]
    private FMOD.Studio.Bus masterBus;

    // Wacky scene change stuff \\
    void OnEnable() { 
        SceneLoader.beforeSceneChanged += stopAllSounds;
    }
    void OnDisable() { 
        SceneLoader.beforeSceneChanged -= stopAllSounds;
    }

    // Get Stuff \\
    void Start()
    {
        masterBus = FMODUnity.RuntimeManager.GetBus("bus:/Master Bus");
    }

    // Stops all events on the master bus \\
    public void stopAllSounds() {
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
