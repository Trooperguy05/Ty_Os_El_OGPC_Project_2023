using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSuspicion : MonoBehaviour
{
    [Header("Suspicion")]
    public int suspicion = 0;
    public int suspicionMax;
    public GameObject shockwave;

    [Header("Mines")]
    public GameObject[] mines;
    public GameObject[] mineExplosion;

    [Header("Mine Spawning")]
    public List<string> tagList = new List<string>();
    public Vector3 mineGridSize;
    public LayerMask groundLayer;
    public float minimumRange;
    public float interval;
    private float timer;
    private Transform player;

    [Header("Suspicon Bar")]
    public Slider sSlider;

    [Header("Scripts")]
    private MonsterMovementNavmesh monsterMovement;
    private MonsterSoundDetection monsterSoundDetection;

    [Header("Player Flashlight")]
    public GameObject flashlight;
    public bool sawFlashlight;

    [Header("LayerMask")]
    public LayerMask raycastLayerMask;

    void Start() {
        player = GameObject.Find("Player").GetComponent<Transform>();
        monsterMovement = GetComponent<MonsterMovementNavmesh>();
        monsterSoundDetection = GameObject.Find("Sound Detection").GetComponent<MonsterSoundDetection>();
        setSuspicionBar();
    }

    // when suspicion is high enough, spawn a mine
    void Update() {
        timer += Time.deltaTime;

        // Check line of sight \\
        Vector3 dir = player.transform.position - transform.position;
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), dir, new Color(255, 165, 0));
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), dir, out hit, Vector3.Distance(player.transform.position, transform.position), raycastLayerMask);
        // if line of sight & player has flashlight on {investigate} \\
        if (hit.collider == null && flashlight.activeSelf && monsterMovement.stateMachine.currentState.name != "InvestigateState" && !sawFlashlight) {
            sawFlashlight = true;
            monsterSoundDetection.pointOfSound = player.transform.position;
            monsterMovement.monsterInvestigate_wrapper();
        }
        // if suspicion reaches the max, fail the player
        if (suspicion >= suspicionMax) {
            GameObject.Find("Player").GetComponent<PlayerDead>().playerFail_wrapper();
            Instantiate(shockwave, transform.position, Quaternion.identity);
            suspicion -= suspicionMax;
        }

        // spawn a mine every interval
        if (timer >= interval) {
            timer -= interval;
            spawnMine();
        }
    }

    // method that spawns a random mine
    public void spawnMine() {
        // random mine
        int ran = Random.Range(0, mines.Length);

        // random position
        Vector3 ranPos = new Vector3(0, 0, 0);
        do {
            ranPos = mineLocation();
        } while (ranPos == new Vector3(0, 0, 0));

        // spawn the mine
        Instantiate(mines[ran], ranPos, Quaternion.identity);
        Instantiate(mineExplosion[ran], ranPos, Quaternion.identity);
    }

    // method that checks for a valid mine location
    private Vector3 mineLocation() {
        // random location
        float ranX = Random.Range(-mineGridSize.x-minimumRange, mineGridSize.x+minimumRange) + player.position.x;
        float ranZ = Random.Range(-mineGridSize.z-minimumRange, mineGridSize.z+minimumRange) + player.position.z;
        
        // check if ground is underneath
        bool isGround = Physics.Raycast(new Vector3(ranX, 2f, ranZ), Vector3.down, 10f, groundLayer);
        if (!isGround) return new Vector3(0, 0, 0);

        // check for space
        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(ranX, 2f, ranZ), 5f);

        // check space
        foreach(Collider hit in hitColliders) {
            // no space
            if (tagList.Contains(hit.gameObject.tag)) {
                return new Vector3(0, 0, 0);
            }
        }

        return new Vector3(ranX, 2f, ranZ);
    }

    // method that sets the slider for the suspicion bar
    public void setSuspicionBar() {
        sSlider.maxValue = suspicionMax;
        sSlider.value = suspicion;
    }

    // method that updates the slider and the suspicion value
    public IEnumerator updateSuspicion(int value) {
        suspicion += value;
        for (int i = 0; i < value; i++) {
            sSlider.value++;
            yield return new WaitForSeconds(0.01f);
        }
    }
    // method that acts as the wrapper for the method above
    public void updateSuspicion_wrapper(int val) {
        StartCoroutine(updateSuspicion(val));
    }
}
