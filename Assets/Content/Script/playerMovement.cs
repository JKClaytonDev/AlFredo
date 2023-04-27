using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
public class playerMovement : MonoBehaviour
{
    public float onionTime, pepperTime, frameTime = 0.3f, moveTime;
    public aiManagerTool AI;
    public bool onion = false, pepper = false, dead, movingTarget = false, keyPressed, verticalFirst;
    public int playerIndex, health = 10, directionSpriteIndex;
    public GameObject deadDog, onionObject;
    public Vector3 movingTargetPos, targetPos, realtimeTargetPos, dir = Vector3.left;
    public SpriteRenderer directionSprite;
    public Sprite[] chefSprites;
    public AudioClip fireSound;
    Rigidbody mainBody;
    public LineRenderer l;
    Vector3 direction, lastPos;

    void Start()
    {
        direction = (Vector3.forward);
        targetPos = realtimeTargetPos;
        health = 10;
        mainBody = GetComponent<Rigidbody>();
        realtimeTargetPos = transform.position;
    }
    public void SetTarget(GameObject target, bool verticalFirstIn)
    {
        verticalFirst = verticalFirstIn;
        movingTarget = true;
        movingTargetPos = target.transform.position;
    }
    public void drawLine(Vector3 target)
    {
        int posCount = l.positionCount;
        l.positionCount = posCount + 4;
        l.SetPosition(posCount, transform.position-Vector3.up*100);
        l.SetPosition(posCount+1, transform.position);
        l.SetPosition(posCount+2, target);
        l.SetPosition(posCount+3, target - Vector3.up * 100);
    }
    public void killPlayer()
    {
        FindObjectOfType<playerStatusManager>().PlayAnimation("Killed", playerIndex);
        dead = true;
        transform.position = movingTargetPos;
        realtimeTargetPos = transform.position;
        targetPos = realtimeTargetPos;
    }
    void Update()
    {
        onionObject.SetActive(onion);
        onion = Time.realtimeSinceStartup < onionTime;
        pepper = Time.realtimeSinceStartup < pepperTime;
        Vector3 pos = transform.position;
        pos.y = -0.5f;
        transform.position = pos;

        if (dead)
            return;

        float spriteFrameTime = Time.realtimeSinceStartup * 10;
        float moveFrameTime = pepper ? frameTime * 0.5f : frameTime;
        directionSprite.sprite = chefSprites[directionSpriteIndex + (int)(spriteFrameTime - (Mathf.Floor(spriteFrameTime / 4) * 4))];

        if (movingTarget)
        {
            Vector3 lastPos = transform.position;
            if (verticalFirst)
            {
                if (transform.position.z != movingTargetPos.z)
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, movingTargetPos.z), Time.deltaTime / frameTime);
                else if (transform.position.x != movingTargetPos.x)
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(movingTargetPos.x, transform.position.y, transform.position.z), Time.deltaTime / frameTime);
            }
            else
            {
                if (transform.position.x != movingTargetPos.x)
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(movingTargetPos.x, transform.position.y, transform.position.z), Time.deltaTime / frameTime);
                else if (transform.position.z != movingTargetPos.z)
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, movingTargetPos.z), Time.deltaTime / frameTime);
            }

            if (lastPos != transform.position)
            {
                direction = Vector3.Normalize(transform.position - lastPos);
                if (lastPos.x < transform.position.x) //Right
                    directionSpriteIndex = 0;
                else if (lastPos.x > transform.position.x) //Left
                    directionSpriteIndex = 5;
                else if (lastPos.z > transform.position.z) //Down
                    directionSpriteIndex = 10;
                else if (lastPos.z < transform.position.z) //Up
                    directionSpriteIndex = 15;
            }

            if (transform.position.x == movingTargetPos.x && transform.position.z == movingTargetPos.z)
            {
                Time.timeScale = 1;
                movingTarget = false;
                realtimeTargetPos = transform.position;
                targetPos = realtimeTargetPos;
            }
            return;
        }
        Time.timeScale = 1;
        transform.position = Vector3.MoveTowards(transform.position, realtimeTargetPos, Time.deltaTime / moveFrameTime);
        switch (Input.GetKeyDown(playerIndex == 0 ? KeyCode.W : KeyCode.UpArrow) || Input.GetKeyDown(playerIndex == 0 ? KeyCode.A : KeyCode.LeftArrow) || Input.GetKeyDown(playerIndex == 0 ? KeyCode.S : KeyCode.DownArrow) || Input.GetKeyDown(playerIndex == 0 ? KeyCode.D : KeyCode.RightArrow))
        {
            case true when Input.GetKeyDown(playerIndex == 0 ? KeyCode.W : KeyCode.UpArrow):
                direction = Vector3.forward;
                directionSpriteIndex = 15;
                break;

            case true when Input.GetKeyDown(playerIndex == 0 ? KeyCode.A : KeyCode.LeftArrow):
                direction = Vector3.left;
                directionSpriteIndex = 5;
                break;

            case true when Input.GetKeyDown(playerIndex == 0 ? KeyCode.S : KeyCode.DownArrow):
                direction = Vector3.back;
                directionSpriteIndex = 10;
                break;

            case true when Input.GetKeyDown(playerIndex == 0 ? KeyCode.D : KeyCode.RightArrow):
                direction = Vector3.right;
                directionSpriteIndex = 0;
                break;
        }

        RaycastHit target;
        if (keyPressed)
        {
            lastPos = realtimeTargetPos;
            keyPressed = false;
            transform.position = realtimeTargetPos;
            if (!Physics.Raycast(realtimeTargetPos, direction, out target, 1))
            {
                realtimeTargetPos = new Vector3(Mathf.Round((realtimeTargetPos + direction).x), realtimeTargetPos.y, Mathf.Round((realtimeTargetPos + direction).z));
            }
            else if (target.transform.gameObject.GetComponent<AIScript>())
            {
                if (onion)
                {
                    GameObject newCheck = Instantiate(deadDog);
                    newCheck.transform.position = target.transform.gameObject.transform.position;
                    newCheck.transform.parent = null;
                    Destroy(target.transform.gameObject);
                }
                else
                    killPlayer();
            }
        }

        if (Physics.Raycast(realtimeTargetPos, direction, out RaycastHit hit) &&
    hit.transform.TryGetComponent(out AIScript aiScript) && direction == aiScript.direction)
        {
            drawLine(hit.point);
            drawLine(hit.point + Vector3.up * 3);
            drawLine(hit.point - Vector3.up * 3);
            Instantiate(deadDog, hit.transform.position, Quaternion.identity);
            Destroy(hit.transform.gameObject);
        }
        else if (Physics.Raycast(realtimeTargetPos, direction, out hit) &&
                 hit.transform.TryGetComponent(out playerMovement player) &&
                 player.gameObject.name != GetComponent<playerMovement>().gameObject.name &&
                 direction == player.direction && !player.onion)
        {
            drawLine(hit.point);
            drawLine(hit.point + Vector3.up * 3);
            drawLine(hit.point - Vector3.up * 3);
            var p = FindObjectOfType<pelletParticles>();
            int score1 = p.score1, score2 = p.score2;
            if (playerIndex == 0)
            {
                p.score1 += Mathf.FloorToInt(score2 / 5);
                p.score2 -= Mathf.FloorToInt(score2 / 5);
            }
            else if (playerIndex == 1)
            {
                p.score2 += Mathf.FloorToInt(score1 / 5);
                p.score1 -= Mathf.FloorToInt(score1 / 5);
            }
            player.killPlayer();
        }
    }
}
