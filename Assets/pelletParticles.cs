using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class pelletParticles : MonoBehaviour
{
    public int RedParticles1;
    public int BlueParticles1;
    public int sausageParticles1;
    public int RedParticles2;
    public int BlueParticles2;
    public int sausageParticles2;
    public ParticleSystem red;
    public ParticleSystem blue;
    public ParticleSystem sausage;
    public Text p1Score;
    public Text p2Score;
    public int score1;
    public int score2;

    // Update is called once per frame
    void Update()
    {
        score1 = (RedParticles1 + BlueParticles1) * (sausageParticles1+1);
        score2 = (RedParticles2 + BlueParticles2) * (sausageParticles2+1);
        red.maxParticles = RedParticles1+RedParticles2;
        blue.maxParticles = BlueParticles1+BlueParticles2;
        sausage.maxParticles = sausageParticles1+sausageParticles2;
        p1Score.text = "" + score1;
        p2Score.text = "" + score2;
    }
}
