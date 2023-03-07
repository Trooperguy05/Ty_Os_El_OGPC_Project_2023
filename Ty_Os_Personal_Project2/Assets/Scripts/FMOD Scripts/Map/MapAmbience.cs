using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAmbience : MonoBehaviour
{
    // FMOD Variables \\
    [Header("Event References")]
    public FMODUnity.EventReference mapAmbienceReference;

    [Header("Event Instances")]
    private FMOD.Studio.EventInstance mapAmbience;

    // Start is called before the first frame update
    void Start()
    {
        // Play the MapAmbience event \\
        mapAmbience = FMODUnity.RuntimeManager.CreateInstance(mapAmbienceReference);
        mapAmbience.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        mapAmbience.start();
    }
}
