using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRedHerrings : MonoBehaviour
{
    // FMOD Variables \\
    [Header("Bank References")]
    [FMODUnity.BankRef]
    public string redHerringsBank;

    [Header("Event References")]
    public FMODUnity.EventReference scoobyDooLaugh;

    [Header("Event Instances")]
    private FMOD.Studio.EventInstance ambienceInstance;

    [Header("Funni ahh playback state")]
    private FMOD.Studio.PLAYBACK_STATE playbackState;

    // Unity Variables \\
    [Header("GameObjects")]
    private GameObject player;

    [Header("FMODManager")]
    private FMODManager FMODManager;

    [Header("Intergers")]
    private int randomizedEffectTest = 50;
    private int randomizeEffect;

    [Header("Bools")]
    public bool playRedHerrings;

    void Awake()
    {
        // Get Stuff \\
        FMODManager = GameObject.Find("FMOD Manager").GetComponent<FMODManager>();
        player = GameObject.Find("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(randomPlayTime());
    }

    // Update is called once per frame
    void Update()
    {
        // DEBUG \\
        // if (Input.GetKeyDown(KeyCode.P)) { StartCoroutine(PlayMapAmbience(scoobyDooLaugh)); }
        // if (Input.GetKeyDown(KeyCode.O)) { RandomPos(); }
    }

    private IEnumerator PlayMapAmbience(FMODUnity.EventReference reference) {
        Debug.Log("Playing Red Herring");
        while (true) {
        // Load the ambience bank to memory \\
        StartCoroutine(FMODManager.LoadBank(redHerringsBank));
        
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
        StartCoroutine(FMODManager.UnloadBank(redHerringsBank));
        yield return null;
        }
    }

    // Move the gameObject to a random position around the player \\
    private void RandomPos() {
        float randomAngle = Random.Range(0, 2f * Mathf.PI);
        gameObject.transform.position = player.transform.position + new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)) * 35;
    }

    // Randomize the play time of the red herrings \\
    private IEnumerator randomPlayTime() {
        while (playRedHerrings) {
            // Wait for 5 minutes \\
            yield return new WaitForSeconds(300);
            // Attempt to play effect \\
            randomizeEffect = Random.Range(0, 1000);
            if (randomizeEffect == randomizedEffectTest) {
                StartCoroutine(PlayMapAmbience(scoobyDooLaugh));
            }
        }
    }

    
}
