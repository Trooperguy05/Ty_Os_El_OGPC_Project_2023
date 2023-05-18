using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    // how long the object should exist
    public float lifetime;
    
    // destroy the object after a certain amount of time
    void Start() {
        Destroy(gameObject, lifetime);
    }
}
