using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRedHerrings : MonoBehaviour
{
    // FMOD Variables \\
    [Header("Bank References")]
    [FMODUnity.BankRef]
    public string ambienceBank;

    [Header("Event References")]
    public FMODUnity.EventReference scoobyDooLaugh;

    [Header("Event Instances")]
    private FMOD.Studio.EventInstance ambienceInstance;

    [Header("Funni ahh playback state")]
    private FMOD.Studio.PLAYBACK_STATE playbackState;

    // Unity Variables \\
    private GameObject player;
    private FMODManager FMODManager;

    // Start is called before the first frame update
    void Awake()
    {
        // Get Stuff \\
        FMODManager = GameObject.Find("FMOD Manager").GetComponent<FMODManager>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // DEBUG \\
        if (Input.GetKeyDown(KeyCode.P)) { StartCoroutine(PlayMapAmbience(scoobyDooLaugh)); }
        if (Input.GetKeyDown(KeyCode.O)) { RandomPos(); }
    }

    private IEnumerator PlayMapAmbience(FMODUnity.EventReference reference) {
        while (true) {
        // Load the ambience bank to memory \\
        StartCoroutine(FMODManager.LoadBank(ambienceBank));
        
        // Move the gameObject \\
        RandomPos();
        
        //  Wait until bank is loaded \\
        while (!FMODUnity.RuntimeManager.HaveAllBanksLoaded) { yield return null; }
        
        // Play ambience event \\
        ambienceInstance = FMODUnity.RuntimeManager.CreateInstance(reference);
        ambienceInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        ambienceInstance.start();
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(ambienceInstance, gameObject.transform);
        
        // Wait for event playback to finish \\
        ambienceInstance.getPlaybackState(out playbackState);
        while (playbackState != FMOD.Studio.PLAYBACK_STATE.STOPPED) {
            ambienceInstance.getPlaybackState(out playbackState);
            yield return null;
        }
        
        // Release event from memory \\
        ambienceInstance.release();
        
        // Unload the ambience bank from memory \\
        StartCoroutine(FMODManager.UnloadBank(ambienceBank));
        yield return null;
        }
    }

    // Move the gameObject to a random position around the player \\
    private void RandomPos() {
        float randomAngle = Random.Range(0, 2f * Mathf.PI);
        gameObject.transform.position = player.transform.position + new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)) * 35;
    }

    
}
