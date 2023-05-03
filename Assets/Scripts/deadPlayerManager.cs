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
    GameVersionManager v;
    // Start is called before the first frame update
    void Start()
    {
        v = FindObjectOfType<GameVersionManager>();
    }
    public GameObject p1DeadText;
    public GameObject p2DeadText;
    public GameObject p1DeadBar;
    public GameObject p2DeadBar;
    public int p1PressIndex = -1;
    public int p2PressIndex = -1;
    float PressTime;
    float p1Presses;
    float p2Presses;
    // Update is called once per frame
    void Update()
    {
        p1DeadText.gameObject.SetActive(p1.dead);
        p2DeadText.gameObject.SetActive(p2.dead);
        p1DeadBar.transform.localScale = new Vector3(((float)(p1Presses+1)) / 11, 1, 1);
        p2DeadBar.transform.localScale = new Vector3(((float)(p2Presses+1)) / 11, 1, 1);
        if (p1.movingTarget && p1.dead)
        {
            p1.dead = false;
            {
                GameObject aliveSpawn = Instantiate(p1.lifeSpawn);
                aliveSpawn.transform.parent = null;
                aliveSpawn.transform.position = p1.transform.position;
            }
        }
        if (p2.movingTarget && p2.dead)
        {
            p2.dead = false;
            {
                GameObject aliveSpawn = Instantiate(p2.lifeSpawn);
                aliveSpawn.transform.parent = null;
                aliveSpawn.transform.position = p2.transform.position;
            }
        }
        p1.gameObject.SetActive(!p1.dead);
        p2.gameObject.SetActive(!p2.dead);
        if (p1.dead && !lastp1DeadCheck)
        {
            p1Presses = 0;
            p1DeadTime = Time.realtimeSinceStartup + 5;
            GetComponent<AudioSource>().PlayOneShot(dieSound);
        }
        if (p2.dead && !lastp2DeadCheck)
        {
            p2Presses = 0;
            p2DeadTime = Time.realtimeSinceStartup + 5;
            GetComponent<AudioSource>().PlayOneShot(dieSound);
        }
        if (p1.dead)
            p1Presses += Time.deltaTime * 5;
        if (p2.dead)
            p2Presses += Time.deltaTime * 5;
        if (Time.realtimeSinceStartup > PressTime)
        {
            bool pressed = false;
            if (v.p1LeftTap && p1PressIndex == -1)
            {
                p1PressIndex = 1;
                p1Presses++;
                pressed = true;
            }
            else if (v.p1RightTap && p1PressIndex != -1)
            {
                p1PressIndex = -1;
                p1Presses++;
                pressed = true;
            }
            if (v.p2RightTap && p2PressIndex == -1)
            {
                p2PressIndex = 1;
                p2Presses++;
                pressed = true;
            }
            else if (v.p2LeftTap && p2PressIndex != -1)
            {
                p2PressIndex = -1;
                p2Presses++;
                pressed = true;
            }
            if (pressed)
            {
                PressTime = Time.realtimeSinceStartup + 0.1f;
            }
        }
        if (p1.dead && p1Presses > 10)
        {
            p1.dead = false;
            {
                GameObject aliveSpawn = Instantiate(p1.lifeSpawn);
                aliveSpawn.transform.parent = null;
                aliveSpawn.transform.position = p1.transform.position;
            }
            FindObjectOfType<playerStatusManager>().PlayAnimation("BackAlive", 0);
        }
        if (p2.dead && p2Presses > 10)
        {
            p2.dead = false;
            {
                GameObject aliveSpawn = Instantiate(p2.lifeSpawn);
                aliveSpawn.transform.parent = null;
                aliveSpawn.transform.position = p2.transform.position;
            }
            FindObjectOfType<playerStatusManager>().PlayAnimation("BackAlive", 1);
        }
        lastp1DeadCheck = p1.dead;
        lastp2DeadCheck = p2.dead;
    }
}
