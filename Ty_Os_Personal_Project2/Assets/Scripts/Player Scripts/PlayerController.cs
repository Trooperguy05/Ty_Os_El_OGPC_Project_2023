using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController cC;
    public bool isRunning;
    public float footstepSpeed;

    // Start is called before the first frame update
    void Start()
    {
        cC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
