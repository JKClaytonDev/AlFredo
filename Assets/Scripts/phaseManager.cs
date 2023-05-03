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
    public Animator p1ReadyAnim, p2ReadyAnim, blackBarsCanvas;
    GameVersionManager v;

    private void Start()
    {
        // Set the target position for player 1 to move to, and flag the move as immediate.
        p1.SetTarget(p1MoveTarget4, true);

        // Set the target position for player 2 to move to, and flag the move as immediate.
        p2.SetTarget(p2MoveTarget4, true);

        // Calculate the current game phase based on the timer and round up to the nearest integer.
        phase = Mathf.CeilToInt(Timer / 15);

        // Save the current game phase as the last phase for later comparison.
        lastPhase = phase;

        // Set flags to indicate that both players are not ready to start playing yet.
        ready1 = false;
        ready2 = false;

        // Set a flag to indicate that the game is currently displaying the menu.
        menu = true;

        // Set the start time to -1 to indicate that the game has not started yet.
        startTime = -1;

        // Get a reference to the GameVersionManager object in the scene.
        v = FindObjectOfType<GameVersionManager>();
    }

    void Update()
    {
        // Set the material for the first player's item mesh based on their current item index.
        item1Mesh.material = itemMats[p1ItemIndex];

        // Set the material for the second player's item mesh based on their current item index.
        item2Mesh.material = itemMats[p2ItemIndex];

        // Disable the selectStuff object.
        selectStuff.SetActive(false);

        // If dialogue is currently showing...
        if (dialogue)
        {
            // ...and the suicide dialogue is currently active, destroy the dialogue object.
            if (dialogueSuicide.activeInHierarchy)
                Destroy(dialogue);
        }

        // If the game is currently showing the menu...
        if (menu)
        {
            // ...call the InMenu() function and exit the Update() function.
            InMenu();
            return;
        }
        else
        {
            // If the game is not in the menu and the time scale is 0...
            if (Time.timeScale == 0)
            {
                // ...activate both player objects, set the time scale to 0.2, and exit the Update() function.
                p1.gameObject.SetActive(true);
                Time.timeScale = 0.2f;
                p2.gameObject.SetActive(true);
            }
        }

        // Disable the menu canvas.
        menuCanvas.gameObject.SetActive(false);

        // Call the MovingTarget() function.
        MovingTarget();

        // Enable the game canvas if the game has not ended, and disable the end canvas.
        gameCanvas.SetActive(!gameEnd);
        endCanvas.SetActive(gameEnd);

        // If the game has ended...
        if (gameEnd)
        {
            // ...call the GameOver() function and exit the Update() function.
            GameOver();
            return;
        }

        // Set the volume of the music manager to 0.1.
        musicManager.volume = 0.1f;

        // If neither player is currently moving towards their target...
        if (!p1.movingTarget && !p2.movingTarget)
        {
            // ...increment the Timer variable by the amount of time that has passed since the last frame, and increase the music volume.
            Timer += Time.deltaTime;
            musicManager.volume = 0.5f;
        }

        // Calculate the current game phase based on the timer and round up to the nearest integer.
        phase = Mathf.CeilToInt(Timer / 60);

        // If the game phase has changed since the last frame...
        if (lastPhase != phase)
        {
            // ...call the SwapPhase() function.
            SwapPhase();
        }

        isMovingTarget();
    }
    void MovingTarget()
    {
        // If player 2 is currently moving towards their target and player 1 is not...
        if (p2.movingTarget && !p1.movingTarget)
            // ...set player 1's position to their target position.
            p1.transform.position = p1.movingTargetPos;

        // If player 1 is currently moving towards their target and player 2 is not...
        if (p1.movingTarget && !p2.movingTarget)
            // ...set player 2's position to their target position.
            p2.transform.position = p2.movingTargetPos;

        // Enable or disable player 1's rigidbody component based on whether they are currently moving towards their target.
        p1.GetComponent<Rigidbody>().isKinematic = !p1.movingTarget;

        // Enable or disable player 2's rigidbody component based on whether they are currently moving towards their target.
        p2.GetComponent<Rigidbody>().isKinematic = !p2.movingTarget;

        // Enable or disable the borders object based on whether both players are currently moving towards their targets.
        borders.SetActive(!p1.movingTarget && !p2.movingTarget);
    }

    void InMenu()
    {
        // Freeze the game time while in the menu.
        Time.timeScale = 0;

        // Disable the game objects for both players while in the menu.
        p1.gameObject.SetActive(false);
        p2.gameObject.SetActive(false);

        // Hide all the phase objects while in the menu.
        phase1Object.gameObject.SetActive(false);
        phase2Object.gameObject.SetActive(false);
        phase3Object.gameObject.SetActive(false);
        phase4Object.gameObject.SetActive(false);

        // Enable the menu canvas and disable the game and end canvases.
        menuCanvas.gameObject.SetActive(true);
        gameCanvas.gameObject.SetActive(false);
        endCanvas.gameObject.SetActive(false);

        // Call the MenuStages() function, which updates the menu UI based on the game's progress.
        MenuStages();

        // If the game is scheduled to start at a specific time...
        if (startTime != -1)
        {
            // ...display a countdown timer indicating how long until the game starts.
            lockInTime.text = "Starting in " + Mathf.CeilToInt(startTime - Time.realtimeSinceStartup);
        }

        // If the current time is past the scheduled start time...
        if (Time.realtimeSinceStartup > startTime && startTime != -1)
        {
            // ...exit the menu and resume the game.
            menu = false;
            blackBarsCanvas.Play("CanvasTransitionAnim");
            startTime = -1;
        }
    }
    void MenuStages()
    {
        // If the menu stage is 0:
        if (menuStage == 0)
        {
            // If player 1 has not yet pressed up, and they just did, play the spawn animation for their ready text and set the ready flag for player 1
            if (!ready1 && v.p1UpTap) { p1ReadyAnim.Play("Spawn"); ready1 = true; }

            // If player 2 has not yet pressed up, and they just did, play the spawn animation for their ready text and set the ready flag for player 2
            if (!ready2 && v.p2UpTap) { p2ReadyAnim.Play("Spawn"); ready2 = true; }

            // Update the ready text for player 1 and player 2 based on whether they have pressed up or not
            p1Ready.text = ready1 ? "Player 1 Ready" : (FindAnyObjectByType<GameVersionManager>().Android ? "Player 1 press Up" : "Player 1 press W");
            p2Ready.text = ready2 ? "Player 2 Ready" : "Player 2 press Up";

            // If both players are ready, move to menu stage 1 and reset the ready flags for player 1 and player 2
            if (ready1 && ready2) { menuStage = 1; ready1 = false; ready2 = false; }
        }
        else if (menuStage == 1)
        {
            lockInTime.text = "Pick your Taste Bud!";
            // Activate the selectStuff game object
            selectStuff.SetActive(true);

            // Set the text of p2Ready to "</> to Scroll, Down to Pick"
            p2Ready.text = "</> to Scroll, Down to Pick";

            // Set the text of p1Ready based on whether the game is being played on an Android device or not
            p1Ready.text = FindAnyObjectByType<GameVersionManager>().Android ? "</> to Scroll, Down to Pick" : "A/D to Scroll, S to Pick";

            // Increment or decrement p1ItemIndex and p2ItemIndex based on the input from the players
            p1ItemIndex += v.p1RightTap ? 1 : (v.p1LeftTap ? -1 : 0);
            p2ItemIndex += v.p2RightTap ? 1 : (v.p2LeftTap ? -1 : 0);

            // Clamp the values of p1ItemIndex and p2ItemIndex to be between 0 and 1
            p1ItemIndex = Mathf.Clamp(p1ItemIndex, 0, 2);
            p2ItemIndex = Mathf.Clamp(p2ItemIndex, 0, 2);

            // Set the text of p1ItemText and p2ItemText to the item descriptions at the indexes of p1ItemIndex and p2ItemIndex respectively
            p1ItemText.text = itemDescriptions[p1ItemIndex];
            p2ItemText.text = itemDescriptions[p2ItemIndex];

            // Set the text of p1ItemName and p2ItemName to the item names at the indexes of p1ItemIndex and p2ItemIndex respectively
            p1ItemName.text = itemNames[p1ItemIndex];
            p2ItemName.text = itemNames[p2ItemIndex];

            // Set the sprite of p1Image and p2Image to the item sprites at the indexes of p1ItemIndex and p2ItemIndex respectively
            p1Image.sprite = itemSprites[p1ItemIndex];
            p2Image.sprite = itemSprites[p2ItemIndex];

            // Set the value of ready1 and ready2 based on the input from the players
            ready1 = v.p1DownTap ? true : (v.p1UpTap ? false : ready1);
            ready2 = v.p2DownTap ? true : (v.p2UpTap ? false : ready2);

            // Set the text of p1Ready to "Up to go back" if ready1 is true, or to its original text if ready1 is false
            p1Ready.text = ready1 ? (FindAnyObjectByType<GameVersionManager>().Android ? "Up to go back" : "W to go back") : p1Ready.text;

            // Set the text of p2Ready to "Up to go back" if ready2 is true, or to its original text if ready2 is false
            p2Ready.text = ready2 ? "Up to go back" : p2Ready.text;

            // Set the active state of p1Image, p1ItemText, and p1ItemName to the opposite of the value of ready1
            p1Image.gameObject.SetActive(!ready1);
            p1ItemText.gameObject.SetActive(!ready1);
            p1ItemName.gameObject.SetActive(!ready1);

            // Set the active state of p2Image, p2ItemText, and p2ItemName to the opposite of the value of ready2
            p2ItemText.gameObject.SetActive(!ready2);
            p2ItemName.gameObject.SetActive(!ready2);
            p2Image.gameObject.SetActive(!ready2);
            startTime = (ready1 && ready2 && startTime == -1) ? Time.realtimeSinceStartup + 3 : (!(ready1 && ready2) ? -1 : startTime);
        }
    }

    // This function is called when the game is over and displays the scores and winner.
    void GameOver()
    {
        // If either player taps down, the game is restarted.
        if (v.p1DownTap || v.p2DownTap)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // Display the scores of both players.
        player1Score.text = "Player 1 Score: " + p.score1;
        player2Score.text = "Player 2 Score: " + p.score2;

        // Determine the winner and display their name or display a tie.
        if (p.score1 > p.score2)
            winnerName.text = "Player 1 Wins!";
        else if (p.score1 < p.score2)
            winnerName.text = "Player 2 Wins!";
        else
            winnerName.text = "Tie!";

        // Disable the phase objects and lower the volume of the music.
        phase1Object.gameObject.SetActive(false);
        phase2Object.gameObject.SetActive(false);
        phase3Object.gameObject.SetActive(false);
        phase4Object.gameObject.SetActive(false);
        musicManager.volume = 0.1f;

        // Deactivate the players and move the camera towards the bowl.
        p1.gameObject.SetActive(false);
        p2.gameObject.SetActive(false);
        c.enabled = false;
        c.gameObject.transform.position = Vector3.MoveTowards(c.gameObject.transform.position, bowl.transform.position + Vector3.up * 3, Time.deltaTime * 100);
    }

    public void isMovingTarget()
    {
        // create an array of move targets for player 1 and player 2
        GameObject[] p1MoveTargets = new GameObject[] { p1MoveTarget1, p1MoveTarget2, p1MoveTarget3, p1MoveTarget4 };
        GameObject[] p2MoveTargets = new GameObject[] { p2MoveTarget1, p2MoveTarget2, p2MoveTarget3, p2MoveTarget4 };

        // check if either player is moving and update the timer text accordingly
        bool isMovingTarget = p1.movingTarget || p2.movingTarget;
        timer.text = isMovingTarget ? "" : "Time: " + (int)(((phase) * 60) - Timer);

        // if neither player is moving, update the objects related to the current phase and return
        if (!isMovingTarget)
        {
            phase1Object.gameObject.SetActive(phase == 1);
            phase2Object.gameObject.SetActive(phase == 2);
            phase3Object.gameObject.SetActive(phase == 3);
            phase4Object.gameObject.SetActive(phase == 4);
            return;
        }

        // determine the target index based on the current phase
        int targetIndex = phase - 1;

        // set the target for each player based on the target index
        // the target index determines whether the player should move to an even or odd target in the array
        p1.SetTarget(targetIndex % 2 == 0 ? p1MoveTargets[targetIndex] : p1MoveTargets[targetIndex], targetIndex % 2 == 1);
        p2.SetTarget(targetIndex % 2 == 0 ? p2MoveTargets[targetIndex] : p2MoveTargets[targetIndex], targetIndex % 2 == 1);

        // if the current phase is 4, set gameEnd to true
        gameEnd = phase == 4;

        // update the objects related to the current phase
        phase1Object.gameObject.SetActive(phase == 1 || phase == 2);
        phase2Object.gameObject.SetActive(phase == 2 || phase == 3);
        phase3Object.gameObject.SetActive(phase == 3 || phase == 4);
        phase4Object.gameObject.SetActive(phase == 4 || phase == 1);

        // if the current phase is 0, show only the first phase object
        if (phase == 0)
        {
            phase1Object.gameObject.SetActive(true);
        }
        // if the current phase is between 1 and 4, update the objects based on whether the phase is odd or even
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
    void SwapPhase()
    {
        
        // store the current phase as the last phase
        lastPhase = phase;

    }
}
