using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class phaseManager : MonoBehaviour
{
    public GameObject gameCanvas;
    public GameObject endCanvas;
    public Text player1Score;
    public Text player2Score;
    public Text winnerName;
    public AudioSource musicManager;
    public pelletParticles p;
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
    public GameObject bowl;
    public PlayerCamera c;
    int phase = 1;
    public Text timer;
    int lastPhase = 1;
    public float Timer;
    bool gameEnd;
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
        gameCanvas.SetActive(!gameEnd);
        endCanvas.SetActive(gameEnd);
        if (gameEnd)
        {
            player1Score.text = "Player 1 Score: " + p.score1;
            player2Score.text = "Player 2 Score: " + p.score2;
            if (p.score1 > p.score2)
                winnerName.text = "Player 1 Wins!";
            else if (p.score1 < p.score2)
                winnerName.text = "Player 2 Wins!";
            else
                winnerName.text = "Tie!";
            phase1Object.gameObject.SetActive(false);
            phase2Object.gameObject.SetActive(false);
            phase3Object.gameObject.SetActive(false);
            phase4Object.gameObject.SetActive(false);
            musicManager.volume = 0.1f;
            p1.gameObject.SetActive(false);
            p2.gameObject.SetActive(false);
            c.enabled = false;
            c.gameObject.transform.position = Vector3.MoveTowards(c.gameObject.transform.position, bowl.transform.position + Vector3.up * 3, Time.deltaTime * 100);
            return;
        }
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
                gameEnd = true;
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
