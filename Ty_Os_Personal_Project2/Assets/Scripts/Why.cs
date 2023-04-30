using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Why : MonoBehaviour
{
    public ParticleSystem topParticles;
    public ParticleSystem bottomParticles;  

    // Update is called once per frame
    void Angy()
    {
        topParticles.Play();
        bottomParticles.Play();
    }
}
