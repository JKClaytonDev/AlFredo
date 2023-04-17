using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class phaseManager : MonoBehaviour
{
    public bool menu;
    public GameObject gameCanvas;
    public GameObject endCanvas;
    public Canvas menuCanvas;
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
    public Text p1Ready;
    public Text p2Ready;
    bool ready1;
    bool ready2;
    bool gameEnd;
    public Animator p1ReadyAnim;
    public Animator p2ReadyAnim;
    private void Start()
    {
        p1.SetTarget(p1MoveTarget4, true);
        p2.SetTarget(p2MoveTarget4, true);
        phase = Mathf.CeilToInt(Timer / 15);
        lastPhase = phase;
        ready1 = false;
        ready2 = false;
        menu = true;
        startTime = -1;
    }
    float startTime;
    // Update is called once per frame
    void Update()
    {
        if (menu)
        {
            Time.timeScale = 0;
            p1.gameObject.SetActive(false);
            p2.gameObject.SetActive(false);
            phase1Object.gameObject.SetActive(false);
            phase2Object.gameObject.SetActive(false);
            phase3Object.gameObject.SetActive(false);
            phase4Object.gameObject.SetActive(false);
            menuCanvas.gameObject.SetActive(true);
            gameCanvas.gameObject.SetActive(false);
            endCanvas.gameObject.SetActive(false);
            if (!ready1 && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S)))
            {
                p1ReadyAnim.Play("Spawn");
                ready1 = true;
            }
            if (!ready2 && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow)))
            {
                p2ReadyAnim.Play("Spawn");
                ready2 = true;
            }
            if (ready1)
                p1Ready.text = "Player 1 Ready";
            else
                p1Ready.text = "Player 1 press WASD";
            if (ready2)
                p2Ready.text = "Player 2 Ready";
            else
                p2Ready.text = "Player 2 press Arrows";
            if (ready1 && ready2 && startTime == -1)
                startTime = Time.realtimeSinceStartup + 3;
            if (startTime != -1)
            {
                p1Ready.text = "";
                p2Ready.text = "Starting in " + Mathf.CeilToInt(startTime - Time.realtimeSinceStartup);
            }
            if (Time.realtimeSinceStartup > startTime && startTime != -1)
            {
                menu = false;
                startTime = -1;
            }

            return;
        }
        else
        {
            if (Time.timeScale == 0)
            {
                p1.gameObject.SetActive(true);
                p2.gameObject.SetActive(true);
            }
            Time.timeScale = 1;
        }
        menuCanvas.gameObject.SetActive(false);
        if (p2.movingTarget && !p1.movingTarget)
            p1.transform.position = p1.movingTargetPos;
        if (p1.movingTarget && !p2.movingTarget)
            p2.transform.position = p2.movingTargetPos;
        gameCanvas.SetActive(!gameEnd);
        endCanvas.SetActive(gameEnd);
        if (gameEnd)
        {
            if (Input.GetKey(KeyCode.Space))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        phase = Mathf.CeilToInt(Timer / 25);
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
            timer.text = "Time: " + (int)(((phase) * 25) - Timer);
            phase1Object.gameObject.SetActive(phase == 1);
            phase2Object.gameObject.SetActive(phase == 2);
            phase3Object.gameObject.SetActive(phase == 3);
            phase4Object.gameObject.SetActive(phase == 4);
        }
    }
}
