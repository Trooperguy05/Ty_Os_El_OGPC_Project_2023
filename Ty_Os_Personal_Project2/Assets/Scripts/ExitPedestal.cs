using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPedestal : MonoBehaviour
{
    public int charge;
    private GameObject exitDoors;
    private PlayerInteract pI;

    void Start() {
        pI = GameObject.Find("Player").GetComponent<PlayerInteract>();
        exitDoors = GameObject.Find("Exit Doors");
    }
}
