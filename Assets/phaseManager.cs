using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class phaseManager : MonoBehaviour
{
    public AudioSource musicManager;
    public playerMovement p1;
    public playerMovement p2;
    public GameObject phase1Object;
    public GameObject p1MoveTarget1;
    public GameObject p2MoveTarget1;
    public GameObject phase2Object;
    public GameObject p1MoveTarget2;
    public GameObject p2MoveTarget2;
    public GameObject phase3Object;
    public GameObject p1MoveTarget3;
    public GameObject p2MoveTarget3;
    public GameObject phase4Object;
    public GameObject p1MoveTarget4;
    public GameObject p2MoveTarget4;
    public GameObject borders;
    int phase = 1;
    public Text timer;
    int lastPhase = 1;
    public float Timer;
    private void Start()
    {
        p1.SetTarget(p1MoveTarget4, true);
        p2.SetTarget(p2MoveTarget4, true);
        phase = Mathf.CeilToInt(Timer / 15);
        lastPhase = phase;
    }
    // Update is called once per frame
    void Update()
    {
        p1.GetComponent<Rigidbody>().isKinematic = !p1.movingTarget;
        p2.GetComponent<Rigidbody>().isKinematic = !p2.movingTarget;
        borders.SetActive(!p1.movingTarget && !p2.movingTarget);
        musicManager.volume = 0.1f;
        if (!p1.movingTarget && !p2.movingTarget)
        {
            Timer += Time.deltaTime;
            musicManager.volume = 0.5f;
        }
        phase = Mathf.CeilToInt(Timer / 15);
        if (lastPhase != phase)
        {
            if (lastPhase == 1)
            {
                p1.SetTarget(p1MoveTarget1, false);
                p2.SetTarget(p2MoveTarget1, false);
            }
            if (lastPhase == 2)
            {
                p1.SetTarget(p1MoveTarget2, true);
                p2.SetTarget(p2MoveTarget2, true);
            }
            if (lastPhase == 3)
            {
                p1.SetTarget(p1MoveTarget3, false);
                p2.SetTarget(p2MoveTarget3, false);
            }
            if (lastPhase == 4)
            {
                p1.SetTarget(p1MoveTarget4, true);
                p2.SetTarget(p2MoveTarget4, true);
            }
            lastPhase = phase;
        }
        if (p1.movingTarget || p2.movingTarget)
        {
            phase1Object.gameObject.SetActive(false);
            phase2Object.gameObject.SetActive(false);
            phase3Object.gameObject.SetActive(false);
            phase4Object.gameObject.SetActive(false);
            if (phase == 0)
            {
                phase4Object.gameObject.SetActive(false);
                phase1Object.gameObject.SetActive(Mathf.Cos(Time.realtimeSinceStartup * 10) > 0);
            }
            if (phase == 1)
            {
                phase1Object.gameObject.SetActive(Mathf.Sin(Time.realtimeSinceStartup * 10) > 0);
                phase2Object.gameObject.SetActive(Mathf.Cos(Time.realtimeSinceStartup * 10) > 0);
            }
            if (phase == 2)
            {
                phase2Object.gameObject.SetActive(Mathf.Sin(Time.realtimeSinceStartup * 10) > 0);
                phase3Object.gameObject.SetActive(Mathf.Cos(Time.realtimeSinceStartup * 10) > 0);
            }
            if (phase == 3)
            {
                phase3Object.gameObject.SetActive(Mathf.Sin(Time.realtimeSinceStartup * 10) > 0);
                phase4Object.gameObject.SetActive(Mathf.Cos(Time.realtimeSinceStartup * 10) > 0);
            }
            if (phase == 4)
            {
                phase4Object.gameObject.SetActive(Mathf.Sin(Time.realtimeSinceStartup * 10) > 0);
                phase1Object.gameObject.SetActive(Mathf.Cos(Time.realtimeSinceStartup * 10) > 0);
            }
            timer.text = "";
        }
        else
        {
            timer.text = "Time: " + (int)(((phase) * 15) - Timer);
            phase1Object.gameObject.SetActive(phase == 1);
            phase2Object.gameObject.SetActive(phase == 2);
            phase3Object.gameObject.SetActive(phase == 3);
            phase4Object.gameObject.SetActive(phase == 4);
        }
    }
}
