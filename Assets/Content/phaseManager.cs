using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class phaseManager : MonoBehaviour
{
    public Material[] itemMats;
    public MeshRenderer item1Mesh;
    public MeshRenderer item2Mesh;
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
    public bool ready1;
    public bool ready2;
    bool gameEnd;
    public Animator p1ReadyAnim;
    public Animator p2ReadyAnim;
    public GameObject dialogue;
    public GameObject dialogueSuicide;
    public GameObject selectStuff;
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
    int menuStage = 0;
    float startTime;
    public Image p1Image;
    public Image p2Image;
    public Text p1ItemText;
    public Text p2ItemText;
    public Text p1ItemName;
    public Text p2ItemName;

    public Sprite[] itemSprites;
    public string[] itemNames;
    public string[] itemDescriptions;
    public Text lockInTime;
    public int p1ItemIndex;
    public int p2ItemIndex;
    // Update is called once per frame
    void Update()
    {
        item1Mesh.material = itemMats[p1ItemIndex];
        item2Mesh.material = itemMats[p2ItemIndex];
        selectStuff.SetActive(false);
        if (dialogue)
        {
            if (dialogueSuicide.activeInHierarchy)
                Destroy(dialogue);
        }
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
            
            if (menuStage == 0)
            {
                if (!ready1 && (Input.GetKey(KeyCode.W)))
                {
                    p1ReadyAnim.Play("Spawn");
                    ready1 = true;
                }
                if (!ready2 && (Input.GetKey(KeyCode.UpArrow)))
                {
                    p2ReadyAnim.Play("Spawn");
                    ready2 = true;
                }
                if (ready1)
                    p1Ready.text = "Player 1 Ready";
                else
                    p1Ready.text = "Player 1 press W";
                if (ready2)
                    p2Ready.text = "Player 2 Ready";
                else
                    p2Ready.text = "Player 2 press Up";
                if (ready1 && ready2)
                {
                    menuStage = 1;
                    ready1 = false;
                    ready2 = false;
                }
            }
            if (menuStage == 1)
            {
                lockInTime.text = "Pick your Taste Bud!";
                selectStuff.SetActive(true);
                p2Ready.text = "</> to Scroll, Down to Pick";
                p1Ready.text = "A/D to Scroll, S to Pick";
                if (Input.GetKeyDown(KeyCode.D))
                    p1ItemIndex++;
                if (Input.GetKeyDown(KeyCode.A))
                    p1ItemIndex--;
                if (Input.GetKeyDown(KeyCode.RightArrow))
                    p2ItemIndex++;
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                    p2ItemIndex--;
                p1ItemIndex = Mathf.Min(p1ItemIndex, 1);
                p2ItemIndex = Mathf.Min(p2ItemIndex, 1);
                p1ItemIndex = Mathf.Max(p1ItemIndex, 0);
                p2ItemIndex = Mathf.Max(p2ItemIndex, 0);
                p1ItemText.text = itemDescriptions[p1ItemIndex];
                p2ItemText.text = itemDescriptions[p2ItemIndex];
                p1ItemName.text = itemNames[p1ItemIndex];
                p2ItemName.text = itemNames[p2ItemIndex];
                p1Image.sprite = itemSprites[p1ItemIndex];
                p2Image.sprite = itemSprites[p2ItemIndex];
                if (Input.GetKeyDown(KeyCode.S))
                    ready1 = true;
                if (Input.GetKeyDown(KeyCode.DownArrow))
                    ready2 = true;
                if (Input.GetKeyDown(KeyCode.W))
                    ready1 = false;
                if (Input.GetKeyDown(KeyCode.UpArrow))
                    ready2 = false;
                if (ready1)
                    p1Ready.text = "W to go back";
                if (ready2)
                    p2Ready.text = "Up to go back";
                p1Image.gameObject.SetActive(!ready1);
                p1ItemText.gameObject.SetActive(!ready1);
                p1ItemName.gameObject.SetActive(!ready1);
                p2ItemText.gameObject.SetActive(!ready2);
                p2ItemName.gameObject.SetActive(!ready2);
                p2Image.gameObject.SetActive(!ready2);
                if (ready1 && ready2 && startTime == -1)
                {
                    startTime = Time.realtimeSinceStartup + 3;
                }
                if (!(ready1 && ready2))
                    startTime = -1;
            }
                //startTime = Time.realtimeSinceStartup + 3;
            if (startTime != -1)
            {
                lockInTime.text = "Starting in " + Mathf.CeilToInt(startTime - Time.realtimeSinceStartup);
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
                Time.timeScale = 0.2f;
                p2.gameObject.SetActive(true);
            }
            
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
        phase = Mathf.CeilToInt(Timer / 60);
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
                //phase1Object.gameObject.SetActive(Mathf.Cos(Time.realtimeSinceStartup * 10) > 0);
                phase1Object.gameObject.SetActive(true);
            }
            if (phase == 1)
            {
                phase1Object.gameObject.SetActive(Mathf.Sin(Time.realtimeSinceStartup * 10) > 0);
                phase2Object.gameObject.SetActive(Mathf.Cos(Time.realtimeSinceStartup * 10) > 0);
                phase1Object.gameObject.SetActive(true);
                phase2Object.gameObject.SetActive(true);
            }
            if (phase == 2)
            {
                phase2Object.gameObject.SetActive(Mathf.Sin(Time.realtimeSinceStartup * 10) > 0);
                phase3Object.gameObject.SetActive(Mathf.Cos(Time.realtimeSinceStartup * 10) > 0);
                phase2Object.gameObject.SetActive(true);
                phase3Object.gameObject.SetActive(true);
            }
            if (phase == 3)
            {
                phase3Object.gameObject.SetActive(Mathf.Sin(Time.realtimeSinceStartup * 10) > 0);
                phase4Object.gameObject.SetActive(Mathf.Cos(Time.realtimeSinceStartup * 10) > 0);
                phase3Object.gameObject.SetActive(true);
                phase4Object.gameObject.SetActive(true);
            }
            if (phase == 4)
            {
                phase4Object.gameObject.SetActive(Mathf.Sin(Time.realtimeSinceStartup * 10) > 0);
                phase1Object.gameObject.SetActive(Mathf.Cos(Time.realtimeSinceStartup * 10) > 0);
                phase4Object.gameObject.SetActive(true);
                phase1Object.gameObject.SetActive(true);
            }
            timer.text = "";
        }
        else
        {
            timer.text = "Time: " + (int)(((phase) * 60) - Timer);
            phase1Object.gameObject.SetActive(phase == 1);
            phase2Object.gameObject.SetActive(phase == 2);
            phase3Object.gameObject.SetActive(phase == 3);
            phase4Object.gameObject.SetActive(phase == 4);
        }
    }
}
