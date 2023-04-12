using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSFX : MonoBehaviour
{
    // FMOD Variables \\
    [Header("Event References")]
    public FMODUnity.EventReference mineEventRef;

    [Header("Event Instance")]
    private FMOD.Studio.EventInstance mineInstance;
    // Start is called before the first frame update
    void Start()
    {
        // Create mine instance and start ambience sound effect \\
        mineInstance = FMODUnity.RuntimeManager.CreateInstance(mineEventRef);
        mineInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        mineInstance.start();
    }

    public void explodeMine() {
        // Set FMOD parameter \\
        mineInstance.setParameterByName("MineState", 1);
    }
}
