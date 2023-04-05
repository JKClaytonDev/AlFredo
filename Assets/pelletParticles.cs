using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pelletParticles : MonoBehaviour
{
    public int RedParticles;
    public int BlueParticles;
    public int sausageParticles;
    public ParticleSystem red;
    public ParticleSystem blue;
    public ParticleSystem sausage;


    // Update is called once per frame
    void Update()
    {
        red.maxParticles = RedParticles;
        blue.maxParticles = BlueParticles;
        sausage.maxParticles = sausageParticles;
    }
}
