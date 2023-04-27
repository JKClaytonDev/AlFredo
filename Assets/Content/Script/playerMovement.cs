using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
public class playerMovement : MonoBehaviour
{
    public float onionTime;
    public bool onion = false;
    public float pepperTime;
    public bool pepper = false;
    public bool dead;
    public Vector3 movingTargetPos;
    public bool movingTarget = false;
    public int playerIndex;
    public GameObject deadDog;
    Rigidbody mainBody;
    public SpriteRenderer directionSprite;
    public float frameTime = 0.3f;
    public int health = 10;
    float lineTime;
    public Sprite[] chefSprites;
    public GameObject onionObject;
    public AudioClip fireSound;
    public LineRenderer l;
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
    private void Start()
    {
        direction = (Vector3.forward);
        targetPos = realtimeTargetPos;
        health = 10;
        mainBody = GetComponent<Rigidbody>();
        realtimeTargetPos = transform.position;
    }
    public float moveTime;
    public Vector3 targetPos;
    public Vector3 realtimeTargetPos;
    Vector3 direction;
    public bool keyPressed;
    int directionSpriteIndex;
    bool verticalFirst;
    Vector3 dir = Vector3.left;
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
        float moveFrameTime = frameTime;
        if (pepper)
            moveFrameTime *= 0.5f;
        directionSprite.sprite = chefSprites[directionSpriteIndex + (int)(spriteFrameTime - (MathF.Floor(spriteFrameTime / 4) * 4))];
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
            if (lastPos.x < transform.position.x) //Right
            {
                direction = (Vector3.right);
                directionSpriteIndex = 0;
            }
            else if (lastPos.x > transform.position.x) //Left
            {
                direction = (Vector3.left);
                directionSpriteIndex = 5;
            }
            else if (lastPos.z > transform.position.z) //Down
            {
                direction = (Vector3.back);
                directionSpriteIndex = 10;
            }
            else if (lastPos.z < transform.position.z) //Up
            {
                direction = (Vector3.forward);
                directionSpriteIndex = 15;
            }
            if (transform.position.x ==movingTargetPos.x && transform.position.z == movingTargetPos.z)
            {
                Time.timeScale = 1;
                movingTarget = false;
                realtimeTargetPos = transform.position;
                targetPos = realtimeTargetPos;
                Debug.Log("REACHED TARGET");
            }
            
            return;
        }
        Time.timeScale = 1;
        transform.position = Vector3.MoveTowards(transform.position, realtimeTargetPos, Time.deltaTime / moveFrameTime);
        if ((Input.GetKeyDown(KeyCode.W) && playerIndex == 0) || (Input.GetKeyDown(KeyCode.UpArrow) && playerIndex == 1))
        {
            direction = (Vector3.forward);
            directionSpriteIndex = 15;
        }
        if ((Input.GetKeyDown(KeyCode.A) && playerIndex == 0) || (Input.GetKeyDown(KeyCode.LeftArrow) && playerIndex == 1))
        {
            direction = (Vector3.left);
            directionSpriteIndex = 5;
        }
        if ((Input.GetKeyDown(KeyCode.S) && playerIndex == 0) || (Input.GetKeyDown(KeyCode.DownArrow) && playerIndex == 1))
        {
            direction = (Vector3.back);
            directionSpriteIndex = 10;

        }
        if ((Input.GetKeyDown(KeyCode.D) && playerIndex == 0) || (Input.GetKeyDown(KeyCode.RightArrow) && playerIndex == 1))
        {
            direction = (Vector3.right);
            directionSpriteIndex = 0;
        }


        RaycastHit target;
        if (keyPressed)
        {
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
        
        {
            RaycastHit hit;
            if (Physics.Raycast(realtimeTargetPos, direction, out hit))
            {
                if (hit.transform.gameObject.GetComponent<AIScript>())
                {
                    if (direction == hit.transform.gameObject.GetComponent<AIScript>().direction)
                    {
                        drawLine(hit.point);
                        drawLine(hit.point+Vector3.up*3);
                        drawLine(hit.point - Vector3.up*3);
                        GameObject newCheck = Instantiate(deadDog);
                        newCheck.transform.position = hit.transform.gameObject.transform.position;
                        newCheck.transform.parent = null;
                        Destroy(hit.transform.gameObject);
                    }
                }
                else if (hit.transform.gameObject.GetComponent<playerMovement>())
                {
                    if (!(hit.transform.gameObject.GetComponent<playerMovement>().gameObject.name == GetComponent<playerMovement>().gameObject.name))
                    {
                        if (direction == hit.transform.gameObject.GetComponent<playerMovement>().direction && !hit.transform.gameObject.GetComponent<playerMovement>().onion)
                        {
                            drawLine(hit.point);
                            drawLine(hit.point + Vector3.up*3);
                            drawLine(hit.point - Vector3.up*3);
                            pelletParticles p = FindObjectOfType<pelletParticles>();
                            if (playerIndex == 0)
                            {
                                p.score1 += Mathf.FloorToInt(p.score2 / 5);
                                p.score2 -= Mathf.FloorToInt(p.score2 / 5);
                            }
                            else if (playerIndex == 1)
                            {
                                p.score2 += Mathf.FloorToInt(p.score1 / 5);
                                p.score1 -= Mathf.FloorToInt(p.score1 / 5);
                            }
                            hit.transform.gameObject.GetComponent<playerMovement>().killPlayer();
                        }
                    }
                }
            }
        }
    }
}
