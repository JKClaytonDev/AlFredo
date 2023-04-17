using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deadPlayerManager : MonoBehaviour
{
    public playerMovement p1;
    public playerMovement p2;
    bool lastp1DeadCheck;
    bool lastp2DeadCheck;
    float p1DeadTime;
    float p2DeadTime;
    public AudioClip dieSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (p1.movingTarget)
            p1.dead = false;
        if (p2.movingTarget)
            p2.dead = false;
        p1.gameObject.SetActive(Time.realtimeSinceStartup > p1DeadTime);
        p2.gameObject.SetActive(Time.realtimeSinceStartup > p2DeadTime);
        if (p1.dead && !lastp1DeadCheck)
        {
            p1DeadTime = Time.realtimeSinceStartup + 5;
            GetComponent<AudioSource>().PlayOneShot(dieSound);
        }
        if (p2.dead && !lastp2DeadCheck)
        {
            p2DeadTime = Time.realtimeSinceStartup + 5;
            GetComponent<AudioSource>().PlayOneShot(dieSound);
        }
        if (p1.dead && Time.realtimeSinceStartup > p1DeadTime)
        {
            p1.dead = false;
            FindObjectOfType<playerStatusManager>().PlayAnimation("BackAlive", 0);
        }
        if (p2.dead && Time.realtimeSinceStartup > p2DeadTime)
        {
            p2.dead = false;
            FindObjectOfType<playerStatusManager>().PlayAnimation("BackAlive", 1);
        }
        lastp1DeadCheck = p1.dead;
        lastp2DeadCheck = p2.dead;
    }
}
