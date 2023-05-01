using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class phaseManager : MonoBehaviour
{
    public Material[] itemMats;
    public MeshRenderer item1Mesh, item2Mesh;
    public bool menu, ready1, ready2, gameEnd;
    public int phase = 1, lastPhase = 1, menuStage = 0, p1ItemIndex, p2ItemIndex;
    public float Timer, startTime;
    public GameObject gameCanvas, endCanvas, phase1Object, p1MoveTarget1, p2MoveTarget1,
    phase2Object, p1MoveTarget2, p2MoveTarget2, phase3Object, p1MoveTarget3, p2MoveTarget3,
    phase4Object, p1MoveTarget4, p2MoveTarget4, borders, bowl, dialogue, dialogueSuicide,
    selectStuff;
    public Canvas menuCanvas;
    public Text player1Score, player2Score, winnerName, timer, p1Ready, p2Ready, lockInTime,
    p1ItemText, p2ItemText, p1ItemName, p2ItemName;
    public AudioSource musicManager;
    public pelletParticles p;
    public playerMovement p1, p2;
    public PlayerCamera c;
    public Image p1Image, p2Image;
    public Sprite[] itemSprites;
    public string[] itemNames, itemDescriptions;
    public Animator p1ReadyAnim, p2ReadyAnim;
    GameVersionManager v;

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
        v = FindObjectOfType<GameVersionManager>();
    }

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
            InMenu();
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
        MovingTarget();
        gameCanvas.SetActive(!gameEnd);
        endCanvas.SetActive(gameEnd);
        if (gameEnd)
        {
            GameOver();
            return;
        }
        musicManager.volume = 0.1f;
        if (!p1.movingTarget && !p2.movingTarget)
        {
            Timer += Time.deltaTime;
            musicManager.volume = 0.5f;
        }
        phase = Mathf.CeilToInt(Timer / 60);
        if (lastPhase != phase)
        {
            SwapPhase();
        }
    }
    void MovingTarget()
    {
        if (p2.movingTarget && !p1.movingTarget)
            p1.transform.position = p1.movingTargetPos;
        if (p1.movingTarget && !p2.movingTarget)
            p2.transform.position = p2.movingTargetPos;
        p1.GetComponent<Rigidbody>().isKinematic = !p1.movingTarget;
        p2.GetComponent<Rigidbody>().isKinematic = !p2.movingTarget;
        borders.SetActive(!p1.movingTarget && !p2.movingTarget);
    }
    void InMenu()
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

        MenuStages();
        if (startTime != -1)
        {
            lockInTime.text = "Starting in " + Mathf.CeilToInt(startTime - Time.realtimeSinceStartup);
        }
        if (Time.realtimeSinceStartup > startTime && startTime != -1)
        {
            menu = false;
            startTime = -1;
        }
    }
    void MenuStages()
    {
        if (menuStage == 0)
        {
            if (!ready1 && v.p1UpTap) { p1ReadyAnim.Play("Spawn"); ready1 = true; }
            if (!ready2 && v.p2UpTap) { p2ReadyAnim.Play("Spawn"); ready2 = true; }
            p1Ready.text = ready1 ? "Player 1 Ready" : (FindAnyObjectByType<GameVersionManager>().Android ? "Player 1 press Up" : "Player 1 press W");
            p2Ready.text = ready2 ? "Player 2 Ready" : "Player 2 press Up";
            if (ready1 && ready2) { menuStage = 1; ready1 = false; ready2 = false; }
        }
        else if (menuStage == 1)
        {
            lockInTime.text = "Pick your Taste Bud!";
            selectStuff.SetActive(true);
            p2Ready.text = "</> to Scroll, Down to Pick";
            p1Ready.text = FindAnyObjectByType<GameVersionManager>().Android ? "</> to Scroll, Down to Pick" : "A/D to Scroll, S to Pick";
            p1ItemIndex += v.p1RightTap ? 1 : (v.p1LeftTap ? -1 : 0);
            p2ItemIndex += v.p2RightTap ? 1 : (v.p2LeftTap ? -1 : 0);
            p1ItemIndex = Mathf.Clamp(p1ItemIndex, 0, 1);
            p2ItemIndex = Mathf.Clamp(p2ItemIndex, 0, 1);
            p1ItemText.text = itemDescriptions[p1ItemIndex];
            p2ItemText.text = itemDescriptions[p2ItemIndex];
            p1ItemName.text = itemNames[p1ItemIndex];
            p2ItemName.text = itemNames[p2ItemIndex];
            p1Image.sprite = itemSprites[p1ItemIndex];
            p2Image.sprite = itemSprites[p2ItemIndex];
            ready1 = v.p1DownTap ? true : (v.p1UpTap ? false : ready1);
            ready2 = v.p2DownTap ? true : (v.p2UpTap ? false : ready2);
            p1Ready.text = ready1 ? (FindAnyObjectByType<GameVersionManager>().Android ? "Up to go back" : "W to go back") : p1Ready.text;
            p2Ready.text = ready2 ? "Up to go back" : p2Ready.text;
            p1Image.gameObject.SetActive(!ready1);
            p1ItemText.gameObject.SetActive(!ready1);
            p1ItemName.gameObject.SetActive(!ready1);
            p2ItemText.gameObject.SetActive(!ready2);
            p2ItemName.gameObject.SetActive(!ready2);
            p2Image.gameObject.SetActive(!ready2);
            startTime = (ready1 && ready2 && startTime == -1) ? Time.realtimeSinceStartup + 3 : (!(ready1 && ready2) ? -1 : startTime);
        }
    }

    void GameOver()
    {
        if (v.p1DownTap || v.p2DownTap)
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
    }


    void SwapPhase()
    {
        GameObject[] p1MoveTargets = new GameObject[] { p1MoveTarget1, p1MoveTarget2, p1MoveTarget3, p1MoveTarget4};

        GameObject[] p2MoveTargets = new GameObject[] { p2MoveTarget1, p2MoveTarget2, p2MoveTarget3, p2MoveTarget4};
        lastPhase = phase;
        bool isMovingTarget = p1.movingTarget || p2.movingTarget;
        timer.text = isMovingTarget ? "" : "Time: " + (int)(((phase) * 60) - Timer);
        if (!isMovingTarget)
        {
            phase1Object.gameObject.SetActive(phase == 1);
            phase2Object.gameObject.SetActive(phase == 2);
            phase3Object.gameObject.SetActive(phase == 3);
            phase4Object.gameObject.SetActive(phase == 4);
            return;
        }

        int targetIndex = phase - 1;
        p1.SetTarget(targetIndex % 2 == 0 ? p1MoveTargets[targetIndex] : p1MoveTargets[targetIndex], targetIndex % 2 == 1);
        p2.SetTarget(targetIndex % 2 == 0 ? p2MoveTargets[targetIndex] : p2MoveTargets[targetIndex], targetIndex % 2 == 1);
        gameEnd = phase == 4;

        phase1Object.gameObject.SetActive(phase == 1 || phase == 2);
        phase2Object.gameObject.SetActive(phase == 2 || phase == 3);
        phase3Object.gameObject.SetActive(phase == 3 || phase == 4);
        phase4Object.gameObject.SetActive(phase == 4 || phase == 1);

        if (phase == 0) phase1Object.gameObject.SetActive(true);
        else if (phase > 0 && phase < 5)
        {
            bool isSin = phase % 2 == 1;
            bool isCos = phase % 2 == 0;
            phase1Object.gameObject.SetActive(isSin ? Mathf.Sin(Time.realtimeSinceStartup * 10) > 0 : isCos);
            phase2Object.gameObject.SetActive(isSin ? Mathf.Cos(Time.realtimeSinceStartup * 10) > 0 : isCos);
            phase3Object.gameObject.SetActive(isSin ? Mathf.Sin(Time.realtimeSinceStartup * 10) > 0 : isCos);
            phase4Object.gameObject.SetActive(isSin ? Mathf.Cos(Time.realtimeSinceStartup * 10) > 0 : isCos);
        }
    }
    }
