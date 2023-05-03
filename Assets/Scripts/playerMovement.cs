using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class playerMovement : MonoBehaviour
{
    public playerMovement otherPlayer;
    public float onionTime, pepperTime, shroomTime, frameTime = 0.3f, moveTime;
    public aiManagerTool AI;
    public bool onion, dead, movingTarget, keyPressed, verticalFirst;
    public int playerIndex, health = 10, directionSpriteIndex;
    public GameObject deadDog, onionObject;
    public Vector3 movingTargetPos, targetPos, realtimeTargetPos, dir = Vector3.left;
    public SpriteRenderer directionSprite;
    public Sprite[] chefSprites;
    public AudioClip fireSound;
    Rigidbody mainBody;
    public LineRenderer l;
    public Vector3 direction, lastPos;
    GameVersionManager v;
    public GameObject[] firstSpawns;
    public GameObject[] secondSpawns;
    public GameObject deathSpawn, lifeSpawn;

    void Start()
    {
        // Get the game version manager
        v = FindObjectOfType<GameVersionManager>();

        // Set the initial direction
        direction = Vector3.forward;

        // Set the initial target position
        realtimeTargetPos = transform.position;
        targetPos = realtimeTargetPos;

        // Set the initial health
        health = 10;

        // Get the rigidbody component
        mainBody = GetComponent<Rigidbody>();

        // Get the AI manager tool
        AI = FindObjectOfType<aiManagerTool>(true);
    }

    // Set the target for the object to move towards
    public void SetTarget(GameObject target, bool verticalFirstIn)
    {
        // Set the movement direction based on the given boolean
        verticalFirst = verticalFirstIn;

        // Set the target and flag the object as moving towards it
        movingTargetPos = target.transform.position;
        movingTarget = true;
    }

    // Draw a line from the current position to the target position
    public void drawLine(Vector3 target)
    {
        // Play a firing sound effect
        GetComponent<AudioSource>().PlayOneShot(fireSound, 0.1f);

        // Add 4 positions to the line renderer
        int posCount = l.positionCount;
        l.positionCount = posCount + 4;

        // Set the positions of the line renderer
        Vector3 startPos = transform.position - Vector3.up * 100;
        Vector3 endPos = target - Vector3.up * 100;
        l.SetPosition(posCount, startPos);
        l.SetPosition(posCount + 1, transform.position);
        l.SetPosition(posCount + 2, target);
        l.SetPosition(posCount + 3, endPos);
    }

    // Kill the player and update its status
    public void killPlayer()
    {
        if (onionTime > Time.realtimeSinceStartup)
            return;
        // Play the "Killed" animation for the player
        FindObjectOfType<playerStatusManager>().PlayAnimation("Killed", playerIndex);
        GameObject killedSpawn = Instantiate(deathSpawn);
        killedSpawn.transform.parent = null;
        killedSpawn.transform.position = transform.position;
        // Set the player's state to dead
        dead = true;

        // Move the player to the target position and update some variables
        if (UnityEngine.Random.value > 0.5)
            transform.position = secondSpawns[FindObjectOfType<phaseManager>().phase - 1].transform.position;
        else
            transform.position = movingTargetPos;
        realtimeTargetPos = transform.position;
        lastPos = transform.position;
        targetPos = realtimeTargetPos;
    }

    // Update the movement of an object towards a target position
    void SetTransitionMovement()
    {
        // Remember the object's last position
        Vector3 lastPos = transform.position;

        // If we're moving vertically first
        if (verticalFirst)
        {
            // Move towards the target position on the Z axis first
            if (transform.position.z != movingTargetPos.z)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    new Vector3(transform.position.x, transform.position.y, movingTargetPos.z),
                    Time.deltaTime / frameTime
                );
            }
            // Once we reach the target Z position, move towards the target X position
            else if (transform.position.x != movingTargetPos.x)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    new Vector3(movingTargetPos.x, transform.position.y, transform.position.z),
                    Time.deltaTime / frameTime
                );
            }
        }
        else
        {
            // Move towards the target position on the X axis first
            if (transform.position.x != movingTargetPos.x)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    new Vector3(movingTargetPos.x, transform.position.y, transform.position.z),
                    Time.deltaTime / frameTime
                );
            }
            // Once we reach the target X position, move towards the target Z position
            else if (transform.position.z != movingTargetPos.z)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    new Vector3(transform.position.x, transform.position.y, movingTargetPos.z),
                    Time.deltaTime / frameTime
                );
            }
        }

        // If the object's position has changed, update its direction and sprite index
        if (lastPos != transform.position)
        {
            direction = Vector3.Normalize(transform.position - lastPos);
            if (lastPos.x < transform.position.x)
                directionSpriteIndex = 0; // Right
            else if (lastPos.x > transform.position.x)
                directionSpriteIndex = 5; // Left
            else if (lastPos.z > transform.position.z)
                directionSpriteIndex = 10; // Down
            else if (lastPos.z < transform.position.z)
                directionSpriteIndex = 15; // Up
        }

        // If the object has reached the target position, reset some variables
        if (transform.position.x == movingTargetPos.x && transform.position.z == movingTargetPos.z)
        {
            if (otherPlayer.transform.position.x == otherPlayer.movingTargetPos.x && otherPlayer.transform.position.z == otherPlayer.movingTargetPos.z)
            {
                Time.timeScale = 1;
                movingTarget = false;
                realtimeTargetPos = transform.position;
                targetPos = realtimeTargetPos;
            }
            else
            {
                transform.position = new Vector3(movingTargetPos.x, transform.position.y, movingTargetPos.z);
            }
        }
    }
    // This function controls the movement of the player
    void GameplayMovement()
    {
        // Set the time scale to normal
        Time.timeScale = 1;
        // Use linear interpolation to move the player to the target position
        transform.position = Vector3.Lerp(realtimeTargetPos, lastPos, (AI.mt - Time.realtimeSinceStartup) / (AI.ft));

        // Dictionary to map directions to their respective sprites and indices
        Dictionary<(bool, bool, bool, bool), (Vector3, int)> directionMap = new Dictionary<(bool, bool, bool, bool), (Vector3, int)>
{
    {(true, false, false, false), (Vector3.forward, 15)},
    {(false, true, false, false), (Vector3.left, 5)},
    {(false, false, true, false), (Vector3.back, 10)},
    {(false, false, false, true), (Vector3.right, 0)}
};

        // Check which player is active and get their input
        if (playerIndex == 0 && (v.p1UpTap || v.p1LeftTap || v.p1DownTap || v.p1RightTap))
        {
            // Check the input against the direction map and set the direction and sprite index
            foreach (var kvp in directionMap)
            {
                var (p1UpTap, p1LeftTap, p1DownTap, p1RightTap) = kvp.Key;
                if (v.p1UpTap == p1UpTap && v.p1LeftTap == p1LeftTap && v.p1DownTap == p1DownTap && v.p1RightTap == p1RightTap)
                {
                    (direction, directionSpriteIndex) = kvp.Value;
                    break;
                }
            }
            if (Time.realtimeSinceStartup < shroomTime)
                direction *= -1;
        }
        else if (playerIndex == 1 && (v.p2UpTap || v.p2LeftTap || v.p2DownTap || v.p2RightTap))
        {
            // Check the input against the direction map and set the direction and sprite index
            foreach (var kvp in directionMap)
            {
                var (p2UpTap, p2LeftTap, p2DownTap, p2RightTap) = kvp.Key;
                if (v.p2UpTap == p2UpTap && v.p2LeftTap == p2LeftTap && v.p2DownTap == p2DownTap && v.p2RightTap == p2RightTap)
                {
                    (direction, directionSpriteIndex) = kvp.Value;
                    break;
                }
            }
            if (Time.realtimeSinceStartup < shroomTime)
                direction *= -1;
        }

        

        RaycastHit target;
        // Check if a key has been pressed and move the player to the target position

        if (keyPressed) // if a key was pressed
        {
            lastPos = realtimeTargetPos; // set last position to current position
            keyPressed = false; // reset keyPressed flag
            transform.position = realtimeTargetPos; // set current position to target position
            if ((Time.realtimeSinceStartup < pepperTime) && !Physics.Raycast(transform.position, direction, out target, 2))
            {
                // if the pepper power-up is active and there is no collision within 2 units in the current direction
                realtimeTargetPos = new Vector3(Mathf.Round((realtimeTargetPos + direction * 2).x), realtimeTargetPos.y, Mathf.Round((realtimeTargetPos + direction * 2).z));
            }
            else if (!Physics.Raycast(transform.position, direction, out target, 1))
            {
                // if there is no collision within 1 unit in the current direction
                realtimeTargetPos = new Vector3(Mathf.Round((realtimeTargetPos + direction).x), realtimeTargetPos.y, Mathf.Round((realtimeTargetPos + direction).z));
            }
            else if (target.transform.gameObject.GetComponent<AIScript>())
            {
                // if there is a collision with an AI object
                if (onion) // if the onion power-up is active
                {
                    GameObject newCheck = Instantiate(deadDog); // create a new deadDog object
                    newCheck.transform.position = target.transform.gameObject.transform.position; // set position of the new object to the position of the AI object
                    newCheck.transform.parent = null; // remove the parent of the new object
                    Destroy(target.transform.gameObject); // destroy the AI object
                }
                else // if the onion power-up is not active
                {
                    killPlayer(); // kill the player
                }
            }
        }
    }

    void PlayerCombat()
    {
        // Reset player shoot tap
        if (playerIndex == 0)
            v.p1ShootTap = false;
        if (playerIndex == 1)
            v.p2ShootTap = false;

        // Perform raycast
        if (Physics.Raycast(realtimeTargetPos, direction, out RaycastHit hit))
        {
            // Adjust camera fire offset
            var playerCamera = FindObjectOfType<PlayerCamera>();
            playerCamera.fireOffset = Mathf.Max(playerCamera.fireOffset, 1);

            // Draw hit markers
            drawLine(hit.point);
            drawLine(hit.point + Vector3.up * 3);
            drawLine(hit.point - Vector3.up * 3);

            // Handle hit AI
            if (hit.transform.TryGetComponent(out AIScript aiScript))
            {
                // Adjust camera fire offset
                playerCamera.fireOffset = Mathf.Max(playerCamera.fireOffset, 3);

                // Spawn dead dog object and destroy hit AI
                Instantiate(deadDog, hit.transform.position, Quaternion.identity);
                Destroy(hit.transform.gameObject);
            }

            // Handle hit player
            if (hit.transform.TryGetComponent(out playerMovement player) && player.gameObject.name != GetComponent<playerMovement>().gameObject.name && !player.onion)
            {
                // Adjust camera fire offset
                playerCamera.fireOffset = Mathf.Max(playerCamera.fireOffset, 5);

                // Calculate and update scores
                var pelletParticles = FindObjectOfType<pelletParticles>();
                int score1 = pelletParticles.score1, score2 = pelletParticles.score2;
                if (playerIndex == 0)
                {
                    pelletParticles.score1 += Mathf.FloorToInt(score2 / 5);
                    pelletParticles.score2 -= Mathf.FloorToInt(score2 / 5);
                }
                else if (playerIndex == 1)
                {
                    pelletParticles.score2 += Mathf.FloorToInt(score1 / 5);
                    pelletParticles.score1 -= Mathf.FloorToInt(score1 / 5);
                }

                // Kill the hit player
                player.killPlayer();
            }
        }
    }
    void Update()
    {
        // Activate onion object if it's still within the onion time
        onionObject.SetActive(Time.realtimeSinceStartup < onionTime);

        // Set the player's position, keeping them at y = -0.5
        transform.position = new Vector3(transform.position.x, -0.5f, transform.position.z);

        // If the player is dead, exit early and do nothing
        if (dead) return;

        // Calculate the current sprite frame time and movement frame time
        float spriteFrameTime = Time.realtimeSinceStartup * 10;
        float moveFrameTime = (Time.realtimeSinceStartup < pepperTime) ? frameTime * 0.5f : frameTime;

        // Set the direction sprite based on the sprite frame time and direction sprite index
        directionSprite.sprite = chefSprites[directionSpriteIndex + (int)(spriteFrameTime - (Mathf.Floor(spriteFrameTime / 4) * 4))];

        // If the player is currently moving towards a target, set the movement transition and return
        if (movingTarget)
        {
            SetTransitionMovement();
            return;
        }

        // Otherwise, handle regular gameplay movement
        GameplayMovement();

        // If the player has pressed the shoot button, handle player combat
        if ((playerIndex == 0 && v.p1ShootTap) || (playerIndex == 1 && v.p2ShootTap))
        {
            PlayerCombat();
        }
    }
}
